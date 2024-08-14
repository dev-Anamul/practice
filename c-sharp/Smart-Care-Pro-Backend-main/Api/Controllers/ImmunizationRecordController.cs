using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 26.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ImmunizationRecord controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class ImmunizationRecordController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ImmunizationRecordController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ImmunizationRecordController(IUnitOfWork context, ILogger<ImmunizationRecordController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/immunization-record
        /// </summary>
        /// <param name="immunizationRecord">ImmunizationRecord object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateImmunizationRecord)]
        public async Task<IActionResult> CreateImmunizationRecord(ImmunizationRecordDto immunizationRecord)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();

                if (immunizationRecord.ImmunizationRecordList.Any())
                {
                    foreach (var item in immunizationRecord.ImmunizationRecordList)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionID = Guid.NewGuid();

                        interaction.Oid = interactionID;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ImmunizationRecord, immunizationRecord.EncounterType);
                        interaction.EncounterId = immunizationRecord.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedIn = immunizationRecord.CreatedIn;
                        interaction.CreatedBy = immunizationRecord.CreatedBy;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        interactions.Add(interaction);

                        item.InteractionId = interactionID;
                        item.ClientId = immunizationRecord.ClientId;
                        item.EncounterId = immunizationRecord.EncounterId;
                        item.EncounterType = immunizationRecord.EncounterType;
                        item.CreatedBy = immunizationRecord.CreatedBy;
                        item.CreatedIn = immunizationRecord.CreatedIn;
                        item.DateCreated = DateTime.Now;
                        item.CreatedBy = immunizationRecord.CreatedBy;
                        item.IsDeleted = false;
                        item.IsSynced = false;
                    }

                    context.InteractionRepository.AddRange(interactions);
                    context.ImmunizationRecordRepository.AddRange(immunizationRecord.ImmunizationRecordList);

                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadImmunizationRecordByKey", new { key = immunizationRecord.ImmunizationRecordList.Select(x => x.InteractionId).FirstOrDefault() }, immunizationRecord.ImmunizationRecordList.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateImmunizationRecord", "ImmunizationRecordController.cs", ex.Message, immunizationRecord.CreatedIn, immunizationRecord.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-record
        /// </summary>
        /// <param name="immunizationRecord"></param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVaccinationRecord)]
        public async Task<IActionResult> CreateVaccinationRecord(ImmunizationRecord immunizationRecord)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ImmunizationRecord, immunizationRecord.EncounterType);
                interaction.EncounterId = immunizationRecord.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedIn = immunizationRecord.CreatedIn;
                interaction.CreatedBy = immunizationRecord.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                immunizationRecord.InteractionId = interactionId;
                immunizationRecord.EncounterId = immunizationRecord.EncounterId;
                immunizationRecord.ClientId = immunizationRecord.ClientId;
                immunizationRecord.DateCreated = DateTime.Now;
                immunizationRecord.IsDeleted = false;
                immunizationRecord.IsSynced = false;

                context.ImmunizationRecordRepository.Add(immunizationRecord);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadImmunizationRecordByKey", new { key = immunizationRecord.InteractionId }, immunizationRecord);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVaccinationRecord", "ImmunizationRecordController.cs", ex.Message, immunizationRecord.CreatedIn, immunizationRecord.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-records
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImmunizationRecords)]
        public async Task<IActionResult> ReadImmunizationRecords()
        {
            try
            {
                var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecords();

                return Ok(immunizationRecordsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImmunizationRecords", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-record/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImmunizationRecordByKey)]
        public async Task<IActionResult> ReadImmunizationRecordByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByKey(key);

                if (immunizationRecordsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(immunizationRecordsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImmunizationRecordByKey", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-record/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImmunizationRecordByClient)]
        public async Task<IActionResult> ReadImmunizationRecordsByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByClient(clientId);
                    var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByClientLast24Hours(clientId);

                    return Ok(immunizationRecordsInDb);
                }
                else
                {
                    var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<ImmunizationRecord> immunizationRecordsDto = new PagedResultDto<ImmunizationRecord>()
                    {
                        Data = immunizationRecordsInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ImmunizationRecordRepository.GetImmunizationRecordByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(immunizationRecordsDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImmunizationRecordsByClientID", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-record/by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImmunizationRecordByEncounterId)]
        public async Task<IActionResult> ReadImmunizationRecordByEncounterId(Guid encounterId)
        {
            try
            {
                var immunizationRecordInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByEncounter(encounterId);

                return Ok(immunizationRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImmunizationRecordByEncounterId", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <param name="immunizationList">ImmunizationRecord to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateImmunizationRecord)]
        public async Task<IActionResult> UpdateImmunizationRecord(Guid key, ImmunizationRecord immunizationRecord)
        {
            try
            {
                if (key != immunizationRecord.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                var immunizationInDB = await context.ImmunizationRecordRepository.GetImmunizationRecordByKey(immunizationRecord.InteractionId);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(immunizationRecord.InteractionId);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = immunizationRecord.ModifiedBy;
                interactionInDb.ModifiedIn = immunizationRecord.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                immunizationInDB.DateModified = DateTime.Now;
                immunizationInDB.IsDeleted = false;
                immunizationInDB.IsSynced = false;

                immunizationInDB.DateGiven = immunizationRecord.DateGiven;
                immunizationInDB.DoseId = immunizationRecord.DoseId;
                immunizationInDB.BatchNumber = immunizationRecord.BatchNumber;

                context.ImmunizationRecordRepository.Update(immunizationInDB);


                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateImmunizationRecord", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <param name="immunizationRecord">ImmunizationRecord to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVaccinationRecord)]
        public async Task<IActionResult> UpdateVaccinationRecord(Guid key, ImmunizationRecord immunizationRecord)
        {
            try
            {


                if (key != immunizationRecord.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = immunizationRecord.ModifiedBy;
                interactionInDb.ModifiedIn = immunizationRecord.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var immunizationInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByKey(immunizationRecord.InteractionId);

                immunizationInDb.DateGiven = immunizationRecord.DateGiven;
                immunizationInDb.DoseId = immunizationRecord.DoseId;
                immunizationInDb.BatchNumber = immunizationRecord.BatchNumber;

                context.ImmunizationRecordRepository.Update(immunizationInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVaccinationRecord", "ImmunizationRecordController.cs", ex.Message, immunizationRecord.ModifiedIn, immunizationRecord.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/immunization-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteImmunizationRecord)]
        public async Task<IActionResult> DeleteImmunizationRecord(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var immunizationRecordsInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByKey(key);

                if (immunizationRecordsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                immunizationRecordsInDb.DateModified = DateTime.Now;
                immunizationRecordsInDb.IsDeleted = true;
                immunizationRecordsInDb.IsSynced = false;

                context.ImmunizationRecordRepository.Update(immunizationRecordsInDb);
                await context.SaveChangesAsync();

                return Ok(immunizationRecordsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteImmunizationRecord", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// immunization-record/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveImmunizationRecord)]
        public async Task<IActionResult> RemoveImmunizationRecord(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var immunizationRecordInDb = await context.ImmunizationRecordRepository.GetImmunizationRecordByEncounter(encounterId);

                if (immunizationRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in immunizationRecordInDb)
                {
                    context.ImmunizationRecordRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(immunizationRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveImmunizationRecord", "ImmunizationRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}