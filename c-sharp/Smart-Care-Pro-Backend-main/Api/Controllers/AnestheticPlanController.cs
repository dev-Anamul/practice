using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 13.04.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// AnestheticPlan controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AnestheticPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AnestheticPlanController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AnestheticPlanController(IUnitOfWork context, ILogger<AnestheticPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan
        /// </summary>
        /// <param name="anestheticPlan">AnestheticPlan object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAnestheticPlan)]
        public async Task<IActionResult> CreateAnestheticPlan(AnestheticPlan anestheticPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.AnestheticPlan, anestheticPlan.EncounterType);
                interaction.EncounterId = anestheticPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = anestheticPlan.CreatedBy;
                interaction.CreatedIn = anestheticPlan.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                anestheticPlan.InteractionId = interactionId;
                anestheticPlan.DateCreated = DateTime.Now;
                anestheticPlan.IsDeleted = false;
                anestheticPlan.IsSynced = false;

                context.AnestheticPlanRepository.Add(anestheticPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAnestheticPlanByKey", new { key = anestheticPlan.InteractionId }, anestheticPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateANCService", "AnestheticPlanController.cs", ex.Message, anestheticPlan.CreatedIn, anestheticPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnestheticPlans)]
        public async Task<IActionResult> ReadAnestheticPlans()
        {
            try
            {
                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlans();

                anestheticPlanIndb = anestheticPlanIndb.OrderByDescending(x => x.DateCreated);

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnestheticPlans", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnestheticPlanByKey)]
        public async Task<IActionResult> ReadAnestheticPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlanByKey(key);

                if (anestheticPlanIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnestheticPlanByKey", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnestheticPlanByEncounter)]
        public async Task<IActionResult> ReadAnestheticPlanByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlanByEncounter(encounterId);

                if (anestheticPlanIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnestheticPlanByEncounter", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/by-surgery/{surgeryId}
        /// </summary>
        /// <param name="surgeryId">Primary key of the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnestheticPlanBySurgery)]
        public async Task<IActionResult> ReadAnestheticPlanBySurgery(Guid surgeryId)
        {
            try
            {
                if (surgeryId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlanBySurgery(surgeryId);

                if (anestheticPlanIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnestheticPlanBySurgery", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/by-surgery/{surgeryId}
        /// </summary>
        /// <param name="surgeryId">Primary key of the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnestheticPlanListBySurgery)]
        public async Task<IActionResult> ReadAnestheticPlanListBySurgery(Guid surgeryId)
        {
            try
            {
                if (surgeryId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlanListBySurgery(surgeryId);

                if (anestheticPlanIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnestheticPlanListBySurgery", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlans.</param>
        /// <param name="anestheticPlan">AnestheticPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAnestheticPlan)]
        public async Task<IActionResult> UpdateAnestheticPlan(Guid key, AnestheticPlan anestheticPlan)
        {
            try
            {
                if (key != anestheticPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = anestheticPlan.ModifiedBy;
                interactionInDb.ModifiedIn = anestheticPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                anestheticPlan.DateModified = DateTime.Now;
                anestheticPlan.IsDeleted = false;
                anestheticPlan.IsSynced = false;

                context.AnestheticPlanRepository.Update(anestheticPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateAnestheticPlan", "AnestheticPlanController.cs", ex.Message, anestheticPlan.CreatedIn, anestheticPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/intra-op/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlan.</param>
        /// <param name="anestheticPlan">AnestheticPlan to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIntraOpAnestheticPlan)]
        public async Task<IActionResult> UpdateIntraOpAnestheticPlan(Guid key, AnestheticPlan anestheticPlan)
        {
            try
            {
                if (key != anestheticPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var anestheticPlanInDb = await context.AnestheticPlanRepository.GetAnestheticPlanByKey(key);

                anestheticPlanInDb.AnesthesiaStartTime = anestheticPlan.AnesthesiaStartTime;
                anestheticPlanInDb.AnesthesiaEndTime = anestheticPlan.AnesthesiaEndTime;
                anestheticPlanInDb.PostAnesthesia = anestheticPlan.PostAnesthesia;
                anestheticPlanInDb.PreOperativeAdverse = anestheticPlan.PreOperativeAdverse;
                anestheticPlanInDb.PostOperative = anestheticPlan.PostOperative;

                anestheticPlanInDb.DateModified = DateTime.Now;
                anestheticPlanInDb.ModifiedBy = anestheticPlan.ModifiedBy;
                anestheticPlanInDb.IsSynced = false;
                anestheticPlanInDb.IsDeleted = false;

                context.AnestheticPlanRepository.Update(anestheticPlanInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateIntraOpAnestheticPlan", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anesthetic-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAnestheticPlan)]
        public async Task<IActionResult> DeleteAnestheticPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anestheticPlanIndb = await context.AnestheticPlanRepository.GetAnestheticPlanByKey(key);

                if (anestheticPlanIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                anestheticPlanIndb.DateModified = DateTime.Now;
                anestheticPlanIndb.IsDeleted = true;
                anestheticPlanIndb.IsSynced = false;

                context.AnestheticPlanRepository.Update(anestheticPlanIndb);
                await context.SaveChangesAsync();

                return Ok(anestheticPlanIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAnestheticPlan", "AnestheticPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}