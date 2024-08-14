using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// HIVRiskFactor controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HIVRiskFactorController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HIVRiskFactorController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HIVRiskFactorController(IUnitOfWork context, ILogger<HIVRiskFactorController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-factor
        /// </summary>
        /// <param name="hivRiskFactor">HIVRiskFactor object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHIVRiskFactor)]
        public async Task<IActionResult> CreateHIVRiskFactor(HIVRiskFactor hivRiskFactor)
        {
            try
            {
                if (await IsHIVRiskFactorDuplicate(hivRiskFactor) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                hivRiskFactor.DateCreated = DateTime.Now;
                hivRiskFactor.IsDeleted = false;
                hivRiskFactor.IsSynced = false;

                context.HIVRiskFactorRepository.Add(hivRiskFactor);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadHIVRiskFactorByKey", new { key = hivRiskFactor.Oid }, hivRiskFactor);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHIVRiskFactor", "HIVRiskFactorController.cs", ex.Message, hivRiskFactor.CreatedIn, hivRiskFactor.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-factors
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskFactors)]
        public async Task<IActionResult> ReadHIVRiskFactors()
        {
            try
            {
                var hivRiskFactor = await context.HIVRiskFactorRepository.GetHIVRiskFactors();

                return Ok(hivRiskFactor);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskFactors", "HIVRiskFactorController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-factor/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskFactors.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskFactorByKey)]
        public async Task<IActionResult> ReadHIVRiskFactorByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hivRiskFactor = await context.HIVRiskFactorRepository.GetHIVRiskFactorByKey(key);

                if (hivRiskFactor == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(hivRiskFactor);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskFactorByKey", "HIVRiskFactorController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-factor/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskFactors.</param>
        /// <param name="hivRiskfactor">HIVRiskFactor to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHIVRiskFactor)]
        public async Task<IActionResult> UpdateHIVRiskFactor(int key, HIVRiskFactor hivRiskfactor)
        {
            try
            {
                if (key != hivRiskfactor.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsHIVRiskFactorDuplicate(hivRiskfactor) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                hivRiskfactor.DateModified = DateTime.Now;
                hivRiskfactor.IsDeleted = false;
                hivRiskfactor.IsSynced = false;

                context.HIVRiskFactorRepository.Update(hivRiskfactor);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHIVRiskFactor", "HIVRiskFactorController.cs", ex.Message, hivRiskfactor.ModifiedIn, hivRiskfactor.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-factor/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskFactors.</param>
        /// <returns>Http status code: OK.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHIVRiskFactor)]
        public async Task<IActionResult> DeleteHIVRiskFactor(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hivRiskFactorInDb = await context.HIVRiskFactorRepository.GetHIVRiskFactorByKey(key);

                if (hivRiskFactorInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                hivRiskFactorInDb.DateModified = DateTime.Now;
                hivRiskFactorInDb.IsDeleted = true;
                hivRiskFactorInDb.IsSynced = false;

                context.HIVRiskFactorRepository.Update(hivRiskFactorInDb);
                await context.SaveChangesAsync();

                return Ok(hivRiskFactorInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHIVRiskFactor", "HIVRiskFactorController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the HIV risk factor is duplicate or not.
        /// </summary>
        /// <param name="hivRiskFactor">HIVRiskFactor object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsHIVRiskFactorDuplicate(HIVRiskFactor hivRiskFactor)
        {
            try
            {
                var hivRiskFactorInDb = await context.HIVRiskFactorRepository.GetHIVRiskFactorByRiskFactor(hivRiskFactor.Description);

                if (hivRiskFactorInDb != null)
                    if (hivRiskFactorInDb.Oid != hivRiskFactor.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsHIVRiskFactorDuplicate", "HIVRiskFactorController.cs", ex.Message);
                throw;
            }
        }
    }
}