using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : Lion
 * Last modified : 26.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// DeathRecord  Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DeathRecordController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DeathRecordController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DeathRecordController(IUnitOfWork context, ILogger<DeathRecordController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/death-record
        /// </summary>
        /// <param name="deathRecord">DeathRecord object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDeathRecord)]
        public async Task<IActionResult> CreateDeathRecord(DeathRecord deathRecord)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DeathRecord, Enums.EncounterType.DeathRecords);
                interaction.EncounterId = deathRecord.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = deathRecord.CreatedBy;
                interaction.CreatedIn = deathRecord.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                if (deathRecord.InformantCellphoneCountryCode == "260" && (!string.IsNullOrEmpty(deathRecord.InformantCellphone) && deathRecord.InformantCellphone[0] == '0'))
                    deathRecord.InformantCellphone = deathRecord.InformantCellphone.Substring(1);

                deathRecord.InteractionId = interactionId;
                deathRecord.EncounterId = deathRecord.EncounterId;
                deathRecord.ClientId = deathRecord.ClientId;

                deathRecord.DateCreated = DateTime.Now;
                deathRecord.IsDeleted = false;
                deathRecord.IsSynced = false;


                foreach (var item in deathRecord.DeathCause)
                {
                    item.CauseType = Enums.CauseType.ContributingCauseOfDeath;
                    item.DeathRecordId = interactionId;
                    item.DateCreated = DateTime.Now;
                    item.CreatedBy = Guid.Empty;
                    item.EncounterId = deathRecord.EncounterId;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                }

                DeathCause deathCause = new DeathCause()
                {
                    DeathRecordId = interactionId,
                    ICD11Id = deathRecord.ICD11ID,
                    ICPC2Id = deathRecord.ICPC2ID,
                    CauseType = Enums.CauseType.MainCauseOfDeath,
                    DateCreated = DateTime.Now,
                    CreatedBy = deathRecord.CreatedBy,
                    CreatedIn = deathRecord.CreatedIn,
                    EncounterId = deathRecord.EncounterId,
                    IsSynced = false,
                    IsDeleted = false
                };
                context.DeathCauseRepository.Add(deathCause);
                //deathRecord.DeathCauses.Add(deathCause);
                context.DeathCauseRepository.AddRange(deathRecord.DeathCause);
                context.DeathRecordRepository.Add(deathRecord);

                ///Updating Client
                ///
                var client = await context.ClientRepository.GetClientByKey(deathRecord.ClientId);
                client.DateModified = DateTime.Now;
                client.IsDeleted = false;
                client.IsSynced = false;
                client.IsDeceased = true;
                client.ModifiedBy = deathRecord.CreatedBy;
                client.ModifiedIn = deathRecord.ModifiedIn;

                context.ClientRepository.Update(client);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDeathRecordByKey", new { key = deathRecord.InteractionId }, deathRecord);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDeathRecord", "DeathRecordController.cs", ex.Message, deathRecord.CreatedIn, deathRecord.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-records
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathRecords)]
        public async Task<IActionResult> ReadDeathRecords()
        {
            try
            {
                var deathRecordInDb = await context.DeathRecordRepository.GetDeathRecords();
                deathRecordInDb = deathRecordInDb.OrderByDescending(x => x.DateCreated);
                return Ok(deathRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathRecords", "DeathRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/ByClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathRecordByClient)]
        public async Task<IActionResult> ReadDeathRecordByClient(Guid clientId)
        {
            try
            {
                var deathRecordInDb = await context.DeathRecordRepository.GetDeathRecordByClient(clientId);

                return Ok(deathRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathRecordByClient", "DeathRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DeathRecords.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathRecordByKey)]
        public async Task<IActionResult> ReadDeathRecordByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deathRecordInDb = await context.DeathRecordRepository.GetDeathRecordByKey(key);

                if (deathRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(deathRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathRecordByKey", "DeathRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/death-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DeathRecords.</param>
        /// <param name="deathRecord">DeathRecord to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDeathRecord)]
        public async Task<IActionResult> UpdateDeathRecord(Guid key, DeathRecord deathRecord)
        {

            try
            {
                if (key != deathRecord.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = deathRecord.ModifiedBy;
                interactionInDb.ModifiedIn = deathRecord.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                deathRecord.DateModified = DateTime.Now;
                deathRecord.IsDeleted = false;
                deathRecord.IsSynced = false;

                if (deathRecord.InformantCellphoneCountryCode == "260" && (!string.IsNullOrEmpty(deathRecord.InformantCellphone) && deathRecord.InformantCellphone[0] == '0'))
                    deathRecord.InformantCellphone = deathRecord.InformantCellphone.Substring(1);

                var deathCausesDB = await context.DeathCauseRepository.GetDeathCauseByDeathRecordID(deathRecord.InteractionId);

                if (deathCausesDB != null)
                {
                    foreach (var data in deathCausesDB)
                    {
                        context.DeathCauseRepository.Delete(data);
                        await context.SaveChangesAsync();
                    }
                }

                foreach (var item in deathRecord.DeathCause)
                {
                    item.DeathRecordId = deathRecord.InteractionId;
                    item.CauseType = Enums.CauseType.ContributingCauseOfDeath;
                    item.DateModified = DateTime.Now;
                    item.ModifiedBy = deathRecord.CreatedBy;
                    item.EncounterId = deathRecord.EncounterId;
                    item.EncounterId = deathRecord.EncounterId;
                    item.IsSynced = false;
                    item.IsDeleted = false;
                }

                DeathCause deathCause = new DeathCause()
                {
                    DeathRecordId = deathRecord.InteractionId,
                    ICD11Id = deathRecord.ICD11ID,
                    ICPC2Id = deathRecord.ICPC2ID,
                    CauseType = Enums.CauseType.MainCauseOfDeath,
                    DateModified = DateTime.Now,
                    ModifiedBy = Guid.Empty,
                    EncounterId = deathRecord.EncounterId,
                    IsSynced = false,
                    IsDeleted = false
                };

                //deathRecord.DeathCauses.Add(deathCause);
                context.DeathCauseRepository.Add(deathCause);
                context.DeathCauseRepository.AddRange(deathRecord.DeathCause);
                context.DeathRecordRepository.Update(deathRecord);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDeathRecord", "DeathRecordController.cs", ex.Message, deathRecord.ModifiedIn, deathRecord.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Deathrecords.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDeathRecord)]
        public async Task<IActionResult> DeleteDeathRecord(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deathRecordInDb = await context.DeathRecordRepository.GetDeathRecordByKey(key);

                if (deathRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deathRecordInDb.DateModified = DateTime.Now;
                deathRecordInDb.IsDeleted = true;
                deathRecordInDb.IsSynced = false;

                context.DeathRecordRepository.Update(deathRecordInDb);
                await context.SaveChangesAsync();

                return Ok(deathRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDeathRecord", "DeathRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}