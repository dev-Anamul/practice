using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 03.05.2023
 * Modified by   : Brian
 * Last modified : 03.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Allergy controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class STIRiskController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<STIRiskController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public STIRiskController(IUnitOfWork context, ILogger<STIRiskController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/sti-risk
        /// </summary>
        /// <param name="sTIRisk">STIRisk object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSTIRisk)]
        public async Task<ActionResult<STIRisk>> CreateSTIRisk(STIRisk sTIRisk)
        {
            try
            {
                if (await IsSTIRiskDuplicate(sTIRisk) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                sTIRisk.DateCreated = DateTime.Now;
                sTIRisk.IsDeleted = false;
                sTIRisk.IsSynced = false;

                context.STIRiskRepository.Add(sTIRisk);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSTIRiskByKey", new { key = sTIRisk.Oid }, sTIRisk);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSTIRisk", "STIRiskController.cs", ex.Message, sTIRisk.CreatedIn, sTIRisk.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/sti-risks
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSTIRisks)]
        public async Task<IActionResult> ReadSTIRisks()
        {
            try
            {
                var stiRiskInDb = await context.STIRiskRepository.GetSTIRisks();

                return Ok(stiRiskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSTIRisks", "STIRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/sti-risk/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table STIRisk.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSTIRiskByKey)]
        public async Task<IActionResult> ReadSTIRiskByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var sTIRiskInDb = await context.STIRiskRepository.GetSTIRiskByKey(key);

                if (sTIRiskInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(sTIRiskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSTIRiskByKey", "STIRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/sti-risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table STIRisks.</param>
        /// <param name="STIRisk">STIRisk to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSTIRisk)]
        public async Task<IActionResult> UpdateSTIRisk(int key, STIRisk sTIRisk)
        {
            try
            {
                if (key != sTIRisk.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                sTIRisk.DateModified = DateTime.Now;
                sTIRisk.IsDeleted = false;
                sTIRisk.IsSynced = false;

                context.STIRiskRepository.Update(sTIRisk);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSTIRisk", "STIRiskController.cs", ex.Message, sTIRisk.ModifiedIn, sTIRisk.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/sti-risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table STIRisks.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSTIRisk)]
        public async Task<IActionResult> DeleteSTIRisk(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var sTIRiskInDb = await context.STIRiskRepository.GetSTIRiskByKey(key);

                if (sTIRiskInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                sTIRiskInDb.DateModified = DateTime.Now;
                sTIRiskInDb.IsDeleted = true;
                sTIRiskInDb.IsSynced = false;

                context.STIRiskRepository.Update(sTIRiskInDb);
                await context.SaveChangesAsync();

                return Ok(sTIRiskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSTIRisk", "STIRiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the STIRisk name is duplicate or not.
        /// </summary>
        /// <param name="STIRisk">STIRisk object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsSTIRiskDuplicate(STIRisk sTIRisk)
        {
            try
            {
                var sTIRiskInDb = await context.STIRiskRepository.GetSTIRiskByName(sTIRisk.Description);

                if (sTIRiskInDb != null)
                    if (sTIRiskInDb.Oid != sTIRisk.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsSTIRiskDuplicate", "STIRiskController.cs", ex.Message);
                throw;
            }
        }
    }
}