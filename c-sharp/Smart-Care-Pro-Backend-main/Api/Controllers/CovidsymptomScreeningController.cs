using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 19.02.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// CovidsymptomScreening controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CovidsymptomScreeningController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CovidsymptomScreeningController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CovidsymptomScreeningController(IUnitOfWork context, ILogger<CovidsymptomScreeningController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/covid-symptom-screening
        /// </summary>
        /// <param name="covidsymptomScreening">CovidsymptomScreening object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCovidsymptomScreening)]
        public async Task<IActionResult> CreateCovidsymptomScreening(CovidSymptomScreening covidsymptomScreening)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.CovidSymptomScreening, covidsymptomScreening.EncounterType);
                interaction.EncounterId = covidsymptomScreening.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = covidsymptomScreening.CreatedBy;
                interaction.CreatedIn = covidsymptomScreening.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                covidsymptomScreening.InteractionId = interactionId;

                context.CovidSymptomScreeningRepository.Add(covidsymptomScreening);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCovidsymptomScreeningByKey", new { key = covidsymptomScreening.InteractionId }, covidsymptomScreening);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCovidsymptomScreening", "CovidsymptomScreeningController.cs", ex.Message, covidsymptomScreening.CreatedIn, covidsymptomScreening.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom-screenings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidsymptomScreenings)]
        public async Task<IActionResult> ReadCovidsymptomScreenings()
        {
            try
            {
                var covidsymptomScreeningInDb = await context.CovidSymptomScreeningRepository.GetCovidSymptomScreenings();
                covidsymptomScreeningInDb = covidsymptomScreeningInDb.OrderByDescending(x => x.DateCreated);
                return Ok(covidsymptomScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidsymptomScreenings", "CovidsymptomScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom-screening/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidsymptomScreenings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidsymptomScreeningByKey)]
        public async Task<IActionResult> ReadCovidsymptomScreeningByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidsymptomScreeningInDb = await context.CovidSymptomScreeningRepository.GetCovidSymptomScreeningByKey(key);

                if (covidsymptomScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(covidsymptomScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidsymptomScreeningByKey", "CovidsymptomScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidsymptomScreenings.</param>
        /// <param name="covidsymptomScreening">CovidsymptomScreening to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCovidsymptomScreening)]
        public async Task<IActionResult> UpdateCovidsymptomScreening(Guid key, CovidSymptomScreening covidsymptomScreening)
        {
            try
            {
                if (key != covidsymptomScreening.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = covidsymptomScreening.ModifiedBy;
                interactionInDb.ModifiedIn = covidsymptomScreening.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                covidsymptomScreening.IsSynced = false;
                covidsymptomScreening.IsDeleted = false;
                covidsymptomScreening.DateModified = DateTime.Now;

                context.CovidSymptomScreeningRepository.Update(covidsymptomScreening);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCovidsymptomScreening", "CovidsymptomScreeningController.cs", ex.Message, covidsymptomScreening.ModifiedIn, covidsymptomScreening.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidsymptomScreenings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCovidsymptomScreening)]
        public async Task<IActionResult> DeleteCovidsymptomScreening(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidsymptomScreeningInDb = await context.CovidSymptomScreeningRepository.GetCovidSymptomScreeningByKey(key);

                if (covidsymptomScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.CovidSymptomScreeningRepository.Update(covidsymptomScreeningInDb);
                await context.SaveChangesAsync();

                return Ok(covidsymptomScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCovidsymptomScreening", "CovidsymptomScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}