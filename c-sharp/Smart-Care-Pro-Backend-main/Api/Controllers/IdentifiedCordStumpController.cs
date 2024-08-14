using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedCordStump controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedCordStumpController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedCordStumpController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedCordStumpController(IUnitOfWork context, ILogger<IdentifiedCordStumpController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump
        /// </summary>
        /// <param name="identifiedCordStump">IdentifiedCordStump object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedCordStump)]
        public async Task<IActionResult> CreateIdentifiedCordStump(IdentifiedCordStump identifiedCordStump)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedCordStump, identifiedCordStump.EncounterType);
                interaction.EncounterId = identifiedCordStump.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedCordStump.CreatedBy;
                interaction.CreatedIn = identifiedCordStump.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedCordStump.InteractionId = interactionId;
                identifiedCordStump.DateCreated = DateTime.Now;
                identifiedCordStump.IsDeleted = false;
                identifiedCordStump.IsSynced = false;

                context.IdentifiedCordStumpRepository.Add(identifiedCordStump);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedCordStumpByKey", new { key = identifiedCordStump.InteractionId }, identifiedCordStump);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedCordStump", "IdentifiedCordStumpController.cs", ex.Message, identifiedCordStump.CreatedIn, identifiedCordStump.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stumps
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCordStumps)]
        public async Task<IActionResult> ReadIdentifiedCordStumps()
        {
            try
            {
                var identifiedCordStumpInDb = await context.IdentifiedCordStumpRepository.GetIdentifiedCordStumps();

                return Ok(identifiedCordStumpInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCordStumps", "IdentifiedCordStumpController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCordStumps.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCordStumpByKey)]
        public async Task<IActionResult> ReadIdentifiedCordStumpByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedCordStumpIndb = await context.IdentifiedCordStumpRepository.GetIdentifiedCordStumpByKey(key);

                if (identifiedCordStumpIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedCordStumpIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCordStumpByKey", "IdentifiedCordStumpController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table IdentifiedCordStumps.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCordStumpByEncounter)]
        public async Task<IActionResult> ReadIdentifiedCordStumpByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedCordStumpInDb = await context.IdentifiedCordStumpRepository.GetIdentifiedCordStumpByEncounter(encounterId);

                return Ok(identifiedCordStumpInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCordStumpByEncounter", "IdentifiedCordStumpController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump/by-assessment/{assessmentId}
        /// </summary>
        /// <param name="assessmentId">Primary key of the table IdentifiedCordStumps.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCordStumpByAssessment)]
        public async Task<IActionResult> ReadIdentifiedCordStumpByAssessment(Guid assessmentId)
        {
            try
            {
                var identifiedCordStumpInDb = await context.IdentifiedCordStumpRepository.ReadIdentifiedCordStumpByAssessment(assessmentId);

                return Ok(identifiedCordStumpInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCordStumpByAssessment", "IdentifiedCordStumpController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCordStumps.</param>
        /// <param name="identifiedCordStump">IdentifiedCordStump to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedCordStump)]
        public async Task<IActionResult> UpdateIdentifiedCordStump(Guid key, IdentifiedCordStump identifiedCordStump)
        {
            try
            {
                if (key != identifiedCordStump.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedCordStump.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedCordStump.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedCordStump.DateModified = DateTime.Now;
                identifiedCordStump.IsDeleted = false;
                identifiedCordStump.IsSynced = false;

                context.IdentifiedCordStumpRepository.Update(identifiedCordStump);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedCordStump", "IdentifiedCordStumpController.cs", ex.Message, identifiedCordStump.ModifiedIn, identifiedCordStump.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-cord-stump/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCordStumps.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedCordStump)]
        public async Task<IActionResult> DeleteIdentifiedCordStump(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedCordStumpInDb = await context.IdentifiedCordStumpRepository.GetIdentifiedCordStumpByKey(key);

                if (identifiedCordStumpInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedCordStumpRepository.Update(identifiedCordStumpInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedCordStumpInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedCordStump", "IdentifiedCordStumpController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}