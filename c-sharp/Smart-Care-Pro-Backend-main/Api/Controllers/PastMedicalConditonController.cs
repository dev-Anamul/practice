using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 03.05.2023
 * Modified by   : Brian
 * Last modified : 01.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PastMedicalConditon controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PastMedicalConditonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PastMedicalConditonController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PastMedicalConditonController(IUnitOfWork context, ILogger<PastMedicalConditonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/past-medical-conditon
        /// </summary>
        /// <param name="pastMedicalConditon">PastMedicalConditon object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePastMedicalConditon)]
        public async Task<ActionResult<PastMedicalCondition>> CreatePastMedicalConditon(PastMedicalCondition pastMedicalConditon)
        {
            try
            {
                if (await IsPastMedicalConditonDuplicate(pastMedicalConditon) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                pastMedicalConditon.DateCreated = DateTime.Now;
                pastMedicalConditon.IsDeleted = false;
                pastMedicalConditon.IsSynced = false;

                context.PastMedicalConditonRepository.Add(pastMedicalConditon);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPastMedicalConditonByKey", new { key = pastMedicalConditon.Oid }, pastMedicalConditon);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePastMedicalConditon", "PastMedicalConditonController.cs", ex.Message, pastMedicalConditon.CreatedIn, pastMedicalConditon.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-medical-conditons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastMedicalConditons)]
        public async Task<IActionResult> ReadPastMedicalConditons()
        {
            try
            {
                var pastMedicalConditonInDb = await context.PastMedicalConditonRepository.GetPastMedicalConditons();

                return Ok(pastMedicalConditonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastMedicalConditons", "PastMedicalConditonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-medical-conditon/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastMedicalConditon.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastMedicalConditonByKey)]
        public async Task<IActionResult> ReadPastMedicalConditonByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pastMedicalConditonInDb = await context.PastMedicalConditonRepository.GetPastMedicalConditonByKey(key);

                if (pastMedicalConditonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pastMedicalConditonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastMedicalConditonByKey", "PastMedicalConditonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-medical-conditon/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastMedicalConditons.</param>
        /// <param name="pastMedicalConditon">PastMedicalConditon to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePastMedicalConditon)]
        public async Task<IActionResult> UpdatePastMedicalConditon(int key, PastMedicalCondition pastMedicalConditon)
        {
            try
            {
                if (key != pastMedicalConditon.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                pastMedicalConditon.DateModified = DateTime.Now;
                pastMedicalConditon.IsDeleted = false;
                pastMedicalConditon.IsSynced = false;

                context.PastMedicalConditonRepository.Update(pastMedicalConditon);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePastMedicalConditon", "PastMedicalConditonController.cs", ex.Message, pastMedicalConditon.ModifiedIn, pastMedicalConditon.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-medical-conditon/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastMedicalConditons.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePastMedicalConditon)]
        public async Task<IActionResult> DeletePastMedicalConditon(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pastMedicalConditonInDb = await context.PastMedicalConditonRepository.GetPastMedicalConditonByKey(key);

                if (pastMedicalConditonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pastMedicalConditonInDb.DateModified = DateTime.Now;
                pastMedicalConditonInDb.IsDeleted = true;
                pastMedicalConditonInDb.IsSynced = false;

                context.PastMedicalConditonRepository.Update(pastMedicalConditonInDb);
                await context.SaveChangesAsync();

                return Ok(pastMedicalConditonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePastMedicalConditon", "PastMedicalConditonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PastMedicalConditon name is duplicate or not.
        /// </summary>
        /// <param name="PastMedicalConditon">PastMedicalConditon object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPastMedicalConditonDuplicate(PastMedicalCondition pastMedicalConditon)
        {
            try
            {
                var pastMedicalConditonInDb = await context.PastMedicalConditonRepository.GetPastMedicalConditonByName(pastMedicalConditon.Description);

                if (pastMedicalConditonInDb != null)
                    if (pastMedicalConditonInDb.Oid != pastMedicalConditon.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPastMedicalConditonDuplicate", "PastMedicalConditonController.cs", ex.Message);
                throw;
            }
        }
    }
}