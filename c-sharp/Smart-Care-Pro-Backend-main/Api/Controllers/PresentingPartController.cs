using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PresentingPart Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PresentingPartController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PresentingPartController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PresentingPartController(IUnitOfWork context, ILogger<PresentingPartController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/presenting-part
        /// </summary>
        /// <param name="presentingPart">PresentingPart object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePresentingPart)]
        public async Task<ActionResult<PresentingPart>> CreatePresentingPart(PresentingPart presentingPart)
        {
            try
            {
                if (await IsPresentingPartDuplicate(presentingPart) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                presentingPart.DateCreated = DateTime.Now;
                presentingPart.IsDeleted = false;
                presentingPart.IsSynced = false;

                context.PresentingPartRepository.Add(presentingPart);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPresentingPartByKey", new { key = presentingPart.Oid }, presentingPart);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePresentingPart", "PresentingPartController.cs", ex.Message, presentingPart.CreatedIn, presentingPart.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/presenting-parts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPresentingParts)]
        public async Task<IActionResult> ReadPresentingParts()
        {
            try
            {
                var presentingPartInDb = await context.PresentingPartRepository.GetPresentingParts();

                return Ok(presentingPartInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPresentingParts", "PresentingPartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/presenting-part/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PresentingParts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPresentingPartByKey)]
        public async Task<IActionResult> ReadPresentingPartByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var presentingPartIndb = await context.PresentingPartRepository.GetPresentingPartByKey(key);

                if (presentingPartIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(presentingPartIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPresentingPartByKey", "PresentingPartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/presenting-part/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PresentingParts.</param>
        /// <param name="presentingPart">PresentingPart to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePresentingPart)]
        public async Task<IActionResult> UpdatePresentingPart(int key, PresentingPart presentingPart)
        {
            try
            {
                if (key != presentingPart.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                presentingPart.DateModified = DateTime.Now;
                presentingPart.IsDeleted = false;
                presentingPart.IsSynced = false;

                context.PresentingPartRepository.Update(presentingPart);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePresentingPart", "PresentingPartController.cs", ex.Message, presentingPart.ModifiedIn, presentingPart.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/presenting-part/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PresentingParts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePresentingPart)]
        public async Task<IActionResult> DeletePresentingPart(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var presentingPartInDb = await context.PresentingPartRepository.GetPresentingPartByKey(key);

                if (presentingPartInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                presentingPartInDb.DateModified = DateTime.Now;
                presentingPartInDb.IsDeleted = true;
                presentingPartInDb.IsSynced = false;

                context.PresentingPartRepository.Update(presentingPartInDb);
                await context.SaveChangesAsync();

                return Ok(presentingPartInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePresentingPart", "PresentingPartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PresentingPart name is duplicate or not.
        /// </summary>
        /// <param name="PresentingPart">PresentingPart object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPresentingPartDuplicate(PresentingPart presentingPart)
        {
            try
            {
                var presentingPartInDb = await context.PresentingPartRepository.GetPresentingPartByName(presentingPart.Description);

                if (presentingPartInDb != null)
                    if (presentingPartInDb.Oid != presentingPart.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPresentingPartDuplicate", "PresentingPartController.cs", ex.Message);
                throw;
            }
        }
    }
}