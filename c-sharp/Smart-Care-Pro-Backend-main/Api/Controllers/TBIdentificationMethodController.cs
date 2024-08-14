using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// TBIdentificationMethod controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBIdentificationMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBIdentificationMethodController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBIdentificationMethodController(IUnitOfWork context, ILogger<TBIdentificationMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/tb-identification-method
        /// </summary>
        /// <param name="tbIdentificationMethod">TBIdentificationMethod object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTBIdentificationMethod)]
        public async Task<ActionResult<TBIdentificationMethodController>> CreateTBIdentificationMethod(TBIdentificationMethod tBIdentificationMethod)
        {
            try
            {
                if (await IsTBIdentificationMethodDuplicate(tBIdentificationMethod) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                tBIdentificationMethod.DateCreated = DateTime.Now;
                tBIdentificationMethod.IsDeleted = false;
                tBIdentificationMethod.IsSynced = false;

                context.TBIdentificationMethodRepository.Add(tBIdentificationMethod);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTBIdentificationMethodByKey", new { key = tBIdentificationMethod.Oid }, tBIdentificationMethod);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTBIdentificationMethod", "TBIdentificationMethodController.cs", ex.Message, tBIdentificationMethod.CreatedIn, tBIdentificationMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/tb-identification-methods
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBIdentificationMethods)]
        public async Task<IActionResult> ReadTBIdentificationMethods()
        {
            try
            {
                var tbIdentificationMethodInDb = await context.TBIdentificationMethodRepository.GetTBIdentificationMethods();

                return Ok(tbIdentificationMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBIdentificationMethods", "TBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-identification-method/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBIdentificationMethod.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBIdentificationMethodByKey)]
        public async Task<IActionResult> ReadTBIdentificationMethodByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbIdentificationMethodInDb = await context.TBIdentificationMethodRepository.GetTBIdentificationMethodByKey(key);

                if (tbIdentificationMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tbIdentificationMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBIdentificationMethodByKey", "TBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/tb-identification-method/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBIdentificationMethod.</param>
        /// <param name="tBIdentificationMethod">TBIdentificationMethod to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTBIdentificationMethodMehtod)]
        public async Task<IActionResult> UpdateTBIdentificationMethodMehtod(int key, TBIdentificationMethod tBIdentificationMethod)
        {
            try
            {
                if (key != tBIdentificationMethod.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tBIdentificationMethod.DateModified = DateTime.Now;
                tBIdentificationMethod.IsDeleted = false;
                tBIdentificationMethod.IsSynced = false;

                context.TBIdentificationMethodRepository.Update(tBIdentificationMethod);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTBIdentificationMethodMehtod", "TBIdentificationMethodController.cs", ex.Message, tBIdentificationMethod.ModifiedIn, tBIdentificationMethod.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/tb-identification-method/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBIdentificationMethod.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTBIdentificationMethodMehtod)]
        public async Task<IActionResult> DeleteTBIdentificationMethodMehtod(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbIdentificationMethodInDb = await context.TBIdentificationMethodRepository.GetTBIdentificationMethodByKey(key);

                if (tbIdentificationMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tbIdentificationMethodInDb.DateModified = DateTime.Now;
                tbIdentificationMethodInDb.IsDeleted = true;
                tbIdentificationMethodInDb.IsSynced = false;

                context.TBIdentificationMethodRepository.Update(tbIdentificationMethodInDb);
                await context.SaveChangesAsync();

                return Ok(tbIdentificationMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBIdentificationMethodMehtod", "TBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the TBIdentificationMethod name is duplicate or not.
        /// </summary>
        /// <param name="tbIdentificationMethod">TBIdentificationMethod object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTBIdentificationMethodDuplicate(TBIdentificationMethod tBIdentificationMethod)
        {
            try
            {
                var tbIdentificationMethodInDb = await context.TBIdentificationMethodRepository.GetTBIdentificationMethodByName(tBIdentificationMethod.Description);

                if (tbIdentificationMethodInDb != null)
                    if (tbIdentificationMethodInDb.Oid != tBIdentificationMethod.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsTBIdentificationMethodDuplicate", "TBIdentificationMethodController.cs", ex.Message);
                throw;
            }
        }
    }
}