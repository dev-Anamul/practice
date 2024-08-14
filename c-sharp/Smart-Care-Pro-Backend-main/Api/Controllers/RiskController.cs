using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;


namespace Api.Controllers
{
    /// <summary>
    /// MedicineBrand controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]


    public class RiskController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<RiskController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public RiskController(IUnitOfWork context, ILogger<RiskController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/risk
        /// </summary>
        /// <param name="risk">Risk object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateRisk)]
        public async Task<ActionResult<Risks>> CreateRisk(Risks risks)
        {
            try
            {
                risks.DateCreated = DateTime.Now;
                risks.IsDeleted = false;
                risks.IsSynced = false;

                context.RiskRepository.Add(risks);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadRiskByKey", new { key = risks.Oid }, risks);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateRisk", "RiskController.cs", ex.Message, risks.CreatedIn, risks.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/risks
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRisk)]
        public async Task<IActionResult> ReadRisk()
        {
            try
            {
                var riskInDb = await context.RiskRepository.GetRisk();
                riskInDb = riskInDb.OrderByDescending(x => x.DateCreated);
                return Ok(riskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRisk", "RiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRiskByKey)]
        public async Task<IActionResult> ReadRiskByKey(int key)
        {
            try
            {
                var riskInDb = await context.RiskRepository.GetRiskByKey(key);

                return Ok(riskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRiskByKey", "RiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Risks.</param>
        /// <param name="risks">Risks to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateRisk)]
        public async Task<IActionResult> UpdateRisk(int key, Risks risks)
        {
            try
            {
                if (key != risks.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                risks.DateModified = DateTime.Now;
                risks.IsDeleted = false;
                risks.IsSynced = false;

                context.RiskRepository.Update(risks);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateRisk", "RiskController.cs", ex.Message, risks.ModifiedIn, risks.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Risk.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRisk)]
        public async Task<IActionResult> DeleteRisk(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var riskInDb = await context.RiskRepository.GetRiskByKey(key);

                if (riskInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                riskInDb.DateModified = DateTime.Now;
                riskInDb.IsDeleted = true;
                riskInDb.IsSynced = false;

                context.RiskRepository.Update(riskInDb);
                await context.SaveChangesAsync();

                return Ok(riskInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteRisk", "RiskController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}
