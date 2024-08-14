using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// LoginHistory controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class LoginHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<LoginHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public LoginHistoryController(IUnitOfWork context, ILogger<LoginHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/login-history
        /// </summary>
        /// <param name="loginHistory">LoginHistory object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateLoginHistory)]
        public async Task<IActionResult> CreateLoginHistory(LoginHistory loginHistory)
        {
            try
            {
                loginHistory.DateCreated = DateTime.Now;
                loginHistory.IsDeleted = false;
                loginHistory.IsSynced = false;

                context.LoginHistoryRepository.Add(loginHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadLoginHistoryByKey", new { key = loginHistory.Oid }, loginHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateloginHistory", "LoginHistoryController.cs", ex.Message, loginHistory.CreatedIn, loginHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/login-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLoginHistories)]
        public async Task<IActionResult> ReadLoginHistories()
        {
            try
            {
                var loginHistoryInDb = await context.LoginHistoryRepository.GetloginHistories();

                return Ok(loginHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLoginHistories", "LoginHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/login-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table LoginHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLoginHistoryByKey)]
        public async Task<IActionResult> ReadLoginHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var loginHistoryInDb = await context.LoginHistoryRepository.GetLoginHistoryByKey(key);

                if (loginHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(loginHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLoginHistoryByKey", "LoginHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/login-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table LoginHistories.</param>
        /// <param name="loginHistory">LoginHistory Object.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateLoginHistory)]
        public async Task<IActionResult> UpdateLoginHistory(Guid key, LoginHistory loginHistory)
        {
            try
            {
                if (key == loginHistory.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                loginHistory.DateModified = DateTime.Now;
                loginHistory.IsDeleted = false;
                loginHistory.IsSynced = false;

                context.LoginHistoryRepository.Update(loginHistory);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateLoginHistory", "LoginHistoryController.cs", ex.Message, loginHistory.ModifiedIn, loginHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/login-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteLoginHistory)]
        public async Task<IActionResult> DeleteLoginHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var loginHistoryInDb = await context.LoginHistoryRepository.GetLoginHistoryByKey(key);

                if (loginHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                loginHistoryInDb.DateModified = DateTime.Now;
                loginHistoryInDb.IsDeleted = true;
                loginHistoryInDb.IsSynced = false;

                context.LoginHistoryRepository.Update(loginHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(loginHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteLoginHistory", "LoginHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}