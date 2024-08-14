using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 01.04.2022
 * Modified by   : Brian
 * Last modified : 06.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// TBDrug controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBDrugController(IUnitOfWork context, ILogger<TBDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-drug
        /// </summary>
        /// <param name="tBDrug">TBDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTBDrug)]
        public async Task<ActionResult<TBDrug>> CreateTBDrug(TBDrug tBDrug)
        {
            try
            {
                if (await IsTBDrugDuplicate(tBDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                tBDrug.DateCreated = DateTime.Now;
                tBDrug.IsDeleted = false;
                tBDrug.IsSynced = false;

                context.TBDrugRepository.Add(tBDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTBDrugByKey", new { key = tBDrug.Oid }, tBDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTBDrug", "TBDrugController.cs", ex.Message, tBDrug.CreatedIn, tBDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBDrugs)]
        public async Task<IActionResult> ReadTBDrugs()
        {
            try
            {
                var tbDrugInDb = await context.TBDrugRepository.GetTBDrugs();

                return Ok(tbDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBDrugs", "TBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBDrug.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBDrugByKey)]
        public async Task<IActionResult> ReadTBDrugByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBDrugInDb = await context.TBDrugRepository.GetTBDrugByKey(key);

                if (tBDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tBDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBDrugByKey", "TBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBDrugs.</param>
        /// <param name="TBDrug">TBDrug to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTBDrug)]
        public async Task<IActionResult> UpdateTBDrug(int key, TBDrug tBDrug)
        {
            try
            {
                if (key != tBDrug.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tBDrug.DateModified = DateTime.Now;
                tBDrug.IsDeleted = false;
                tBDrug.IsSynced = false;

                context.TBDrugRepository.Update(tBDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTBDrug", "TBDrugController.cs", ex.Message, tBDrug.ModifiedIn, tBDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTBDrug)]
        public async Task<IActionResult> DeleteTBDrug(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBDrugInDb = await context.TBDrugRepository.GetTBDrugByKey(key);

                if (tBDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tBDrugInDb.DateModified = DateTime.Now;
                tBDrugInDb.IsDeleted = true;
                tBDrugInDb.IsSynced = false;

                context.TBDrugRepository.Update(tBDrugInDb);
                await context.SaveChangesAsync();

                return Ok(tBDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBDrug", "TBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the TBDrug name is duplicate or not.
        /// </summary>
        /// <param name="TBDrug">TBDrug object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTBDrugDuplicate(TBDrug tBDrug)
        {
            try
            {
                var tBDrugInDb = await context.TBDrugRepository.GetTBDrugByName(tBDrug.Description);

                if (tBDrugInDb != null)
                    if (tBDrugInDb.Oid != tBDrug.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsTBDrugDuplicate", "TBDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}