using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    ///Ca Cx Screening Method Controller Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CaCxScreeningMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CaCxScreeningMethodController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CaCxScreeningMethodController(IUnitOfWork context, ILogger<CaCxScreeningMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/cacx-screening-method
        /// </summary>
        /// <param name="caCxScreeningMethod">CaCx Screening Method object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCaCxScreeningMethod)]
        public async Task<IActionResult> CreateCaCxScreeningMethod(CaCxScreeningMethod caCxScreeningMethod)
        {
            try
            {
                caCxScreeningMethod.DateCreated = DateTime.Now;
                caCxScreeningMethod.IsDeleted = false;
                caCxScreeningMethod.IsSynced = false;

                context.CaCxScreeningMethodRepository.Add(caCxScreeningMethod);
                await context.SaveChangesAsync();

                return Ok(caCxScreeningMethod);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCaCxScreeningMethod", "CaCxScreeningMethodController.cs", ex.Message, caCxScreeningMethod.CreatedIn, caCxScreeningMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cacx-screening-methods
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCaCxScreeningMethods)]
        public async Task<IActionResult> ReadCaCxScreeningMethods()
        {
            try
            {
                var caCxScreeningMethodInDb = await context.CaCxScreeningMethodRepository.GetCaCxScreeningMethods();

                caCxScreeningMethodInDb = caCxScreeningMethodInDb.OrderByDescending(x => x.DateCreated);

                return Ok(caCxScreeningMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCaCxScreeningMethods", "CaCxScreeningMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/cacx-screening-method/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CaCxScreeningMethod.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCaCxScreeningMethodByKey)]
        public async Task<IActionResult> ReadCaCxScreeningMethodByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);
                var caCxScreeningMethodsInDb = await context.CaCxScreeningMethodRepository.GetCaCxScreeningMethodByKey(key);

                if(caCxScreeningMethodsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(caCxScreeningMethodsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCaCxScreeningMethodByKey", "CaCxScreeningMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        ///URL: sc-api/cacx-screening-method/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CaCxScreeningMethod.</param>
        /// <param name="caCxScreeningMethod">CaCxScreeningMethod to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCaCxScreeningMethod)]
        public async Task<IActionResult> UpdateCaCxScreeningMethod(int key, CaCxScreeningMethod caCxScreeningMethod)
        {
            try
            {
                if (key != caCxScreeningMethod.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCaCxScreeningMethodDuplicate(caCxScreeningMethod) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                caCxScreeningMethod.DateModified = DateTime.Now;
                caCxScreeningMethod.IsDeleted = false;
                caCxScreeningMethod.IsSynced = false;

                context.CaCxScreeningMethodRepository.Update(caCxScreeningMethod);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);


            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCaCxScreeningMethod", "CaCxScreeningMethodController.cs", ex.Message, caCxScreeningMethod.ModifiedIn, caCxScreeningMethod.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        private async Task<bool> IsCaCxScreeningMethodDuplicate(CaCxScreeningMethod caCxScreeningMethod)
        {
            try
            {
                var caCxScreeningMethodInDb = await context.CaCxScreeningMethodRepository.GetCaCxScreeningMethodByName(caCxScreeningMethod.Description);

                if (caCxScreeningMethodInDb != null)
                    if (caCxScreeningMethodInDb.Oid != caCxScreeningMethod.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCaCxScreeningMethodDuplicate", "CaCxScreeningMethodController.cs", ex.Message);
                throw;
            }
        }

        [HttpDelete]
        [Route(RouteConstants.DeleteCaCxScreeningMethod)]
        public async Task<IActionResult> DeleteCaCxScreeningMethod(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var caCxScreeningMethodInDb = await context.CaCxScreeningMethodRepository.GetCaCxScreeningMethodByKey(key);

                if (caCxScreeningMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                caCxScreeningMethodInDb.DateModified = DateTime.Now;
                caCxScreeningMethodInDb.IsDeleted = true;
                caCxScreeningMethodInDb.IsSynced = false;

                context.CaCxScreeningMethodRepository.Update(caCxScreeningMethodInDb);
                await context.SaveChangesAsync();

                return Ok(caCxScreeningMethodInDb);


            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCaCxScreeningMethod", "CaCxScreeningMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
   
}