using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Lion
 * Last modified: 21.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// CauseOfNeonatalDeath Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class CauseOfNeonatalDeathController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CauseOfNeonatalDeathController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CauseOfNeonatalDeathController(IUnitOfWork context, ILogger<CauseOfNeonatalDeathController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/cause-of-neonatal-deaths
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCauseOfNeonatalDeaths)]
        public async Task<IActionResult> ReadCauseOfNeonatalDeaths()
        {
            try
            {
                var causeOfNeonatalDeathInDb = await context.CauseOfNeonatalDeathRepository.GetCauseOfNeonatalDeaths();

                causeOfNeonatalDeathInDb = causeOfNeonatalDeathInDb.OrderByDescending(x => x.DateCreated);

                return Ok(causeOfNeonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCauseOfNeonatalDeaths", "CauseOfNeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-neonatal-death/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfNeonatalDeaths.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCauseOfNeonatalDeathByKey)]
        public async Task<IActionResult> ReadCauseOfNeonatalDeathByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var causeOfNeonatalDeathInDb = await context.CauseOfNeonatalDeathRepository.GetCauseOfNeonatalDeathByKey(key);

                if (causeOfNeonatalDeathInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(causeOfNeonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCauseOfNeonatalDeathByKey", "CauseOfNeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-neonatal-death
        /// </summary>
        /// <param name="causeOfNeonatalDeath">CauseOfNeonatalDeath object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCauseOfNeonatalDeath)]
        public async Task<IActionResult> CreateCauseOfNeonatalDeath(CauseOfNeonatalDeath causeOfNeonatalDeath)
        {
            try
            {
                causeOfNeonatalDeath.DateCreated = DateTime.Now;
                causeOfNeonatalDeath.IsDeleted = false;
                causeOfNeonatalDeath.IsSynced = false;

                context.CauseOfNeonatalDeathRepository.Add(causeOfNeonatalDeath);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCauseOfNeonatalDeathByKey", new { key = causeOfNeonatalDeath.Oid }, causeOfNeonatalDeath);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCauseOfNeonatalDeath", "CauseOfNeonatalDeathController.cs", ex.Message, causeOfNeonatalDeath.CreatedIn, causeOfNeonatalDeath.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/cause-of-neonatal-death/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfNeonatalDeath.</param>
        /// <param name="causeOfNeonatalDeath">CauseOfNeonatalDeath to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCauseOfNeonatalDeath)]
        public async Task<IActionResult> UpdateCauseOfNeonatalDeath(int key, CauseOfNeonatalDeath causeOfNeonatalDeath)
        {
            try
            {
                if (key != causeOfNeonatalDeath.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCauseOfNeonatalDeathDuplicate(causeOfNeonatalDeath) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                causeOfNeonatalDeath.DateModified = DateTime.Now;
                causeOfNeonatalDeath.IsDeleted = false;
                causeOfNeonatalDeath.IsSynced = false;

                context.CauseOfNeonatalDeathRepository.Update(causeOfNeonatalDeath);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCauseOfNeonatalDeath", "CauseOfNeonatalDeathController.cs", ex.Message, causeOfNeonatalDeath.ModifiedIn, causeOfNeonatalDeath.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-neonatal-death/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCauseOfNeonatalDeath)]
        public async Task<IActionResult> DeleteCauseOfNeonatalDeath(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var causeOfNeonatalDeathInDb = await context.CauseOfNeonatalDeathRepository.GetCauseOfNeonatalDeathByKey(key);

                if (causeOfNeonatalDeathInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                causeOfNeonatalDeathInDb.DateModified = DateTime.Now;
                causeOfNeonatalDeathInDb.IsDeleted = true;
                causeOfNeonatalDeathInDb.IsSynced = false;

                context.CauseOfNeonatalDeathRepository.Update(causeOfNeonatalDeathInDb);
                await context.SaveChangesAsync();

                return Ok(causeOfNeonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCauseOfNeonatalDeath", "CauseOfNeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the CauseOfNeonatalDeath name is duplicate or not.
        /// </summary>
        /// <param name="CauseOfNeonatalDeath">CauseOfNeonatalDeath object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCauseOfNeonatalDeathDuplicate(CauseOfNeonatalDeath causeOfNeonatalDeath)
        {
            try
            {
                var causeOfNeonatalDeathInDb = await context.CauseOfNeonatalDeathRepository.GetCauseOfNeonatalDeathByName(causeOfNeonatalDeath.Description);

                if (causeOfNeonatalDeathInDb != null)
                    if (causeOfNeonatalDeathInDb.Oid != causeOfNeonatalDeath.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCauseOfNeonatalDeathDuplicate", "CauseOfNeonatalDeathController.cs", ex.Message);
                throw;
            }
        }
    }
}