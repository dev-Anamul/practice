using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class WHOStagesConditionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<WHOStagesConditionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public WHOStagesConditionController(IUnitOfWork context, ILogger<WHOStagesConditionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: who-stages-condition
        /// </summary>
        /// <param name="whoStageesCondition">WHOStagesCondition object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateWHOStagesCondition)]
        public async Task<IActionResult> CreateWHOStagesCondition(WHOStagesCondition whoStageesCondition)
        {
            try
            {
                whoStageesCondition.DateCreated = DateTime.Now;
                whoStageesCondition.IsDeleted = false;
                whoStageesCondition.IsSynced = false;

                context.WHOStagesConditionRepository.Add(whoStageesCondition);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadWHOStagesConditionByKey", new { key = whoStageesCondition.Oid }, whoStageesCondition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateWHOStagesCondition", "WHOStagesConditionController.cs", ex.Message, whoStageesCondition.CreatedIn, whoStageesCondition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpGet]
        [Route(RouteConstants.ReadWHOStagesConditions)]
        public async Task<IActionResult> ReadWHOStagesConditions()
        {
            try
            {
                var whoStagging = await context.WHOStagesConditionRepository.GetWHOStagesConditions();

                return Ok(whoStagging);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOStagesConditions", "WHOStagesConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/who-stages-condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOStagesCondition.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadWHOStagesConditionByKey)]
        public async Task<IActionResult> ReadWHOStagesConditionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var whoStageesConditionIndb = await context.WHOStagesConditionRepository.GetWHOStagesConditionByKey(key);

                if (whoStageesConditionIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(whoStageesConditionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOStagesConditionByKey", "WHOStagesConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/who-stages-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOStagesCondition.</param>
        /// <param name="whoStageesCondition">WHOStagesCondition to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateWHOStagesCondition)]
        public async Task<IActionResult> UpdateWHOStagesCondition(int key, WHOStagesCondition whoStageesCondition)
        {
            try
            {
                if (key != whoStageesCondition.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                whoStageesCondition.DateModified = DateTime.Now;
                whoStageesCondition.IsDeleted = false;
                whoStageesCondition.IsSynced = false;

                context.WHOStagesConditionRepository.Update(whoStageesCondition);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateWHOStagesCondition", "WHOStagesConditionController.cs", ex.Message, whoStageesCondition.ModifiedIn, whoStageesCondition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-stages-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOStagesCondition.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteWHOStagesCondition)]
        public async Task<IActionResult> DeleteWHOStagesCondition(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var whoStageesConditionIndb = await context.WHOStagesConditionRepository.GetWHOStagesConditionByKey(key);

                if (whoStageesConditionIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                whoStageesConditionIndb.DateModified = DateTime.Now;
                whoStageesConditionIndb.IsDeleted = true;
                whoStageesConditionIndb.IsSynced = false;

                context.WHOStagesConditionRepository.Update(whoStageesConditionIndb);
                await context.SaveChangesAsync();

                return Ok(whoStageesConditionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteWHOStagesCondition", "WHOStagesConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}