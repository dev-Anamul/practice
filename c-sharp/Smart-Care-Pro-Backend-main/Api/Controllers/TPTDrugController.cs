using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// TPTDrug controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TPTDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TPTDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TPTDrugController(IUnitOfWork context, ILogger<TPTDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tpt-drug
        /// </summary>
        /// <param name="tptDrug">TPTDrug object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTPTDrug)]
        public async Task<IActionResult> CreateTPTDrug(TPTDrug tptDrug)
        {
            try
            {
                if (await IsTPTDrugDuplicate(tptDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                tptDrug.DateCreated = DateTime.Now;
                tptDrug.IsDeleted = false;
                tptDrug.IsSynced = false;

                context.TPTDrugRepository.Add(tptDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTPTDrugByKey", new { key = tptDrug.Oid }, tptDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTPTDrug", "TPTDrugController.cs", ex.Message, tptDrug.CreatedIn, tptDrug.CreatedBy);

                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTPTDrugs)]
        public async Task<IActionResult> ReadTPTDrugs()
        {
            try
            {
                var tptDrugInDb = await context.TPTDrugRepository.GetTPTDrugs();

                return Ok(tptDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTPTDrugs", "TPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTPTDrugByKey)]
        public async Task<IActionResult> ReadTPTDrugByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tptDrugInDb = await context.TPTDrugRepository.GetTPTDrugByKey(key);

                if (tptDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tptDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTPTDrugByKey", "TPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTDrugs.</param>
        /// <param name="tptDrug">TPTDrug to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTPTDrug)]
        public async Task<IActionResult> UpdateTPTDrug(int key, TPTDrug tptDrug)
        {
            try
            {
                if (key != tptDrug.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsTPTDrugDuplicate(tptDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                tptDrug.DateModified = DateTime.Now;
                tptDrug.IsDeleted = false;
                tptDrug.IsSynced = false;

                context.TPTDrugRepository.Update(tptDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTPTDrug", "TPTDrugController.cs", ex.Message, tptDrug.ModifiedIn, tptDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTPTDrug)]
        public async Task<IActionResult> DeleteTPTDrug(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tptDrugInDb = await context.TPTDrugRepository.GetTPTDrugByKey(key);

                if (tptDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tptDrugInDb.DateModified = DateTime.Now;
                tptDrugInDb.IsDeleted = true;
                tptDrugInDb.IsSynced = false;

                context.TPTDrugRepository.Update(tptDrugInDb);
                await context.SaveChangesAsync();

                return Ok(tptDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTPTDrug", "TPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the drug name is duplicate or not.
        /// </summary>
        /// <param name="tptDrug">TPTDrug object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTPTDrugDuplicate(TPTDrug tptDrug)
        {
            try
            {
                var tptDrugInDb = await context.TPTDrugRepository.GetTPTDrugByName(tptDrug.Description);

                if (tptDrugInDb != null)
                    if (tptDrugInDb.Oid != tptDrug.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsTPTDrugDuplicate", "TPTDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}