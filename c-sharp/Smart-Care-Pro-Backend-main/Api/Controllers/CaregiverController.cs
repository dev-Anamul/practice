using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by: Bella
 * Date created: 12.09.2022
 * Modified by: Lion
 * Last modified: 06.11.2022
 */

namespace Api.Controllers
{
    /// <summary>
    /// Caregiver controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CaregiverController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CaregiverController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CaregiverController(IUnitOfWork context, ILogger<CaregiverController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/caregiver
        /// </summary>
        /// <param name="caregiver">Caregiver object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCaregiver)]
        public async Task<IActionResult> CreateCaregiver(Caregiver caregiver)
        {
            try
            {
                if (await IsCaregiverDuplicate(caregiver) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                if (caregiver.CountryCode == "260" && caregiver.Cellphone[0] == '0')
                    caregiver.Cellphone = caregiver.Cellphone.Substring(1);

                caregiver.DateCreated = DateTime.Now;
                caregiver.IsDeleted = false;
                caregiver.IsSynced = false;

                context.CaregiverRepository.Add(caregiver);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCaregiverByKey", new { key = caregiver.Oid }, caregiver);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}",
                    DateTime.Now, "BusinessLayer", "CreateCaregiver", "CaregiverController.cs", ex.Message,
                    caregiver.CreatedIn, caregiver.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/caregivers
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCaregivers)]
        public async Task<IActionResult> ReadCaregivers()
        {
            try
            {
                var caregiverIndb = await context.CaregiverRepository.GetCaregivers();

                caregiverIndb = caregiverIndb.OrderByDescending(x => x.DateCreated);

                return Ok(caregiverIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCaregivers", "CaregiverController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/caregiver/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Caregivers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCaregiverByKey)]
        public async Task<IActionResult> ReadCaregiverByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var caregiverIndb = await context.CaregiverRepository.GetCaregiverByKey(key);

                if (caregiverIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(caregiverIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCaregiverByKey", "CaregiverController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/caregiver/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Caregivers.</param>
        /// <param name="caregiver">Caregiver to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCaregiver)]
        public async Task<IActionResult> UpdateCaregiver(Guid key, Caregiver caregiver)
        {
            try
            {
                if (key != caregiver.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCaregiverDuplicate(caregiver) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                caregiver.DateModified = DateTime.Now;
                caregiver.IsDeleted = false;
                caregiver.IsSynced = false;

                context.CaregiverRepository.Update(caregiver);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCaregiver", "CaregiverController.cs", ex.Message, caregiver.ModifiedIn, caregiver.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/caregiver/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Caregivers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCaregiver)]
        public async Task<IActionResult> DeleteCaregiver(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var caregiverInDb = await context.CaregiverRepository.GetCaregiverByKey(key);

                if (caregiverInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                caregiverInDb.DateModified = DateTime.Now;
                caregiverInDb.IsDeleted = true;
                caregiverInDb.IsSynced = false;

                context.CaregiverRepository.Update(caregiverInDb);
                await context.SaveChangesAsync();

                return Ok(caregiverInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCaregiver", "CaregiverController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the caregiver name is duplicate or not. 
        /// </summary>
        /// <param name="caregiver">Caregiver object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCaregiverDuplicate(Caregiver caregiver)
        {
            try
            {
                var caregiverInDb = await context.CaregiverRepository.GetCaregiverByName(caregiver.FirstName);

                if (caregiverInDb != null)
                    if (caregiverInDb.Oid != caregiver.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCaregiverDuplicate", "CaregiverController.cs", ex.Message);
                throw;
            }
        }
    }
}