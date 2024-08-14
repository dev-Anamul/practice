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
    public class LeepsTreatmentMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<LeepsTreatmentMethodController> logger;
        public LeepsTreatmentMethodController(IUnitOfWork context, ILogger<LeepsTreatmentMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/LeepsTreatmentMethod
        /// </summary>
        /// <param name="LeepsTreatmentMethod">LeepsTreatmentMethod object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateLeepsTreatmentMethod)]
        public async Task<IActionResult> CreateLeepsTreatmentMethod(LeepsTreatmentMethod leepsTreatmentMethod)
        {
            try
            {
                leepsTreatmentMethod.DateCreated = DateTime.Now;
                leepsTreatmentMethod.IsDeleted = false;
                leepsTreatmentMethod.IsSynced = false;

                context.LeepsTreatmentMethodRepository.Add(leepsTreatmentMethod);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadLeepsTreatmentMethodByKey", new { key = leepsTreatmentMethod.Oid }, leepsTreatmentMethod);
                //return Ok();

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "leepsTreatmentMethod", "LeepsTreatmentMethodController.cs", ex.Message, leepsTreatmentMethod.CreatedIn, leepsTreatmentMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/LeepsTreatmentMethod
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLeepsTreatmentMethod)]
        public async Task<IActionResult> ReadLeepsTreatmentMethod()
        {
            try
            {
                var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethodRepository.GetLeepsTreatmentMethod();

                leepsTreatmentMethodInDb = leepsTreatmentMethodInDb.OrderByDescending(x => x.DateCreated);

                return Ok(leepsTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLeepsTreatmentMethod", "LeepsTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLeepsTreatmentMethodByKey)]
        public async Task<IActionResult> ReaLeepsTreatmentMethodByKey(int key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethodRepository.GetLeepsTreatmentMethodByKey(key);

                if (leepsTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(leepsTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLeepsTreatmentMethodByKey", "LeepsTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/LeepsTreatmentMethod/{key}
        /// </summary>
        /// <param name="key">Primary key of the table LeepsTreatmentMethod.</param>
        /// <param name="LeepsTreatmentMethod">LeepsTreatmentMethod to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateLeepsTreatmentMethod)]
        public async Task<IActionResult> UpdateLeepsTreatmentMethod(int key, LeepsTreatmentMethod leepsTreatmentMethod)
        {
            try
            {
                if (key != leepsTreatmentMethod.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethodRepository.GetLeepsTreatmentMethodByKey(key);

                if (leepsTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                leepsTreatmentMethod.DateModified = DateTime.Now;
                leepsTreatmentMethod.IsDeleted = false;
                leepsTreatmentMethod.IsSynced = false;

                context.LeepsTreatmentMethodRepository.Update(leepsTreatmentMethod);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateLeepsTreatmentMethod", "LeepsTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/LeepsTreatmentMethod/{key}
        /// </summary>
        /// <param name="key">Primary key of the table  LeepsTreatmentMethod.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteLeepsTreatmentMethod)]
        public async Task<IActionResult> DeleteLeepsTreatmentMethod(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethodRepository.GetLeepsTreatmentMethodByKey(key);

                if (leepsTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                leepsTreatmentMethodInDb.IsDeleted = true;
                leepsTreatmentMethodInDb.IsSynced = false;
                leepsTreatmentMethodInDb.DateModified = DateTime.Now;

                context.LeepsTreatmentMethodRepository.Update(leepsTreatmentMethodInDb);
                await context.SaveChangesAsync();

                return Ok(leepsTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteLeepsTreatmentMethod", "LeepsTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }

}
