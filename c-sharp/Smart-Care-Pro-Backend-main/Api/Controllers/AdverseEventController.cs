using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Utilities.Constants;
using Interaction = Domain.Entities.Interaction;

/*
 * Created by    : Stephan
 * Date created  : 16.02.2023
 * Modified by   : Lion
 * Last modified : 16.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Adverse Event controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AdverseEventController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AdverseEventController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AdverseEventController(IUnitOfWork context, ILogger<AdverseEventController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/adverse-event
        /// </summary>
        /// <param name="adverseEvent">AdverseEvent object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAdverseEvent)]
        public async Task<IActionResult> CreateAdverseEvent(AdverseEvent adverseEvent)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.AdverseEvent, Enums.EncounterType.AdverseEvent);
                interaction.EncounterId = adverseEvent.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = adverseEvent.CreatedBy;
                interaction.CreatedIn = adverseEvent.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;
                context.InteractionRepository.Add(interaction);
                adverseEvent.InteractionId = interactionId;
                adverseEvent.DateCreated = DateTime.Now;
                adverseEvent.IsDeleted = false;
                adverseEvent.IsSynced = false;

                context.AdverseEventRepository.Add(adverseEvent);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAdverseEventByKey", new { key = adverseEvent.InteractionId }, adverseEvent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAdverseEvent", "AdverseEventController.cs", ex.Message, adverseEvent.CreatedIn, adverseEvent.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-events
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAdverseEvents)]
        public async Task<IActionResult> ReadAdverseEvents()
        {
            try
            {
                var adverseEventInDb = await context.AdverseEventRepository.GetAdverseEvents();

                adverseEventInDb = adverseEventInDb.OrderByDescending(x => x.DateCreated);

                return Ok(adverseEventInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAdverseEvents", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-event/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AdverseEvents.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAdverseEventByKey)]
        public async Task<IActionResult> ReadAdverseEventByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var adverseEventInDb = await context.AdverseEventRepository.GetAdverseEventByKey(key);

                if (adverseEventInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(adverseEventInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAdverseEventByKey", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-event/by-immunization/{immunizationId}
        /// </summary>
        /// <param name="immunizationId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAdverseEventByImmunization)]
        public async Task<IActionResult> ReadAdverseEventByImmunization(Guid immunizationId)
        {
            try
            {
                var adverseEventInDb = await context.AdverseEventRepository.GetAdverseEventByImmunization(immunizationId);

                return Ok(adverseEventInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAdverseEventByImmunization", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-event/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAdverseEventByEncounter)]
        public async Task<IActionResult> ReadAdverseEventByEncounter(Guid encounterId)
        {
            try
            {
                var adverseEventInDb = await context.AdverseEventRepository.GetAdverseEventByEncounter(encounterId);

                return Ok(adverseEventInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAdverseEventByEncounter", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-event/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AdverseEvents.</param>
        /// <param name="adverseEvent">AdverseEvent to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAdverseEvent)]
        public async Task<IActionResult> UpdateAdverseEvent(Guid key, AdverseEvent adverseEvent)
        {
            try
            {
                if (key != adverseEvent.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = adverseEvent.ModifiedBy;
                interactionInDb.ModifiedIn = adverseEvent.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                adverseEvent.DateModified = DateTime.Now;
                adverseEvent.IsDeleted = false;
                adverseEvent.IsSynced = false;

                context.AdverseEventRepository.Update(adverseEvent);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateAdverseEvent", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/adverse-event/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AdverseEvents.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAdverseEvent)]
        public async Task<IActionResult> DeleteAdverseEvent(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var adverseEventInDb = await context.AdverseEventRepository.GetAdverseEventByKey(key);

                if (adverseEventInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                adverseEventInDb.DateModified = DateTime.Now;
                adverseEventInDb.IsDeleted = true;
                adverseEventInDb.IsSynced = false;

                context.AdverseEventRepository.Update(adverseEventInDb);
                await context.SaveChangesAsync();

                return Ok(adverseEventInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAdverseEvent", "AdverseEventController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}