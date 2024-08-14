using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// HIVNotTestingReason controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HIVNotTestingReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HIVNotTestingReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HIVNotTestingReasonController(IUnitOfWork context, ILogger<HIVNotTestingReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/hiv-not-testing-reason
        /// </summary>
        /// <param name="notTestingReason">HIVNotTestingReason object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHIVNotTestingReason)]
        public async Task<IActionResult> CreateHIVNotTestingReason(HIVNotTestingReason notTestingReason)
        {
            try
            {
                if (await IsHIVNotTestingReasonDuplicate(notTestingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                notTestingReason.DateCreated = DateTime.Now;
                notTestingReason.IsDeleted = false;
                notTestingReason.IsSynced = false;

                context.HIVNotTestingReasonRepository.Add(notTestingReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadHIVNotTestingReasonByKey", new { key = notTestingReason.Oid }, notTestingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHIVNotTestingReason", "HIVNotTestingReasonController.cs", ex.Message, notTestingReason.CreatedIn, notTestingReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-not-testing-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVNotTestingReasons)]
        public async Task<IActionResult> ReadHIVNotTestingReasons()
        {
            try
            {
                var hivNotTestingInDB = await context.HIVNotTestingReasonRepository.GetHIVNotTestingReasons();

                return Ok(hivNotTestingInDB);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVNotTestingReasons", "HIVNotTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-not-testing-reason/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVNotTestingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVNotTestingReasonByKey)]
        public async Task<IActionResult> ReadHIVNotTestingReasonByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hivNotTestingInDB = await context.HIVNotTestingReasonRepository.GetHIVNotTestingReasonByKey(key);

                if (hivNotTestingInDB == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(hivNotTestingInDB);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVNotTestingReasonByKey", "HIVNotTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-not-testing-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVNotTestingReasons.</param>
        /// <param name="notTestingReason">HIVNotTestingReason to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHIVNotTestingReason)]
        public async Task<IActionResult> UpdateHIVNotTestingReason(int key, HIVNotTestingReason notTestingReason)
        {
            try
            {
                if (key != notTestingReason.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsHIVNotTestingReasonDuplicate(notTestingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                notTestingReason.DateModified = DateTime.Now;
                notTestingReason.IsDeleted = false;
                notTestingReason.IsSynced = false;

                context.HIVNotTestingReasonRepository.Update(notTestingReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHIVNotTestingReason", "HIVNotTestingReasonController.cs", ex.Message, notTestingReason.ModifiedIn, notTestingReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-not-testing-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVNotTestingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHIVNotTestingReason)]
        public async Task<IActionResult> DeleteHIVNotTestingReason(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var notTestingReasonInDb = await context.HIVNotTestingReasonRepository.GetHIVNotTestingReasonByKey(key);

                if (notTestingReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                notTestingReasonInDb.DateModified = DateTime.Now;
                notTestingReasonInDb.IsDeleted = true;
                notTestingReasonInDb.IsSynced = false;

                context.HIVNotTestingReasonRepository.Update(notTestingReasonInDb);
                await context.SaveChangesAsync();

                return Ok(notTestingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHIVNotTestingReason", "HIVNotTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the HIV not testing reason is duplicate or not.
        /// </summary>
        /// <param name="hivNotTestingReason">HIVNotTestingReason object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsHIVNotTestingReasonDuplicate(HIVNotTestingReason hivNotTestingReason)
        {
            try
            {
                var notTestingReasonInDB = await context.HIVNotTestingReasonRepository.GetHIVNotTestingReasonByNotTestingReason(hivNotTestingReason.Description);

                if (notTestingReasonInDB != null)
                    if (notTestingReasonInDB.Oid != hivNotTestingReason.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsHIVNotTestingReasonDuplicate", "HIVNotTestingReasonController.cs", ex.Message);
                throw;
            }
        }
    }
}