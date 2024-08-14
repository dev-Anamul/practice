using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : 
 * Date created : 
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ReasonofReferralController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ReasonofReferralController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ReasonofReferralController(IUnitOfWork context, ILogger<ReasonofReferralController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/reasons-ofreferral
        /// </summary>
        /// <param name="reasonOfReferral">ReasonOfReferral object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateReasonsOfReferral)]
        public async Task<ActionResult<ReasonOfReferral>> CreateReasonOfReferral(ReasonOfReferral reasonOfReferral)
        {
            try
            {
                if (await IsReasonOfReferralDuplicate(reasonOfReferral) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                reasonOfReferral.DateCreated = DateTime.Now;
                reasonOfReferral.IsDeleted = false;
                reasonOfReferral.IsSynced = false;

                context.ReasonsOfReferalRepository.Add(reasonOfReferral);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadReasonOfReferralByKey", new { key = reasonOfReferral.Oid }, reasonOfReferral);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateReasonOfReferral", "ReasonofReferralController.cs", ex.Message, reasonOfReferral.CreatedIn, reasonOfReferral.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/reasons-ofreferrals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReasonsOfReferrals)]
        public async Task<IActionResult> ReadReasonsOfReferrals()
        {
            try
            {
                var reasonsofreferralInDb = await context.ReasonsOfReferalRepository.GetReasonOfReferrals();

                return Ok(reasonsofreferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReasonsOfReferrals", "ReasonofReferralController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/reasons-ofreferral/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReasonOfReferral.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReasonsOfReferralByKey)]
        public async Task<IActionResult> ReadReasonOfReferralByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var reasonOfReferralInDb = await context.ReasonsOfReferalRepository.GetReasonOfReferralByKey(key);

                if (reasonOfReferralInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(reasonOfReferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReasonOfReferralByKey", "ReasonofReferralController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/reasons-ofreferral/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReasonOfReferrals.</param>
        /// <param name="reasonOfReferral">ReasonOfReferral to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateReasonsOfReferral)]
        public async Task<IActionResult> UpdateReasonOfReferral(int key, ReasonOfReferral reasonOfReferral)
        {
            try
            {
                if (key != reasonOfReferral.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                reasonOfReferral.DateModified = DateTime.Now;
                reasonOfReferral.IsDeleted = false;
                reasonOfReferral.IsSynced = false;

                context.ReasonsOfReferalRepository.Update(reasonOfReferral);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateReasonOfReferral", "ReasonofReferralController.cs", ex.Message, reasonOfReferral.ModifiedIn, reasonOfReferral.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/reasons-ofreferral/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReasonOfReferrals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteReasonsOfReferral)]
        public async Task<IActionResult> DeleteReasonOfReferral(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var reasonOfReferralInDb = await context.ReasonsOfReferalRepository.GetReasonOfReferralByKey(key);

                if (reasonOfReferralInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                reasonOfReferralInDb.DateModified = DateTime.Now;
                reasonOfReferralInDb.IsDeleted = true;
                reasonOfReferralInDb.IsSynced = false;

                context.ReasonsOfReferalRepository.Update(reasonOfReferralInDb);
                await context.SaveChangesAsync();

                return Ok(reasonOfReferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteReasonOfReferral", "ReasonofReferralController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ReasonOfReferral name is duplicate or not.
        /// </summary>
        /// <param name="ReasonOfReferral">ReasonOfReferral object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsReasonOfReferralDuplicate(ReasonOfReferral reasonOfReferral)
        {
            try
            {
                var reasonOfReferralInDb = await context.ReasonsOfReferalRepository.GetReasonOfReferralByName(reasonOfReferral.Description);

                if (reasonOfReferralInDb != null)
                    if (reasonOfReferralInDb.Oid != reasonOfReferral.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsReasonOfReferralDuplicate", "ReasonofReferralController.cs", ex.Message);
                throw;
            }
        }
    }
}