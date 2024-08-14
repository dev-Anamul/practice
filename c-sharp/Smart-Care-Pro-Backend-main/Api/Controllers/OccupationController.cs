using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Occupation controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class OccupationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<OccupationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public OccupationController(IUnitOfWork context, ILogger<OccupationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/occupation
        /// </summary>
        /// <param name="occupation">Occupation object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateOccupation)]
        public async Task<IActionResult> CreateOccupation(Occupation occupation)
        {
            try
            {
                if (await IsOccupationDuplicate(occupation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                occupation.DateCreated = DateTime.Now;
                occupation.IsDeleted = false;
                occupation.IsSynced = false;

                context.OccupationRepository.Add(occupation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadOccupationByKey", new { key = occupation.Oid }, occupation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateOccupation", "OccupationController.cs", ex.Message, occupation.CreatedIn, occupation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/occupations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOccupations)]
        public async Task<IActionResult> ReadOccupations()
        {
            try
            {
                var occupationInDb = await context.OccupationRepository.GetOccupations();

                return Ok(occupationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOccupations", "OccupationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/occupation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Occupations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOccupationByKey)]
        public async Task<IActionResult> ReadOccupationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var occupationInDb = await context.OccupationRepository.GetOccupationByKey(key);

                if (occupationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(occupationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOccupationByKey", "OccupationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/occupation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Occupations.</param>
        /// <param name="occupation">Occupation to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateOccupation)]
        public async Task<IActionResult> UpdateOccupation(int key, Occupation occupation)
        {
            try
            {
                if (key != occupation.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsOccupationDuplicate(occupation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                occupation.DateModified = DateTime.Now;
                occupation.IsDeleted = false;
                occupation.IsSynced = false;

                context.OccupationRepository.Update(occupation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateOccupation", "OccupationController.cs", ex.Message, occupation.ModifiedIn, occupation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/occupation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Occupations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteOccupation)]
        public async Task<IActionResult> DeleteOccupation(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var occupationInDb = await context.OccupationRepository.GetOccupationByKey(key);

                if (occupationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
                occupationInDb.DateModified = DateTime.Now;
                occupationInDb.IsDeleted = true;
                occupationInDb.IsSynced = false;

                context.OccupationRepository.Update(occupationInDb);
                await context.SaveChangesAsync();

                return Ok(occupationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteOccupation", "OccupationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the occupation name is duplicate or not.
        /// </summary>
        /// <param name="occupation">Occupation object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsOccupationDuplicate(Occupation occupation)
        {
            try
            {
                var occupationInDb = await context.OccupationRepository.GetOccupationByName(occupation.Description);

                if (occupationInDb != null)
                    if (occupationInDb.Oid != occupation.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsOccupationDuplicate", "OccupationController.cs", ex.Message);
                throw;
            }
        }
    }
}