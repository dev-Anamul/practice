using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 12.03.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Frequency Interval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FrequencyIntervalController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FrequencyIntervalController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FrequencyIntervalController(IUnitOfWork context, ILogger<FrequencyIntervalController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/frequency-interval
        /// </summary>
        /// <param name="frequencyInterval">FrequencyInterval object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFrequencyInterval)]
        public async Task<ActionResult<FrequencyInterval>> CreateFrequencyInterval(FrequencyInterval frequencyInterval)
        {
            try
            {
                if (await IsFrequencyIntervalDuplicate(frequencyInterval) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                frequencyInterval.DateCreated = DateTime.Now;
                frequencyInterval.IsDeleted = false;
                frequencyInterval.IsSynced = false;

                context.FrequencyIntervalRepository.Add(frequencyInterval);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFrequencyIntervalByKey", new { key = frequencyInterval.Oid }, frequencyInterval);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFrequencyInterval", "FrequencyIntervalController.cs", ex.Message, frequencyInterval.CreatedIn, frequencyInterval.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-intervals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFrequencyIntervals)]
        public async Task<IActionResult> ReadFrequencyIntervals()
        {
            try
            {
                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyInterval();

                return Ok(frequencyIntervalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFrequencyIntervals", "FrequencyIntervalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-interval/by-time-intervals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFrequencyIntervalByTimeIntervals)]
        public async Task<IActionResult> ReadFrequencyIntervalByTimeInterval()
        {
            try
            {
                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyIntervalByTimeInterval();

                return Ok(frequencyIntervalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFrequencyIntervalByTimeInterval", "FrequencyIntervalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-interval/by-interval
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFrequencyIntervalByFrequency)]
        public async Task<IActionResult> ReadFrequencyIntervalByFrequency()
        {
            try
            {
                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyIntervalByFrequency();

                return Ok(frequencyIntervalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFrequencyIntervalByFrequency", "FrequencyIntervalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-interval/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReadFrequencyIntervalByKey By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFrequencyIntervalByKey)]
        public async Task<IActionResult> ReadFrequencyIntervalByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyIntervalByKey(key);

                if (frequencyIntervalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(frequencyIntervalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFrequencyIntervalByKey", "FrequencyIntervalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-interval/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FrequencyIntervals.</param>
        /// <param name="frequencyInterval">FrequencyInterval to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFrequencyInterval)]
        public async Task<IActionResult> UpdateFrequencyInterval(int key, FrequencyInterval frequencyInterval)
        {
            try
            {
                if (key != frequencyInterval.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                frequencyInterval.DateModified = DateTime.Now;
                frequencyInterval.IsDeleted = false;
                frequencyInterval.IsSynced = false;

                context.FrequencyIntervalRepository.Update(frequencyInterval);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFrequencyInterval", "FrequencyIntervalController.cs", ex.Message, frequencyInterval.ModifiedIn, frequencyInterval.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/frequency-interval/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FrequencyIntervals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFrequencyInterval)]
        public async Task<IActionResult> DeleteFrequencyInterval(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyIntervalByKey(key);

                if (frequencyIntervalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                frequencyIntervalInDb.DateModified = DateTime.Now;
                frequencyIntervalInDb.IsDeleted = true;
                frequencyIntervalInDb.IsSynced = false;

                context.FrequencyIntervalRepository.Update(frequencyIntervalInDb);
                await context.SaveChangesAsync();

                return Ok(frequencyIntervalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFrequencyInterval", "FrequencyIntervalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the FrequencyInterval name is duplicate or not.
        /// </summary>
        /// <param name="frequencyInterval">FrequencyInterval object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFrequencyIntervalDuplicate(FrequencyInterval frequencyInterval)
        {
            try
            {
                var frequencyIntervalInDb = await context.FrequencyIntervalRepository.GetFrequencyIntervalByName(frequencyInterval.Description);

                if (frequencyIntervalInDb != null)
                    if (frequencyIntervalInDb.Oid != frequencyInterval.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFrequencyIntervalDuplicate", "FrequencyIntervalController.cs", ex.Message);
                throw;
            }
        }
    }
}