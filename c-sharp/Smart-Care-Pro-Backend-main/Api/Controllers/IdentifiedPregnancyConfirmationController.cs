using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedPregnancyConfirmation controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPregnancyConfirmationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPregnancyConfirmationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPregnancyConfirmationController(IUnitOfWork context, ILogger<IdentifiedPregnancyConfirmationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-pregnancy-confirmation
        /// </summary>
        /// <param name="identifiedPregnancyConfirmation">identifiedPregnancyConfirmation object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPregnancyConfirmation)]
        public async Task<IActionResult> CreateIdentifiedPregnancyConfirmation(IdentifiedPregnancyConfirmation identifiedPregnancyConfirmation)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPregnancyConfirmation, identifiedPregnancyConfirmation.EncounterType);
                interaction.EncounterId = identifiedPregnancyConfirmation.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPregnancyConfirmation.CreatedBy;
                interaction.CreatedIn = identifiedPregnancyConfirmation.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedPregnancyConfirmation.InteractionId = interactionID;
                identifiedPregnancyConfirmation.DateCreated = DateTime.Now;
                identifiedPregnancyConfirmation.IsDeleted = false;
                identifiedPregnancyConfirmation.IsSynced = false;

                context.IdentifiedPregnancyConfirmationRepository.Add(identifiedPregnancyConfirmation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPregnancyConfirmationByKey", new { key = identifiedPregnancyConfirmation.InteractionId }, identifiedPregnancyConfirmation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPregnancyConfirmation", "IdentifiedPregnancyConfirmationController.cs", ex.Message, identifiedPregnancyConfirmation.CreatedIn, identifiedPregnancyConfirmation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pregnancy-confirmations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPregnancyConfirmations)]
        public async Task<IActionResult> ReadIdentifiedPregnancyConfirmations()
        {
            try
            {
                var identifiedPregnancyConfirmationInDb = await context.IdentifiedPregnancyConfirmationRepository.GetIdentifiedPregnancyConfirmations();

                return Ok(identifiedPregnancyConfirmationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPregnancyConfirmations", "IdentifiedPregnancyConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pregnancy-confirmation/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPregnancyConfirmationByEncounter)]
        public async Task<IActionResult> ReadIdentifiedPregnancyConfirmationByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedPregnancyConfirmationInDb = await context.IdentifiedPregnancyConfirmationRepository.GetIdentifiedPregnancyConfirmationByEncounter(encounterId);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPregnancyConfirmationByEncounter", "IdentifiedPregnancyConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pregnancy-confirmation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPregnancyConfirmations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPregnancyConfirmationByKey)]
        public async Task<IActionResult> ReadIdentifiedPregnancyConfirmationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPregnancyConfirmationInDb = await context.IdentifiedPregnancyConfirmationRepository.GetIdentifiedPregnancyConfirmationByEncounter(key);

                if (identifiedPregnancyConfirmationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPregnancyConfirmationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPregnancyConfirmationByKey", "IdentifiedPregnancyConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/identified-pregnancy-confirmation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPregnancyConfirmations.</param>
        /// <param name="identifiedPregnancyConfirmation">identifiedPregnancyConfirmation to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPregnancyConfirmation)]
        public async Task<IActionResult> UpdateIdentifiedPregnancyConfirmation(Guid key, IdentifiedPregnancyConfirmation identifiedPregnancyConfirmation)
        {
            try
            {
                if (key != identifiedPregnancyConfirmation.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPregnancyConfirmation.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPregnancyConfirmation.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPregnancyConfirmation.DateModified = DateTime.Now;
                identifiedPregnancyConfirmation.IsDeleted = false;
                identifiedPregnancyConfirmation.IsSynced = false;

                context.IdentifiedPregnancyConfirmationRepository.Update(identifiedPregnancyConfirmation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPregnancyConfirmation", "IdentifiedPregnancyConfirmationController.cs", ex.Message, identifiedPregnancyConfirmation.ModifiedIn, identifiedPregnancyConfirmation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pregnancy-confirmation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPregnancyConfirmations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPregnancyConfirmation)]
        public async Task<IActionResult> DeleteIdentifiedPregnancyConfirmation(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPregnancyConfirmationInDb = await context.IdentifiedPregnancyConfirmationRepository.GetIdentifiedPregnancyConfirmationByKey(key);

                if (identifiedPregnancyConfirmationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedPregnancyConfirmationInDb.DateModified = DateTime.Now;
                identifiedPregnancyConfirmationInDb.IsDeleted = true;
                identifiedPregnancyConfirmationInDb.IsSynced = false;

                context.IdentifiedPregnancyConfirmationRepository.Update(identifiedPregnancyConfirmationInDb);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPregnancyConfirmation", "IdentifiedPregnancyConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}