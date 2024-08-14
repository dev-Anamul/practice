using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 06.04.2023
 * Modified by   : Brian
 * Last modified : 06.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// TBFinding controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBFindingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBFindingController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBFindingController(IUnitOfWork context, ILogger<TBFindingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tB-Finding
        /// </summary>
        /// <param name="tBFinding">TBFinding object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTBFinding)]
        public async Task<ActionResult<TBFinding>> CreateTBFinding(TBFinding tBFinding)
        {
            try
            {
                if (await IsTBFindingDuplicate(tBFinding) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                tBFinding.DateCreated = DateTime.Now;
                tBFinding.IsDeleted = false;
                tBFinding.IsSynced = false;

                context.TBFindingRepository.Add(tBFinding);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTBFindingByKey", new { key = tBFinding.Oid }, tBFinding);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTBFinding", "TBFindingController.cs", ex.Message, tBFinding.CreatedIn, tBFinding.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tB-Findings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBFindings)]
        public async Task<IActionResult> ReadTBFindings()
        {
            try
            {
                var tBFindingInDb = await context.TBFindingRepository.GetTBFindings();

                return Ok(tBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBFindings", "TBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tB-Finding/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBFinding.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBFindingByKey)]
        public async Task<IActionResult> ReadTBFindingByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBFindingInDb = await context.TBFindingRepository.GetTBFindingByKey(key);

                if (tBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBFindingByKey", "TBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tB-Finding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBFindings.</param>
        /// <param name="TBFinding">TBFinding to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTBFinding)]
        public async Task<IActionResult> UpdateTBFinding(int key, TBFinding tBFinding)
        {
            try
            {
                if (key != tBFinding.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tBFinding.DateModified = DateTime.Now;
                tBFinding.IsDeleted = false;
                tBFinding.IsSynced = false;

                context.TBFindingRepository.Update(tBFinding);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTBFinding", "TBFindingController.cs", ex.Message, tBFinding.ModifiedIn, tBFinding.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tB-Finding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBFindings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTBFinding)]
        public async Task<IActionResult> DeleteTBFinding(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBFindingInDb = await context.TBFindingRepository.GetTBFindingByKey(key);

                if (tBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tBFindingInDb.DateModified = DateTime.Now;
                tBFindingInDb.IsDeleted = true;
                tBFindingInDb.IsSynced = false;

                context.TBFindingRepository.Update(tBFindingInDb);
                await context.SaveChangesAsync();

                return Ok(tBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBFinding", "TBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the TBFinding name is duplicate or not.
        /// </summary>
        /// <param name="tBFinding">TBFinding object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTBFindingDuplicate(TBFinding tBFinding)
        {
            try
            {
                var tBFindingInDb = await context.TBFindingRepository.GetTBFindingByName(tBFinding.Description);

                if (tBFindingInDb != null)
                    if (tBFindingInDb.Oid != tBFinding.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsTBFindingDuplicate", "TBFindingController.cs", ex.Message);
                throw;
            }
        }
    }
}