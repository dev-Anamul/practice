using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// FamilyPlanningSubClass controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FamilyPlanningSubClassController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FamilyPlanningSubClassController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FamilyPlanningSubClassController(IUnitOfWork context, ILogger<FamilyPlanningSubClassController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/family-planning-sub-class
        /// </summary>
        /// <param name="familyPlanningSubclass">FamilyPlanningSubclass object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFamilyPlanningSubClass)]
        public async Task<ActionResult<FamilyPlanningSubclass>> CreateFamilyPlanningSubclass(FamilyPlanningSubclass familyPlanningSubclass)
        {
            try
            {
                if (await IsFamilyPlanningSubClassDuplicate(familyPlanningSubclass) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                familyPlanningSubclass.DateCreated = DateTime.Now;
                familyPlanningSubclass.IsDeleted = false;
                familyPlanningSubclass.IsSynced = false;

                context.FamilyPlanningSubClassRepository.Add(familyPlanningSubclass);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFamilyPlanningSubClassByKey", new { key = familyPlanningSubclass.Oid }, familyPlanningSubclass);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFamilyPlanningSubclass", "FamilyPlanningSubClassController.cs", ex.Message, familyPlanningSubclass.CreatedIn, familyPlanningSubclass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-sub-classes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanningSubClasses)]
        public async Task<IActionResult> ReadFamilyPlanningSubClasses()
        {
            try
            {
                var familyPlanningSubClassInDb = await context.FamilyPlanningSubClassRepository.GetFamilyPlanningSubClasses();

                return Ok(familyPlanningSubClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanningSubClasses", "FamilyPlanningSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-sub-class/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningSubClasses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanningSubClassByKey)]
        public async Task<IActionResult> ReadFamilyPlanningSubClassByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanningSubClassInDb = await context.FamilyPlanningSubClassRepository.GetFamilyPlanningSubClassByKey(key);

                if (familyPlanningSubClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(familyPlanningSubClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanningSubClassByKey", "FamilyPlanningSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning/subclass-by-class/{classId}
        /// </summary>
        /// <param name="classId">Foreign key of the table FamilyPlanningClass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.FamilyPlanningSubclassByClass)]
        public async Task<IActionResult> FamilyPlanningSubclassByClass(int classId)
        {
            try
            {
                var familyPlanningSubClassInDb = await context.FamilyPlanningSubClassRepository.GetFamilyPlanningSubclassByClass(classId);

                return Ok(familyPlanningSubClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "FamilyPlanningSubclassByClass", "FamilyPlanningSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-sub-classes/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningSubClasses.</param>
        /// <param name="familyPlanningSubclass">FamilyPlanningSubClass to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFamilyPlanningSubClass)]
        public async Task<IActionResult> UpdateFamilyPlanningSubClass(int key, FamilyPlanningSubclass familyPlanningSubclass)
        {
            try
            {
                if (key != familyPlanningSubclass.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                familyPlanningSubclass.DateModified = DateTime.Now;
                familyPlanningSubclass.IsDeleted = false;
                familyPlanningSubclass.IsSynced = false;

                context.FamilyPlanningSubClassRepository.Update(familyPlanningSubclass);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFamilyPlanningSubClass", "FamilyPlanningSubClassController.cs", ex.Message, familyPlanningSubclass.ModifiedIn, familyPlanningSubclass.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-sub-classes/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningSubClasses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFamilyPlanningSubClass)]
        public async Task<IActionResult> DeleteFamilyPlanningSubClass(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanningSubclassInDb = await context.FamilyPlanningSubClassRepository.GetFamilyPlanningSubClassByKey(key);

                if (familyPlanningSubclassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                familyPlanningSubclassInDb.DateModified = DateTime.Now;
                familyPlanningSubclassInDb.IsDeleted = true;
                familyPlanningSubclassInDb.IsSynced = false;

                context.FamilyPlanningSubClassRepository.Update(familyPlanningSubclassInDb);
                await context.SaveChangesAsync();

                return Ok(familyPlanningSubclassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFamilyPlanningSubClass", "FamilyPlanningSubClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the FamilyPlanningSubClass name is duplicate or not.
        /// </summary>
        /// <param name="FamilyPlanningSubClass">FamilyPlanningSubClass object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFamilyPlanningSubClassDuplicate(FamilyPlanningSubclass familyPlanningSubclass)
        {
            try
            {
                var familyPlanningSubclassInDb = await context.FamilyPlanningSubClassRepository.GetFamilyPlanningSubclassByName(familyPlanningSubclass.Description);

                if (familyPlanningSubclassInDb != null)
                    if (familyPlanningSubclassInDb.Oid != familyPlanningSubclass.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFamilyPlanningSubClassDuplicate", "FamilyPlanningSubClassController.cs", ex.Message);
                throw;
            }
        }
    }
}