using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 12.09.2022
 * Modified by   : Stephan
 * Last modified : 06.11.2022
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Facility Queue controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FacilityQueueController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FacilityQueueController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FacilityQueueController(IUnitOfWork context, ILogger<FacilityQueueController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/facility-queue
        /// </summary>
        /// <param name="facility-queue">facility-queue object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacilityQueue)]
        public async Task<IActionResult> CreateFacilityQueue(FacilityQueue facilityqueue)
        {
            try
            {
                if (await IsFacilityQueueDuplicate(facilityqueue) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                facilityqueue.DateCreated = DateTime.Now;
                facilityqueue.IsDeleted = false;
                facilityqueue.IsSynced = false;

                context.FacilityQueueRepository.Add(facilityqueue);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFacilityQueueByKey", new { key = facilityqueue.Oid }, facilityqueue);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFacilityQueue", "FacilityQueueController.cs", ex.Message, facilityqueue.CreatedIn, facilityqueue.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/countries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityQueues)]
        public async Task<IActionResult> ReadFacilityQueues()
        {
            try
            {
                var facilityqueueIndb = await context.FacilityQueueRepository.GetAllFacilityQueues();

                return Ok(facilityqueueIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityQueues", "FacilityQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facilityqueue/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityQueueByKey)]
        public async Task<IActionResult> ReadFacilityQueueByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityqueueIndb = await context.FacilityQueueRepository.GetFacilityQueueById(key);

                if (facilityqueueIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityqueueIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityQueueByKey", "FacilityQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facilityqueue/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityQueueByFacilityText)]
        public async Task<IActionResult> ReadFacilityQueueByFacilityText(int facilityId, string? search)
        {
            try
            {
                if (facilityId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityqueueIndb = await context.FacilityQueueRepository.GetFacilityQueueByFacilityId(facilityId, search);

                if (facilityqueueIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityqueueIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityQueueByFacility", "FacilityQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadFacilityQueueByFacilityWithActivePatientCount)]
        public async Task<IActionResult> ReadFacilityQueueByFacilityWithActivePatientCount(int facilityId)
        {
            try
            {
                if (facilityId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var servicePoint = await context.FacilityQueueRepository.GetFacilityQueueByFacilityWithActivePatientCount(facilityId);

                if (servicePoint == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(servicePoint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityQueueByFacilityWithActivePatientCount", "FacilityQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/facilityqueue/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <param name="facilityqueue">facilityqueue to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacilityQueue)]
        public async Task<IActionResult> UpdateFacilityQueue(int key, FacilityQueue facilityqueue)
        {
            try
            {
                if (key != facilityqueue.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsFacilityQueueDuplicate(facilityqueue) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                facilityqueue.DateModified = DateTime.Now;
                facilityqueue.IsDeleted = false;
                facilityqueue.IsSynced = false;

                context.FacilityQueueRepository.Update(facilityqueue);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFacilityQueue", "FacilityQueueController.cs", ex.Message, facilityqueue.ModifiedIn, facilityqueue.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facilityqueue/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacilityQueue)]
        public async Task<IActionResult> DeleteFacilityQueue(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityqueueInDb = await context.FacilityQueueRepository.GetFacilityQueueById(key);

                if (facilityqueueInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                facilityqueueInDb.DateModified = DateTime.Now;
                facilityqueueInDb.IsDeleted = true;
                facilityqueueInDb.IsSynced = false;

                context.FacilityQueueRepository.Update(facilityqueueInDb);
                await context.SaveChangesAsync();

                return Ok(facilityqueueInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFacilityQueue", "FacilityQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the facilityqueue name is duplicate or not. 
        /// </summary>
        /// <param name="facilityqueue">facilityqueue object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFacilityQueueDuplicate(FacilityQueue facilityqueue)
        {
            try
            {
                var facilityqueueInDb = await context.FacilityQueueRepository.GetFacilityQueueByName(facilityqueue.Description, facilityqueue.FacilityId);

                if (facilityqueueInDb != null)
                    if (facilityqueueInDb.Oid != facilityqueue.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFacilityQueueDuplicate", "FacilityQueueController.cs", ex.Message);
                throw;
            }
        }
    }
}