using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// UterusCondition Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class UterusConditionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<UterusConditionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public UterusConditionController(IUnitOfWork context, ILogger<UterusConditionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/uterus-condition
        /// </summary>
        /// <param name="UterusCondition">UterusCondition object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateUterusCondition)]
        public async Task<IActionResult> CreateUterusCondition(UterusCondition uterusCondition)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.UterusCondition, uterusCondition.EncounterType);
                interaction.EncounterId = uterusCondition.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = uterusCondition.CreatedBy;
                interaction.CreatedIn = uterusCondition.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                var motherDelivery = await context.MotherDeliverySummaryRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == uterusCondition.DeliveryId);

                if (motherDelivery == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                if (uterusCondition.ConditionOfUterusList != null)
                {
                    foreach (var item in uterusCondition.ConditionOfUterusList)
                    {
                        UterusCondition uterusConditionItem = new UterusCondition();

                        uterusConditionItem.DeliveryId = motherDelivery.InteractionId;
                        uterusConditionItem.CreatedBy = uterusCondition.CreatedBy;
                        uterusConditionItem.CreatedIn = uterusCondition.CreatedIn;
                        uterusConditionItem.InteractionId = Guid.NewGuid();
                        uterusConditionItem.EncounterId = uterusCondition.EncounterId;
                        uterusConditionItem.ConditionOfUterus = item;
                        uterusConditionItem.Other = uterusCondition.Other;
                        uterusConditionItem.DateCreated = DateTime.Now;
                        uterusConditionItem.EncounterId = uterusCondition.EncounterId;
                        uterusConditionItem.IsDeleted = false;
                        uterusConditionItem.IsSynced = false;

                        context.UterusConditionRepository.Add(uterusConditionItem);
                    }
                }
                else
                {
                    uterusCondition.DeliveryId = motherDelivery.InteractionId;
                    uterusCondition.InteractionId = interactionID;
                    uterusCondition.DateCreated = DateTime.Now;
                    uterusCondition.EncounterId = uterusCondition.EncounterId;
                    uterusCondition.CreatedIn = motherDelivery.CreatedIn;
                    uterusCondition.CreatedBy = motherDelivery.CreatedBy;
                    uterusCondition.IsDeleted = false;
                    uterusCondition.IsSynced = false;

                    context.UterusConditionRepository.Add(uterusCondition);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadUterusConditionByKey", new { key = uterusCondition.InteractionId }, uterusCondition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateUterusCondition", "UterusConditionController.cs", ex.Message, uterusCondition.CreatedIn, uterusCondition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/uterus-conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUterusConditions)]
        public async Task<IActionResult> ReadUterusConditions()
        {
            try
            {
                var uterusConditionInDb = await context.UterusConditionRepository.GetUterusConditions();
                uterusConditionInDb = uterusConditionInDb.OrderByDescending(x => x.DateCreated);
                return Ok(uterusConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUterusConditions", "UterusConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/by-delivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table MotherDelivery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUterusConditionByDelivery)]
        public async Task<IActionResult> ReadUterusConditionByDelivery(Guid deliveryId)
        {
            try
            {
                var uterusConditionInDb = await context.UterusConditionRepository.GetUterusConditionByDelivery(deliveryId);

                return Ok(uterusConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUterusConditionByDelivery", "UterusConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/uterus-condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UterusConditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUterusConditionByKey)]
        public async Task<IActionResult> ReadUterusConditionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var uterusConditionIndb = await context.UterusConditionRepository.GetUterusConditionByKey(key);

                if (uterusConditionIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(uterusConditionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUterusConditionByKey", "UterusConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/uterus-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UterusConditions.</param>
        /// <param name="uterusCondition">UterusCondition to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateUterusCondition)]
        public async Task<IActionResult> UpdateUterusCondition(Guid key, UterusCondition uterusCondition)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = uterusCondition.ModifiedBy;
                interactionInDb.ModifiedIn = uterusCondition.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != uterusCondition.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                uterusCondition.DateModified = DateTime.Now;
                uterusCondition.IsDeleted = false;
                uterusCondition.IsSynced = false;

                context.UterusConditionRepository.Update(uterusCondition);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateUterusCondition", "UterusConditionController.cs", ex.Message, uterusCondition.ModifiedIn, uterusCondition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/uterus-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UterusConditions.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteUterusCondition)]
        public async Task<IActionResult> DeleteUterusCondition(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var uterusConditionInDb = await context.UterusConditionRepository.GetUterusConditionByEncounter(key);

                if (uterusConditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                uterusConditionInDb.ToList().ForEach(x =>
                {
                    x.IsDeleted = true;
                    x.IsSynced = false;

                    context.UterusConditionRepository.Update(x);
                });

                await context.SaveChangesAsync();

                return Ok(uterusConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUterusCondition", "UterusConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}