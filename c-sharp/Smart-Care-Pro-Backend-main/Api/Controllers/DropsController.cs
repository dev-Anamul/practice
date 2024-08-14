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
    /// Drops Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DropsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DropsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DropsController(IUnitOfWork context, ILogger<DropsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drop
        /// </summary>
        /// <param name="drops">Drop object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrop)]
        public async Task<IActionResult> CreateDrop(DropCreateDto drops)
        {
            try
            {
                var dropsList = drops.Data?.Select(x => new Drop()
                {
                    DropsDetails = Convert.ToInt32(x[1]),
                    DropsTime = Convert.ToInt64(x[0]),
                    PartographId = drops.PartographId,
                }).ToList() ?? new List<Drop>();

                foreach (var item in dropsList)
                {
                    context.DropsRepository.UpdateDrop(item);
                }

                await context.SaveChangesAsync();

                return Ok(dropsList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDrop", "DropsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/drops
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrops)]
        public async Task<IActionResult> ReadDrops(Guid partographId)
        {
            try
            {
                var dropsInDb = context.DropsRepository
                    .GetAll()
                    .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = dropsInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.DropsDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var drops = new DropCreateDto();
                if (data.Count > 0)
                {
                    drops = new DropCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(drops);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrops", "DropsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}