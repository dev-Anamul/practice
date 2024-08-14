using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 07-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PainRecordController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PainRecordController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PainRecordController(IUnitOfWork context, ILogger<PainRecordController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pain-record
        /// </summary>
        /// <param name="painRecord">PainRecord object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePainRecord)]
        public async Task<IActionResult> CreatePainRecord(PainRecord painRecord)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PainRecord, painRecord.EncounterType);
                interaction.EncounterId = painRecord.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = painRecord.CreatedBy;
                interaction.CreatedIn = painRecord.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                painRecord.InteractionId = interactionId;
                painRecord.ClientId = painRecord.ClientId;
                painRecord.DateCreated = DateTime.Now;
                painRecord.IsDeleted = false;
                painRecord.IsSynced = false;

                context.PainRecordRepository.Add(painRecord);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPainRecordByKey", new { key = painRecord.InteractionId }, painRecord);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePainRecord", "PainRecordController.cs", ex.Message, painRecord.CreatedIn, painRecord.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-records
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainRecords)]
        public async Task<IActionResult> ReadPainRecords()
        {
            try
            {
                var painRecordInDb = await context.PainRecordRepository.GetPainRecords();

                return Ok(painRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainRecords", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainRecordByClient)]
        public async Task<IActionResult> ReadPainRecordByClient(Guid clientId)
        {
            try
            {
                var painRecordInDb = await context.PainRecordRepository.GetPainRecordByClient(clientId); ;

                return Ok(painRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainRecordByClient", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPD Visit.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainRecordByEncounter)]
        public async Task<IActionResult> ReadPainRecordByEncounter(Guid encounterId)
        {
            try
            {
                var painRecordInDb = await context.PainRecordRepository.GetPainRecordByEncounter(encounterId);

                return Ok(painRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainRecordByEncounter", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainRecords.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainRecordByKey)]
        public async Task<IActionResult> ReadPainRecordByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var painRecordInDb = await context.PainRecordRepository.GetPainRecordByKey(key);

                if (painRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(painRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainRecordByKey", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainRecord.</param>
        /// <param name="painRecord">PainRecord to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePainRecord)]
        public async Task<IActionResult> UpdatePainRecord(Guid key, PainRecord painRecord)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = painRecord.ModifiedBy;
                interactionInDb.ModifiedIn = painRecord.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != painRecord.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                painRecord.DateModified = DateTime.Now;
                painRecord.IsDeleted = false;
                painRecord.IsSynced = false;

                context.PainRecordRepository.Update(painRecord);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePainRecord", "PainRecordController.cs", ex.Message, painRecord.ModifiedIn, painRecord.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainRecord.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePainRecord)]
        public async Task<IActionResult> DeletePainRecord(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var painRecordInDb = await context.PainRecordRepository.GetPainRecordByKey(key);

                if (painRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                painRecordInDb.DateModified = DateTime.Now;
                painRecordInDb.IsDeleted = true;
                painRecordInDb.IsSynced = false;

                context.PainRecordRepository.Update(painRecordInDb);
                await context.SaveChangesAsync();

                return Ok(painRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePainRecord", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-record/remove/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainRecord.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePainRecord)]
        public async Task<IActionResult> RemovePainRecord(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintdb = await context.PainRecordRepository.GetPainRecordByOpdVisit(encounterId);

                if (chiefComplaintdb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in chiefComplaintdb)
                {
                    context.PainRecordRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(chiefComplaintdb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePainRecord", "PainRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}