using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;


namespace Api.Controllers
{
    /// <summary>
    /// StoppingReason controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class StoppingReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<StoppingReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public StoppingReasonController(IUnitOfWork context, ILogger<StoppingReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/stopping-reason
        /// </summary>
        /// <param name="stoppingReason">StoppingReason object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateStoppingReason)]
        public async Task<ActionResult<StoppingReason>> CreateStoppingReason(StoppingReason stoppingReason)
        {
            try
            {
                if (await IsStoppingReasonDuplicate(stoppingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                stoppingReason.DateCreated = DateTime.Now;
                stoppingReason.IsDeleted = false;
                stoppingReason.IsSynced = false;

                context.StoppingReasonRepository.Add(stoppingReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadStoppingReasonByKey", new { key = stoppingReason.Oid }, stoppingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateStoppingReason", "StoppingReasonController.cs", ex.Message, stoppingReason.CreatedIn, stoppingReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/stopping-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadStoppingReason)]
        public async Task<IActionResult> ReadStoppingReason()
        {
            try
            {
                var stoppingReasonInDb = await context.StoppingReasonRepository.GetStoppingReason();

                return Ok(stoppingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadStoppingReason", "StoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/stopping-reason/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table StoppingReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadStoppingReasonByKey)]
        public async Task<IActionResult> ReadStoppingReasonByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var stoppingReasonInDb = await context.StoppingReasonRepository.GetStoppingReasonByKey(key);

                if (stoppingReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(stoppingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadStoppingReasonByKey", "StoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/stopping-reason
        /// </summary>
        /// <param name="key">Primary key of the table StoppingReason.</param>
        /// <param name="stoppingReasonInDb">StoppingReason to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateStoppingReason)]
        public async Task<IActionResult> UpdateStoppingReason(int key, StoppingReason stoppingReason)
        {
            try
            {
                if (key != stoppingReason.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                stoppingReason.DateModified = DateTime.Now;
                stoppingReason.IsDeleted = false;
                stoppingReason.IsSynced = false;

                context.StoppingReasonRepository.Update(stoppingReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateStoppingReason", "StoppingReasonController.cs", ex.Message, stoppingReason.ModifiedIn, stoppingReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/stopping-reason
        /// </summary>
        /// <param name="key">Primary key of the table StoppingReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteStoppingReason)]
        public async Task<IActionResult> DeleteStoppingReason(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var stoppingReasonInDb = await context.StoppingReasonRepository.GetStoppingReasonByKey(key);

                if (stoppingReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                stoppingReasonInDb.DateModified = DateTime.Now;
                stoppingReasonInDb.IsDeleted = true;
                stoppingReasonInDb.IsSynced = false;

                context.StoppingReasonRepository.Update(stoppingReasonInDb);
                await context.SaveChangesAsync();

                return Ok(stoppingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteStoppingReason", "StoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the StoppingReason name is duplicate or not.
        /// </summary>
        /// <param name="stoppingReason">StoppingReason object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsStoppingReasonDuplicate(StoppingReason stoppingReason)
        {
            try
            {
                var stoppingReasonInDb = await context.StoppingReasonRepository.GetStoppingReasonByName(stoppingReason.Description);

                if (stoppingReasonInDb != null)
                    if (stoppingReasonInDb.Oid != stoppingReasonInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsStoppingReasonDuplicate", "StoppingReasonController.cs", ex.Message);
                throw;
            }
        }
    }
}
