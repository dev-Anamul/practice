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
    /// DescentOfHeads controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DescentOfHeadsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DescentOfHeadsController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DescentOfHeadsController(IUnitOfWork context, ILogger<DescentOfHeadsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/descent-of-head
        /// </summary>
        /// <param name="descentOfHead">DescentOfHead object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDescentOfHead)]
        public async Task<IActionResult> CreateDescentOfHead(DescentOfHeadCreateDto descentOfHead)
        {
            try
            {
                var descentOfHeadList = descentOfHead.Data?.Select(x => new DescentOfHead()
                {
                    DescentOfHeadDetails = Convert.ToInt32(x[1]),
                    DescentOfHeadTime = Convert.ToInt64(x[0]),
                    PartographId = descentOfHead.PartographId,
                }).ToList() ?? new List<DescentOfHead>();

                foreach (var item in descentOfHeadList)
                {
                    context.DescentOfHeadsRepository.UpdateDescentOfHead(item);
                }

                await context.SaveChangesAsync();

                return Ok(descentOfHeadList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDescentOfHead", "DescentOfHeadsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/descent-of-heads
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDescentOfHeads)]
        public async Task<IActionResult> ReadDescentOfHeads(Guid partographId)
        {
            try
            {
                var descentOfHeadInDb = context.DescentOfHeadsRepository
                    .GetAll()
                    .Where(d => d.PartographId == partographId && d.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = descentOfHeadInDb.Select(x => new long[]
                {
                    DateTime.Now.Ticks,
                    x.DescentOfHeadDetails
                })
                .OrderBy(i => i[0])
                .ToList();

                var descentOfHead = new DescentOfHeadCreateDto();

                if (data.Count > 0)
                {
                    descentOfHead = new DescentOfHeadCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(descentOfHead);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDescentOfHeads", "DescentOfHeadsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}