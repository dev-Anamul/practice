using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : Brian
 * Last modified: 31.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Department controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DepartmentController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<DepartmentController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DepartmentController(IUnitOfWork context, ILogger<DepartmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/department
        /// </summary>
        /// <param name="Department">Department object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDepartment)]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            try
            {
                var departmentInDb = await context.DepartmentRepository.GetDepartmentByFacility(department.Description, department.FacilityId);

                if (departmentInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                department.IsDeleted = false;
                department.IsSynced = false;
                department.DateCreated = DateTime.Now;

                var newDepartmentInDb = context.DepartmentRepository.Add(department);

                await context.SaveChangesAsync();

                return Ok(newDepartmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDepartment", "DepartmentController.cs", ex.Message, department.CreatedIn, department.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/departments/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDepartments)]
        public async Task<IActionResult> ReadDepartments(int key)
        {
            try
            {
                var departmentInDb = await context.DepartmentRepository.GetDepartments(key);
                departmentInDb = departmentInDb.OrderByDescending(x => x.DateCreated);
                return Ok(departmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDepartments", "DepartmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/department/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Department.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDepartmentByKey)]
        public async Task<IActionResult> ReadDepartmentByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var departmentInDb = await context.DepartmentRepository.GetDepartmentByKey(key);

                if (departmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(departmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDepartmentByKey", "DepartmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/department/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Department.</param>
        /// <param name="Department">Department to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDepartment)]
        public async Task<IActionResult> UpdateDepartment(int key, Department department)
        {
            try
            {
                if (key != department.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var departmentInDb = await context.DepartmentRepository.GetDepartmentByFacility(department.Description, department.FacilityId);

                if (departmentInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                 departmentInDb = await context.DepartmentRepository.GetDepartmentByKey(department.Oid);

                departmentInDb.Description = department.Description;
                departmentInDb.DateModified = DateTime.Now;
                departmentInDb.ModifiedBy = department.ModifiedBy;
                departmentInDb.IsSynced = false;
                departmentInDb.IsDeleted = false;

                context.DepartmentRepository.Update(departmentInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDepartment", "DepartmentController.cs", ex.Message, department.ModifiedIn, department.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/department/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Department.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDepartment)]
        public async Task<IActionResult> DeleteDepartment(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var departmentInDb = await context.DepartmentRepository.GetDepartmentByKey(key);

                if (departmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                departmentInDb.DateModified = DateTime.Now;
                departmentInDb.IsDeleted = true;
                departmentInDb.IsSynced = false;

                context.DepartmentRepository.Update(departmentInDb);
                await context.SaveChangesAsync();

                return Ok(departmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDepartment", "DepartmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}