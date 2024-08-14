using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NursingPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NursingPlanController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NursingPlanController(IUnitOfWork context, ILogger<NursingPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/nursing-plan
        /// </summary>
        /// <param name="nursingPlan">NursingPlan object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNursingPlan)]
        public async Task<IActionResult> CreateNursingPlan(NursingPlan nursingPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.NursingPlan, nursingPlan.EncounterType);
                interaction.EncounterId = nursingPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = nursingPlan.CreatedBy;
                interaction.CreatedIn = nursingPlan.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                nursingPlan.InteractionId = interactionId;
                nursingPlan.EncounterId = nursingPlan.EncounterId;
                nursingPlan.ClientId = nursingPlan.ClientId;
                nursingPlan.DateCreated = DateTime.Now;
                nursingPlan.IsDeleted = false;
                nursingPlan.IsSynced = false;

                context.NursingPlanRepository.Add(nursingPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNursingPlanByKey", new { key = nursingPlan.InteractionId }, nursingPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNursingPlan", "NursingPlanController.cs", ex.Message, nursingPlan.CreatedIn, nursingPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/nursing-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNursingPlans)]
        public async Task<IActionResult> ReadNursingPlans()
        {
            try
            {
                var nursingPlanInDb = await context.NursingPlanRepository.GetNursingPlans();

                return Ok(nursingPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNursingPlans", "NursingPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nursing-plan/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNursingPlanByClient)]
        public async Task<IActionResult> ReadNursingPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var nursingPlanInDb = await context.NursingPlanRepository.GetNursingPlanByClient(clientId);

                    return Ok(nursingPlanInDb);
                }
                else
                {
                    var nursingPlanInDb = await context.NursingPlanRepository.GetNursingPlanByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<NursingPlan> nursingPlanDto = new PagedResultDto<NursingPlan>()
                    {
                        Data = nursingPlanInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.NursingPlanRepository.GetNursingPlanByClientTotalCount(clientId, encounterType)
                    };
                    return Ok(nursingPlanDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNursingPlanByClient", "NursingPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nursing-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNursingPlanByKey)]
        public async Task<IActionResult> ReadNursingPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nursingPlanInDb = await context.NursingPlanRepository.GetNursingPlanByKey(key);

                if (nursingPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(nursingPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNursingPlanByKey", "NursingPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/nursing-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlans.</param>
        /// <param name="nursingPlan">NursingPlan to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNursingPlan)]
        public async Task<IActionResult> UpdateNursingPlan(Guid key, NursingPlan nursingPlan)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = nursingPlan.ModifiedBy;
                interactionInDb.ModifiedIn = nursingPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != nursingPlan.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nursingPlan.DateModified = DateTime.Now;
                nursingPlan.IsDeleted = false;
                nursingPlan.IsSynced = false;

                context.NursingPlanRepository.Update(nursingPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNursingPlan", "NursingPlanController.cs", ex.Message, nursingPlan.ModifiedIn, nursingPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nursing-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteNursingPlan)]
        public async Task<IActionResult> DeleteNursingPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nursingPlanInDb = await context.NursingPlanRepository.GetNursingPlanByKey(key);

                if (nursingPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                nursingPlanInDb.DateModified = DateTime.Now;
                nursingPlanInDb.IsDeleted = true;
                nursingPlanInDb.IsSynced = false;

                context.NursingPlanRepository.Update(nursingPlanInDb);
                await context.SaveChangesAsync();

                return Ok(nursingPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNursingPlan", "NursingPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}