using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Brian
 * Last modified : 01.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PhysicalSystemController controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PhysicalSystemController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PhysicalSystemController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PhysicalSystemController(IUnitOfWork context, ILogger<PhysicalSystemController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/physical-system
        /// </summary>
        /// <param name="physicalSystem">PhysicalSystem object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePhysicalSystem)]
        public async Task<ActionResult<PhysicalSystem>> CreatePhysicalSystem(PhysicalSystem physicalSystem)
        {
            try
            {
                if (await IsPhysicalSystemDuplicate(physicalSystem) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                physicalSystem.DateCreated = DateTime.Now;
                physicalSystem.IsDeleted = false;
                physicalSystem.IsSynced = false;

                context.PhysicalSystemRepository.Add(physicalSystem);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPhysicalSystemByKey", new { key = physicalSystem.Oid }, physicalSystem);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePhysicalSystem", "PhysicalSystemController.cs", ex.Message, physicalSystem.CreatedIn, physicalSystem.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/physical-systems
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPhysicalSystems)]
        public async Task<IActionResult> ReadPhysicalSystems()
        {
            try
            {
                var physicalSystemInDb = await context.PhysicalSystemRepository.GetPhysicalSystems();

                return Ok(physicalSystemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPhysicalSystems", "PhysicalSystemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/physical-system/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PhysicalSystem.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPhysicalSystemByKey)]
        public async Task<IActionResult> ReadPhysicalSystemByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var physicalSystemInDb = await context.PhysicalSystemRepository.GetPhysicalSystemByKey(key);

                if (physicalSystemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(physicalSystemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPhysicalSystemByKey", "PhysicalSystemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/physical-system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PhysicalSystems.</param>
        /// <param name="physicalSystem">PhysicalSystem to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePhysicalSystem)]
        public async Task<IActionResult> UpdatePhysicalSystem(int key, PhysicalSystem physicalSystem)
        {
            try
            {
                if (key != physicalSystem.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                physicalSystem.DateModified = DateTime.Now;
                physicalSystem.IsDeleted = false;
                physicalSystem.IsSynced = false;

                context.PhysicalSystemRepository.Update(physicalSystem);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePhysicalSystem", "PhysicalSystemController.cs", ex.Message, physicalSystem.ModifiedIn, physicalSystem.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/physical-system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PhysicalSystems.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePhysicalSystem)]
        public async Task<IActionResult> DeletePhysicalSystem(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var physicalSystemInDb = await context.PhysicalSystemRepository.GetPhysicalSystemByKey(key);

                if (physicalSystemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                physicalSystemInDb.DateModified = DateTime.Now;
                physicalSystemInDb.IsDeleted = true;
                physicalSystemInDb.IsSynced = false;

                context.PhysicalSystemRepository.Update(physicalSystemInDb);
                await context.SaveChangesAsync();

                return Ok(physicalSystemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePhysicalSystem", "PhysicalSystemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PhysicalSystem name is duplicate or not.
        /// </summary>
        /// <param name="PhysicalSystem">PhysicalSystem object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPhysicalSystemDuplicate(PhysicalSystem physicalSystem)
        {
            try
            {
                var physicalSystemInDb = await context.PhysicalSystemRepository.GetPhysicalSystemByName(physicalSystem.Description);

                if (physicalSystemInDb != null)
                    if (physicalSystemInDb.Oid != physicalSystem.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPhysicalSystemDuplicate", "PhysicalSystemController.cs", ex.Message);
                throw;
            }
        }
    }
}