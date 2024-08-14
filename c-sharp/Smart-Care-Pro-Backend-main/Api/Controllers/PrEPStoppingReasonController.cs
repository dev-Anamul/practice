using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 30.01.2023
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
    public class PrEPStoppingReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PrEPStoppingReasonController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PrEPStoppingReasonController(IUnitOfWork context, ILogger<PrEPStoppingReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/prep-stopping-reason
        /// </summary>
        /// <param name="prEPStoppingReason">PrEPStoppingReason object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEPStoppingReason)]
        public async Task<IActionResult> CreatePrEPStoppingReason(StoppingReason prEPStoppingReason)
        {
            try
            {
                if (await IsprEPStoppingReasonInDuplicate(prEPStoppingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                prEPStoppingReason.DateCreated = DateTime.Now;
                prEPStoppingReason.IsDeleted = false;
                prEPStoppingReason.IsSynced = false;

                context.PrEPStoppingReasonRepository.Add(prEPStoppingReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPrEPStoppingReasonByKey", new { key = prEPStoppingReason.Oid }, prEPStoppingReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrEPStoppingReason", "PrEPStoppingReasonController.cs", ex.Message, prEPStoppingReason.CreatedIn, prEPStoppingReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-stopping-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPStoppingReasons)]
        public async Task<IActionResult> ReadPrEPStoppingReasons()
        {
            try
            {
                var prEPStoppingReasonIndb = await context.PrEPStoppingReasonRepository.GetPrEPStoppingReasons();

                return Ok(prEPStoppingReasonIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPStoppingReasons", "PrEPStoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-stopping-reasons/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEPStoppingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPStoppingReasonByKey)]
        public async Task<IActionResult> ReadPrEPStoppingReasonByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var prEPStoppingReasonIndb = await context.PrEPStoppingReasonRepository.GetPrEPStoppingReasonByKey(key);

                if (prEPStoppingReasonIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(prEPStoppingReasonIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPStoppingReasonByKey", "PrEPStoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-stopping-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEPStoppingReasons.</param>
        /// <param name="prEPStoppingReason">PrEPStoppingReason to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePrEPStoppingReason)]
        public async Task<IActionResult> UpdatePrEPStoppingReason(int key, StoppingReason prEPStoppingReason)
        {
            try
            {
                if (key != prEPStoppingReason.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsprEPStoppingReasonInDuplicate(prEPStoppingReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                prEPStoppingReason.DateModified = DateTime.Now;
                prEPStoppingReason.IsDeleted = false;
                prEPStoppingReason.IsSynced = false;

                context.PrEPStoppingReasonRepository.Update(prEPStoppingReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePrEPStoppingReason", "PrEPStoppingReasonController.cs", ex.Message, prEPStoppingReason.ModifiedIn, prEPStoppingReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-stopping-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEPStoppingReasons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePrEPStoppingReason)]
        public async Task<IActionResult> DeletePrEPStoppingReason(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var prEPStoppingReasonInDb = await context.PrEPStoppingReasonRepository.GetPrEPStoppingReasonByKey(key);

                if (prEPStoppingReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                prEPStoppingReasonInDb.DateModified = DateTime.Now;
                prEPStoppingReasonInDb.IsDeleted = true;
                prEPStoppingReasonInDb.IsSynced = false;

                context.PrEPStoppingReasonRepository.Update(prEPStoppingReasonInDb);
                await context.SaveChangesAsync();

                return Ok(prEPStoppingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePrEPStoppingReason", "PrEPStoppingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PrEPStoppingReason name is duplicate or not. 
        /// </summary>
        /// <param name="prEPStoppingReason">PrEPStoppingReason object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsprEPStoppingReasonInDuplicate(StoppingReason prEPStoppingReason)
        {
            try
            {
                var prEPStoppingReasonInDb = await context.PrEPStoppingReasonRepository.GetPrEPStoppingReasonByName(prEPStoppingReason.Description);

                if (prEPStoppingReasonInDb != null)
                    if (prEPStoppingReasonInDb.Oid != prEPStoppingReason.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsprEPStoppingReasonInDuplicate", "PrEPStoppingReasonController.cs", ex.Message);
                throw;
            }
        }
    }
}