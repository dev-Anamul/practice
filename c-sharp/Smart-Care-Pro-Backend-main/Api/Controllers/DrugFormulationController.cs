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
    /// Drug Formulation Interval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugFormulationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugFormulationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugFormulationController(IUnitOfWork context, ILogger<DrugFormulationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugFormulation
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugFormulations)]
        public async Task<IActionResult> ReadDrugFormulation()
        {
            try
            {
                var drugFormulationInDb = await context.DrugFormulationRepository.GetDrugFormulation();

                return Ok(drugFormulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugFormulation", "DrugFormulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugFormulation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugFormulation By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugFormulationByKey)]
        public async Task<IActionResult> ReadDrugFormulationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugFormulationInDb = await context.DrugFormulationRepository.GetDrugFormulationByKey(key);

                if (drugFormulationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugFormulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugFormulationByKey", "DrugFormulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugFormulation
        /// </summary>
        /// <param name="drugFormulation">DrugFormulation object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugFormulation)]
        public async Task<ActionResult<DrugFormulation>> CreateDrugFormulation(DrugFormulation drugFormulation)
        {
            try
            {
                if (await IsDrugFormulationDuplicate(drugFormulation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugFormulation.DateCreated = DateTime.Now;
                drugFormulation.IsDeleted = false;
                drugFormulation.IsSynced = false;

                context.DrugFormulationRepository.Add(drugFormulation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDrugFormulationByKey", new { key = drugFormulation.Oid }, drugFormulation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugFormulation", "DrugFormulationController.cs", ex.Message, drugFormulation.CreatedIn, drugFormulation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugFormulation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugFormulations.</param>
        /// <param name="drugFormulation">DrugFormulation to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugFormulation)]
        public async Task<IActionResult> UpdateDrugFormulation(int key, DrugFormulation drugFormulation)
        {
            try
            {
                if (key != drugFormulation.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                drugFormulation.DateModified = DateTime.Now;
                drugFormulation.IsDeleted = false;
                drugFormulation.IsSynced = false;

                context.DrugFormulationRepository.Update(drugFormulation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugFormulation", "DrugFormulationController.cs", ex.Message, drugFormulation.ModifiedIn, drugFormulation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugFormulation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugFormulations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugFormulation)]
        public async Task<IActionResult> DeleteDrugFormulation(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugFormulationInDb = await context.DrugFormulationRepository.GetDrugFormulationByKey(key);

                if (drugFormulationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugFormulationInDb.DateModified = DateTime.Now;
                drugFormulationInDb.IsDeleted = true;
                drugFormulationInDb.IsSynced = false;

                context.DrugFormulationRepository.Update(drugFormulationInDb);
                await context.SaveChangesAsync();

                return Ok(drugFormulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugFormulation", "DrugFormulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the DrugFormulation name is duplicate or not.
        /// </summary>
        /// <param name="DrugFormulation">DrugFormulation object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDrugFormulationDuplicate(DrugFormulation drugFormulation)
        {
            try
            {
                var drugFormulationInDb = await context.DrugFormulationRepository.GetDrugFormulationByName(drugFormulation.Description);

                if (drugFormulationInDb != null)
                    if (drugFormulationInDb.Oid != drugFormulation.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDrugFormulationDuplicate", "DrugFormulationController.cs", ex.Message);
                throw;
            }
        }
    }
}