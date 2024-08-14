using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// FamilyPlan controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FamilyPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FamilyPlanController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FamilyPlanController(IUnitOfWork context, ILogger<FamilyPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/family-plan
        /// </summary>
        /// <param name="familyPlan">FamilyPlan object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFamilyPlan)]
        public async Task<IActionResult> CreateFamilyPlan(FamilyPlan familyPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FamilyPlan, familyPlan.EncounterType);
                interaction.EncounterId = familyPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = familyPlan.CreatedBy;
                interaction.CreatedIn = familyPlan.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                familyPlan.InteractionId = interactionId;
                familyPlan.DateCreated = DateTime.Now;
                familyPlan.IsDeleted = false;
                familyPlan.IsSynced = false;

                context.FamilyPlanRepository.Add(familyPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFamilyPlanByKey", new { key = familyPlan.InteractionId }, familyPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFamilyPlan", "FamilyPlanController.cs", ex.Message, familyPlan.CreatedIn, familyPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlans)]
        public async Task<IActionResult> ReadFamilyPlans()
        {
            try
            {
                var familyPlanInDb = await context.FamilyPlanRepository.GetFamilyPlans();
                familyPlanInDb = familyPlanInDb.OrderByDescending(x => x.DateCreated);
                return Ok(familyPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlans", "FamilyPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanByKey)]
        public async Task<IActionResult> ReadFamilyPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanInDb = await context.FamilyPlanRepository.GetFamilyPlanByKey(key);

                if (familyPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(familyPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanByKey", "FamilyPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan/by-client/{clientId}
        /// </summary>
        /// <param name="ClientId">Primary key of the table FamilyPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanByClient)]
        public async Task<IActionResult> ReadFamilyPlanByClient(Guid clientId)
        {
            try
            {
                var familyPlanInDb = await context.FamilyPlanRepository.GetFamilyPlanByClient(clientId);

                return Ok(familyPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanByClient", "FamilyPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table FamilyPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanByEncounter)]
        public async Task<IActionResult> ReadFamilyPlanByEncounter(Guid encounterId)
        {
            try
            {
                var familyPlanInDb = await context.FamilyPlanRepository.GetFamilyPlanByEncounter(encounterId);

                return Ok(familyPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanByEncounter", "FamilyPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlans.</param>
        /// <param name="familyPlan">FamilyPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFamilyPlan)]
        public async Task<IActionResult> UpdateFamilyPlan(Guid key, FamilyPlan familyPlan)
        {
            try
            {
                if (key != familyPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = familyPlan.ModifiedBy;
                interactionInDb.ModifiedIn = familyPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                familyPlan.DateModified = DateTime.Now;
                familyPlan.IsDeleted = false;
                familyPlan.IsSynced = false;

                context.FamilyPlanRepository.Update(familyPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFamilyPlan", "FamilyPlanController.cs", ex.Message, familyPlan.ModifiedIn, familyPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlans.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteFamilyPlan)]
        public async Task<IActionResult> DeleteFamilyPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanInDb = await context.FamilyPlanRepository.GetFamilyPlanByKey(key);

                if (familyPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                familyPlanInDb.DateModified = DateTime.Now;
                familyPlanInDb.IsDeleted = true;
                familyPlanInDb.IsSynced = false;

                context.FamilyPlanRepository.Update(familyPlanInDb);
                await context.SaveChangesAsync();

                return Ok(familyPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFamilyPlan", "FamilyPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}