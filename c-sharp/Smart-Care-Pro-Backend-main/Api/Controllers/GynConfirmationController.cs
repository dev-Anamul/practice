using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 13.02.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class GynConfirmationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GynConfirmationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GynConfirmationController(IUnitOfWork context, ILogger<GynConfirmationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/gyn-confirmation
        /// </summary>
        /// <param name="gynConfirmation">GynConfirmation object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGynConfirmation)]
        public async Task<ActionResult<GynConfirmation>> CreateGynConfirmation(GynConfirmation gynConfirmation)
        {
            try
            {
                if (await IsGynConfirmationDuplicate(gynConfirmation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                gynConfirmation.DateCreated = DateTime.Now;
                gynConfirmation.IsDeleted = false;
                gynConfirmation.IsSynced = false;

                context.GynConfirmationRepository.Add(gynConfirmation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadGynConfirmationByKey", new { key = gynConfirmation.Oid }, gynConfirmation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGynConfirmation", "GynConfirmationController.cs", ex.Message, gynConfirmation.CreatedIn, gynConfirmation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-confirmations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynConfirmations)]
        public async Task<IActionResult> ReadGynConfirmations()
        {
            try
            {
                var gynConfirmations = await context.GynConfirmationRepository.GetGynConfirmation();

                return Ok(gynConfirmations);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynConfirmations", "GynConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-confirmation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynConfirmation.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynConfirmationByKey)]
        public async Task<IActionResult> ReadGynConfirmationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynConfirmationInDb = await context.GynConfirmationRepository.GetGynConfirmationByKey(key);

                if (gynConfirmationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(gynConfirmationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynConfirmationByKey", "GynConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-confirmation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynConfirmations.</param>
        /// <param name="gynConfirmation">GynConfirmation to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGynConfirmation)]
        public async Task<IActionResult> UpdateGynConfirmation(int key, GynConfirmation gynConfirmation)
        {
            try
            {
                if (key != gynConfirmation.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                gynConfirmation.DateModified = DateTime.Now;
                gynConfirmation.IsDeleted = false;
                gynConfirmation.IsSynced = false;

                context.GynConfirmationRepository.Update(gynConfirmation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGynConfirmation", "GynConfirmationController.cs", ex.Message, gynConfirmation.ModifiedIn, gynConfirmation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-confirmation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynConfirmations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGynConfirmation)]
        public async Task<IActionResult> DeleteGynConfirmation(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynConfirmationInDb = await context.GynConfirmationRepository.GetGynConfirmationByKey(key);

                if (gynConfirmationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                gynConfirmationInDb.DateModified = DateTime.Now;
                gynConfirmationInDb.IsDeleted = true;
                gynConfirmationInDb.IsSynced = false;

                context.GynConfirmationRepository.Update(gynConfirmationInDb);
                await context.SaveChangesAsync();

                return Ok(gynConfirmationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGynConfirmation", "GynConfirmationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the GynConfirmation name is duplicate or not.
        /// </summary>
        /// <param name="gynConfirmation">GynConfirmation object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsGynConfirmationDuplicate(GynConfirmation gynConfirmation)
        {
            try
            {
                var gynConfirmationInDb = await context.GynConfirmationRepository.GetGynConfirmationByName(gynConfirmation.Description);

                if (gynConfirmationInDb != null)
                    if (gynConfirmationInDb.Oid != gynConfirmation.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsGynConfirmationDuplicate", "GynConfirmationController.cs", ex.Message);
                throw;
            }
        }
    }
}