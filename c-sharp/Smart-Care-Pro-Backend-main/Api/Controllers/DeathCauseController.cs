using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 19.02.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DeathCauseController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DeathCauseController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DeathCauseController(IUnitOfWork context, ILogger<DeathCauseController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/death-record
        /// </summary>
        /// <param name="deathRecord">BirthHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDeathCause)]
        public async Task<IActionResult> CreateDeathCause(DeathCause deathCause)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DeathCause, deathCause.EncounterType);
                interaction.EncounterId = deathCause.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = deathCause.CreatedBy;
                interaction.CreatedIn = deathCause.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                deathCause.InteractionId = interactionId;
                deathCause.EncounterId = deathCause.EncounterId;
                deathCause.DeathRecordId = deathCause.DeathRecordId;
                deathCause.DateCreated = DateTime.Now;
                deathCause.IsDeleted = false;
                deathCause.IsSynced = false;

                context.DeathCauseRepository.Add(deathCause);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDeathCauseByKey", new { key = deathCause.InteractionId }, deathCause);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDeathCause", "DeathCauseController.cs", ex.Message, deathCause.CreatedIn, deathCause.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-records
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathCauses)]
        public async Task<IActionResult> ReadDeathCauses()
        {
            try
            {
                var deathCauseInDb = await context.DeathCauseRepository.GetDeathCauses();
                deathCauseInDb = deathCauseInDb.OrderByDescending(x => x.DateCreated);
                return Ok(deathCauseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathCauses", "DeathCauseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/ByClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathCauseByDeathRecord)]
        public async Task<IActionResult> ReadDeathCauseByDeathRecord(Guid deathRecordId)
        {
            try
            {
                var deathCauseInDb = await context.DeathCauseRepository.GetDeathCauseByDeathRecordID(deathRecordId);

                return Ok(deathCauseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathCauseByDeathRecord", "DeathCauseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeathCauseByKey)]
        public async Task<IActionResult> ReadDeathCauseByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deathCauseInDb = await context.DeathCauseRepository.GetDeathCauseByKey(key);

                if (deathCauseInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(deathCauseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeathCauseByKey", "DeathCauseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/death-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="birthHistory">BirthHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDeathCause)]
        public async Task<IActionResult> UpdateDeathCause(Guid key, DeathCause deathCause)
        {
            try
            {
                if (key != deathCause.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = deathCause.ModifiedBy;
                interactionInDb.ModifiedIn = deathCause.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                deathCause.DateModified = DateTime.Now;
                deathCause.IsDeleted = false;
                deathCause.IsSynced = false;

                context.DeathCauseRepository.Update(deathCause);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDeathCause", "DeathCauseController.cs", ex.Message, deathCause.ModifiedIn, deathCause.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/death-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDeathCause)]
        public async Task<IActionResult> DeleteDeathCause(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deathCauseInDb = await context.DeathCauseRepository.GetDeathCauseByKey(key);

                if (deathCauseInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deathCauseInDb.DateModified = DateTime.Now;
                deathCauseInDb.IsDeleted = true;
                deathCauseInDb.IsSynced = false;

                context.DeathCauseRepository.Update(deathCauseInDb);
                await context.SaveChangesAsync();

                return Ok(deathCauseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDeathCause", "DeathCauseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}