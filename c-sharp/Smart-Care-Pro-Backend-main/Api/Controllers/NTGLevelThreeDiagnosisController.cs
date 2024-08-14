using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Tomas
 * Date created  : 04.01.2022
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// NTGLevelThreeDiagnosis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NTGLevelThreeDiagnosisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NTGLevelThreeDiagnosisController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NTGLevelThreeDiagnosisController(IUnitOfWork context, ILogger<NTGLevelThreeDiagnosisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnoses
        /// </summary>
        /// <param name="nTgLevelThreeDiagnosis">NTGLevelThreeDiagnosis object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNTGLevelThreeDiagnosis)]
        public async Task<ActionResult<NTGLevelThreeDiagnosis>> CreateNTGLevelThreeDiagnosis(NTGLevelThreeDiagnosis nTgLevelThreeDiagnosis)
        {
            try
            {
                if (await IsNTGLevelThreeDiagnosisDuplicate(nTgLevelThreeDiagnosis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                nTgLevelThreeDiagnosis.DateCreated = DateTime.Now;
                nTgLevelThreeDiagnosis.IsDeleted = false;
                nTgLevelThreeDiagnosis.IsSynced = false;

                context.NTGLevelThreeDiagnosisRepository.Add(nTgLevelThreeDiagnosis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNTGLevelThreeDiagnosisByKey", new { key = nTgLevelThreeDiagnosis.Oid }, nTgLevelThreeDiagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNTGLevelThreeDiagnosis", "NTGLevelThreeDiagnosisController.cs", ex.Message, nTgLevelThreeDiagnosis.CreatedIn, nTgLevelThreeDiagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelThreeDiagnoses)]
        public async Task<IActionResult> ReadNTGLevelThreeDiagnoses()
        {
            try
            {
                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnoses();

                return Ok(ntgLevelThreeDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelThreeDiagnoses", "NTGLevelThreeDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnosis/ntg-level-three-by-ntg-level-two-diagnosis/{NTGLevelTwoID}
        /// </summary>
        /// <param name="nTgLevelTwoId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis)]
        public async Task<IActionResult> ReadNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis(int nTgLevelTwoId)
        {
            try
            {
                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis(nTgLevelTwoId);

                return Ok(ntgLevelThreeDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis", "NTGLevelThreeDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnoses/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelThreeDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelThreeDiagnosisByKey)]
        public async Task<IActionResult> ReadNTGLevelThreeDiagnosisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnosisByKey(key);

                if (ntgLevelThreeDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(ntgLevelThreeDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelThreeDiagnosisByKey", "NTGLevelThreeDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelThreeDiagnosis.</param>
        /// <param name="nTgLevelThreeDiagnosis">NTGLevelThreeDiagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNTGLevelThreeDiagnosis)]
        public async Task<IActionResult> UpdateNTGLevelThreeDiagnosis(int key, NTGLevelThreeDiagnosis nTgLevelThreeDiagnosis)
        {
            try
            {
                if (key != nTgLevelThreeDiagnosis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nTgLevelThreeDiagnosis.DateModified = DateTime.Now;
                nTgLevelThreeDiagnosis.IsDeleted = false;
                nTgLevelThreeDiagnosis.IsSynced = false;

                context.NTGLevelThreeDiagnosisRepository.Update(nTgLevelThreeDiagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNTGLevelThreeDiagnosis", "NTGLevelThreeDiagnosisController.cs", ex.Message, nTgLevelThreeDiagnosis.ModifiedIn, nTgLevelThreeDiagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ntg-level-three-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelThreeDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeteleNTGLevelThreeDiagnosis)]
        public async Task<IActionResult> DeteleNTGLevelThreeDiagnosis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnosisByKey(key);

                if (ntgLevelThreeDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                ntgLevelThreeDiagnosisInDb.DateModified = DateTime.Now;
                ntgLevelThreeDiagnosisInDb.IsDeleted = true;
                ntgLevelThreeDiagnosisInDb.IsSynced = false;

                context.NTGLevelThreeDiagnosisRepository.Update(ntgLevelThreeDiagnosisInDb);
                await context.SaveChangesAsync();

                return Ok(ntgLevelThreeDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeteleNTGLevelThreeDiagnosis", "NTGLevelThreeDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// Checks whether the NTGLevelThreeDiagnosis name is duplicate or not.
        /// </summary>
        /// <param name="nTgLevelThreeDiagnosis">NTGLevelThreeDiagnosis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsNTGLevelThreeDiagnosisDuplicate(NTGLevelThreeDiagnosis nTgLevelThreeDiagnosis)
        {
            try
            {
                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnosesByName(nTgLevelThreeDiagnosis.Description);

                if (ntgLevelThreeDiagnosisInDb != null)
                    if (ntgLevelThreeDiagnosisInDb.Oid != ntgLevelThreeDiagnosisInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsNTGLevelThreeDiagnosisDuplicate", "NTGLevelThreeDiagnosisController.cs", ex.Message);
                throw;
            }
        }
    }
}
