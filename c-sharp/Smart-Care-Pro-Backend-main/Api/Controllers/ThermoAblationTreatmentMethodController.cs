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
    public class ThermoAblationTreatmentMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ThermoAblationTreatmentMethodController> logger;
        public ThermoAblationTreatmentMethodController(IUnitOfWork context, ILogger<ThermoAblationTreatmentMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        /// <summary>
        /// URL: sc-api/ThermoAblationTreatmentMethod
        /// </summary>
        /// <param name="ThermoAblationTreatmentMethod">ThermoAblationTreatmentMethod object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateThermoAblationTreatmentMethod)]
        public async Task<IActionResult> CreateThermoAblationTreatmentMethod(ThermoAblationTreatmentMethod thermoAblationTreatmentMethod)
        {
            try
            {
                thermoAblationTreatmentMethod.DateCreated = DateTime.Now;
                thermoAblationTreatmentMethod.IsDeleted = false;
                thermoAblationTreatmentMethod.IsSynced = false;

                context.ThermoAblationTreatmentMethodRepository.Add(thermoAblationTreatmentMethod);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadThermoAblationTreatmentMethodByKey", new { key = thermoAblationTreatmentMethod.Oid }, thermoAblationTreatmentMethod);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "thermoAblationTreatmentMethod", "ThermoAblationTreatmentMethodController.cs", ex.Message, thermoAblationTreatmentMethod.CreatedIn, thermoAblationTreatmentMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/thermoablationtreatmentmethods
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThermoAblationTreatmentMethod)]
        public async Task<IActionResult> ReadThermoAblationTreatmentMethod()
        {
            try
            {
                var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethodRepository.GetThermoAblationTreatmentMethod();

                thermoAblationTreatmentMethodInDb = thermoAblationTreatmentMethodInDb.OrderByDescending(x => x.DateCreated);

                return Ok(thermoAblationTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblationTreatmentMethod", "ThermoAblationTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadThermoAblationTreatmentMethodByKey)]
        public async Task<IActionResult> ReadThermoAblationTreatmentMethodByKey(int key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethodRepository.GetThermoAblationTreatmentMethodByKey(key);

                if (thermoAblationTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(thermoAblationTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblationTreatmentMethodByKey", "ThermoAblationTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/thermoablationtreatmentmethods/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThermoAblationTreatmentMethod.</param>
        /// <param name="ThermoAblationTreatmentMethod">ThermoAblationTreatmentMethod to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateThermoAblationTreatmentMethod)]
        public async Task<IActionResult> UpdateThermoAblationTreatmentMethod(int key, ThermoAblationTreatmentMethod thermoAblationTreatmentMethod)
        {
            try
            {
                if (key != thermoAblationTreatmentMethod.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethodRepository.GetThermoAblationTreatmentMethodByKey(key);

                if (thermoAblationTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                thermoAblationTreatmentMethod.DateModified = DateTime.Now;
                thermoAblationTreatmentMethod.IsDeleted = false;
                thermoAblationTreatmentMethod.IsSynced = false;

                context.ThermoAblationTreatmentMethodRepository.Update(thermoAblationTreatmentMethod);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateThermoAblationTreatmentMethod", "ThermoAblationTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/thermoablationtreatmentmethods/{key}
        /// </summary>
        /// <param name="key">Primary key of the table  thermoablationtreatmentmethods.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteThermoAblationTreatmentMethod)]
        public async Task<IActionResult> DeleteThermoAblationTreatmentMethod(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethodRepository.GetThermoAblationTreatmentMethodByKey(key);

                if (thermoAblationTreatmentMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                thermoAblationTreatmentMethodInDb.IsDeleted = true;
                thermoAblationTreatmentMethodInDb.IsSynced = false;
                thermoAblationTreatmentMethodInDb.DateModified = DateTime.Now;

                context.ThermoAblationTreatmentMethodRepository.Update(thermoAblationTreatmentMethodInDb);
                await context.SaveChangesAsync();

                return Ok(thermoAblationTreatmentMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteThermoAblationTreatmentMethod", "ThermoAblationTreatmentMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
