using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by: Sphinx(2)
 * Date created: 13.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 07.11.2022
 */

namespace Api.Controllers
{
    /// <summary>
    /// VisitType controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VisitTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VisitTypeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VisitTypeController(IUnitOfWork context, ILogger<VisitTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/visit-type
        /// </summary>
        /// <param name="visitType">VisitType object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVisitType)]
        public async Task<IActionResult> CreateVisitType(VisitType visitType)
        {
            try
            {
                if (await IsVisitTypeDuplicate(visitType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                visitType.DateCreated = DateTime.Now;
                visitType.IsDeleted = false;
                visitType.IsSynced = false;

                context.VisitTypeRepository.Add(visitType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVisitTypeByKey", new { key = visitType.Oid }, visitType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVisitType", "VisitTypeController.cs", ex.Message, visitType.CreatedIn, visitType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitTypes)]
        public async Task<IActionResult> ReadVisitTypes()
        {
            try
            {
                var visitType = await context.VisitTypeRepository.GetVisitTypes();
                visitType = visitType.OrderByDescending(x => x.DateCreated);
                return Ok(visitType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitTypes", "VisitTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitTypeByKey)]
        public async Task<IActionResult> ReadVisitTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitType = await context.VisitTypeRepository.GetVisitTypeByKey(key);

                if (visitType == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(visitType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitTypeByKey", "VisitTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitTypes.</param>
        /// <param name="visitType">VisitType to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVisitType)]
        public async Task<IActionResult> UpdateVisitType(int key, VisitType visitType)
        {
            try
            {
                if (key != visitType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsVisitTypeDuplicate(visitType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                visitType.DateModified = DateTime.Now;
                visitType.IsDeleted = false;
                visitType.IsSynced = false;

                context.VisitTypeRepository.Update(visitType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVisitType", "VisitTypeController.cs", ex.Message, visitType.ModifiedIn, visitType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table.</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVisitType)]
        public async Task<IActionResult> DeleteVisitType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitTypeInDb = await context.VisitTypeRepository.GetVisitTypeByKey(key);

                if (visitTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                visitTypeInDb.DateModified = DateTime.Now;
                visitTypeInDb.IsDeleted = true;
                visitTypeInDb.IsSynced = false;

                context.VisitTypeRepository.Update(visitTypeInDb);
                await context.SaveChangesAsync();

                return Ok(visitTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVisitType", "VisitTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the visit type is duplicate or not.
        /// </summary>
        /// <param name="visitType">VisitType object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsVisitTypeDuplicate(VisitType visitType)
        {
            try
            {
                var visitTypeInDb = await context.VisitTypeRepository.GetVisitTypeByType(visitType.Description);

                if (visitTypeInDb != null)
                    if (visitTypeInDb.Oid != visitType.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsVisitTypeDuplicate", "VisitTypeController.cs", ex.Message);
                throw;
            }
        }
    }
}