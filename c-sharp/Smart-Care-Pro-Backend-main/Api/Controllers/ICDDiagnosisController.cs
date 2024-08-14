using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 04.01.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ICDDiagnosisController controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ICDDiagnosisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ICDDiagnosisController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ICDDiagnosisController(IUnitOfWork context, ILogger<ICDDiagnosisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/icd-diagnoses
        /// </summary>
        /// <param name="iCDDiagnosis">ICDDiagnosis object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateICDDiagnosis)]
        public async Task<ActionResult<ICDDiagnosis>> CreateICDDiagnosis(ICDDiagnosis iCDDiagnosis)
        {
            try
            {
                if (await IsICDDiagnosisDuplicate(iCDDiagnosis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                iCDDiagnosis.DateCreated = DateTime.Now;
                iCDDiagnosis.IsDeleted = false;
                iCDDiagnosis.IsSynced = false;

                context.ICDDiagnosisRepository.Add(iCDDiagnosis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadICDDiagnosisByKey", new { key = iCDDiagnosis.Oid }, iCDDiagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateICDDiagnosis", "ICDDiagnosisController.cs", ex.Message, iCDDiagnosis.CreatedIn, iCDDiagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/icd-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICDDiagnoses)]
        public async Task<IActionResult> ReadICDDiagnoses()
        {
            try
            {
                var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnoses();

                return Ok(icdDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICDDiagnoses", "ICDDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/icd-diagnosis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICDDiagnosis By Key.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICDDiagnosisByKey)]
        public async Task<IActionResult> ReadICDDiagnosisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnosisByKey(key);

                if (icdDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(icdDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICDDiagnosisByKey", "ICDDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpGet]
        [Route(RouteConstants.ReadICDDiagnosesSearch)]
        public async Task<IActionResult> ReadICDDiagnosesSearch(string searchTerm)
        {
            try
            {
                var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnosesBySearchTerm(searchTerm);

                return Ok(icdDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICDDiagnosesSearch", "ICDDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/icd-diagnosis/icpc/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICDDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICDDiagnosisByICP2)]
        public async Task<IActionResult> ReadICDDiagnosesByICPC2(int key)
        {
            try
            {
                var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnosesByICPC2(key);

                return Ok(icdDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICDDiagnosesByICPC2", "ICDDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/icd-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICDDiagnosis.</param>
        /// <param name="iCDDiagnosis">ICDDiagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateICDDiagnosis)]
        public async Task<IActionResult> UpdateICDDiagnosis(int key, ICDDiagnosis iCDDiagnosis)
        {
            try
            {
                if (key != iCDDiagnosis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                iCDDiagnosis.DateModified = DateTime.Now;
                iCDDiagnosis.IsDeleted = false;
                iCDDiagnosis.IsSynced = false;

                context.ICDDiagnosisRepository.Update(iCDDiagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateICDDiagnosis", "ICDDiagnosisController.cs", ex.Message, iCDDiagnosis.ModifiedIn, iCDDiagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/icd-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICDDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeteleICDDiagnosis)]
        public async Task<IActionResult> DeleteICDDiagnosis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var iCDDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnosisByKey(key);

                if (iCDDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                iCDDiagnosisInDb.DateModified = DateTime.Now;
                iCDDiagnosisInDb.IsDeleted = true;
                iCDDiagnosisInDb.IsSynced = false;

                context.ICDDiagnosisRepository.Update(iCDDiagnosisInDb);
                await context.SaveChangesAsync();

                return Ok(iCDDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteICDDiagnosis", "ICDDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ICDDiagnosis name is duplicate or not.
        /// </summary>
        /// <param name="iCDDiagnosis">ICDDiagnosis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsICDDiagnosisDuplicate(ICDDiagnosis iCDDiagnosis)
        {
            try
            {
                var iCDDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnosisByName(iCDDiagnosis.Description);

                if (iCDDiagnosisInDb != null)
                    if (iCDDiagnosisInDb.Oid != iCDDiagnosis.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsICDDiagnosisDuplicate", "ICDDiagnosisController.cs", ex.Message);
                throw;
            }
        }
    }
}