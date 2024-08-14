using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// MeasuringUnit controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MeasuringUnitController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MeasuringUnitController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MeasuringUnitController(IUnitOfWork context, ILogger<MeasuringUnitController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/measuring-unit
        /// </summary>
        /// <param name="measuringUnit">MeasuringUnit object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMeasuringUnit)]
        public async Task<ActionResult<MeasuringUnit>> CreateMeasuringUnit(MeasuringUnit measuringUnit)
        {
            try
            {
                if (await IsMeasuringUnitDuplicate(measuringUnit) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                measuringUnit.DateCreated = DateTime.Now;
                measuringUnit.IsDeleted = false;
                measuringUnit.IsSynced = false;

                context.MeasuringUnitRepository.Add(measuringUnit);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMeasuringUnitByKey", new { key = measuringUnit.Oid }, measuringUnit);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMeasuringUnit", "MeasuringUnitController.cs", ex.Message, measuringUnit.CreatedIn, measuringUnit.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/measuring-units
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMeasuringUnits)]
        public async Task<IActionResult> ReadMeasuringUnits()
        {
            try
            {
                var measuringUnitInDb = await context.MeasuringUnitRepository.GetMeasuringUnits();

                return Ok(measuringUnitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMeasuringUnits", "MeasuringUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/measuring-unit/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnit.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMeasuringUnitByKey)]
        public async Task<IActionResult> ReadMeasuringUnitByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var measuringUnitInDb = await context.MeasuringUnitRepository.GetMeasuringUnitByKey(key);

                if (measuringUnitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(measuringUnitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMeasuringUnitByKey", "MeasuringUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/measuring-unit/test/{testid}
        /// </summary>
        /// <param name="testId">Primary key of the table MeasuringUnit.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMeasuringUnitByTest)]
        public async Task<IActionResult> ReadMeasuringUnitByTest(int testId)
        {
            try
            {
                if (testId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var measuringUnitInDb = await context.MeasuringUnitRepository.GetMeasuringUnitByTest(testId);

                if (measuringUnitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(measuringUnitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMeasuringUnitByTest", "MeasuringUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/measuring-unit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnits.</param>
        /// <param name="measuringUnit">MeasuringUnit to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMeasuringUnit)]
        public async Task<IActionResult> UpdateMeasuringUnit(int key, MeasuringUnit measuringUnit)
        {
            try
            {
                if (key != measuringUnit.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                measuringUnit.DateModified = DateTime.Now;
                measuringUnit.IsDeleted = false;
                measuringUnit.IsSynced = false;

                context.MeasuringUnitRepository.Update(measuringUnit);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMeasuringUnit", "MeasuringUnitController.cs", ex.Message, measuringUnit.ModifiedIn, measuringUnit.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/measuring-unit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMeasuringUnit)]
        public async Task<IActionResult> DeleteMeasuringUnit(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var measuringUnitInDb = await context.MeasuringUnitRepository.GetMeasuringUnitByKey(key);

                if (measuringUnitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                measuringUnitInDb.DateModified = DateTime.Now;
                measuringUnitInDb.IsDeleted = true;
                measuringUnitInDb.IsSynced = false;

                context.MeasuringUnitRepository.Update(measuringUnitInDb);
                await context.SaveChangesAsync();

                return Ok(measuringUnitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMeasuringUnit", "MeasuringUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the MeasuringUnit name is duplicate or not.
        /// </summary>
        /// <param name="measuringUnit">MeasuringUnit object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsMeasuringUnitDuplicate(MeasuringUnit measuringUnit)
        {
            try
            {
                var measuringUnitInDb = await context.MeasuringUnitRepository.GetMeasuringUnitByName(measuringUnit.Description);

                if (measuringUnitInDb != null)
                    if (measuringUnitInDb.Oid != measuringUnit.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsMeasuringUnitDuplicate", "MeasuringUnitController.cs", ex.Message);
                throw;
            }
        }
    }
}