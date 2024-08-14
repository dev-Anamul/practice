using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedPerineumIntact Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPerineumIntactController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPerineumIntactController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPerineumIntactController(IUnitOfWork context, ILogger<IdentifiedPerineumIntactController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-perineum-intact
        /// </summary>
        /// <param name="IdentifiedPeriuneumIntact">IdentifiedPerineumIntact object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPerineumIntact)]
        public async Task<IActionResult> CreateIdentifiedPerineumIntact(IdentifiedPerineumIntact identifiedPerineumIntact)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPerineumIntact, identifiedPerineumIntact.EncounterType);
                interaction.EncounterId = identifiedPerineumIntact.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPerineumIntact.CreatedBy;
                interaction.CreatedIn = identifiedPerineumIntact.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedPerineumIntact.InteractionId = interactionID;
                identifiedPerineumIntact.DateCreated = DateTime.Now;
                identifiedPerineumIntact.IsDeleted = false;
                identifiedPerineumIntact.IsSynced = false;

                context.IdentifiedPerineumIntactRepository.Add(identifiedPerineumIntact);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPerineumIntactByKey", new { key = identifiedPerineumIntact.InteractionId }, identifiedPerineumIntact);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPerineumIntact", "IdentifiedPerineumIntactController.cs", ex.Message, identifiedPerineumIntact.CreatedIn, identifiedPerineumIntact.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-perineum-intacts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPerineumIntacts)]
        public async Task<IActionResult> ReadIdentifiedPerineumIntacts()
        {
            try
            {
                var identifiedPerineumIntactInDb = await context.IdentifiedPerineumIntactRepository.GetIdentifiedPerineumIntacts();

                return Ok(identifiedPerineumIntactInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPerineumIntacts", "IdentifiedPerineumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/by-delivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPerineumIntactByDeliveryId)]
        public async Task<IActionResult> ReadIdentifiedPerineumIntactDeliveryId(Guid deliveryId)
        {
            try
            {
                var inDb = await context.IdentifiedPerineumIntactRepository.GetIdentifiedPerineumIntactByDelivery(deliveryId);

                return Ok(inDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPerineumIntactDeliveryId", "IdentifiedPerineumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-perineum-intact/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPerineumIntacts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPerineumIntactByKey)]
        public async Task<IActionResult> ReadIdentifiedPerineumIntactByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPerineumIntactInDb = await context.IdentifiedPerineumIntactRepository.GetIdentifiedPerineumIntactByKey(key);

                if (identifiedPerineumIntactInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPerineumIntactInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPerineumIntactByKey", "IdentifiedPerineumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-perineum-intact/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPerineumIntacts.</param>
        /// <param name="identifiedPerineumIntact">IdentifiedPerineumIntact to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPerineumIntact)]
        public async Task<IActionResult> UpdateIdentifiedPerineumIntact(Guid key, IdentifiedPerineumIntact identifiedPerineumIntact)
        {
            try
            {
                if (key != identifiedPerineumIntact.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPerineumIntact.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPerineumIntact.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPerineumIntact.DateModified = DateTime.Now;
                identifiedPerineumIntact.IsDeleted = false;
                identifiedPerineumIntact.IsSynced = false;

                context.IdentifiedPerineumIntactRepository.Update(identifiedPerineumIntact);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPerineumIntact", "IdentifiedPerineumIntactController.cs", ex.Message, identifiedPerineumIntact.ModifiedIn, identifiedPerineumIntact.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-perineum-intact/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPerineumIntacts.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPerineumIntact)]
        public async Task<IActionResult> DeleteIdentifiedPerineumIntact(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPerineumIntactInDb = await context.IdentifiedPerineumIntactRepository.GetIdentifiedPerineumIntactByKey(key);

                if (identifiedPerineumIntactInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedPerineumIntactInDb.IsDeleted = true;

                context.IdentifiedPerineumIntactRepository.Update(identifiedPerineumIntactInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedPerineumIntactInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPerineumIntact", "IdentifiedPerineumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}