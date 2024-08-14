using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 01.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedDeliveryIntervention controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedDeliveryInterventionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedDeliveryInterventionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedDeliveryInterventionController(IUnitOfWork context, ILogger<IdentifiedDeliveryInterventionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-intervention
        /// </summary>
        /// <param name="identifiedDeliveryIntervention">IdentifiedDeliveryIntervention object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedDeliveryIntervention)]
        public async Task<IActionResult> CreateIdentifiedDeliveryIntervention(IdentifiedDeliveryIntervention identifiedDeliveryIntervention)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedDeliveryIntervention, identifiedDeliveryIntervention.EncounterType);
                interaction.EncounterId = identifiedDeliveryIntervention.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedDeliveryIntervention.CreatedBy;
                interaction.CreatedIn = identifiedDeliveryIntervention.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedDeliveryIntervention.InteractionId = interactionId;
                identifiedDeliveryIntervention.DateCreated = DateTime.Now;
                identifiedDeliveryIntervention.IsDeleted = false;
                identifiedDeliveryIntervention.IsSynced = false;

                context.IdentifiedDeliveryInterventionRepository.Add(identifiedDeliveryIntervention);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedDeliveryInterventionByKey", new { key = identifiedDeliveryIntervention.InteractionId }, identifiedDeliveryIntervention);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedDeliveryIntervention", "IdentifiedDeliveryInterventionController.cs", ex.Message, identifiedDeliveryIntervention.CreatedIn, identifiedDeliveryIntervention.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-interventions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedDeliveryInterventions)]
        public async Task<IActionResult> ReadIdentifiedDeliveryInterventions()
        {
            try
            {
                var identifiedDeliveryInterventionsInDb = await context.IdentifiedDeliveryInterventionRepository.GetIdentifiedDeliveryInterventions();

                return Ok(identifiedDeliveryInterventionsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedDeliveryInterventions", "IdentifiedDeliveryInterventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-intervention/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedDeliveryInterventions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedDeliveryInterventionByKey)]
        public async Task<IActionResult> ReadIdentifiedDeliveryInterventionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedDeliveryInterventionsInDb = await context.IdentifiedDeliveryInterventionRepository.GetIdentifiedDeliveryInterventionByKey(key);

                if (identifiedDeliveryInterventionsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedDeliveryInterventionsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedDeliveryInterventionByKey", "IdentifiedDeliveryInterventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-intervention/byDelivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table IdentifiedDeliveryInterventions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedDeliveryInterventionByDelivery)]
        public async Task<IActionResult> ReadIdentifiedDeliveryInterventionByDelivery(Guid deliveryId)
        {
            try
            {
                var identifiedDeliveryInterventionsInDb = await context.IdentifiedDeliveryInterventionRepository.GetIdentifiedDeliveryInterventionByDelivery(deliveryId);

                return Ok(identifiedDeliveryInterventionsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedDeliveryInterventionByDelivery", "IdentifiedDeliveryInterventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-intervention/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedDeliveryInterventions.</param>
        /// <param name="identifiedDeliveryIntervention">IdentifiedDeliveryIntervention to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedDeliveryIntervention)]
        public async Task<IActionResult> UpdateIdentifiedDeliveryIntervention(Guid key, IdentifiedDeliveryIntervention identifiedDeliveryIntervention)
        {
            try
            {
                if (key != identifiedDeliveryIntervention.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedDeliveryIntervention.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedDeliveryIntervention.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedDeliveryIntervention.DateModified = DateTime.Now;
                identifiedDeliveryIntervention.IsDeleted = false;
                identifiedDeliveryIntervention.IsSynced = false;

                context.IdentifiedDeliveryInterventionRepository.Update(identifiedDeliveryIntervention);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedDeliveryIntervention", "IdentifiedDeliveryInterventionController.cs", ex.Message, identifiedDeliveryIntervention.ModifiedIn, identifiedDeliveryIntervention.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-delivery-intervention/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedDeliveryInterventions.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedDeliveryIntervention)]
        public async Task<IActionResult> DeleteIdentifiedDeliveryIntervention(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedDeliveryInterventionsInDb = await context.IdentifiedDeliveryInterventionRepository.GetIdentifiedDeliveryInterventionByKey(key);

                if (identifiedDeliveryInterventionsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedDeliveryInterventionRepository.Update(identifiedDeliveryInterventionsInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedDeliveryInterventionsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedDeliveryIntervention", "IdentifiedDeliveryInterventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}