using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.03.2023
 * Modified by  : Brian
 * Last modified: 03.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Special Drug Interval controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SpecialDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SpecialDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SpecialDrugController(IUnitOfWork context, ILogger<SpecialDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/specialDrug
        /// </summary>
        /// <param name="specialDrug">SpecialDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSpecialDrug)]
        public async Task<ActionResult<SpecialDrug>> CreateSpecialDrug(SpecialDrug specialDrug)
        {
            try
            {
                if (await IsSpecialDrugDuplicate(specialDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                specialDrug.DateCreated = DateTime.Now;
                specialDrug.IsDeleted = false;
                specialDrug.IsSynced = false;

                context.SpecialDrugRepository.Add(specialDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSpecialDrugByKey", new { key = specialDrug.Oid }, specialDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSpecialDrug", "SpecialDrugController.cs", ex.Message, specialDrug.CreatedIn, specialDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/special-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSpecialDrugs)]
        public async Task<IActionResult> ReadSpecialDrug()
        {
            try
            {
                var specialDrugInDb = await context.SpecialDrugRepository.GetSpecialDrug();

                return Ok(specialDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSpecialDrug", "SpecialDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/special-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SpecialDrug By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSpecialDrugByKey)]
        public async Task<IActionResult> ReadSpecialDrugByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var specialDrugInDb = await context.SpecialDrugRepository.GetSpecialDrugByKey(key);

                if (specialDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);


                return Ok(specialDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSpecialDrugByKey", "SpecialDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/special-drug/by-regimenId/{regimenId}
        /// </summary>
        /// <param name="regimenId">regimenId of the table DrugRegimen.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSpecialDrugByRegimenId)]
        public async Task<IActionResult> ReadSpecialDrugByRegimenId(int regimenId)
        {
            try
            {
                if (regimenId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var specialDrugsInDb = await context.SpecialDrugRepository.GetSpecialDrugsByRegimenId(regimenId);

                if (specialDrugsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(specialDrugsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSpecialDrugByRegimenId", "SpecialDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/special-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SpecialDrugs.</param>
        /// <param name="specialDrug">SpecialDrug to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSpecialDrug)]
        public async Task<IActionResult> UpdateSpecialDrug(int key, SpecialDrug specialDrug)
        {
            try
            {
                if (key != specialDrug.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                specialDrug.DateModified = DateTime.Now;
                specialDrug.IsDeleted = false;
                specialDrug.IsSynced = false;

                context.SpecialDrugRepository.Update(specialDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSpecialDrug", "SpecialDrugController.cs", ex.Message, specialDrug.ModifiedIn, specialDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/special-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SpecialDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSpecialDrug)]
        public async Task<IActionResult> DeleteSpecialDrug(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var specialDrugInDb = await context.SpecialDrugRepository.GetSpecialDrugByKey(key);

                if (specialDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                specialDrugInDb.DateModified = DateTime.Now;
                specialDrugInDb.IsDeleted = true;
                specialDrugInDb.IsSynced = false;

                context.SpecialDrugRepository.Update(specialDrugInDb);
                await context.SaveChangesAsync();

                return Ok(specialDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSpecialDrug", "SpecialDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the SpecialDrug name is duplicate or not.
        /// </summary>
        /// <param name="SpecialDrug">SpecialDrug object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsSpecialDrugDuplicate(SpecialDrug specialDrug)
        {
            try
            {
                var specialDrugInDb = await context.SpecialDrugRepository.GetSpecialDrugByName(specialDrug.Description);

                if (specialDrugInDb != null)
                    if (specialDrugInDb.Oid != specialDrug.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsSpecialDrugDuplicate", "SpecialDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}
