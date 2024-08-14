using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 19.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Exposure controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ExposureController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ExposureController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ExposureController(IUnitOfWork context, ILogger<ExposureController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/exposure
        /// </summary>
        /// <param name="exposure">Exposure object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateExposure)]
        public async Task<ActionResult<Exposure>> CreateExposure(Exposure exposure)
        {
            try
            {
                exposure.Oid = exposure.EncounterId;

                exposure.DateCreated = DateTime.Now;
                exposure.IsDeleted = false;
                exposure.IsSynced = false;

                context.ExposureRepository.Add(exposure);
                await context.SaveChangesAsync();

                return Ok(exposure);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateExposure", "ExposureController.cs", ex.Message, exposure.CreatedIn, exposure.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/exposures
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadExposures)]
        public async Task<IActionResult> ReadExposures()
        {
            try
            {
                var exposureInDb = await context.ExposureRepository.GetExposures();

                return Ok(exposureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadExposures", "ExposureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/exposure /{key}
        /// </summary>
        /// <param name="key">Primary key of the table Exposures.</param>
        /// <param name="exposure">Exposure to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateExposure)]
        public async Task<IActionResult> UpdateExposure(Guid key, Exposure exposure)
        {
            try
            {
                if (key != exposure.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                exposure.DateModified = DateTime.Now;
                exposure.IsDeleted = false;
                exposure.IsSynced = false;

                context.ExposureRepository.Update(exposure);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateExposure", "ExposureController.cs", ex.Message, exposure.ModifiedIn, exposure.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}