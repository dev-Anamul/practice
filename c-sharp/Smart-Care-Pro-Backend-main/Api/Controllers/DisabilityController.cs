using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DisabilityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DisabilityController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DisabilityController(IUnitOfWork context, ILogger<DisabilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/disabilities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDisabilities)]
        public async Task<IActionResult> ReadDisabilities()
        {
            try
            {
                var disabilityIndb = await context.DisabilityRepository.GetDisabilities();

                return Ok(disabilityIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDisabilities", "DisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/disability
        /// </summary>
        /// <param name="disability">Disability object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDisability)]
        public async Task<ActionResult<Disability>> CreateDisability(Disability disability)
        {
            try
            {
                if (await IsDisabilityDuplicate(disability) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                disability.DateCreated = DateTime.Now;
                disability.IsDeleted = false;
                disability.IsSynced = false;

                context.DisabilityRepository.Add(disability);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDisabilityByKey", new { key = disability.Oid }, disability);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDisability", "DisabilityController.cs", ex.Message, disability.CreatedIn, disability.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/disability/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Disability.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDisabilityByKey)]
        public async Task<IActionResult> ReadDisabilityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var disabilityInDb = await context.DisabilityRepository.GetDisabilityByKey(key);

                if (disabilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(disabilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDisabilityByKey", "DisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/disability/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Disabilities.</param>
        /// <param name="disability">Disability to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDisability)]
        public async Task<IActionResult> UpdateDisability(int key, Disability disability)
        {
            try
            {
                if (key != disability.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                disability.DateModified = DateTime.Now;
                disability.IsDeleted = false;
                disability.IsSynced = false;

                context.DisabilityRepository.Update(disability);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDisability", "DisabilityController.cs", ex.Message, disability.ModifiedIn, disability.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/disability/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Disabilities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDisability)]
        public async Task<IActionResult> DeleteDisability(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var disabilityInDb = await context.DisabilityRepository.GetDisabilityByKey(key);

                if (disabilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                disabilityInDb.DateModified = DateTime.Now;
                disabilityInDb.IsDeleted = true;
                disabilityInDb.IsSynced = false;

                context.DisabilityRepository.Update(disabilityInDb);
                await context.SaveChangesAsync();

                return Ok(disabilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDisability", "DisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the Disability name is duplicate or not.
        /// </summary>
        /// <param name="Disability">Disability object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDisabilityDuplicate(Disability disability)
        {
            try
            {
                var disabilityInDb = await context.DisabilityRepository.GetDisabilityByName(disability.Description);

                if (disabilityInDb != null)
                    if (disabilityInDb.Oid != disability.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDisabilityDuplicate", "DisabilityController.cs", ex.Message);
                throw;
            }
        }
    }
}