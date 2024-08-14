using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PathologyAxis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PathologyAxisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PathologyAxisController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PathologyAxisController(IUnitOfWork context, ILogger<PathologyAxisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pathology-axis
        /// </summary>
        /// <param name="pathologyAxis">PathologyAxis object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePathologyAxis)]
        public async Task<IActionResult> CreatePathologyAxis(PathologyAxis pathologyAxis)
        {
            try
            {
                if (await IsPathologyAxisDuplicate(pathologyAxis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                pathologyAxis.DateCreated = DateTime.Now;
                pathologyAxis.IsDeleted = false;
                pathologyAxis.IsSynced = false;

                context.PathologyAxisRepository.Add(pathologyAxis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPathologyAxisByKey", new { key = pathologyAxis.Oid }, pathologyAxis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePathologyAxis", "PathologyAxisController.cs", ex.Message, pathologyAxis.CreatedIn, pathologyAxis.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/pathology-axes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPathologyAxes)]
        public async Task<IActionResult> ReadPathologyAxes()
        {
            try
            {
                var pathologyAxisInDb = await context.PathologyAxisRepository.GetPathologyAxes();

                return Ok(pathologyAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPathologyAxes", "PathologyAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pathology-axis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PathologyAxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPathologyAxisByKey)]
        public async Task<IActionResult> ReadPathologyAxisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pathologyAxisInDb = await context.PathologyAxisRepository.GetPathologyAxisByKey(key);

                if (pathologyAxisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pathologyAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPathologyAxisByKey", "PathologyAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pathology-axis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PathologyAxes.</param>
        /// <param name="PathologyAxis">PathologyAxis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePathologyAxis)]
        public async Task<IActionResult> UpdatePathologyAxis(int key, PathologyAxis pathologyAxis)
        {
            try
            {
                if (key != pathologyAxis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsPathologyAxisDuplicate(pathologyAxis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                pathologyAxis.DateModified = DateTime.Now;
                pathologyAxis.IsDeleted = false;
                pathologyAxis.IsSynced = false;

                context.PathologyAxisRepository.Update(pathologyAxis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePathologyAxis", "PathologyAxisController.cs", ex.Message, pathologyAxis.ModifiedIn, pathologyAxis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pathology-axis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PathologyAxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePathologyAxis)]
        public async Task<IActionResult> DeletePathologyAxis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pathologyAxisInDb = await context.PathologyAxisRepository.GetPathologyAxisByKey(key);

                if (pathologyAxisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pathologyAxisInDb.DateModified = DateTime.Now;
                pathologyAxisInDb.IsDeleted = true;
                pathologyAxisInDb.IsSynced = false;

                context.PathologyAxisRepository.Update(pathologyAxisInDb);
                await context.SaveChangesAsync();

                return Ok(pathologyAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePathologyAxis", "PathologyAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the Pathology axis name is duplicate or not. 
        /// </summary>
        /// <param name="pathologyAxis">PathologyAxis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPathologyAxisDuplicate(PathologyAxis pathologyAxis)
        {
            try
            {
                var pathologyAxisInDb = await context.PathologyAxisRepository.GetPathologyAxisByName(pathologyAxis.Description);

                if (pathologyAxisInDb != null)
                    if (pathologyAxisInDb.Oid != pathologyAxis.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPathologyAxisDuplicate", "PathologyAxisController.cs", ex.Message);
                throw;
            }
        }
    }
}