using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 21.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{

    /// <summary>
    /// PEPRisk controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PEPRiskController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PEPRiskController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PEPRiskController(IUnitOfWork context, ILogger<PEPRiskController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pep-risk
        /// </summary>
        /// <param name="pEPRisk">PEPRisk object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPRisk)]
        public async Task<IActionResult> CreatePEPRisk(Risks pEPRisk)
        {
            try
            {
                if (await IsPEPRiskDuplicate(pEPRisk) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                pEPRisk.DateCreated = DateTime.Now;
                pEPRisk.IsDeleted = false;
                pEPRisk.IsSynced = false;

                context.PEPRiskRepository.Add(pEPRisk);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPEPRiskByKey", new { key = pEPRisk.Oid }, pEPRisk);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPRisk", "PEPRiskController.cs", ex.Message, pEPRisk.CreatedIn, pEPRisk.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risks
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRisks)]
        public async Task<IActionResult> ReadPEPRisks()
        {
            try
            {
                var pEPRiskIndb = await context.PEPRiskRepository.GetPEPRisks();

                return Ok(pEPRiskIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPRisks", "PEPRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risk/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPRisks.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRiskByKey)]
        public async Task<IActionResult> ReadPEPRiskByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pEPRiskIndb = await context.PEPRiskRepository.GetPEPRiskByKey(key);

                if (pEPRiskIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pEPRiskIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPRiskByKey", "PEPRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPRisks.</param>
        /// <param name="pEPRisk">PEPRisk to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPRisk)]
        public async Task<IActionResult> UpdatePEPRisk(int key, Risks pEPRisk)
        {
            try
            {
                if (key != pEPRisk.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsPEPRiskDuplicate(pEPRisk) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                pEPRisk.DateModified = DateTime.Now;
                pEPRisk.IsDeleted = false;
                pEPRisk.IsSynced = false;

                context.PEPRiskRepository.Update(pEPRisk);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePEPRisk", "PEPRiskController.cs", ex.Message, pEPRisk.ModifiedIn, pEPRisk.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPRisks.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePEPRisk)]
        public async Task<IActionResult> DeletePEPRisk(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pEPRiskIndb = await context.PEPRiskRepository.GetPEPRiskByKey(key);

                if (pEPRiskIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pEPRiskIndb.DateModified = DateTime.Now;
                pEPRiskIndb.IsDeleted = true;
                pEPRiskIndb.IsSynced = false;

                context.PEPRiskRepository.Update(pEPRiskIndb);
                await context.SaveChangesAsync();

                return Ok(pEPRiskIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePEPRisk", "PEPRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PEP risk is duplicate or not. 
        /// </summary>
        /// <param name="pEPRisk">PEPRisk object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPEPRiskDuplicate(Risks pEPRisk)
        {
            try
            {
                var pEPRiskIndb = await context.PEPRiskRepository.GetPEPRiskByName(pEPRisk.Description);

                if (pEPRiskIndb != null)
                    if (pEPRiskIndb.Oid != pEPRisk.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPEPRiskDuplicate", "PEPRiskController.cs", ex.Message);
                throw;
            }
        }
    }
}