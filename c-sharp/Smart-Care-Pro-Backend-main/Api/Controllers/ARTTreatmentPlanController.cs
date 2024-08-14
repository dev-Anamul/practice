using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Tomas
 * Date created  : 10.04.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTTreatmentPlan controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTTreatmentPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ARTTreatmentPlanController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTTreatmentPlanController(IUnitOfWork context, ILogger<ARTTreatmentPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplan
        /// </summary>
        /// <param name="treatmentPlan">ARTTreatmentPlan object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTTreatmentPlan)]
        public async Task<IActionResult> CreateARTTreatmentPlan(ARTTreatmentPlan treatmentPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ARTTreatmentPlan, treatmentPlan.EncounterType);
                interaction.EncounterId = treatmentPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = treatmentPlan.CreatedBy;
                interaction.CreatedIn = treatmentPlan.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                treatmentPlan.InteractionId = interactionId;
                treatmentPlan.DateCreated = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.ARTTreatmentPlan.Add(treatmentPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTTreatmentPlan", "ARTTreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTTreatmentPlan)]
        public async Task<IActionResult> ReadTreatmentPlans()
        {
            try
            {
                var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlan();

                planInDb = planInDb.OrderByDescending(x => x.DateCreated);

                return Ok(planInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlans", "ARTTreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplans/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTTreatmentPlanByKey)]
        public async Task<IActionResult> ReadTreatmentPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlanByKey(key);

                if (planInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(planInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlanByKey", "ARTTreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplan/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table TreatmentPlan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTTreatmentPlanByClient)]
        public async Task<IActionResult> ReadTreatmentPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlanByClient(clientId);
                    var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlanByClientLast24Hours(clientId);

                    return Ok(planInDb);
                }
                else
                {
                    var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlanByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<ARTTreatmentPlan> planDto = new PagedResultDto<ARTTreatmentPlan>()
                    {
                        Data = planInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ARTTreatmentPlan.GetARTTreatmentPlanByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(planDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlanByClient", "ARTTreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplans/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <param name="treatmentPlan">TreatmentPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTTreatmentPlan)]
        public async Task<IActionResult> UpdateARTTreatmentPlan(Guid key, ARTTreatmentPlan treatmentPlan)
        {
            try
            {
                if (key != treatmentPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = treatmentPlan.ModifiedBy;
                interactionInDb.ModifiedIn = treatmentPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                treatmentPlan.IsSynced = false;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.DateModified = DateTime.Now;

                context.ARTTreatmentPlan.Update(treatmentPlan);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateARTTreatmentPlan", "ARTTreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-treatmentplan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTTreatmentPlan.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteARTTreatmentPlan)]
        public async Task<IActionResult> DeleteTBHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var planInDb = await context.ARTTreatmentPlan.GetARTTreatmentPlanByKey(key);

                if (planInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                planInDb.IsDeleted = true;

                context.ARTTreatmentPlan.Update(planInDb);
                await context.SaveChangesAsync();

                return Ok(planInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBHistory", "ARTTreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}