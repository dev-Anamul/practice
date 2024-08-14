using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MotherDeliverySummaryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MotherDeliverySummaryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MotherDeliverySummaryController(IUnitOfWork context, ILogger<MotherDeliverySummaryController> logger)
        {
            this.context = context;
            this.logger = logger;

        }

        /// <summary>
        /// URL: sc-api/mother-delivery-summary
        /// </summary>
        /// <param name="motherDeliverySummary">BirthHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMotherDeliverySummary)]
        public async Task<IActionResult> CreateMotherDeliverySummary(MotherDeliverySummary motherDeliverySummary)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MotherDeliverySummary, motherDeliverySummary.EncounterType);
                interaction.EncounterId = motherDeliverySummary.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = motherDeliverySummary.CreatedBy;
                interaction.CreatedIn = motherDeliverySummary.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                motherDeliverySummary.InteractionId = interactionId;
                motherDeliverySummary.EncounterId = motherDeliverySummary.EncounterId;
                motherDeliverySummary.ClientId = motherDeliverySummary.ClientId;
                motherDeliverySummary.DateCreated = DateTime.Now;
                motherDeliverySummary.IsDeleted = false;
                motherDeliverySummary.IsSynced = false;

                context.MotherDeliverySummaryRepository.Add(motherDeliverySummary);
                await context.SaveChangesAsync();

                if (motherDeliverySummary.InterventionsList != null)
                {
                    foreach (var item in motherDeliverySummary.InterventionsList)
                    {
                        IdentifiedDeliveryIntervention identifiedDeliveryIntervention = new IdentifiedDeliveryIntervention();

                        identifiedDeliveryIntervention.InteractionId = Guid.NewGuid();
                        identifiedDeliveryIntervention.Interventions = item;
                        identifiedDeliveryIntervention.DeliveryId = motherDeliverySummary.InteractionId;
                        identifiedDeliveryIntervention.Other = motherDeliverySummary.OtherIntervention;
                        identifiedDeliveryIntervention.CreatedBy = motherDeliverySummary.CreatedBy;
                        identifiedDeliveryIntervention.CreatedIn = motherDeliverySummary.CreatedIn;
                        identifiedDeliveryIntervention.IsDeleted = false;
                        identifiedDeliveryIntervention.IsSynced = false;

                        context.IdentifiedDeliveryInterventionRepository.Add(identifiedDeliveryIntervention);
                        await context.SaveChangesAsync();
                    }
                }

                if (motherDeliverySummary.DeliveryComplicationsList != null)
                {
                    foreach (var item in motherDeliverySummary.DeliveryComplicationsList)
                    {
                        IdentifiedCurrentDeliveryComplication identifiedCurrentDeliveryComplication = new IdentifiedCurrentDeliveryComplication();

                        identifiedCurrentDeliveryComplication.InteractionId = Guid.NewGuid();
                        identifiedCurrentDeliveryComplication.Complications = item;
                        identifiedCurrentDeliveryComplication.DeliveryId = motherDeliverySummary.InteractionId;
                        identifiedCurrentDeliveryComplication.Other = motherDeliverySummary.OtherDeliveryComplication;
                        identifiedCurrentDeliveryComplication.CreatedBy = motherDeliverySummary.CreatedBy;
                        identifiedCurrentDeliveryComplication.CreatedIn = motherDeliverySummary.CreatedIn;
                        identifiedCurrentDeliveryComplication.IsSynced = false;
                        identifiedCurrentDeliveryComplication.IsDeleted = false;

                        context.IdentifiedCurrentDeliveryComplicationRepository.Add(identifiedCurrentDeliveryComplication);

                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadMotherDeliverySummaryByKey", new { key = motherDeliverySummary.InteractionId }, motherDeliverySummary);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMotherDeliverySummary", "MotherDeliverySummaryController.cs", ex.Message, motherDeliverySummary.CreatedIn, motherDeliverySummary.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/mother-delivery-summaries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDeliverySummaries)]
        public async Task<IActionResult> ReadMotherDeliverySummaries()
        {
            try
            {
                var motherDeliverySummaryInDb = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaries();

                return Ok(motherDeliverySummaryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDeliverySummaries", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadMotherDeliverySummaryByClient)]
        public async Task<IActionResult> ReadMotherDeliverySummaryByClient(Guid clientId)
        {
            try
            {
                var motherDeliverySummaryInDb = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaryByClient(clientId);

                return Ok(motherDeliverySummaryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDeliverySummaryByClient", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadMotherDeliverySummaryHistory)]
        public async Task<IActionResult> ReadMotherDeliverySummaryHistory(Guid deliveryId)
        {
            try
            {
                MotherDetailsHistoryDto motherDetailsHistoryDto = new();

                motherDetailsHistoryDto.ThirdStageDeliveries = context.ThirdStageDeliveryRepository.GetThirdStageDeliveryByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.PPHTreatments = context.PPHTreatmentRepository.GetPPHTreatmentByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.PlacentaRemovals = context.PlacentaRemovalRepository.GetPlacentaRemovalByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.IdentifiedPerineumIntacts = context.IdentifiedPerineumIntactRepository.GetIdentifiedPerineumIntactByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.MedicalTreatments = context.MedicalTreatmentRepository.GetMedicalTreatmentByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.PerineumIntacts = context.PeriuneumIntactRepository.GetPeriuneumIntactByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.UterusConditions = context.UterusConditionRepository.GetUterusConditionByDelivery(deliveryId).Result.ToList();
                motherDetailsHistoryDto.IsDeleted = false;
                motherDetailsHistoryDto.IsSynced = false;

                return Ok(motherDetailsHistoryDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDeliverySummaryHistory", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadMotherDeliverySummaryByEncounter)]
        public async Task<IActionResult> ReadMotherDeliverySummaryByEncounter(Guid encounterId)
        {
            try
            {
                var motherDeliverySummaryInDb = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaryByEncounter(encounterId);

                return Ok(motherDeliverySummaryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDeliverySummaryByEncounter", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-delivery-summary/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDeliverySummaryByKey)]
        public async Task<IActionResult> ReadMotherDeliverySummaryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var motherDeliverySummaryInDb = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaryByKey(key);

                if (motherDeliverySummaryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(motherDeliverySummaryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDeliverySummaryByKey", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/mother-delivery-summary/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="motherDeliverySummary">BirthHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMotherDeliverySummary)]
        public async Task<IActionResult> UpdateMotherDeliverySummary(Guid key, MotherDeliverySummary motherDeliverySummary)
        {
            try
            {
                if (key != motherDeliverySummary.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = motherDeliverySummary.ModifiedBy;
                interactionInDb.ModifiedIn = motherDeliverySummary.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                motherDeliverySummary.DateModified = DateTime.Now;
                motherDeliverySummary.IsDeleted = false;
                motherDeliverySummary.IsSynced = false;

                context.MotherDeliverySummaryRepository.Update(motherDeliverySummary);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMotherDeliverySummary", "MotherDeliverySummaryController.cs", ex.Message, motherDeliverySummary.ModifiedIn, motherDeliverySummary.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-delivery-summary/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMotherDeliverySummary)]
        public async Task<IActionResult> DeleteMotherDeliverySummary(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var motherDeliverySummaryInDb = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaryByKey(key);

                if (motherDeliverySummaryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                motherDeliverySummaryInDb.DateModified = DateTime.Now;
                motherDeliverySummaryInDb.IsDeleted = true;
                motherDeliverySummaryInDb.IsSynced = false;

                context.MotherDeliverySummaryRepository.Update(motherDeliverySummaryInDb);
                await context.SaveChangesAsync();

                return Ok(motherDeliverySummaryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMotherDeliverySummary", "MotherDeliverySummaryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}