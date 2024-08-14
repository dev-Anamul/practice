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
    /// Drug Dosage Interval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugDosageUnitController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugDosageUnitController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugDosageUnitController(IUnitOfWork context, ILogger<DrugDosageUnitController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugDosage
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugDosages)]
        public async Task<IActionResult> ReadDrugDosageUnit()
        {
            try
            {
                var drugDosageInDb = await context.DrugDosageUnitRepository.GetDrugDosageUnit();

                return Ok(drugDosageInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugDosageUnit", "DrugDosageUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugDosage/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugDosage By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugDosageByKey)]
        public async Task<IActionResult> ReadDrugDosageUnitByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugDosageInDb = await context.DrugDosageUnitRepository.GetDrugDosageUnitByKey(key);

                if (drugDosageInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugDosageInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugDosageUnitByKey", "DrugDosageUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugDosage
        /// </summary>
        /// <param name="drugDosageUnit">DrugDosageUnit object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugDosage)]
        public async Task<ActionResult<DrugDosageUnit>> CreateDrugDosageUnit(DrugDosageUnit drugDosageUnit)
        {
            try
            {
                if (await IsDrugDosageUnitDuplicate(drugDosageUnit) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugDosageUnit.DateCreated = DateTime.Now;
                drugDosageUnit.IsDeleted = false;
                drugDosageUnit.IsSynced = false;

                context.DrugDosageUnitRepository.Add(drugDosageUnit);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDrugDosageUnitByKey", new { key = drugDosageUnit.Oid }, drugDosageUnit);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugDosageUnit", "DrugDosageUnitController.cs", ex.Message, drugDosageUnit.CreatedIn, drugDosageUnit.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugDosage/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugDosageUnits.</param>
        /// <param name="drugDosageUnit">DrugDosageUnit to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugDosage)]
        public async Task<IActionResult> UpdateDrugDosageUnit(int key, DrugDosageUnit drugDosageUnit)
        {
            try
            {
                if (key != drugDosageUnit.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                drugDosageUnit.DateModified = DateTime.Now;
                drugDosageUnit.IsDeleted = false;
                drugDosageUnit.IsSynced = false;

                context.DrugDosageUnitRepository.Update(drugDosageUnit);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugDosageUnit", "DrugDosageUnitController.cs", ex.Message, drugDosageUnit.ModifiedIn, drugDosageUnit.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugDosage/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugDosageUnits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugDosage)]
        public async Task<IActionResult> DeleteDrugDosageUnit(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugDosageUnitInDb = await context.DrugDosageUnitRepository.GetDrugDosageUnitByKey(key);

                if (drugDosageUnitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugDosageUnitInDb.DateModified = DateTime.Now;
                drugDosageUnitInDb.IsDeleted = true;
                drugDosageUnitInDb.IsSynced = false;

                context.DrugDosageUnitRepository.Update(drugDosageUnitInDb);
                await context.SaveChangesAsync();

                return Ok(drugDosageUnitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugDosageUnit", "DrugDosageUnitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the DrugDosageUnit name is duplicate or not.
        /// </summary>
        /// <param name="DrugDosageUnit">DrugDosageUnit object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDrugDosageUnitDuplicate(DrugDosageUnit drugDosageUnit)
        {
            try
            {
                var drugDosageUnitInDb = await context.DrugDosageUnitRepository.GetDrugDosageUnitByName(drugDosageUnit.Description);

                if (drugDosageUnitInDb != null)
                    if (drugDosageUnitInDb.Oid != drugDosageUnit.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDrugDosageUnitDuplicate", "DrugDosageUnitController.cs", ex.Message);
                throw;
            }
        }
    }
}