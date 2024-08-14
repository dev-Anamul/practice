using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Stephan
 * Last modified: 06.11.2022
 */

namespace Api.Controllers
{
    /// <summary>
    /// EducationLevel controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class EducationLevelController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<EducationLevelController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public EducationLevelController(IUnitOfWork context, ILogger<EducationLevelController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/education-level
        /// </summary>
        /// <param name="educationLevel">EducationLevel object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEducationLevel)]
        public async Task<IActionResult> CreateEducationLevel(EducationLevel educationLevel)
        {
            try
            {
                if (await IsEducationLevelDuplicate(educationLevel) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                educationLevel.DateCreated = DateTime.Now;
                educationLevel.IsDeleted = false;
                educationLevel.IsSynced = false;

                context.EducationLevelRepository.Add(educationLevel);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadEducationLevelByKey", new { key = educationLevel.Oid }, educationLevel);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateEducationLevel", "EducationLevelController.cs", ex.Message, educationLevel.CreatedIn, educationLevel.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/education-levels
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEducationLevels)]
        public async Task<IActionResult> ReadEducationLevels()
        {
            try
            {
                var educationLevelIndb = await context.EducationLevelRepository.GetEducationLevels();

                return Ok(educationLevelIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEducationLevels", "EducationLevelController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/education-level/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EducationLevels.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEducationLevelByKey)]
        public async Task<IActionResult> ReadEducationLevelByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var educationLevelIndb = await context.EducationLevelRepository.GetEducationLevelByKey(key);

                if (educationLevelIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(educationLevelIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEducationLevelByKey", "EducationLevelController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/education-level/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EducationLevels.</param>
        /// <param name="educationLevel">EducationLevel to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateEducationLevel)]
        public async Task<IActionResult> UpdateEducationLevel(int key, EducationLevel educationLevel)
        {
            try
            {
                if (key != educationLevel.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsEducationLevelDuplicate(educationLevel) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                educationLevel.DateModified = DateTime.Now;
                educationLevel.IsDeleted = false;
                educationLevel.IsSynced = false;

                context.EducationLevelRepository.Update(educationLevel);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateEducationLevel", "EducationLevelController.cs", ex.Message, educationLevel.ModifiedIn, educationLevel.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/education-level/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EducationLevels.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEducationLevel)]
        public async Task<IActionResult> DeleteEducationLevel(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var educationLevelInDb = await context.EducationLevelRepository.GetEducationLevelByKey(key);

                if (educationLevelInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                educationLevelInDb.DateModified = DateTime.Now;
                educationLevelInDb.IsDeleted = true;
                educationLevelInDb.IsSynced = false;

                context.EducationLevelRepository.Update(educationLevelInDb);
                await context.SaveChangesAsync();

                return Ok(educationLevelInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteEducationLevel", "EducationLevelController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the education level is duplicate or not.
        /// </summary>
        /// <param name="educationLevel">EducationLevel object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsEducationLevelDuplicate(EducationLevel educationLevel)
        {
            try
            {
                var educationLevelInDb = await context.EducationLevelRepository.GetEducationLevelByName(educationLevel.Description);

                if (educationLevelInDb != null)
                    if (educationLevelInDb.Oid != educationLevel.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsEducationLevelDuplicate", "EducationLevelController.cs", ex.Message);
                throw;
            }
        }
    }
}