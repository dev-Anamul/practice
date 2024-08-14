using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 03.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Client controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class PatientTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PatientTypeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PatientTypeController(IUnitOfWork context, ILogger<PatientTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        #region Create
        /// <summary>
        /// URL: sc-api/patient-type
        /// </summary>
        /// <param name="armedForceService">PatientType object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePatientType)]
        public async Task<IActionResult> CreatePatientType(DFZPatientType patientType)
        {
            try
            {
                if (await IsPatientTypeDuplicate(patientType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                patientType.DateCreated = DateTime.Now;
                patientType.IsDeleted = false;
                patientType.IsSynced = false;

                context.PatientTypeRepository.Add(patientType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadArmedForceServiceByKey", new { key = patientType.Oid }, patientType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePatientType", "PatientTypeController.cs", ex.Message, patientType.CreatedIn, patientType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// URL: sc-api/patient-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPatientTypes)]
        public async Task<IActionResult> ReadPatientTypes()
        {
            try
            {
                var patientTypeIndb = await context.PatientTypeRepository.GetPatientTypes();

                return Ok(patientTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientTypes", "PatientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/patient-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PatientType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPatientTypeByKey)]
        public async Task<IActionResult> ReadPatientTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var patientTypeIndb = await context.PatientTypeRepository.GetPatientTypeByKey(key);

                if (patientTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(patientTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientTypeByKey", "PatientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/patient-type-by-armedforce/{armedForceId}
        /// </summary>
        /// <param name="armedForceId">Foreign key of the table PatientType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPatientTypeByArmedForceService)]
        public async Task<IActionResult> ReadPatientTypeByArmedForceService(int armedForceId)
        {
            try
            {
                var patientTypeIndb = await context.PatientTypeRepository.GetPatientTypeByArmedForce(armedForceId);

                return Ok(patientTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientTypeByArmedForceService", "PatientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// URL: sc-api/patient-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PatientType.</param>
        /// <param name="patientType">PatientType to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePatientType)]
        public async Task<IActionResult> UpdatePatientType(int key, DFZPatientType patientType)
        {
            try
            {
                if (key != patientType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsPatientTypeDuplicate(patientType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                patientType.DateModified = DateTime.Now;
                patientType.IsDeleted = false;
                patientType.IsSynced = false;

                context.PatientTypeRepository.Update(patientType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePatientType", "PatientTypeController.cs", ex.Message, patientType.ModifiedIn, patientType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// URL: sc-api/patient-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PatientType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePatientType)]
        public async Task<IActionResult> DeletePatientType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var patientTypeIndb = await context.PatientTypeRepository.GetPatientTypeByKey(key);

                if (patientTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                patientTypeIndb.DateModified = DateTime.Now;
                patientTypeIndb.IsDeleted = true;
                patientTypeIndb.IsSynced = false;
                
                context.PatientTypeRepository.Update(patientTypeIndb);
                await context.SaveChangesAsync();

                return Ok(patientTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePatientType", "PatientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Duplicate check
        /// <summary>
        /// Checks whether the ArmedForceService name is duplicate or not. 
        /// </summary>
        /// <param name="country">ArmedForceService object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPatientTypeDuplicate(DFZPatientType patientType)
        {
            try
            {
                var patientTypeInDb = await context.PatientTypeRepository.GetPatientTypeByName(patientType.Description);

                if (patientTypeInDb != null)
                    if (patientTypeInDb.Oid != patientTypeInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPatientTypeDuplicate", "PatientTypeController.cs", ex.Message);
                throw;
            }
        }
        #endregion
    }
}