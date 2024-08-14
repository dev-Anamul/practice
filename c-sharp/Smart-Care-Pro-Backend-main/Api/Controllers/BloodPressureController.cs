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
    ///BloodPressure Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BloodPressureController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BloodPressureController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BloodPressureController(IUnitOfWork context, ILogger<BloodPressureController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/blood-pressure
        /// </summary>
        /// <param name="bloodPressure">BloodPressure object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBloodPressure)]
        public async Task<IActionResult> CreateBloodPressure(BloodPressureCreateDto bloodPressure)
        {
            try
            {
                var bloodPressureList = bloodPressure.Data?.Select(x => new BloodPressure()
                {
                    SystolicPressure = Convert.ToInt32(x[1]),
                    DiastolicPressure = Convert.ToInt32(x[2]),
                    BloodPressureTime = Convert.ToInt64(x[0]),
                    PartographId = bloodPressure.PartographId,

                }).ToList() ?? new List<BloodPressure>();

                foreach (var item in bloodPressureList)
                {
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.DateCreated = DateTime.Now;

                    context.BloodPressureRepository.UpdateBloodPressure(item);
                }

                await context.SaveChangesAsync();

                return Ok(bloodPressureList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBloodPressure", "BloodPressureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/blood-pressures
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBloodPressure)]
        public async Task<IActionResult> ReadBloodPressure(Guid partographId)
        {
            try
            {
                var bloodPressureInDb = context.BloodPressureRepository
                    .GetAll()
                    .Where(c => c.PartographId == partographId && c.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = bloodPressureInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.SystolicPressure,
                    x.DiastolicPressure
                })
                .OrderBy(i => i[0])
                .ToList();

                var bloodPressure = new BloodPressureCreateDto();

                if (data.Count > 0)
                {
                    bloodPressure = new BloodPressureCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(bloodPressure);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBloodPressure", "BloodPressureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}