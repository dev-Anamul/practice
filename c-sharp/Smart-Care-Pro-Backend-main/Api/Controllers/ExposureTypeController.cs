using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 19.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ExposureTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ExposureTypeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ExposureTypeController(IUnitOfWork context, ILogger<ExposureTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/exposure-types
        /// </summary>
        /// <param name="exposureType">ExposureType object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateExposureType)]
        public async Task<IActionResult> CreateExposureType(ExposureType exposureType)
        {
            try
            {
                if (await IsExposureTypeDuplicate(exposureType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                exposureType.DateCreated = DateTime.Now;
                exposureType.IsDeleted = false;
                exposureType.IsSynced = false;

                context.ExposureTypeRepository.Add(exposureType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadExposureTypeByKey", new { key = exposureType.Oid }, exposureType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateExposureType", "ExposureTypeController.cs", ex.Message, exposureType.CreatedIn, exposureType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }





        /// <summary>
        /// URL: sc-api/exposure-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadExposureTypes)]
        public async Task<IActionResult> ReadExposureTypes()
        {
            try
            {
                var exposureInDb = await context.ExposureTypeRepository.GetExposureTypes();

                return Ok(exposureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadExposureTypes", "ExposureTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/exposure-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ExposureType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadExposureTypeByKey)]
        public async Task<IActionResult> ReadExposureTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var exposureInDb = await context.ExposureTypeRepository.GetExposureTypeByKey(key);

                if (exposureInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(exposureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadExposureTypeByKey", "ExposureTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/exposure-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ExposureType.</param>
        /// <param name="exposureType">ExposureType to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateExposureType)]
        public async Task<IActionResult> UpdateExposureType(int key, ExposureType exposureType)
        {
            try
            {
                if (key != exposureType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsExposureTypeDuplicate(exposureType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                exposureType.DateModified = DateTime.Now;
                exposureType.IsDeleted = false;
                exposureType.IsSynced = false;

                context.ExposureTypeRepository.Update(exposureType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateExposureType", "ExposureTypeController.cs", ex.Message, exposureType.ModifiedIn, exposureType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/exposure-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ExposureType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteExposureType)]
        public async Task<IActionResult> DeleteExposureType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var exposureInDb = await context.ExposureTypeRepository.GetExposureTypeByKey(key);

                if (exposureInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                exposureInDb.DateModified = DateTime.Now;
                exposureInDb.IsDeleted = true;
                exposureInDb.IsSynced = false;

                context.ExposureTypeRepository.Update(exposureInDb);
                await context.SaveChangesAsync();

                return Ok(exposureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteExposureType", "ExposureTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// Checks whether the exposure type is duplicate or not.
        /// </summary> 
        /// <param name="exposureType">ExposureType object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsExposureTypeDuplicate(ExposureType exposureType)
        {
            try
            {
                var exposureInDb = await context.ExposureTypeRepository.GetExposureTypeByType(exposureType.Description);

                if (exposureInDb != null)
                    if (exposureInDb.Oid != exposureType.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsExposureTypeDuplicate", "ExposureTypeController.cs", ex.Message);
                throw;
            }
        }

    }
}