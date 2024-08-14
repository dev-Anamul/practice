using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 03.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Client controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class ArmedForceServiceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ArmedForceServiceController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ArmedForceServiceController(IUnitOfWork context, ILogger<ArmedForceServiceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region Create
        /// <summary>
        /// URL: sc-api/armed-force-service
        /// </summary>
        /// <param name="armedForceService">ArmedForceService object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateArmedForceService)]
        public async Task<IActionResult> CreateArmedForceService(ArmedForceService armedForceService)
        {
            try
            {
                if (await IsArmedForceDuplicate(armedForceService) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                armedForceService.DateCreated = DateTime.Now;
                armedForceService.IsDeleted = false;
                armedForceService.IsSynced = false;

                context.ArmedForceServiceRepository.Add(armedForceService);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadArmedForceServiceByKey", new { key = armedForceService.Oid }, armedForceService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateArmedForceService", "ArmedForceServiceController.cs", ex.Message, armedForceService.CreatedIn, armedForceService.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// URL: sc-api/armed-force-serviceses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadArmedForceServiceses)]
        public async Task<IActionResult> ReadArmedForceServiceses()
        {
            try
            {
                var armedForceServiceIndb = await context.ArmedForceServiceRepository.GetArmedForceServiceses();

                return Ok(armedForceServiceIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadArmedForceServiceses", "ArmedForceServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/armed-force-serviceses/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ArmedForceServiceses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadArmedForceServiceByKey)]
        public async Task<IActionResult> ReadArmedForceServiceByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var armedForceServiceIndb = await context.ArmedForceServiceRepository.GetArmedForceServiceByKey(key);

                if (armedForceServiceIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(armedForceServiceIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadArmedForceServiceByKey", "ArmedForceServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// URL: sc-api/armed-force-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ArmedForceService.</param>
        /// <param name="country">ArmedForceService to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateArmedForceService)]
        public async Task<IActionResult> UpdateArmedForceService(int key, ArmedForceService armedForceService)
        {
            try
            {
                if (key != armedForceService.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsArmedForceDuplicate(armedForceService) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                armedForceService.DateModified = DateTime.Now;
                armedForceService.IsDeleted = false;
                armedForceService.IsSynced = false;

                context.ArmedForceServiceRepository.Update(armedForceService);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateArmedForceService", "ArmedForceServiceController.cs", ex.Message, armedForceService.ModifiedIn, armedForceService.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// URL: sc-api/armed-force-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ArmedForceServiceses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteArmedForceService)]
        public async Task<IActionResult> DeleteArmedForceService(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var armedForceServiceIndb = await context.ArmedForceServiceRepository.GetArmedForceServiceByKey(key);

                if (armedForceServiceIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                armedForceServiceIndb.DateModified = DateTime.Now;
                armedForceServiceIndb.IsDeleted = true;
                armedForceServiceIndb.IsSynced = false;

                context.ArmedForceServiceRepository.Update(armedForceServiceIndb);
                await context.SaveChangesAsync();

                return Ok(armedForceServiceIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteArmedForceService", "ArmedForceServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Duplicate Check
        /// <summary>
        /// Checks whether the ArmedForceService name is duplicate or not. 
        /// </summary>
        /// <param name="country">ArmedForceService object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsArmedForceDuplicate(ArmedForceService armedForceService)
        {
            try
            {
                var armedforceInDb = await context.ArmedForceServiceRepository.GetArmedForceServiceByName(armedForceService.Description);

                if (armedforceInDb != null)
                    if (armedforceInDb.Oid != armedforceInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsArmedForceDuplicate", "ArmedForceServiceController.cs", ex.Message);
                throw;
            }
        }
        #endregion
    }
}