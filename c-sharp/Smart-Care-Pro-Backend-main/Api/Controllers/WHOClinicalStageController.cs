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
    public class WHOClinicalStageController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<WHOClinicalStageController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public WHOClinicalStageController(IUnitOfWork context, ILogger<WHOClinicalStageController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: who-clinical-stage
        /// </summary>
        /// <param name="whoClinicalStage">WHOClinicalStage object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateWHOClinicalStage)]
        public async Task<IActionResult> CreateWHOClinicalStage(WHOClinicalStage wHOClinicalStage)
        {
            try
            {
                wHOClinicalStage.DateCreated = DateTime.Now;
                wHOClinicalStage.IsDeleted = false;
                wHOClinicalStage.IsSynced = false;

                context.WHOClinicalStageRepository.Add(wHOClinicalStage);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadWHOClinicalStageByKey", new { key = wHOClinicalStage.Oid }, wHOClinicalStage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateWHOClinicalStage", "WHOClinicalStageController.cs", ex.Message, wHOClinicalStage.CreatedIn, wHOClinicalStage.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadWHOClinicalStages)]
        public async Task<IActionResult> ReadWHOClinicalStages()
        {
            try
            {
                var whoClientStagging = await context.WHOClinicalStageRepository.GetWHOClinicalStages();

                return Ok(whoClientStagging);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOClinicalStages", "WHOClinicalStageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/who-clinical-stage/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOClinicalStage.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadWHOClinicalStageByKey)]
        public async Task<IActionResult> ReadWHOClinicalStageByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var wHOClinicalStageIndb = await context.WHOClinicalStageRepository.GetWHOClinicalStageByKey(key);

                if (wHOClinicalStageIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(wHOClinicalStageIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOClinicalStageByKey", "WHOClinicalStageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/who-clinical-stage/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOClinicalStage.</param>
        /// <param name="wHOClinicalStage">WHOClinicalStage to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateWHOClinicalStage)]
        public async Task<IActionResult> UpdateWHOClinicalStage(int key, WHOClinicalStage wHOClinicalStage)
        {
            try
            {
                if (key != wHOClinicalStage.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                wHOClinicalStage.DateModified = DateTime.Now;
                wHOClinicalStage.IsDeleted = false;
                wHOClinicalStage.IsSynced = false;

                context.WHOClinicalStageRepository.Update(wHOClinicalStage);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateWHOClinicalStage", "WHOClinicalStageController.cs", ex.Message, wHOClinicalStage.ModifiedIn, wHOClinicalStage.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-clinical-stage/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOClinicalStage.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteWHOClinicalStage)]
        public async Task<IActionResult> DeleteWHOClinicalStage(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var wHOClinicalStageIndb = await context.WHOClinicalStageRepository.GetWHOClinicalStageByKey(key);

                if (wHOClinicalStageIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                wHOClinicalStageIndb.DateModified = DateTime.Now;
                wHOClinicalStageIndb.IsDeleted = true;
                wHOClinicalStageIndb.IsSynced = false;

                context.WHOClinicalStageRepository.Update(wHOClinicalStageIndb);
                await context.SaveChangesAsync();

                return Ok(wHOClinicalStageIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteWHOClinicalStage", "WHOClinicalStageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
