using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 29-01-2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Firm controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FirmController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FirmController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FirmController(IUnitOfWork context, ILogger<FirmController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/firm
        /// </summary>
        /// <param name="firm">Firm object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFirm)]
        public async Task<ActionResult<Firm>> CreateFirm(Firm firm)
        {
            try
            {
                var firmInDb = await context.FirmRepository.checkDuplicateFirmByDepartment(firm.Description, firm.DepartmentId);

                if (firmInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                firm.IsDeleted = false;
                firm.IsSynced = false;
                firm.DateCreated = DateTime.Now;

                var newFirmInDb = context.FirmRepository.Add(firm);

                await context.SaveChangesAsync();

                return Ok(newFirmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFirm", "FirmController.cs", ex.Message, firm.CreatedIn, firm.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/firms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFirms)]
        public async Task<IActionResult> ReadFirms()
        {
            try
            {
                var firmInDb = await context.FirmRepository.GetFirms();

                return Ok(firmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFirms", "FirmController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/firms/facilityId/{facilityId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFirmsByFacilityId)]
        public async Task<IActionResult> ReadFirmsByFacilityId(int facilityId)
        {
            try
            {
                var firmByFacilityIdInDb = await context.FirmRepository.GetFirmsByFacilityId(facilityId);

                return Ok(firmByFacilityIdInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFirmsByFacilityId", "FirmController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadFirmsByDepartmentId)]
        public async Task<IActionResult> ReadFirmsByDepartmentId(int departmentId)
        {
            try
            {
                var firmByFacilityIdInDb = await context.FirmRepository.GetFirmByDepartment(departmentId);

                return Ok(firmByFacilityIdInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFirmsByDepartmentId", "FirmController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/firm/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Firm.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFirmByKey)]
        public async Task<IActionResult> ReadFirmByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var firmInDb = await context.FirmRepository.GetFirmByKey(key);

                if (firmInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(firmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFirmByKey", "FirmController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/firm/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Firm.</param>
        /// <param name="firm">Firm to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFirm)]
        public async Task<IActionResult> UpdateFirm(int key, Firm firm)
        {
            try
            {
                if (key != firm.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var firmInDb = await context.FirmRepository.checkDuplicateFirmByDepartment(firm.Description, firm.DepartmentId, firm.Oid);

                if (firmInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                firmInDb = await context.FirmRepository.GetFirmByKey(firm.Oid);

                firmInDb.Description = firm.Description;
                firmInDb.DateModified = DateTime.Now;
                firmInDb.ModifiedBy = firm.ModifiedBy;
                firmInDb.IsSynced = false;
                firmInDb.IsDeleted = false;

                context.FirmRepository.Update(firmInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFirm", "FirmController.cs", ex.Message, firm.CreatedIn, firm.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/firm/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Firms.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFirm)]
        public async Task<IActionResult> DeleteFirm(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var firmInDb = await context.FirmRepository.GetFirmByKey(key);

                if (firmInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                firmInDb.DateModified = DateTime.Now;
                firmInDb.IsDeleted = true;
                firmInDb.IsSynced = false;

                context.FirmRepository.Update(firmInDb);
                await context.SaveChangesAsync();

                return Ok(firmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFirm", "FirmController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}