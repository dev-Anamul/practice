using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
    /// <summary>
    /// RiskAssessment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class RiskAssessmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<RiskAssessmentController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public RiskAssessmentController(IUnitOfWork context, ILogger<RiskAssessmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/risk-assessment
        /// </summary>
        /// <param name="riskAssessment">RiskAssessment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateRiskAssessment)]
        public async Task<IActionResult> CreateRiskAssessment(RiskAssessment riskAssessment)
        {
            try
            {
                riskAssessment.DateCreated = DateTime.Now;
                riskAssessment.IsDeleted = false;
                riskAssessment.IsSynced = false;

                var riskAssessmentInDb = context.RiskAssessmentRepository.Add(riskAssessment);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadRiskAssessmentByKey", new { key = riskAssessment.Oid }, riskAssessment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateRiskAssessment", "RiskAssessmentController.cs", ex.Message, riskAssessment.CreatedIn, riskAssessment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRiskAssessments)]
        public async Task<IActionResult> ReadRiskAssessments()
        {
            try
            {
                var riskAssessment = await context.RiskAssessmentRepository.GetRiskAssessments();

                return Ok(riskAssessment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRiskAssessments", "RiskAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessment/key/{key}
        /// </summary>
        /// <param name="key">Foreign key of the table RiskAssessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRiskAssessmentByKey)]
        public async Task<IActionResult> ReadRiskAssessmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var riskAssessment = await context.RiskAssessmentRepository.GetRiskAssessmentByKey(key);

                if (riskAssessment == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(riskAssessment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRiskAssessmentByKey", "RiskAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessment/riskassessmentbyhts/{HTSID}
        /// </summary>
        /// <param name="HTSID">Foreign key of the table RiskAssesments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRiskAssessmentByHTS)]
        public async Task<IActionResult> ReadRiskAssesmentByHTS(Guid HTSID)
        {
            try
            {
                if (HTSID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var htsreferredtoInDb = await context.RiskAssessmentRepository.LoadListWithChildAsync<IEnumerable<RiskAssessment>>(x => x.HTSId == HTSID, p => p.HIVRiskFactor);

                if (htsreferredtoInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(htsreferredtoInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRiskAssesmentByHTS", "RiskAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RiskAssessments.</param>
        /// <param name="riskAssessment">RiskAssessment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateRiskAssessment)]
        public async Task<IActionResult> UpdateRiskAssessment(Guid key, RiskAssessment riskAssessment)
        {
            try
            {
                if (key == riskAssessment.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                riskAssessment.DateModified = DateTime.Now;
                riskAssessment.IsDeleted = false;
                riskAssessment.IsSynced = false;

                context.RiskAssessmentRepository.Update(riskAssessment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateRiskAssessment", "RiskAssessmentController.cs", ex.Message, riskAssessment.ModifiedIn, riskAssessment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RiskAssessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRiskAssessment)]
        public async Task<IActionResult> DeleteRiskAssessment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var riskAssessmentInDb = await context.RiskAssessmentRepository.GetRiskAssessmentByKey(key);

                if (riskAssessmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                riskAssessmentInDb.DateModified = DateTime.Now;
                riskAssessmentInDb.IsDeleted = true;
                riskAssessmentInDb.IsSynced = false;

                context.RiskAssessmentRepository.Update(riskAssessmentInDb);
                await context.SaveChangesAsync();

                return Ok(riskAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteRiskAssessment", "RiskAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}