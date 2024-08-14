using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Temperatures controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TemperaturesController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TemperaturesController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TemperaturesController(IUnitOfWork context, ILogger<TemperaturesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/temperature
        /// </summary>
        /// <param name="temperatures">Temperatures object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTemperature)]
        public async Task<IActionResult> CreateTemperature(TemperaturesCreateDto temperatures)
        {
            try
            {
                var temperaturesList = temperatures.Data?.Select(x => new Temperature()
                {
                    TemperaturesDetails = Convert.ToInt32(x[1]),
                    TemperatureTime = Convert.ToInt64(x[0]),
                    PartographId = temperatures.PartographId,
                }).ToList() ?? new List<Temperature>();

                foreach (var item in temperaturesList)
                {
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.TemperaturesRepository.UpdateTemperature(item);
                }

                await context.SaveChangesAsync();

                return Ok(temperaturesList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTemperature", "TemperaturesController.cs", ex.Message, "Temperatures.CreatedIn", "Temperatures.CreatedBy");
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/temperatures
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTemperatures)]
        public async Task<IActionResult> ReadTemperatures(Guid partographId)
        {
            try
            {
                var temperaturesInDb = context.TemperaturesRepository
                    .GetAll()
                    .Where(c => c.PartographId == partographId && c.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = temperaturesInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.TemperaturesDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var temperatures = new TemperaturesCreateDto();
                if (data.Count > 0)
                {
                    temperatures = new TemperaturesCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(temperatures);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTemperatures", "TemperaturesController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}