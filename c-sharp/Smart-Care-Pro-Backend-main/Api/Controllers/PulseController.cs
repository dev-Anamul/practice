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
    /// Pulse controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PulseController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PulseController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PulseController(IUnitOfWork context, ILogger<PulseController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pulse
        /// </summary>
        /// <param name="pulse">Pulse object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePulse)]
        public async Task<IActionResult> CreatePulse(PulseCreateDto pulse)
        {
            try
            {
                var pulseList = pulse.Data?.Select(x => new Pulse()
                {
                    PulseDetails = Convert.ToInt32(x[1]),
                    PulseTime = Convert.ToInt64(x[0]),
                    PartographId = pulse.PartographId,
                }).ToList() ?? new List<Pulse>();

                foreach (var item in pulseList)
                {
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.PulseRepository.UpdatePulse(item);
                }

                await context.SaveChangesAsync();

                return Ok(pulseList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "Createpulse", "PulseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/pulses
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPulses)]
        public async Task<IActionResult> ReadPulses(Guid partographId)
        {
            try
            {
                var pulseInDb = context.PulseRepository
                    .GetAll()
                    .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = pulseInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.PulseDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var pulse = new PulseCreateDto();
                if (data.Count > 0)
                {
                    pulse = new PulseCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(pulse);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPulses", "PulseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}