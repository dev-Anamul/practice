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
    /// Oxytocin controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class OxytocinController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<OxytocinController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public OxytocinController(IUnitOfWork context, ILogger<OxytocinController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/oxytocin
        /// </summary>
        /// <param name="oxytocin">Oxytocin object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateOxytocin)]
        public async Task<IActionResult> CreateOxytocin(OxytocinCreateDto oxytocin)
        {
            try
            {
                var oxytocinList = oxytocin.Data?.Select(x => new Oxytocin()
                {
                    OxytocinDetails = Convert.ToInt32(x[1]),
                    OxytocinTime = Convert.ToInt64(x[0]),
                    PartographId = oxytocin.PartographId,
                }).ToList() ?? new List<Oxytocin>();

                foreach (var item in oxytocinList)
                {
                    context.OxytocinRepository.UpdateOxytocin(item);
                }

                await context.SaveChangesAsync();

                return Ok(oxytocinList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateOxytocin", "OxytocinController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/oxytocins
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOxytocins)]
        public async Task<IActionResult> ReadOxytocins(Guid partographId)
        {
            try
            {
                var oxytocinInDb = context.OxytocinRepository
                    .GetAll()
                    .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = oxytocinInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.OxytocinDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var oxytocins = new OxytocinCreateDto();
                if (data.Count > 0)
                {
                    oxytocins = new OxytocinCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(oxytocins);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOxytocins", "OxytocinController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}