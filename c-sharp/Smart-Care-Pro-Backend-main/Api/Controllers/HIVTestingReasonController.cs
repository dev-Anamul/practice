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
    /// HIVTestingReason controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HIVTestingReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HIVTestingReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HIVTestingReasonController(IUnitOfWork context, ILogger<HIVTestingReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/hiv-testing-reason
        /// </summary>
        /// <param name="testingReason">HIVTestingReason object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHIVTestingReason)]
        public async Task<IActionResult> CreateHIVTestingReason(HIVTestingReason testingReason)
        {
            try
            {
                if (await IsHIVTestingReasonDuplicate(testingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                testingReason.DateCreated = DateTime.Now;
                testingReason.IsDeleted = false;
                testingReason.IsSynced = false;

                context.HIVTestingReasonRepository.Add(testingReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadHIVTestingReasonByKey", new { key = testingReason.Oid }, testingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHIVTestingReason", "HIVTestingReasonController.cs", ex.Message, testingReason.CreatedIn, testingReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-testing-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVTestingReasons)]
        public async Task<IActionResult> ReadHIVTestingReasons()
        {
            try
            {
                var hivTestingReason = await context.HIVTestingReasonRepository.GetHIVTestingReasons();

                return Ok(hivTestingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVTestingReasons", "HIVTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-testing-reason/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVTestingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVTestingReasonByKey)]
        public async Task<IActionResult> ReadHIVTestingReasonByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hivTestingReason = await context.HIVTestingReasonRepository.GetHIVTestingReasonByKey(key);

                if (hivTestingReason == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(hivTestingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVTestingReasonByKey", "HIVTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-testing-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVTestingReasons.</param>
        /// <param name="testingReason">HIVTestingReason to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHIVTestingReason)]
        public async Task<IActionResult> UpdateHIVTestingReason(int key, HIVTestingReason testingReason)
        {
            try
            {
                if (key != testingReason.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsHIVTestingReasonDuplicate(testingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                testingReason.DateModified = DateTime.Now;
                testingReason.IsDeleted = false;
                testingReason.IsSynced = false;

                context.HIVTestingReasonRepository.Update(testingReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHIVTestingReason", "HIVTestingReasonController.cs", ex.Message, testingReason.ModifiedIn, testingReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-testing-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVTestingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHIVTestingReason)]
        public async Task<IActionResult> DeleteHIVTestingReason(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hivTestingReasonInDb = await context.HIVTestingReasonRepository.GetHIVTestingReasonByKey(key);

                if (hivTestingReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                hivTestingReasonInDb.DateModified = DateTime.Now;
                hivTestingReasonInDb.IsDeleted = true;
                hivTestingReasonInDb.IsSynced = false;

                context.HIVTestingReasonRepository.Update(hivTestingReasonInDb);
                await context.SaveChangesAsync();

                return Ok(hivTestingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHIVTestingReason", "HIVTestingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the HIV testing reason is duplicate or not. 
        /// </summary>
        /// <param name="testingReason">HIVTestingReason object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsHIVTestingReasonDuplicate(HIVTestingReason testingReason)
        {
            try
            {
                var hivTestingReasonInDb = await context.HIVTestingReasonRepository.GetHIVTestingReasonByTestingReason(testingReason.Description);

                if (hivTestingReasonInDb != null)
                    if (hivTestingReasonInDb.Oid != testingReason.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsHIVTestingReasonDuplicate", "HIVTestingReasonController.cs", ex.Message);
                throw;
            }
        }
    }
}