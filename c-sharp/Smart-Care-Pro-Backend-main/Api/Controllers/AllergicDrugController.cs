using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Lion
 * Last modified : 27.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Allergic Drug controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AllergicDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AllergicDrugController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AllergicDrugController(IUnitOfWork context, ILogger<AllergicDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/allergic-drugs
        /// </summary>
        /// <param name="allergicDrug">AllergicDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAllergicDrug)]
        public async Task<IActionResult> CreateAllergicDrug(AllergicDrug allergicDrug)
        {
            try
            {
                allergicDrug.DateCreated = DateTime.Now;
                allergicDrug.IsDeleted = false;
                allergicDrug.IsSynced = false;

                context.AllergicDrugRepository.Add(allergicDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAllergicDrugByKey", new { key = allergicDrug.Oid }, allergicDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAllergicDrug", "AllergicDrugController.cs", ex.Message,allergicDrug.CreatedIn,allergicDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergic-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAllergicDrug)]
        public async Task<IActionResult> ReadAllergicDrug()
        {
            try
            {
                var allergicDrugInDb = await context.AllergicDrugRepository.GetAllergicDrugs();

                allergicDrugInDb = allergicDrugInDb.OrderByDescending(x => x.DateCreated);

                return Ok(allergicDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllergicDrug", "AllergicDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergic-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AllergicDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAllergicDrugByKey)]
        public async Task<IActionResult> ReadAllergicDrugByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var allergicDrugInDb = await context.AllergicDrugRepository.GetAllergicDrugByKey(key);

                if (allergicDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(allergicDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllergicDrugByKey", "AllergicDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/allergic-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AllergicDrugs.</param>
        /// <param name="allergicDrug">AllergicDrug to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAllergicDrug)]
        public async Task<IActionResult> UpdateAllergicDrug(int key, AllergicDrug allergicDrug)
        {
            try
            {
                if (key != allergicDrug.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                allergicDrug.DateModified = DateTime.Now;
                allergicDrug.IsDeleted = false;
                allergicDrug.IsSynced = false;

                context.AllergicDrugRepository.Update(allergicDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateAllergicDrug", "AllergicDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergic-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAllergicDrug)]
        public async Task<IActionResult> DeleteAllergicDrug(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var allergicDrugInDb = await context.AllergicDrugRepository.GetAllergicDrugByKey(key);

                if (allergicDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                allergicDrugInDb.DateModified = DateTime.Now;
                allergicDrugInDb.IsDeleted = true;
                allergicDrugInDb.IsSynced = false;

                context.AllergicDrugRepository.Update(allergicDrugInDb);
                await context.SaveChangesAsync();

                return Ok(allergicDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAllergicDrug", "AllergicDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the Allergic Drug name is duplicate or not.
        /// </summary>
        /// <param name="allergicDrug">AllergicDrug object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsAllergicDrugDuplicate(AllergicDrug allergicDrug)
        {
            try
            {
                var allergicDrugInDb = await context.AllergicDrugRepository.GetAllergicDrugByName(allergicDrug.Description);

                if (allergicDrugInDb != null)
                    if (allergicDrugInDb.Oid != allergicDrug.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAllergicDrugDuplicate", "AllergicDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}