using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ResultOption controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ResultOptionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ResultOptionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ResultOptionController(IUnitOfWork context, ILogger<ResultOptionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/result-option
        /// </summary>
        /// <param name="resultOption">ResultOption object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateResultOption)]
        public async Task<ActionResult<ResultOption>> CreateResultOption(ResultOption resultOption)
        {
            try
            {
                resultOption.DateCreated = DateTime.Now;
                resultOption.IsDeleted = false;
                resultOption.IsSynced = false;

                context.ResultOptionRepository.Add(resultOption);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadResultOptionByKey", new { key = resultOption.Oid }, resultOption);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateResultOption", "ResultOptionController.cs", ex.Message, resultOption.CreatedIn, resultOption.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result-options
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadResultOptions)]
        public async Task<IActionResult> ReadResultOptions()
        {
            try
            {
                var resultOptionInDb = await context.ResultOptionRepository.GetResultOptions();

                return Ok(resultOptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadResultOptions", "ResultOptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result-option/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ResultOption.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadResultOptionByKey)]
        public async Task<IActionResult> ReadResultOptionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultOptionInDb = await context.ResultOptionRepository.GetResultOptionByKey(key);

                if (resultOptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(resultOptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadResultOptionByKey", "ResultOptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result-option/{testId}
        /// </summary>
        /// <param name="testid"> testid of the table ResultOption.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadResultOptionByTest)]
        public async Task<IActionResult> ReadResultOptionByTest(int testid)
        {
            try
            {
                if (testid <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultOptionInDb = await context.ResultOptionRepository.GetResultOptionByTest(testid);

                if (resultOptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(resultOptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadResultOptionByTest", "ResultOptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result-option/{key}
        /// </summary>
        /// <param name="key">Primary key of the table resultOptions.</param>
        /// <param name="resultOption">ResultOption to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateResultOption)]
        public async Task<IActionResult> UpdateResultOption(int key, ResultOption resultOption)
        {
            try
            {
                if (key != resultOption.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                resultOption.DateModified = DateTime.Now;
                resultOption.IsDeleted = false;
                resultOption.IsSynced = false;

                context.ResultOptionRepository.Update(resultOption);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateResultOption", "ResultOptionController.cs", ex.Message, resultOption.ModifiedIn, resultOption.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result-option/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ResultOptions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteResultOption)]
        public async Task<IActionResult> DeleteResultOption(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultOptionInDb = await context.ResultOptionRepository.GetResultOptionByKey(key);

                if (resultOptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                resultOptionInDb.DateModified = DateTime.Now;
                resultOptionInDb.IsDeleted = true;
                resultOptionInDb.IsSynced = false;

                context.ResultOptionRepository.Update(resultOptionInDb);
                await context.SaveChangesAsync();

                return Ok(resultOptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteResultOption", "ResultOptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ResultOption name is duplicate or not.
        /// </summary>
        /// <param name="resultOption">ResultOption object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsResultOptionDuplicate(ResultOption resultOption)
        {
            try
            {
                var resultOptionInDb = await context.ResultOptionRepository.GetResultOptionByName(resultOption.Description);

                if (resultOptionInDb != null)
                    if (resultOptionInDb.Oid != resultOption.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsResultOptionDuplicate", "ResultOptionController.cs", ex.Message);
                throw;
            }
        }
    }
}