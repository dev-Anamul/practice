using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   :    Stephan
 * Date created :    19.01.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date Reviewed:
*/
namespace Api.Controllers
{
    /// <summary>
    /// Covid controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CovidComobidityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CovidComobidityController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CovidComobidityController(IUnitOfWork context, ILogger<CovidComobidityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/comobidity
        /// </summary>
        /// <param name="covid">Covid Comobidity object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateComobidity)]
        public async Task<IActionResult> CreateComobidity(CovidComorbidity covidComorbidity)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.CovidComorbidity, covidComorbidity.EncounterType);
                interaction.EncounterId = covidComorbidity.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = covidComorbidity.CreatedBy;
                interaction.CreatedIn = covidComorbidity.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);
                covidComorbidity.InteractionId = interactionId;
                covidComorbidity.DateCreated = DateTime.Now;
                covidComorbidity.IsDeleted = false;
                covidComorbidity.IsSynced = false;

                context.CovidComorbidityRepository.Add(covidComorbidity);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadComobidityByKey", new { key = covidComorbidity.InteractionId }, covidComorbidity);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateComobidity", "CovidComobidityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/comobidities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComobidities)]
        public async Task<IActionResult> ReadComobidities()
        {
            try
            {
                var covidComobidityInDb = await context.CovidComorbidityRepository.GetCovidComorbidities();

                covidComobidityInDb = covidComobidityInDb.OrderByDescending(x => x.DateCreated);

                return Ok(covidComobidityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComobidities", "CovidComobidityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        // <summary>
        /// URL: sc-api/comobidity/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Covid Comobidities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComobidityByKey)]
        public async Task<IActionResult> ReadComobidityByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidCopmobidityInDb = await context.CovidComorbidityRepository.GetCovidComobidityByKey(key);

                if (covidCopmobidityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(covidCopmobidityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComobidityByKey", "CovidComobidityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}