using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : Stephan
 * Last modified : 16.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BirthRecordController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BirthRecordController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BirthRecordController(IUnitOfWork context, ILogger<BirthRecordController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/birth-record
        /// </summary>
        /// <param name="birthRecord">BirthRecord object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBirthRecord)]
        public async Task<IActionResult> CreateBirthRecord(BirthRecord birthRecord)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.BirthRecord, Enums.EncounterType.BirthRecords);
                interaction.EncounterId = birthRecord.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = birthRecord.CreatedBy;
                interaction.CreatedIn = birthRecord.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                birthRecord.InteractionId = interactionId;
                birthRecord.EncounterId = birthRecord.EncounterId;
                birthRecord.ClientId = birthRecord.ClientId;
                birthRecord.DateCreated = DateTime.Now;
                birthRecord.IsDeleted = false;
                birthRecord.IsSynced = false;

                if (birthRecord.InformantCellphoneCountryCode == "260" && (!string.IsNullOrEmpty(birthRecord.InformantCellphone)&& birthRecord.InformantCellphone[0] == '0'))
                    birthRecord.InformantCellphone = birthRecord.InformantCellphone.Substring(1);
                
                context.BirthRecordRepository.Add(birthRecord);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadBirthRecordByKey", new { key = birthRecord.InteractionId }, birthRecord);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBirthRecord", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/birth-records
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthRecords)]
        public async Task<IActionResult> ReadBirthRecords()
        {
            try
            {
                var birthRecordInDb = await context.BirthRecordRepository.GetBirthRecords();

                birthRecordInDb = birthRecordInDb.OrderByDescending(x => x.DateCreated);

                return Ok(birthRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthRecords", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-record/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthRecordByClient)]
        public async Task<IActionResult> ReadBirthRecordByClient(Guid clientId)
        {
            try
            {
                var birthRecordInDb = await context.BirthRecordRepository.GetBirthRecordByClient(clientId);

                return Ok(birthRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthRecordByClient", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-record/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthRecordByKey)]
        public async Task<IActionResult> ReadBirthRecordByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthRecordInDb = await context.BirthRecordRepository.GetBirthRecordByKey(key);

                if (birthRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(birthRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthRecordByKey", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/birth-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <param name="birthRecord">BirthRecord to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBirthRecord)]
        public async Task<IActionResult> UpdateBirthRecord(Guid key, BirthRecord birthRecord)
        {
            try
            {
                if (key != birthRecord.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = birthRecord.ModifiedBy;
                interactionInDb.ModifiedIn = birthRecord.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                birthRecord.DateModified = DateTime.Now;
                birthRecord.IsDeleted = false;
                birthRecord.IsSynced = false;

                if (birthRecord.InformantCellphoneCountryCode == "260" && (!string.IsNullOrEmpty(birthRecord.InformantCellphone) && birthRecord.InformantCellphone[0] == '0'))
                    birthRecord.InformantCellphone = birthRecord.InformantCellphone.Substring(1);

                context.BirthRecordRepository.Update(birthRecord);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBirthRecord", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteBirthRecord)]
        public async Task<IActionResult> DeleteBirthRecord(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthRecordInDb = await context.BirthRecordRepository.GetBirthRecordByKey(key);

                if (birthRecordInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                birthRecordInDb.DateModified = DateTime.Now;
                birthRecordInDb.IsDeleted = true;
                birthRecordInDb.IsSynced = false;

                context.BirthRecordRepository.Update(birthRecordInDb);
                await context.SaveChangesAsync();

                return Ok(birthRecordInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteBirthRecord", "BirthRecordController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}