using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.03.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Drug Utility Interval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugUtilityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugUtilityController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugUtilityController(IUnitOfWork context, ILogger<DrugUtilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugutility
        /// </summary>
        /// <param name="drugUtility">DrugUtility object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugUtility)]
        public async Task<ActionResult<DrugUtility>> CreateDrugUtility(DrugUtility drugUtility)
        {
            try
            {
                if (await IsDrugUtilityDuplicate(drugUtility) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugUtility.DateCreated = DateTime.Now;
                drugUtility.IsDeleted = false;
                drugUtility.IsSynced = false;

                context.DrugUtilityRepository.Add(drugUtility);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDrugUtilityByKey", new { key = drugUtility.Oid }, drugUtility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugUtility", "DrugUtilityController.cs", ex.Message, drugUtility.CreatedIn, drugUtility.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugutility
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugUtilities)]
        public async Task<IActionResult> ReadDrugUtility()
        {
            try
            {
                var drugUtilityInDb = await context.DrugUtilityRepository.GetDrugUtility();

                return Ok(drugUtilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugUtility", "DrugUtilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugutility/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReadDrugUtility By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugUtilityByKey)]
        public async Task<IActionResult> ReadDrugUtilityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugUtilityInDb = await context.DrugUtilityRepository.GetDrugUtilityByKey(key);

                if (drugUtilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugUtilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugUtilityByKey", "DrugUtilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugutility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugUtilities.</param>
        /// <param name="drugUtility">DrugUtility to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugUtility)]
        public async Task<IActionResult> UpdateDrugUtility(int key, DrugUtility drugUtility)
        {
            try
            {
                if (key != drugUtility.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                drugUtility.DateModified = DateTime.Now;
                drugUtility.IsDeleted = false;
                drugUtility.IsSynced = false;

                context.DrugUtilityRepository.Update(drugUtility);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugUtility", "DrugUtilityController.cs", ex.Message, drugUtility.ModifiedIn, drugUtility.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugutility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugUtilities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugUtility)]
        public async Task<IActionResult> DeleteDrugUtility(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugUtilityInDb = await context.DrugUtilityRepository.GetDrugUtilityByKey(key);

                if (drugUtilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugUtilityInDb.DateModified = DateTime.Now;
                drugUtilityInDb.IsDeleted = true;
                drugUtilityInDb.IsSynced = false;

                context.DrugUtilityRepository.Update(drugUtilityInDb);
                await context.SaveChangesAsync();

                return Ok(drugUtilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugUtility", "DrugUtilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the DrugUtility name is duplicate or not.
        /// </summary>
        /// <param name="DrugUtility">DrugUtility object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDrugUtilityDuplicate(DrugUtility drugUtility)
        {
            try
            {
                var drugUtilityInDb = await context.DrugUtilityRepository.GetDrugUtilityByName(drugUtility.Description);

                if (drugUtilityInDb != null)
                    if (drugUtilityInDb.Oid != drugUtility.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDrugUtilityDuplicate", "DrugUtilityController.cs", ex.Message);
                throw;
            }
        }
    }
}