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
    /// DrugSubclass controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugSubClassController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugSubClassController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugSubClassController(IUnitOfWork context, ILogger<DrugSubClassController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugSubclass
        /// </summary>
        /// <param name="DrugSubclass">DrugSubclass object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugSubclass)]
        public async Task<ActionResult<DrugSubclass>> CreateDrugSubclass(DrugSubclass drugSubclass)
        {
            try
            {
                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubclassByDescription(drugSubclass.Description);

                if (drugSubclassInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugSubclass.IsSynced = false;
                drugSubclass.IsSynced = false;
                drugSubclass.DateCreated = DateTime.Now;

                var newDrugSubclassInDb = context.DrugSubclassRepository.Add(drugSubclass);

                await context.SaveChangesAsync();

                return Ok(newDrugSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugSubclass", "DrugSubClassController.cs", ex.Message, drugSubclass.CreatedIn, drugSubclass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugSubclasses/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugSubclasses)]
        public async Task<IActionResult> ReadDrugSubclasses()
        {
            try
            {
                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubclasses();

                return Ok(drugSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugSubclasses", "DrugSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugSubclass/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugSubclass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugSubclassByKey)]
        public async Task<IActionResult> ReadDrugSubclassByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubclassByKey(key);

                if (drugSubclassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugSubclassByKey", "DrugSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugSubclass/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugSubclass.</param>
        /// <param name="DrugSubclass">DrugSubclass to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugSubclass)]
        public async Task<IActionResult> UpdateDrugSubclass(int key, DrugSubclass drugSubclass)
        {
            try
            {
                if (key != drugSubclass.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubclassByKey(drugSubclass.Oid);

                drugSubclassInDb.Description = drugSubclass.Description;
                drugSubclassInDb.DateModified = DateTime.Now;
                drugSubclassInDb.ModifiedBy = drugSubclass.ModifiedBy;
                drugSubclassInDb.IsSynced = false;
                drugSubclassInDb.IsDeleted = false;

                context.DrugSubclassRepository.Update(drugSubclassInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugSubclass", "DrugSubClassController.cs", ex.Message, drugSubclass.ModifiedIn, drugSubclass.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugSubClass/drugSubClassByClass/{drugClassId}
        /// </summary>
        /// <param name="drugClassId">drugClassId of the table DrugSubclass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugSubClassByClass)]
        public async Task<IActionResult> ReadDrugSubClassByClass(int drugClassId)
        {
            try
            {
                if (drugClassId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubClassByClassId(drugClassId);

                if (drugSubclassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugSubClassByClass", "DrugSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugSubclass/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugSubclass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugSubclass)]
        public async Task<IActionResult> DeleteDrugSubclass(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugSubclassInDb = await context.DrugSubclassRepository.GetDrugSubclassByKey(key);

                if (drugSubclassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugSubclassInDb.DateModified = DateTime.Now;
                drugSubclassInDb.IsDeleted = true;
                drugSubclassInDb.IsSynced = false;

                context.DrugSubclassRepository.Update(drugSubclassInDb);
                await context.SaveChangesAsync();

                return Ok(drugSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugSubclass", "DrugSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}