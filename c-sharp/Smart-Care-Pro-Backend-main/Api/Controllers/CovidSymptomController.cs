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
    /// CovidSymptom controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CovidSymptomController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CovidSymptomController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CovidSymptomController(IUnitOfWork context, ILogger<CovidSymptomController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/covid-symptom
        /// </summary>
        /// <param name="covidSymptom">CovidSymptom object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCovidSymptom)]
        public async Task<ActionResult<CovidSymptom>> CreateCovidSymptom(CovidSymptom covidSymptom)
        {
            try
            {
                if (await IsCovidSymptomDuplicate(covidSymptom) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                covidSymptom.DateCreated = DateTime.Now;
                covidSymptom.IsDeleted = false;
                covidSymptom.IsSynced = false;

                context.CovidSymptomRepository.Add(covidSymptom);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCovidSymptomByKey", new { key = covidSymptom.Oid }, covidSymptom);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCovidSymptom", "CovidSymptomController.cs", ex.Message, covidSymptom.CreatedIn, covidSymptom.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptoms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidSymptoms)]
        public async Task<IActionResult> ReadCovidSymptoms()
        {
            try
            {
                var covidSymptomInDb = await context.CovidSymptomRepository.GetCovidSymptoms();
                covidSymptomInDb = covidSymptomInDb.OrderByDescending(x => x.DateCreated);
                return Ok(covidSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidSymptoms", "CovidSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidSymptomByKey)]
        public async Task<IActionResult> ReadCovidSymptomByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidSymptomInDb = await context.CovidSymptomRepository.GetCovidSymptomByKey(key);

                if (covidSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(covidSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidSymptomByKey", "CovidSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidSymptoms.</param>
        /// <param name="covidSymptom">CovidSymptom to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCovidSymptom)]
        public async Task<IActionResult> UpdateCovidSymptom(int key, CovidSymptom covidSymptom)
        {
            try
            {
                if (key != covidSymptom.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                covidSymptom.DateModified = DateTime.Now;
                covidSymptom.IsDeleted = false;
                covidSymptom.IsSynced = false;

                context.CovidSymptomRepository.Update(covidSymptom);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCovidSymptom", "CovidSymptomController.cs", ex.Message, covidSymptom.ModifiedIn, covidSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CovidSymptoms.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCovidSymptom)]
        public async Task<IActionResult> DeleteCovidSymptom(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidSymptomInDb = await context.CovidSymptomRepository.GetCovidSymptomByKey(key);

                if (covidSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                covidSymptomInDb.DateModified = DateTime.Now;
                covidSymptomInDb.IsDeleted = true;
                covidSymptomInDb.IsSynced = false;

                context.CovidSymptomRepository.Update(covidSymptomInDb);
                await context.SaveChangesAsync();

                return Ok(covidSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCovidSymptom", "CovidSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the CovidSymptom name is duplicate or not.
        /// </summary>
        /// <param name="CovidSymptom">CovidSymptom object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCovidSymptomDuplicate(CovidSymptom covidSymptom)
        {
            try
            {
                var covidSymptomInDb = await context.CovidSymptomRepository.GetCovidSymptomByName(covidSymptom.Description);

                if (covidSymptomInDb != null)
                    if (covidSymptomInDb.Oid != covidSymptom.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCovidSymptomDuplicate", "CovidSymptomController.cs", ex.Message);
                throw;
            }
        }
    }
}