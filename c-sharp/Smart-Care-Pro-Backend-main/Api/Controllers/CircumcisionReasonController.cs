using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 08.04.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// CircumcisionReason Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CircumcisionReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CircumcisionReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CircumcisionReasonController(IUnitOfWork context, ILogger<CircumcisionReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/circumcision-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCircumcisionReasons)]
        public async Task<IActionResult> ReadCircumcisionReasons()
        {
            try
            {
                var circumcisionReasonInDb = await context.CircumcisionReasonRepository.GetCircumcisionReasons();

                circumcisionReasonInDb = circumcisionReasonInDb.OrderByDescending(x => x.DateCreated);

                return Ok(circumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCircumcisionReasons", "CircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/circumcision-reasons
        /// </summary>
        /// <param name="circumcisionReason">CircumcisionReason object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCircumcisionReason)]
        public async Task<IActionResult> CreateCircumcisionReason(CircumcisionReason circumcisionReason)
        {
            try
            {
                if (await IsCircumcisionReasonDuplicate(circumcisionReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                circumcisionReason.DateCreated = DateTime.Now;
                circumcisionReason.IsDeleted = false;
                circumcisionReason.IsSynced = false;

                context.CircumcisionReasonRepository.Add(circumcisionReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCircumcisionReasonByKey", new { key = circumcisionReason.Oid }, circumcisionReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCircumcisionReason", "CircumcisionReasonController.cs", ex.Message, circumcisionReason.CreatedIn, circumcisionReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-class/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CircumcisionReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCircumcisionReasonByKey)]
        public async Task<IActionResult> ReadCircumcisionReasonByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var circumcisionReasonInDb = await context.CircumcisionReasonRepository.GetCircumcisionReasonByKey(key);

                if (circumcisionReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(circumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCircumcisionReasonByKey", "CircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/art-drug-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CircumcisionReasones.</param>
        /// <param name="circumcisionReason">CircumcisionReason to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCircumcisionReason)]
        public async Task<IActionResult> UpdateCircumcisionReason(int key, CircumcisionReason circumcisionReason)
        {
            try
            {
                if (key != circumcisionReason.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCircumcisionReasonDuplicate(circumcisionReason) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                circumcisionReason.DateModified = DateTime.Now;
                circumcisionReason.IsDeleted = false;
                circumcisionReason.IsSynced = false;

                context.CircumcisionReasonRepository.Update(circumcisionReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCircumcisionReason", "CircumcisionReasonController.cs", ex.Message, circumcisionReason.ModifiedIn, circumcisionReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCircumcisionReason)]
        public async Task<IActionResult> DeleteCircumcisionReason(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var circumcisionReasonInDb = await context.CircumcisionReasonRepository.GetCircumcisionReasonByKey(key);

                if (circumcisionReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                circumcisionReasonInDb.DateModified = DateTime.Now;
                circumcisionReasonInDb.IsDeleted = true;
                circumcisionReasonInDb.IsSynced = false;

                context.CircumcisionReasonRepository.Update(circumcisionReasonInDb);
                await context.SaveChangesAsync();

                return Ok(circumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCircumcisionReason", "CircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the CircumcisionReason name is duplicate or not.
        /// </summary>
        /// <param name="CircumcisionReason">CircumcisionReason object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCircumcisionReasonDuplicate(CircumcisionReason circumcisionReason)
        {
            try
            {
                var circumcisionReasonInDb = await context.CircumcisionReasonRepository.GetCircumcisionReasonByName(circumcisionReason.Description);

                if (circumcisionReasonInDb != null)
                    if (circumcisionReasonInDb.Oid != circumcisionReason.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCircumcisionReasonDuplicate", "CircumcisionReasonController.cs", ex.Message);
                throw;
            }
        }
    }
}