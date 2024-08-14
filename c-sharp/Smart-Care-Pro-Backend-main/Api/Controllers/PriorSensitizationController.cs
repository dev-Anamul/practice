using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PriorSensitization controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PriorSensitizationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PriorSensitizationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PriorSensitizationController(IUnitOfWork context, ILogger<PriorSensitizationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/prior-sensitization
        /// </summary>
        /// <param name="priorSensitization">PriorSensitization object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePriorSensitization)]
        public async Task<ActionResult<PriorSensitization>> CreatePriorSensitization(PriorSensitization priorSensitization)
        {
            try
            {
                if (await IsPriorSensitizationDuplicate(priorSensitization) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                priorSensitization.DateCreated = DateTime.Now;
                priorSensitization.IsDeleted = false;
                priorSensitization.IsSynced = false;

                context.PriorSensitizationRepository.Add(priorSensitization);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPriorSensitizationByKey", new { key = priorSensitization.Oid }, priorSensitization);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePriorSensitization", "PriorSensitizationController.cs", ex.Message, priorSensitization.CreatedIn, priorSensitization.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-sensitizations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPriorSensitizations)]
        public async Task<IActionResult> ReadPriorSensitizations()
        {
            try
            {
                var priorSensitizationInDb = await context.PriorSensitizationRepository.GetPriorSensitizations();

                return Ok(priorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPriorSensitizations", "PriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-sensitizations/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorSensitization.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPriorSensitizationByKey)]
        public async Task<IActionResult> ReadPriorSensitizationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var priorSensitizationInDb = await context.PriorSensitizationRepository.GetPriorSensitizationByKey(key);

                if (priorSensitizationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(priorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPriorSensitizationByKey", "PriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-sensitization/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorSensitizations.</param>
        /// <param name="priorSensitization">PriorSensitization to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePriorSensitization)]
        public async Task<IActionResult> UpdatePriorSensitization(int key, PriorSensitization priorSensitization)
        {
            try
            {
                if (key != priorSensitization.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                priorSensitization.DateModified = DateTime.Now;
                priorSensitization.IsDeleted = false;
                priorSensitization.IsSynced = false;

                context.PriorSensitizationRepository.Update(priorSensitization);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePriorSensitization", "PriorSensitizationController.cs", ex.Message, priorSensitization.ModifiedIn, priorSensitization.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-sensitization/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorSensitizations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePriorSensitization)]
        public async Task<IActionResult> DeletePriorSensitization(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var priorSensitizationInDb = await context.PriorSensitizationRepository.GetPriorSensitizationByKey(key);

                if (priorSensitizationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                priorSensitizationInDb.DateModified = DateTime.Now;
                priorSensitizationInDb.IsDeleted = true;
                priorSensitizationInDb.IsSynced = false;

                context.PriorSensitizationRepository.Update(priorSensitizationInDb);
                await context.SaveChangesAsync();

                return Ok(priorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePriorSensitization", "PriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PriorSensitization name is duplicate or not.
        /// </summary>
        /// <param name="PriorSensitization">PriorSensitization object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPriorSensitizationDuplicate(PriorSensitization priorSensitization)
        {
            try
            {
                var priorSensitizationInDb = await context.PriorSensitizationRepository.GetPriorSensitizationByName(priorSensitization.Description);

                if (priorSensitizationInDb != null)
                    if (priorSensitizationInDb.Oid != priorSensitization.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPriorSensitizationDuplicate", "PriorSensitizationController.cs", ex.Message);
                throw;
            }
        }
    }
}