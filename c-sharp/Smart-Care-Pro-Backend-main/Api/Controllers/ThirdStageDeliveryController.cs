using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 01.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ThirdStageDelivery controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ThirdStageDeliveryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ThirdStageDeliveryController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ThirdStageDeliveryController(IUnitOfWork context, ILogger<ThirdStageDeliveryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/third-stage-delivery
        /// </summary>
        /// <param name="thirdStageDelivery">ThirdStageDelivery object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateThirdStageDelivery)]
        public async Task<IActionResult> CreateThirdStageDelivery(ThirdStageDelivery thirdStageDelivery)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ThirdStageDelivery, thirdStageDelivery.EncounterType);
                interaction.EncounterId = thirdStageDelivery.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = thirdStageDelivery.CreatedBy;
                interaction.CreatedIn = thirdStageDelivery.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                thirdStageDelivery.InteractionId = interactionId;
                thirdStageDelivery.DateCreated = DateTime.Now;
                thirdStageDelivery.IsDeleted = false;
                thirdStageDelivery.IsSynced = false;

                var motherDelivery = await context.MotherDeliverySummaryRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == thirdStageDelivery.DeliveryId);

                context.ThirdStageDeliveryRepository.Add(thirdStageDelivery);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadThirdStageDeliveryByKey", new { key = thirdStageDelivery.InteractionId }, thirdStageDelivery);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateThirdStageDelivery", "ThirdStageDeliveryController.cs", ex.Message, thirdStageDelivery.CreatedIn, thirdStageDelivery.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/third-stage-deliveries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThirdStageDeliveries)]
        public async Task<IActionResult> ReadThirdStageDeliveries()
        {
            try
            {
                var thirdStageDeliveryInDb = await context.ThirdStageDeliveryRepository.GetThirdStageDeliveries();

                return Ok(thirdStageDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThirdStageDeliveries", "ThirdStageDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/third-stage-delivery/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThirdStageDeliveries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThirdStageDeliveryByKey)]
        public async Task<IActionResult> ReadThirdStageDeliveryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thirdStageDeliveryInDb = await context.ThirdStageDeliveryRepository.GetThirdStageDeliveryByKey(key);

                if (thirdStageDeliveryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(thirdStageDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThirdStageDeliveryByKey", "ThirdStageDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/third-stage-delivery/by-delivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table ThirdStageDeliverys.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThirdStageDeliveryByDelivery)]
        public async Task<IActionResult> ReadThirdStageDeliveryByDelivery(Guid deliveryId)
        {
            try
            {
                var thirdStageDeliveryInDb = await context.ThirdStageDeliveryRepository.GetThirdStageDeliveryByDelivery(deliveryId);

                return Ok(thirdStageDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThirdStageDeliveryByDelivery", "ThirdStageDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/third-stage-delivery/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThirdStageDeliverys.</param>
        /// <param name="ThirdStageDelivery">ThirdStageDelivery to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateThirdStageDelivery)]
        public async Task<IActionResult> UpdateThirdStageDelivery(Guid key, ThirdStageDelivery thirdStageDelivery)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = thirdStageDelivery.ModifiedBy;
                interactionInDb.ModifiedIn = thirdStageDelivery.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != thirdStageDelivery.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);



                ThirdStageDelivery dbStageDelivery = await context.ThirdStageDeliveryRepository.GetThirdStageDeliveryByKey(key);

                dbStageDelivery.Others = thirdStageDelivery.Others;
                dbStageDelivery.IsMalpresentationOfFetus = thirdStageDelivery.IsMalpresentationOfFetus;
                dbStageDelivery.IsFetalMacrosomia = thirdStageDelivery.IsFetalMacrosomia;
                dbStageDelivery.IsBirthTrauma = thirdStageDelivery.IsBirthTrauma;
                dbStageDelivery.IsLatrogenicInjury = thirdStageDelivery.IsLatrogenicInjury;
                dbStageDelivery.ActiveManagement = thirdStageDelivery.ActiveManagement;
                dbStageDelivery.BloodLoss = thirdStageDelivery.BloodLoss;
                dbStageDelivery.IsAbnormalPlacemental = thirdStageDelivery.IsAbnormalPlacemental;
                dbStageDelivery.IsAbnormalPlacentation = thirdStageDelivery.IsAbnormalPlacentation;
                dbStageDelivery.IsAnesthesia = thirdStageDelivery.IsAnesthesia;
                dbStageDelivery.IsFetalMacrosomia = thirdStageDelivery.IsFetalMacrosomia;
                dbStageDelivery.IsCoagulationDisorder = thirdStageDelivery.IsCoagulationDisorder;
                dbStageDelivery.IsHemophilia = thirdStageDelivery.IsHemophilia;
                dbStageDelivery.IsLatrogenicInjury = thirdStageDelivery.IsLatrogenicInjury;
                dbStageDelivery.IsMultiplePregnancy = thirdStageDelivery.IsMultiplePregnancy;
                dbStageDelivery.IsPreviousUterineInversion = thirdStageDelivery.IsPreviousUterineInversion;
                dbStageDelivery.IsProlongedOxytocinUse = thirdStageDelivery.IsProlongedOxytocinUse;
                dbStageDelivery.IsRetainedPlacenta = thirdStageDelivery.IsRetainedPlacenta;
                dbStageDelivery.IsUncontrolledCordContraction = thirdStageDelivery.IsUncontrolledCordContraction;
                dbStageDelivery.IsUterineAtony = thirdStageDelivery.IsUterineAtony;
                dbStageDelivery.IsUterineInversion = thirdStageDelivery.IsUterineInversion;
                dbStageDelivery.IsUterineLeiomyoma = thirdStageDelivery.IsUterineLeiomyoma;
                dbStageDelivery.IsUterineRapture = thirdStageDelivery.IsUterineRapture;
                dbStageDelivery.IsVelamentousCordInsertion = thirdStageDelivery.IsVelamentousCordInsertion;
                dbStageDelivery.IsVonWillebrand = thirdStageDelivery.IsVonWillebrand;
                dbStageDelivery.IsRetainedProductOfConception = thirdStageDelivery.IsRetainedProductOfConception;
                dbStageDelivery.PPH = thirdStageDelivery.PPH;

                dbStageDelivery.ModifiedBy = thirdStageDelivery.ModifiedBy;
                dbStageDelivery.ModifiedIn = thirdStageDelivery.ModifiedIn;
                dbStageDelivery.DateModified = DateTime.Now;
                dbStageDelivery.IsDeleted = false;
                dbStageDelivery.IsSynced = false;


                context.ThirdStageDeliveryRepository.Update(dbStageDelivery);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateThirdStageDelivery", "ThirdStageDeliveryController.cs", ex.Message, thirdStageDelivery.ModifiedIn, thirdStageDelivery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/third-stage-delivery/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThirdStageDeliveries.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteThirdStageDelivery)]
        public async Task<IActionResult> DeleteThirdStageDelivery(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thirdStageDeliveryInDb = await context.ThirdStageDeliveryRepository.GetThirdStageDeliveryByKey(key);

                if (thirdStageDeliveryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                thirdStageDeliveryInDb.IsDeleted = true;

                context.ThirdStageDeliveryRepository.Update(thirdStageDeliveryInDb);
                await context.SaveChangesAsync();

                return Ok(thirdStageDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteThirdStageDelivery", "ThirdStageDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}