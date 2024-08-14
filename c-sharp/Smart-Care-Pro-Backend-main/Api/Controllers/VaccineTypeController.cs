using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : Stephan
 * Last modified: 20.02.2023
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// VaccineType Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VaccineTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VaccineTypeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VaccineTypeController(IUnitOfWork context, ILogger<VaccineTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/vaccine-type
        /// </summary>
        /// <param name="vaccineType">VaccineType object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVaccineType)]
        public async Task<IActionResult> CreateVaccineType(VaccineType vaccineType)
        {
            try
            {
                vaccineType.DateCreated = DateTime.Now;
                vaccineType.IsDeleted = false;
                vaccineType.IsSynced = false;

                context.VaccineTypeRepository.Add(vaccineType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVaccineTypeByKey", new { key = vaccineType.Oid }, vaccineType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVaccineType", "VaccineTypeController.cs", ex.Message, vaccineType.CreatedIn, vaccineType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/vaccine-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VaccineTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccineTypeByKey)]
        public async Task<IActionResult> ReadVaccineTypeByKey(int key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vaccineTypeInDb = await context.VaccineTypeRepository.GetVaccineTypeByKey(key);

                if (vaccineTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vaccineTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccineTypeByKey", "VaccineTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccineTypes)]
        public async Task<IActionResult> ReadVaccineTypes()
        {
            try
            {
                var vaccineTypeInDb = await context.VaccineTypeRepository.GetVaccineTypes();
                vaccineTypeInDb = vaccineTypeInDb.OrderByDescending(x => x.DateCreated);
                return Ok(vaccineTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccineTypes", "VaccineTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/vaccine-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VaccineTypes.</param>
        /// <param name="vaccineType">VaccineTypes to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVaccineType)]
        public async Task<IActionResult> UpdateVaccineType(int key, VaccineType vaccineType)
        {
            try
            {
                if (key != vaccineType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                vaccineType.DateModified = DateTime.Now;
                vaccineType.IsDeleted = false;
                vaccineType.IsSynced = false;

                context.VaccineTypeRepository.Update(vaccineType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVaccineType", "VaccineTypeController.cs", ex.Message, vaccineType.ModifiedIn, vaccineType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VaccineTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVaccineType)]
        public async Task<IActionResult> DeleteVaccineType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vaccineTypeInDb = await context.VaccineTypeRepository.GetVaccineTypeByKey(key);

                if (vaccineTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                vaccineTypeInDb.DateModified = DateTime.Now;
                vaccineTypeInDb.IsDeleted = true;
                vaccineTypeInDb.IsSynced = false;

                context.VaccineTypeRepository.Update(vaccineTypeInDb);
                await context.SaveChangesAsync();

                return Ok(vaccineTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVaccineType", "VaccineTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }


        }
    }
}