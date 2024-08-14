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
    /// NTGLevelOneDiagnosis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NTGLevelOneDiagnosisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NTGLevelOneDiagnosisController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NTGLevelOneDiagnosisController(IUnitOfWork context, ILogger<NTGLevelOneDiagnosisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/ntg-level-one-diagnoses
        /// </summary>
        /// <param name="nTgLevelOneDiagnosis">NTGLevelOneDiagnosis object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNTGLevelOneDiagnosis)]
        public async Task<ActionResult<NTGLevelOneDiagnosis>> CreateNTGLevelOneDiagnosis(NTGLevelOneDiagnosis nTgLevelOneDiagnosis)
        {
            try
            {
                if (await IsNTGLevelOneDiagnosisDuplicate(nTgLevelOneDiagnosis) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                nTgLevelOneDiagnosis.DateCreated = DateTime.Now;
                nTgLevelOneDiagnosis.IsDeleted = false;
                nTgLevelOneDiagnosis.IsSynced = false;

                context.NTGLevelOneDiagnosisRepository.Add(nTgLevelOneDiagnosis);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNTGLevelOneDiagnosisByKey", new { key = nTgLevelOneDiagnosis.Oid }, nTgLevelOneDiagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNTGLevelOneDiagnosis", "NTGLevelOneDiagnosisController.cs", ex.Message, nTgLevelOneDiagnosis.CreatedIn, nTgLevelOneDiagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-one-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelOneDiagnoses)]
        public async Task<IActionResult> ReadNTGLevelOneDiagnoses()
        {
            try
            {
                var ntgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnoses();

                foreach (var item in ntgLevelOneDiagnosisInDb)
                {
                    foreach (var item2 in item.NTGLevelTwoDiagnoses)
                    {
                        item2.NTGLevelThreeDiagnoses = await context.NTGLevelThreeDiagnosisRepository.LoadListWithChildAsync<NTGLevelThreeDiagnosis>(x => x.IsDeleted == false && x.NTGLevelTwoId == item2.Oid);
                    }
                }

                return Ok(ntgLevelOneDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateNTGLevelOneDiagnosis", "NTGLevelOneDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-one-diagnoses/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelOneDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNTGLevelOneDiagnosisByKey)]
        public async Task<IActionResult> ReadNTGLevelOneDiagnosisByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nTgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnosisByKey(key);

                if (nTgLevelOneDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(nTgLevelOneDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNTGLevelOneDiagnosisByKey", "NTGLevelOneDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-one-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelOneDiagnosis.</param>
        /// <param name="nTgLevelOneDiagnosis">NTGLevelOneDiagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNTGLevelOneDiagnosis)]
        public async Task<IActionResult> UpdateNTGLevelOneDiagnosis(int key, NTGLevelOneDiagnosis nTgLevelOneDiagnosis)
        {
            try
            {
                if (key != nTgLevelOneDiagnosis.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nTgLevelOneDiagnosis.DateModified = DateTime.Now;
                nTgLevelOneDiagnosis.IsDeleted = false;
                nTgLevelOneDiagnosis.IsSynced = false;

                context.NTGLevelOneDiagnosisRepository.Update(nTgLevelOneDiagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNTGLevelOneDiagnosis", "NTGLevelOneDiagnosisController.cs", ex.Message, nTgLevelOneDiagnosis.ModifiedIn, nTgLevelOneDiagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ntg-level-one-diagnoses/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelOneDiagnosis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeteleNTGLevelOneDiagnosis)]
        public async Task<IActionResult> DeleteNTGLevelOneDiagnosis(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nTgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnosisByKey(key);

                if (nTgLevelOneDiagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                nTgLevelOneDiagnosisInDb.DateModified = DateTime.Now;
                nTgLevelOneDiagnosisInDb.IsDeleted = true;
                nTgLevelOneDiagnosisInDb.IsSynced = false;

                context.NTGLevelOneDiagnosisRepository.Update(nTgLevelOneDiagnosisInDb);
                await context.SaveChangesAsync();

                return Ok(nTgLevelOneDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNTGLevelOneDiagnosis", "NTGLevelOneDiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the NTGLevelOneDiagnosis name is duplicate or not.
        /// </summary>
        /// <param name="nTgLevelOneDiagnosis">NTGLevelOneDiagnosis object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsNTGLevelOneDiagnosisDuplicate(NTGLevelOneDiagnosis nTgLevelOneDiagnosis)
        {
            try
            {
                var nTgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnosisByName(nTgLevelOneDiagnosis.Description);

                if (nTgLevelOneDiagnosisInDb != null)
                    if (nTgLevelOneDiagnosisInDb.Oid != nTgLevelOneDiagnosis.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsNTGLevelOneDiagnosisDuplicate", "NTGLevelOneDiagnosisController.cs", ex.Message);
                throw;
            }
        }
    }
}