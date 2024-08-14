using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Brian
 * Last modified: 02.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PreferredFeeding controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PreferredFeedingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PreferredFeedingController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PreferredFeedingController(IUnitOfWork context, ILogger<PreferredFeedingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/preferred-feeding
        /// </summary>
        /// <param name="preferredFeeding">PreferredFeeding object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePreferredFeeding)]
        public async Task<ActionResult<PreferredFeeding>> CreatePreferredFeeding(PreferredFeeding preferredFeeding)
        {
            try
            {
                if (await IsPreferredFeedingDuplicate(preferredFeeding) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                preferredFeeding.DateCreated = DateTime.Now;
                preferredFeeding.IsDeleted = false;
                preferredFeeding.IsSynced = false;

                context.PreferredFeedingRepository.Add(preferredFeeding);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPreferredFeedingByKey", new { key = preferredFeeding.Oid }, preferredFeeding);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePreferredFeeding", "PreferredFeedingController.cs", ex.Message, preferredFeeding.CreatedIn, preferredFeeding.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/preferred-feedings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreferredFeedings)]
        public async Task<IActionResult> ReadPreferredFeedings()
        {
            try
            {
                var preferredFeedingInDb = await context.PreferredFeedingRepository.GetPreferredFeedings();

                return Ok(preferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreferredFeedings", "PreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/preferred-feeding/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreferredFeedingByKey)]
        public async Task<IActionResult> ReadPreferredFeedingByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var preferredFeedingIndb = await context.PreferredFeedingRepository.GetPreferredFeedingByKey(key);

                if (preferredFeedingIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(preferredFeedingIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreferredFeedingByKey", "PreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/preferred-feeding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreferredFeedings.</param>
        /// <param name="preferredFeeding">PreferredFeeding to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePreferredFeeding)]
        public async Task<IActionResult> UpdatePreferredFeeding(int key, PreferredFeeding preferredFeeding)
        {
            try
            {
                if (key != preferredFeeding.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                preferredFeeding.DateModified = DateTime.Now;
                preferredFeeding.IsDeleted = false;
                preferredFeeding.IsSynced = false;

                context.PreferredFeedingRepository.Update(preferredFeeding);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePreferredFeeding", "PreferredFeedingController.cs", ex.Message, preferredFeeding.ModifiedIn, preferredFeeding.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/preferred-feeding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePreferredFeeding)]
        public async Task<IActionResult> DeletePreferredFeeding(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var preferredFeedingInDb = await context.PreferredFeedingRepository.GetPreferredFeedingByKey(key);

                if (preferredFeedingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                preferredFeedingInDb.DateModified = DateTime.Now;
                preferredFeedingInDb.IsDeleted = true;
                preferredFeedingInDb.IsSynced = false;

                context.PreferredFeedingRepository.Update(preferredFeedingInDb);
                await context.SaveChangesAsync();

                return Ok(preferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePreferredFeeding", "PreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PreferredFeeding name is duplicate or not.
        /// </summary>
        /// <param name="PreferredFeeding">PreferredFeeding object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPreferredFeedingDuplicate(PreferredFeeding preferredFeeding)
        {
            try
            {
                var preferredFeedingInDb = await context.PreferredFeedingRepository.GetPreferredFeedingByName(preferredFeeding.Description);

                if (preferredFeedingInDb != null)
                    if (preferredFeedingInDb.Oid != preferredFeeding.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPreferredFeedingDuplicate", "PreferredFeedingController.cs", ex.Message);
                throw;
            }
        }
    }
}