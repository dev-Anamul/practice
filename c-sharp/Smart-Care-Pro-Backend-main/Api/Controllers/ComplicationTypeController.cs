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
    /// ComplicationType controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ComplicationTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ComplicationTypeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ComplicationTypeController(IUnitOfWork context, ILogger<ComplicationTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/complication-type
        /// </summary>
        /// <param name="complicationType">ComplicationType object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateComplicationType)]
        public async Task<IActionResult> CreateComplicationType(ComplicationType complicationType)
        {
            try
            {
                complicationType.DateCreated = DateTime.Now;
                complicationType.IsDeleted = false;
                complicationType.IsSynced = false;

                context.ComplicationTypeRepository.Add(complicationType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadComplicationTypeByKey", new { key = complicationType.Oid }, complicationType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateComplicationType", "ComplicationTypeController.cs", ex.Message, complicationType.CreatedIn, complicationType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplicationTypes)]
        public async Task<IActionResult> ReadComplicationTypes()
        {
            try
            {
                var complicationTypeIndb = await context.ComplicationTypeRepository.GetComplicationTypes();

                complicationTypeIndb = complicationTypeIndb.OrderByDescending(x => x.DateCreated);

                return Ok(complicationTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplicationTypes", "ComplicationTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ComplicationTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplicationTypeByKey)]
        public async Task<IActionResult> ReadComplicationTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationTypeIndb = await context.ComplicationTypeRepository.GetComplicationTypeByKey(key);

                if (complicationTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(complicationTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplicationTypeByKey", "ComplicationTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ComplicationTypes.</param>
        /// <param name="complicationType">ComplicationType to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateComplicationType)]
        public async Task<IActionResult> UpdateComplicationType(int key, ComplicationType complicationType)
        {
            try
            {
                if (key != complicationType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                complicationType.DateModified = DateTime.Now;
                complicationType.IsDeleted = false;
                complicationType.IsSynced = false;

                context.ComplicationTypeRepository.Update(complicationType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateComplicationType", "ComplicationTypeController.cs", ex.Message, complicationType.ModifiedIn, complicationType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ComplicationTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteComplicationType)]
        public async Task<IActionResult> DeleteComplicationType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationTypeIndb = await context.ComplicationTypeRepository.GetComplicationTypeByKey(key);

                if (complicationTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                complicationTypeIndb.DateModified = DateTime.Now;
                complicationTypeIndb.IsDeleted = true;
                complicationTypeIndb.IsSynced = false;

                context.ComplicationTypeRepository.Update(complicationTypeIndb);
                await context.SaveChangesAsync();

                return Ok(complicationTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteComplicationType", "ComplicationTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}