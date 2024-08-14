using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Bed controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BedController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<BedController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BedController(IUnitOfWork context, ILogger<BedController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/bed
        /// </summary>
        /// <param name="Bed">Bed object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBed)]
        public async Task<ActionResult<Bed>> CreateBed(Bed bed)
        {
            try
            {
                var BedInDb = await context.BedRepository.GetBedByName(bed.Description);

                if (BedInDb != null && BedInDb.WardId == bed.WardId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                bed.IsDeleted = false;
                bed.IsSynced = false;
                bed.DateCreated = DateTime.Now;

                var newBedInDb = context.BedRepository.Add(bed);

                await context.SaveChangesAsync();

                return Ok(newBedInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBed", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/beds
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBeds)]
        public async Task<IActionResult> ReadBeds()
        {
            try
            {
                var bedInDb = await context.BedRepository.GetBeds();

                return Ok(bedInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBeds", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/beds/facility/{facilityId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBedsByFacilityId)]
        public async Task<IActionResult> ReadBedsByFacilityId(int facilityId)
        {
            try
            {
                var bedsByFacilityIdInDb = await context.BedRepository.GetBedsByFacilityId(facilityId);

                return Ok(bedsByFacilityIdInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBedsByFacilityId", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/bed/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Bed.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBedByKey)]
        public async Task<IActionResult> ReadBedByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var bedInDb = await context.BedRepository.GetBedByKey(key);

                if (bedInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(bedInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBedByKey", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/bed/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Bed.</param>
        /// <param name="Bed">Bed to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBed)]
        public async Task<IActionResult> UpdateBed(int key, Bed bed)
        {
            try
            {
                if (key != bed.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var bedInDb = await context.BedRepository.GetBedByName(bed.Description);

                if (bedInDb != null && bedInDb.WardId == bed.WardId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                bedInDb = await context.BedRepository.GetBedByKey(bed.Oid);

                bedInDb.Description = bed.Description;
                bedInDb.DateModified = DateTime.Now;
                bedInDb.ModifiedBy = bed.ModifiedBy;
                bedInDb.IsSynced = false;
                bedInDb.IsDeleted = false;

                context.BedRepository.Update(bedInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBed", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/bed/bed-by-ward/{wardId}
        /// </summary>
        /// <param name="wardId">Foreign key of the table Ward.</param>
        /// <returns>Http status code: Ok.</returns>
        /// 
        [HttpGet]
        [Route(RouteConstants.ReadBedByWard)]
        public async Task<IActionResult> BedByWard(int wardId)
        {
            try
            {
                var wardsByFirmInDb = await context.BedRepository.GetBedByWard(wardId);

                return Ok(wardsByFirmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "BedByWard", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/bed/bed-by-ward/{wardId}
        /// </summary>
        /// <param name="wardId">Foreign key of the table Ward.</param>
        /// <returns>Http status code: Ok.</returns>
        /// 
        [HttpGet]
        [Route(RouteConstants.ReadBedByWardForDropdown)]
        public async Task<IActionResult> BedByWardForDropdown(int wardId)
        {
            try
            {
                var wardsByFirmInDb = await context.BedRepository.GetBedByWardForDropDown(wardId);

                return Ok(wardsByFirmInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "BedByWardForDropdown", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpDelete]
        [Route(RouteConstants.DeleteBed)]
        public async Task<IActionResult> DeleteBed(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);
                
                var deleteInDb = await context.BedRepository.GetBedByKey(key);

                if (deleteInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deleteInDb.DateModified = DateTime.Now;
                deleteInDb.IsDeleted = true;
                deleteInDb.IsSynced = false;

                context.BedRepository.Update(deleteInDb);
                await context.SaveChangesAsync();

                return Ok(deleteInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteBed", "BedController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }
    }
}