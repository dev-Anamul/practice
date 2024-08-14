using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Cervix controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CervixController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CervixController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CervixController(IUnitOfWork context, ILogger<CervixController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/cervix
        /// </summary>
        /// <param name="cervix">Cervix object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCervix)]
        public async Task<IActionResult> CreateCervix(CervixCreateDto cervix)
        {
            try
            {
                var cervixList = cervix.Data?.Select(x => new Cervix()
                {
                    CervixDetails = Convert.ToInt32(x[1]),
                    CervixTime = Convert.ToInt64(x[0]),
                    PartographId = cervix.PartographId,

                }).ToList() ?? new List<Cervix>();

                foreach (var item in cervixList)
                {
                    context.CervixRepository.UpdateCervix(item);
                }

                await context.SaveChangesAsync();

                return Ok(cervixList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateCervix", "CervixController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/cervixs
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCervixs)]
        public async Task<IActionResult> ReadCervixs(Guid partographId)
        {
            try
            {
                var cervixInDb = context.CervixRepository
                    .GetAll()
                    .Where(c => c.PartographId == partographId && c.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = cervixInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.CervixDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var cervix = new CervixCreateDto();
                if (data.Count > 0)
                {
                    cervix = new CervixCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(cervix);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCervixs", "CervixController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}