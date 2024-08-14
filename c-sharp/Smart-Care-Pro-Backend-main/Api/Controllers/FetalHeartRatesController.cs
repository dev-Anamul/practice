using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// FetalHeartRates controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FetalHeartRatesController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FetalHeartRatesController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FetalHeartRatesController(IUnitOfWork context, ILogger<FetalHeartRatesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/fetal-heart-rate
        /// </summary>
        /// <param name="fetalHeartRates">FetalHeartRate object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFetalHeartRate)]
        public async Task<IActionResult> CreateFetalHeartRates(FetalHeartRateCreateDto fetalHeartRates)
        {
            try
            {
                var fetalHeartRateList = fetalHeartRates.Data?.Select(x => new FetalHeartRate()
                {
                    FetalRate = Convert.ToInt32(x[1]),
                    FetalRateTime = Convert.ToInt64(x[0]),
                    PartographId = fetalHeartRates.PartographID,
                }).ToList() ?? new List<FetalHeartRate>();

                foreach (var item in fetalHeartRateList)
                {
                    context.FetalHeartRatesRepository.UpdateFatalRate(item);
                }

                await context.SaveChangesAsync();

                return Ok(fetalHeartRateList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFetalHeartRates", "FetalHeartRatesController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fetal-heart-rates
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFetalHeartRates)]
        public async Task<IActionResult> ReadFetalHeartRates(Guid partographId)
        {
            try
            {
                var fetalHeartRatesInDb = context.FetalHeartRatesRepository
                    .GetAll()
                    .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = fetalHeartRatesInDb.Select(x => new long[]
                {
                    x.FetalRateTime,
                    x.FetalRate
                })
                .OrderBy(i => i[0])
                .ToList();

                var fetalHeartRates = new FetalHeartRateCreateDto()
                {
                    PartographID = partographId,
                    Data = data
                };

                return Ok(fetalHeartRates);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFetalHeartRates", "FetalHeartRatesController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}