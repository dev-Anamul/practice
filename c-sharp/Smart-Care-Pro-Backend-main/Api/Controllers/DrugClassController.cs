using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 07.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// DrugClass controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugClassController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugClassController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugClassController(IUnitOfWork context, ILogger<DrugClassController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugClass
        /// </summary>
        /// <param name="DrugClass">DrugClass object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugClass)]
        public async Task<ActionResult<DrugClass>> CreateDrugClass(DrugClass drugClass)
        {
            try
            {
                var drugClassInDb = await context.DrugClassRepository.GetDrugClassByDescription(drugClass.Description);

                if (drugClassInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugClass.IsDeleted = false;
                drugClass.IsSynced = false;
                drugClass.DateCreated = DateTime.Now;

                var newDrugClassInDb = context.DrugClassRepository.Add(drugClass);
                await context.SaveChangesAsync();

                return Ok(newDrugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugClass", "DrugClassController.cs", ex.Message, drugClass.CreatedIn, drugClass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugClasses/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugClasses)]
        public async Task<IActionResult> ReadDrugClasses()
        {
            try
            {
                var drugClassInDb = await context.DrugClassRepository.GetDrugClasses();

                return Ok(drugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugClasses", "DrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugClass/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugClass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugClassByKey)]
        public async Task<IActionResult> ReadDrugClassByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugClassInDb = await context.DrugClassRepository.GetDrugClassByKey(key);

                if (drugClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugClassByKey", "DrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugClass/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugClass.</param>
        /// <param name="DrugClass">DrugClass to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugClass)]
        public async Task<IActionResult> UpdateDrugClass(int key, DrugClass drugClass)
        {
            try
            {
                if (key != drugClass.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var drugClassInDb = await context.DrugClassRepository.GetDrugClassByKey(drugClass.Oid);

                drugClassInDb.Description = drugClass.Description;
                drugClassInDb.DateModified = DateTime.Now;
                drugClassInDb.ModifiedBy = drugClass.ModifiedBy;
                drugClassInDb.IsSynced = false;
                drugClassInDb.IsDeleted = false;

                context.DrugClassRepository.Update(drugClassInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugClass", "DrugClassController.cs", ex.Message, drugClass.ModifiedIn, drugClass.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugClass/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugClass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugClass)]
        public async Task<IActionResult> DeleteDrugClass(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugClassInDb = await context.DrugClassRepository.GetDrugClassByKey(key);

                if (drugClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugClassInDb.DateModified = DateTime.Now;
                drugClassInDb.IsDeleted = true;
                drugClassInDb.IsSynced = false;

                context.DrugClassRepository.Update(drugClassInDb);
                await context.SaveChangesAsync();

                return Ok(drugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugClass", "DrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}