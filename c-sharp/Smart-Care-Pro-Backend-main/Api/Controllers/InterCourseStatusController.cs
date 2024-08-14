using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class InterCourseStatusController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<InterCourseStatusController> logger;
        public InterCourseStatusController(IUnitOfWork context, ILogger<InterCourseStatusController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        /// <summary>
        /// URL: sc-api/InterCourseStatus
        /// </summary>
        /// <param name="InterCourseStatus">InterCourseStatus object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateInterCourseStatus)]
        public async Task<IActionResult> CreateInterCourseStatus(InterCourseStatus interCourseStatus)
        {
            try
            {    
                interCourseStatus.DateCreated = DateTime.Now;
                interCourseStatus.IsDeleted = false;
                interCourseStatus.IsSynced = false;

                context.InterCourseStatusRepository.Add(interCourseStatus);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadInterCourseStatusByKey", new { key = interCourseStatus.Oid }, interCourseStatus);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "interCourseStatus", "InterCourseStatusController.cs", ex.Message, interCourseStatus.CreatedIn, interCourseStatus.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/interCourseStatus
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInterCourseStatus)]
        public async Task<IActionResult> ReadInterCourseStatus()
        {
            try
            {
                var interCourseStatusInDb = await context.InterCourseStatusRepository.GetInterCourseStatus();

                interCourseStatusInDb = interCourseStatusInDb.OrderByDescending(x => x.DateCreated);

                return Ok(interCourseStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInterCourseStatus", "InterCourseStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadInterCourseStatusByKey)]
        public async Task<IActionResult> ReaInterCourseStatusByKey(int key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var interCourseStatusInDb = await context.InterCourseStatusRepository.GetInterCourseStatusByKey(key);

                if (interCourseStatusInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(interCourseStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInterCourseStatusByKey", "InterCourseStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/InterCourseStatus/{key}
        /// </summary>
        /// <param name="key">Primary key of the table InterCourseStatus.</param>
        /// <param name="InterCourseStatus">InterCourseStatus to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateInterCourseStatus)]
        public async Task<IActionResult> UpdateInterCourseStatus(int key, InterCourseStatus interCourseStatus)
        {
            try
            {
                if (key != interCourseStatus.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interCourseStatusInDb = await context.InterCourseStatusRepository.GetInterCourseStatusByKey(key);

                if (interCourseStatusInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interCourseStatus.DateModified = DateTime.Now;
                interCourseStatus.IsDeleted = false;
                interCourseStatus.IsSynced = false;

                context.InterCourseStatusRepository.Update(interCourseStatus);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateInterCourseStatus", "InterCourseStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/InterCourseStatus/{key}
        /// </summary>
        /// <param name="key">Primary key of the table  InterCourseStatus.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteInterCourseStatus)]
        public async Task<IActionResult> DeleteInterCourseStatus(int key)
        {
            try
            {
                if (key ==0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var interCourseStatusInDb = await context.InterCourseStatusRepository.GetInterCourseStatusByKey(key);

                if (interCourseStatusInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                interCourseStatusInDb.IsDeleted = true;
                interCourseStatusInDb.IsSynced = false;
                interCourseStatusInDb.DateModified = DateTime.Now;

                context.InterCourseStatusRepository.Update(interCourseStatusInDb);
                await context.SaveChangesAsync();

                return Ok(interCourseStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteInterCourseStatus", "InterCourseStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
