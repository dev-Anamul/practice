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
    /// IdentifiedCurrentDeliveryComplication controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedCurrentDeliveryComplicationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedCurrentDeliveryComplicationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedCurrentDeliveryComplicationController(IUnitOfWork context, ILogger<IdentifiedCurrentDeliveryComplicationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication
        /// </summary>
        /// <param name="identifiedCurrentDeliveryComplication">IdentifiedCurrentDeliveryComplication object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedCurrentDeliveryComplication)]
        public async Task<IActionResult> CreateIdentifiedCurrentDeliveryComplication(IdentifiedCurrentDeliveryComplication identifiedCurrentDeliveryComplication)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedCurrentDeliveryComplication, identifiedCurrentDeliveryComplication.EncounterType);
                interaction.EncounterId = identifiedCurrentDeliveryComplication.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedCurrentDeliveryComplication.CreatedBy;
                interaction.CreatedIn = identifiedCurrentDeliveryComplication.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedCurrentDeliveryComplication.InteractionId = interactionId;
                identifiedCurrentDeliveryComplication.DateCreated = DateTime.Now;
                identifiedCurrentDeliveryComplication.IsDeleted = false;
                identifiedCurrentDeliveryComplication.IsSynced = false;

                context.IdentifiedCurrentDeliveryComplicationRepository.Add(identifiedCurrentDeliveryComplication);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedCurrentDeliveryComplicationByKey", new { key = identifiedCurrentDeliveryComplication.InteractionId }, identifiedCurrentDeliveryComplication);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedCurrentDeliveryComplication", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message, identifiedCurrentDeliveryComplication.CreatedIn, identifiedCurrentDeliveryComplication.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complications
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCurrentDeliveryComplications)]
        public async Task<IActionResult> ReadIdentifiedCurrentDeliveryComplications()
        {
            try
            {
                var identifiedCurrentDeliveryComplicationInDb = await context.IdentifiedCurrentDeliveryComplicationRepository.GetIdentifiedCurrentDeliveryComplications();

                return Ok(identifiedCurrentDeliveryComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCurrentDeliveryComplications", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCurrentDeliveryComplicationByKey)]
        public async Task<IActionResult> ReadIdentifiedCurrentDeliveryComplicationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedCurrentDeliveryComplicationInDb = await context.IdentifiedCurrentDeliveryComplicationRepository.GetIdentifiedCurrentDeliveryComplicationByKey(key);

                if (identifiedCurrentDeliveryComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedCurrentDeliveryComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCurrentDeliveryComplicationByKey", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/byDelivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedCurrentDeliveryComplicationByDelivery)]
        public async Task<IActionResult> ReadIdentifiedCurrentDeliveryComplicationByDelivery(Guid deliveryId)
        {
            try
            {
                var identifiedCurrentDeliveryComplicationInDb = await context.IdentifiedCurrentDeliveryComplicationRepository.GetIdentifiedCurrentDeliveryComplicationByDelivery(deliveryId);

                return Ok(identifiedCurrentDeliveryComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedCurrentDeliveryComplicationByDelivery", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
        /// <param name="identifiedCurrentDeliveryComplication">IdentifiedCurrentDeliveryComplication to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedCurrentDeliveryComplication)]
        public async Task<IActionResult> UpdateIdentifiedCurrentDeliveryComplication(Guid key, IdentifiedCurrentDeliveryComplication identifiedCurrentDeliveryComplication)
        {
            try
            {
                if (key != identifiedCurrentDeliveryComplication.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedCurrentDeliveryComplication.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedCurrentDeliveryComplication.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedCurrentDeliveryComplication.DateModified = DateTime.Now;
                identifiedCurrentDeliveryComplication.IsDeleted = false;
                identifiedCurrentDeliveryComplication.IsSynced = false;

                context.IdentifiedCurrentDeliveryComplicationRepository.Update(identifiedCurrentDeliveryComplication);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedCurrentDeliveryComplication", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message, identifiedCurrentDeliveryComplication.ModifiedIn, identifiedCurrentDeliveryComplication.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedCurrentDeliveryComplication)]
        public async Task<IActionResult> DeleteIdentifiedCurrentDeliveryComplication(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedCurrentDeliveryComplicationInDb = await context.IdentifiedCurrentDeliveryComplicationRepository.GetIdentifiedCurrentDeliveryComplicationByKey(key);

                if (identifiedCurrentDeliveryComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedCurrentDeliveryComplicationRepository.Update(identifiedCurrentDeliveryComplicationInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedCurrentDeliveryComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedCurrentDeliveryComplication", "IdentifiedCurrentDeliveryComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}