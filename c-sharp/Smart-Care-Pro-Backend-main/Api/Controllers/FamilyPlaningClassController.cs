using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// FamilyPlanningClass controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FamilyPlanningClassController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FamilyPlanningClassController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FamilyPlanningClassController(IUnitOfWork context, ILogger<FamilyPlanningClassController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/family-planning-class
        /// </summary>
        /// <param name="familyPlaningClass">FamilyPlanningClass object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFamilyPlanningClass)]
        public async Task<ActionResult<FamilyPlanningClass>> CreateFamilyPlanningClass(FamilyPlanningClass familyPlanningClass)
        {
            try
            {
                if (await IsFamilyPlanningClassDuplicate(familyPlanningClass) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                familyPlanningClass.DateCreated = DateTime.Now;
                familyPlanningClass.IsDeleted = false;
                familyPlanningClass.IsSynced = false;

                context.FamilyPlanningClassRepository.Add(familyPlanningClass);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFamilyPlanningClassByKey", new { key = familyPlanningClass.Oid }, familyPlanningClass);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFamilyPlanningClass", "FamilyPlanningClassController.cs", ex.Message, familyPlanningClass.CreatedIn, familyPlanningClass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-classes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanningClasses)]
        public async Task<IActionResult> ReadFamilyPlanningClasses()
        {
            try
            {
                var familyPlanningClassInDb = await context.FamilyPlanningClassRepository.GetFamilyPlanningClasses();

                return Ok(familyPlanningClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanningClasses", "FamilyPlanningClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-class/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningClasses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanningClassByKey)]
        public async Task<IActionResult> ReadFamilyPlanningClassByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanningClassInDb = await context.FamilyPlanningClassRepository.GetFamilyPlanningClassByKey(key);

                if (familyPlanningClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(familyPlanningClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanningClassByKey", "FamilyPlanningClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningClass.</param>
        /// <param name="familyPlanningClass">FamilyPlanningClass to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFamilyPlanningClass)]
        public async Task<IActionResult> UpdateFamilyPlanningClass(int key, FamilyPlanningClass familyPlanningClass)
        {
            try
            {
                if (key != familyPlanningClass.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                familyPlanningClass.DateModified = DateTime.Now;
                familyPlanningClass.IsDeleted = false;
                familyPlanningClass.IsSynced = false;

                context.FamilyPlanningClassRepository.Update(familyPlanningClass);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFamilyPlanningClass", "FamilyPlanningClassController.cs", ex.Message, familyPlanningClass.ModifiedIn, familyPlanningClass.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-planning-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanningClasses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFamilyPlanningClass)]
        public async Task<IActionResult> DeleteFamilyPlanningClass(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanningClassInDb = await context.FamilyPlanningClassRepository.GetFamilyPlanningClassByKey(key);

                if (familyPlanningClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                familyPlanningClassInDb.DateModified = DateTime.Now;
                familyPlanningClassInDb.IsDeleted = true;
                familyPlanningClassInDb.IsSynced = false;

                context.FamilyPlanningClassRepository.Update(familyPlanningClassInDb);
                await context.SaveChangesAsync();

                return Ok(familyPlanningClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFamilyPlanningClass", "FamilyPlanningClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the FamilyPlanningClass name is duplicate or not.
        /// </summary>
        /// <param name="familyPlanningClass">FamilyPlanningClass object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFamilyPlanningClassDuplicate(FamilyPlanningClass familyPlanningClass)
        {
            try
            {
                var familyPlanningClassInDb = await context.FamilyPlanningClassRepository.GetFamilyPlanningClassByName(familyPlanningClass.Description);

                if (familyPlanningClassInDb != null)
                    if (familyPlanningClassInDb.Oid != familyPlanningClass.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFamilyPlanningClassDuplicate", "FamilyPlanningClassController.cs", ex.Message);
                throw;
            }
        }
    }
}