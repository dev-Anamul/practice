using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 12.03.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// System Relevance controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SystemRelevanceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SystemRelevanceController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SystemRelevanceController(IUnitOfWork context, ILogger<SystemRelevanceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/system-relevance
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemRelevance)]
        public async Task<IActionResult> ReadSystemRelevance()
        {
            try
            {
                var systemRelevanceInDb = await context.SystemRelevanceRepository.GetSystemRelevance();

                return Ok(systemRelevanceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemRelevance", "SystemRelevanceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/systemRelevance
        /// </summary>
        /// <param name="systemRelevance">SystemRelevance object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSystemRelevance)]
        public async Task<ActionResult<SystemRelevance>> CreateSystemRelevance(SystemRelevance systemRelevance)
        {
            try
            {
                systemRelevance.DateCreated = DateTime.Now;
                systemRelevance.IsDeleted = false;
                systemRelevance.IsSynced = false;

                context.SystemRelevanceRepository.Add(systemRelevance);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSystemRelevanceByKey", new { key = systemRelevance.Oid }, systemRelevance);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSystemRelevance", "SystemRelevanceController.cs", ex.Message, systemRelevance.CreatedIn, systemRelevance.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-relevance/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemRelevance.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemRelevanceByKey)]
        public async Task<IActionResult> ReadSystemRelevanceByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemRelevanceInDb = await context.SystemRelevanceRepository.GetSystemsRelevanceByKey(key);

                if (systemRelevanceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(systemRelevanceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemRelevanceByKey", "SystemRelevanceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-relevance/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemRelevances.</param>
        /// <param name="SystemRelevance">SystemRelevance to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSystemRelevance)]
        public async Task<IActionResult> UpdateSystemRelevance(int key, SystemRelevance systemRelevance)
        {
            try
            {
                if (key != systemRelevance.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                systemRelevance.DateModified = DateTime.Now;
                systemRelevance.IsDeleted = false;
                systemRelevance.IsSynced = false;

                context.SystemRelevanceRepository.Update(systemRelevance);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSystemRelevance", "SystemRelevanceController.cs", ex.Message, systemRelevance.ModifiedIn, systemRelevance.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-relevance/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemRelevances.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystemRelevance)]
        public async Task<IActionResult> DeleteSystemRelevance(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemRelevanceInDb = await context.SystemRelevanceRepository.GetSystemsRelevanceByKey(key);

                if (systemRelevanceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemRelevanceInDb.DateModified = DateTime.Now;
                systemRelevanceInDb.IsDeleted = true;
                systemRelevanceInDb.IsSynced = false;

                context.SystemRelevanceRepository.Update(systemRelevanceInDb);
                await context.SaveChangesAsync();

                return Ok(systemRelevanceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystemRelevance", "SystemRelevanceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
