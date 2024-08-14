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
    /// NTGLevelTwoDiagnosis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NTGLevelTwoDiagnosisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NTGLevelTwoDiagnosisController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NTGLevelTwoDiagnosisController(IUnitOfWork context, ILogger<NTGLevelTwoDiagnosisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnoses
        /// </summary>
        /// <param name="nTgLevelTwoDiagnosis">NTGLevelTwoDiagnosis object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNTGLevelTwoDiagnosis)]
        public async Task<ActionResult<NTGLevelTwoDiagnosis>> CreateNTGLevelTwoDiagnosis(NTGLevelTwoDiagnosis nTgLevelTwoDiagnosis)
        {
            try
            {
                if (await IsNTGLevelTwoDiagnosisDuplicate(nTgLevelTwoDiagnosis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                nTgLevelTwoDiagnosis.DateCreated = DateTime.Now;
                nTgLevelTwoDiagnosis.IsDeleted = false;
                nTgLevelTwoDiagnosis.IsSynced = false;

                context.NTGLevelTwoDiagnosisRepository.Add(nTgLevelTwoDiagnosis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNTGLevelTwoDiagnosisByKey", new { key = nTgLevelTwoDiagnosis.Oid }, nTgLevelTwoDiagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNTGLevelTwoDiagnosis", "NTGLevelTwoDiagnosisController.cs", ex.Message, nTgLevelTwoDiagnosis.CreatedIn, nTgLevelTwoDiagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelTwoDiagnoses)]
        public async Task<IActionResult> ReadNTGLevelTwoDiagnoses()
        {
            try
            {
                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnoses();

                return Ok(ntgLevelTwoDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelTwoDiagnoses", "NTGLevelTwoDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnosis/by-ntg-level-one-diagnosis/{ntgLevelOneId}
        /// </summary>
        /// <param name="nTgLevelOneId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis)]
        public async Task<IActionResult> ReadNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis(int nTgLevelOneId)
        {
            try
            {
                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis(nTgLevelOneId);

                return Ok(ntgLevelTwoDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis", "NTGLevelTwoDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnoses/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelTwoDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelTwoDiagnosisByKey)]
        public async Task<IActionResult> ReadNTGLevelTwoDiagnosisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnosisByKey(key);

                if (ntgLevelTwoDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(ntgLevelTwoDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelTwoDiagnosisByKey", "NTGLevelTwoDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelTwoDiagnosis.</param>
        /// <param name="nTgLevelTwoDiagnosis">NTGLevelTwoDiagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNTGLevelTwoDiagnosis)]
        public async Task<IActionResult> UpdateNTGLevelTwoDiagnosis(int key, NTGLevelTwoDiagnosis nTgLevelTwoDiagnosis)
        {
            try
            {
                if (key != nTgLevelTwoDiagnosis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nTgLevelTwoDiagnosis.DateModified = DateTime.Now;
                nTgLevelTwoDiagnosis.IsDeleted = false;
                nTgLevelTwoDiagnosis.IsSynced = false;

                context.NTGLevelTwoDiagnosisRepository.Update(nTgLevelTwoDiagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNTGLevelTwoDiagnosis", "NTGLevelTwoDiagnosisController.cs", ex.Message, nTgLevelTwoDiagnosis.ModifiedIn, nTgLevelTwoDiagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ntg-level-two-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelTwoDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeteleNTGLevelTwoDiagnosis)]
        public async Task<IActionResult> DeteleNTGLevelTwoDiagnosis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnosisByKey(key);

                if (ntgLevelTwoDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                ntgLevelTwoDiagnosisInDb.DateModified = DateTime.Now;
                ntgLevelTwoDiagnosisInDb.IsDeleted = true;
                ntgLevelTwoDiagnosisInDb.IsSynced = false;

                context.NTGLevelTwoDiagnosisRepository.Update(ntgLevelTwoDiagnosisInDb);
                await context.SaveChangesAsync();

                return Ok(ntgLevelTwoDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeteleNTGLevelTwoDiagnosis", "NTGLevelTwoDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// Checks whether the NTGLevelTwoDiagnosis name is duplicate or not.
        /// </summary>
        /// <param name="nTgLevelTwoDiagnosis">NTGLevelTwoDiagnosis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsNTGLevelTwoDiagnosisDuplicate(NTGLevelTwoDiagnosis nTgLevelTwoDiagnosis)
        {
            try
            {
                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnosesByName(nTgLevelTwoDiagnosis.Description);

                if (ntgLevelTwoDiagnosisInDb != null)
                    if (ntgLevelTwoDiagnosisInDb.Oid != ntgLevelTwoDiagnosisInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsNTGLevelTwoDiagnosisDuplicate", "NTGLevelTwoDiagnosisController.cs", ex.Message);
                throw;
            }
        }


    }
}
