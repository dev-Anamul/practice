using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   :Brain
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// CauseOfStillBirth Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CauseOfStillBirthController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CauseOfStillBirthController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CauseOfStillBirthController(IUnitOfWork context, ILogger<CauseOfStillBirthController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/cause-of-still-births
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCauseOfStillBirths)]
        public async Task<IActionResult> ReadCauseOfStillBirths()
        {
            try
            {
                var causeOfStillBirthInDb = await context.CauseOfStillBirthRepository.GetCauseOfStillBirths();

                causeOfStillBirthInDb = causeOfStillBirthInDb.OrderByDescending(x => x.DateCreated);

                return Ok(causeOfStillBirthInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCauseOfStillBirths", "CauseOfStillBirthController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-still-birth/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfStillBirths.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCauseOfStillBirthByKey)]
        public async Task<IActionResult> ReadCauseOfStillBirthByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var causeOfStillBirthInDb = await context.CauseOfStillBirthRepository.GetCauseOfStillBirthByKey(key);

                if (causeOfStillBirthInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(causeOfStillBirthInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCauseOfStillBirthByKey", "CauseOfStillBirthController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-still-birth
        /// </summary>
        ///   /// <param name="causeOfStillbirth">CauseOfStillBirth object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCauseOfStillBirth)]
        public async Task<IActionResult> CreateCauseOfStillBirth(CauseOfStillbirth causeOfStillbirth)
        {
            try
            {
                causeOfStillbirth.DateCreated = DateTime.Now;
                causeOfStillbirth.IsDeleted = false;
                causeOfStillbirth.IsSynced = false;

                context.CauseOfStillBirthRepository.Add(causeOfStillbirth);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCauseOfStillBirthByKey", new { key = causeOfStillbirth.Oid }, causeOfStillbirth);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCauseOfStillBirth", "CauseOfStillBirthController.cs", ex.Message, causeOfStillbirth.CreatedIn, causeOfStillbirth.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }




        /// <summary>
        ///URL: sc-api/cause-of-still-birth/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfStillBirth.</param>
        /// <param name="causeOfStillBirth">CauseOfStillBirth to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCauseOfStillBirth)]
        public async Task<IActionResult> UpdateCauseOfStillBirth(int key, CauseOfStillbirth causeOfStillbirth)
        {
            try
            {
                if (key != causeOfStillbirth.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCauseOfStillBirthDuplicate(causeOfStillbirth) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                causeOfStillbirth.DateModified = DateTime.Now;
                causeOfStillbirth.IsDeleted = false;
                causeOfStillbirth.IsSynced = false;

                context.CauseOfStillBirthRepository.Update(causeOfStillbirth);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCauseOfStillBirth", "CauseOfStillBirthController.cs", ex.Message, causeOfStillbirth.ModifiedIn, causeOfStillbirth.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cause-of-still-birth/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCauseOfStillBirth)]
        public async Task<IActionResult> DeleteCauseOfStillBirth(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var causeOfStillbirthInDb = await context.CauseOfStillBirthRepository.GetCauseOfStillBirthByKey(key);

                if (causeOfStillbirthInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                causeOfStillbirthInDb.DateModified = DateTime.Now;
                causeOfStillbirthInDb.IsDeleted = true;
                causeOfStillbirthInDb.IsSynced = false;

                context.CauseOfStillBirthRepository.Update(causeOfStillbirthInDb);
                await context.SaveChangesAsync();

                return Ok(causeOfStillbirthInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCauseOfStillBirth", "CauseOfStillBirthController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the CauseOfStillbirth name is duplicate or not.
        /// </summary>
        /// <param name="CauseOfStillbirth">CauseOfStillbirth object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCauseOfStillBirthDuplicate(CauseOfStillbirth causeOfStillbirth)
        {
            try
            {
                var causeOfStillbirthInDb = await context.CauseOfStillBirthRepository.GetCauseOfStillBirthByName(causeOfStillbirth.Description);

                if (causeOfStillbirthInDb != null)
                    if (causeOfStillbirthInDb.Oid != causeOfStillbirth.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCauseOfStillBirthDuplicate", "CauseOfStillBirthController.cs", ex.Message);
                throw;
            }
        }
    }
}