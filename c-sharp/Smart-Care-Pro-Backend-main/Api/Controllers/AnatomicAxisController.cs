using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 23.02.2023
 * Modified by  : Stephan
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// AnatomicAxis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AnatomicAxisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AnatomicAxisController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AnatomicAxisController(IUnitOfWork context, ILogger<AnatomicAxisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/anatomic-axis
        /// </summary>
        /// <param name="anatomicAxis">AnatomicAxis object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAnatomicAxis)]
        public async Task<IActionResult> CreateAnatomicAxis(AnatomicAxis anatomicAxis)
        {
            try
            {
                if (await IsAnatomicAxisDuplicate(anatomicAxis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                anatomicAxis.DateCreated = DateTime.Now;
                anatomicAxis.IsDeleted = false;
                anatomicAxis.IsSynced = false;

                context.AnatomicAxisRepository.Add(anatomicAxis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAnatomicAxisByKey", new { key = anatomicAxis.Oid }, anatomicAxis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAnatomicAxis", "AnatomicAxisController.cs", ex.Message, anatomicAxis.CreatedIn, anatomicAxis.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/anatomic-axes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnatomicAxes)]
        public async Task<IActionResult> ReadAnatomicAxes()
        {
            try
            {
                var anatomicAxisInDb = await context.AnatomicAxisRepository.GetAnatomicAxes();

                anatomicAxisInDb = anatomicAxisInDb.OrderByDescending(x => x.DateCreated);

                return Ok(anatomicAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnatomicAxes", "AnatomicAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anatomic-axis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnatomicAxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAnatomicAxisByKey)]
        public async Task<IActionResult> ReadAnatomicAxisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anatomicAxisInDb = await context.AnatomicAxisRepository.GetAnatomicAxisByKey(key);

                if (anatomicAxisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(anatomicAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAnatomicAxisByKey", "AnatomicAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anatomic-axis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnatomicAxes.</param>
        /// <param name="anatomicAxis">AnatomicAxis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAnatomicAxis)]
        public async Task<IActionResult> UpdateAnatomicAxis(int key, AnatomicAxis anatomicAxis)
        {
            try
            {
                if (key != anatomicAxis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsAnatomicAxisDuplicate(anatomicAxis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                anatomicAxis.DateModified = DateTime.Now;
                anatomicAxis.IsDeleted = false;
                anatomicAxis.IsSynced = false;

                context.AnatomicAxisRepository.Update(anatomicAxis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateAnatomicAxis", "AnatomicAxisController.cs", ex.Message, anatomicAxis.CreatedIn, anatomicAxis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anatomic-axis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AnatomicAxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAnatomicAxis)]
        public async Task<IActionResult> DeleteAnatomicAxis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var anatomicAxisInDb = await context.AnatomicAxisRepository.GetAnatomicAxisByKey(key);

                if (anatomicAxisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                anatomicAxisInDb.DateModified = DateTime.Now;
                anatomicAxisInDb.IsDeleted = true;
                anatomicAxisInDb.IsSynced = false;

                context.AnatomicAxisRepository.Update(anatomicAxisInDb);
                await context.SaveChangesAsync();

                return Ok(anatomicAxisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAnatomicAxis", "AnatomicAxisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the anatomic axis name is duplicate or not. 
        /// </summary>
        /// <param name="anatomicAxis">AnatomicAxis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsAnatomicAxisDuplicate(AnatomicAxis anatomicAxis)
        {
            try
            {
                var anatomicAxisInDb = await context.AnatomicAxisRepository.GetAnatomicAxisByName(anatomicAxis.Description);

                if (anatomicAxisInDb != null)
                    if (anatomicAxisInDb.Oid != anatomicAxis.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAnatomicAxisDuplicate", "AnatomicAxisController.cs", ex.Message);
                throw;
            }
        }
    }
}