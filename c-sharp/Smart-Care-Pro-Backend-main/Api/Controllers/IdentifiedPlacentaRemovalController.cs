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
    /// IdentifiedPlacentaRemoval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPlacentaRemovalController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPlacentaRemovalController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPlacentaRemovalController(IUnitOfWork context, ILogger<IdentifiedPlacentaRemovalController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removal
        /// </summary>
        /// <param name="identifiedPlacentaRemoval">IdentifiedPlacentaRemoval object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPlacentaRemoval)]
        public async Task<IActionResult> CreateIdentifiedPlacentaRemoval(IdentifiedPlacentaRemoval identifiedPlacentaRemoval)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPlacentaRemoval, identifiedPlacentaRemoval.EncounterType);
                interaction.EncounterId = identifiedPlacentaRemoval.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPlacentaRemoval.CreatedBy;
                interaction.CreatedIn = identifiedPlacentaRemoval.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedPlacentaRemoval.InteractionId = interactionId;
                identifiedPlacentaRemoval.DateCreated = DateTime.Now;
                identifiedPlacentaRemoval.IsDeleted = false;
                identifiedPlacentaRemoval.IsSynced = false;

                context.IdentifiedPlacentaRemovalRepository.Add(identifiedPlacentaRemoval);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPlacentaRemovalByKey", new { key = identifiedPlacentaRemoval.InteractionId }, identifiedPlacentaRemoval);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPlacentaRemoval", "IdentifiedPlacentaRemovalController.cs", ex.Message, identifiedPlacentaRemoval.CreatedIn, identifiedPlacentaRemoval.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPlacentaRemovals)]
        public async Task<IActionResult> ReadIdentifiedPlacentaRemovals()
        {
            try
            {
                var identifiedPlacentaRemovalInDb = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovals();

                return Ok(identifiedPlacentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPlacentaRemovals", "IdentifiedPlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removal/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPlacentaRemovals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPlacentaRemovalByKey)]
        public async Task<IActionResult> ReadIdentifiedPlacentaRemovalByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPlacentaRemovalInDb = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovalByKey(key);

                if (identifiedPlacentaRemovalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPlacentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPlacentaRemovalByKey", "IdentifiedPlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removal/by-placentaRemoval/{PlacentaRemovalId}
        /// </summary>
        /// <param name="placentaRemovalId">Primary key of the table IdentifiedPlacentaRemovals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPlacentaRemovalByPlacentaRemoval)]
        public async Task<IActionResult> ReadIdentifiedPlacentaRemovalByPlacentaRemoval(Guid placentaRemovalId)
        {
            try
            {
                var identifiedPlacentaRemovalInDb = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovalByPlacentaRemoval(placentaRemovalId);

                return Ok(identifiedPlacentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPlacentaRemovalByPlacentaRemoval", "IdentifiedPlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removal/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPlacentaRemovals.</param>
        /// <param name="identifiedPlacentaRemoval">IdentifiedPlacentaRemoval to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPlacentaRemoval)]
        public async Task<IActionResult> UpdateIdentifiedPlacentaRemoval(Guid key, IdentifiedPlacentaRemoval identifiedPlacentaRemoval)
        {
            try
            {
                if (key != identifiedPlacentaRemoval.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPlacentaRemoval.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPlacentaRemoval.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPlacentaRemoval.DateModified = DateTime.Now;
                identifiedPlacentaRemoval.IsDeleted = false;
                identifiedPlacentaRemoval.IsSynced = false;

                context.IdentifiedPlacentaRemovalRepository.Update(identifiedPlacentaRemoval);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPlacentaRemoval", "IdentifiedPlacentaRemovalController.cs", ex.Message, identifiedPlacentaRemoval.ModifiedIn, identifiedPlacentaRemoval.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-placenta-removal/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPlacentaRemovals.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPlacentaRemoval)]
        public async Task<IActionResult> DeleteIdentifiedPlacentaRemoval(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPlacentaRemovalInDb = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovalByKey(key);

                if (identifiedPlacentaRemovalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedPlacentaRemovalRepository.Update(identifiedPlacentaRemovalInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedPlacentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPlacentaRemoval", "IdentifiedPlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}