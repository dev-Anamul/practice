using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Interaction controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class InteractionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<InteractionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public InteractionController(IUnitOfWork context, ILogger<InteractionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/interaction
        /// </summary>
        /// <param name="interaction">Interaction object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateInteraction)]
        public async Task<IActionResult> CreateInteraction(Domain.Entities.Interaction interaction)
        {
            try
            {
                interaction.DateCreated = DateTime.Now;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                var interactionInDb = context.InteractionRepository.Add(interaction);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadInteractionByKey", new { key = interaction.Oid }, interaction);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "Createinteraction", "InteractionController.cs", ex.Message, interaction.CreatedIn, interaction.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/interactions
        /// </summary>
        /// <returns>Http status code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInteractions)]
        public async Task<IActionResult> ReadInteractions()
        {
            try
            {
                var interaction = await context.InteractionRepository.GetInteractions();

                return Ok(interaction);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInteractions", "InteractionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/interaction/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Interactions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInteractionByKey)]
        public async Task<IActionResult> ReadInteractionByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var interaction = await context.InteractionRepository.GetInteractionByKey(key);

                if (interaction == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(interaction);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInteractionByKey", "InteractionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/interaction/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Interactions.</param>
        /// <param name="interaction">Interaction to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateInteraction)]
        public async Task<IActionResult> UpdateInteraction(Guid key, Domain.Entities.Interaction interaction)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interaction.DateModified = DateTime.Now;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Update(interaction);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateInteraction", "InteractionController.cs", ex.Message, interaction.ModifiedIn, interaction.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/interaction/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Interaction.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteInteraction)]
        public async Task<IActionResult> DeleteInteraction(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = true;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);
                await context.SaveChangesAsync();

                return Ok(interactionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteInteraction", "InteractionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}