using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Moulding controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MouldingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MouldingController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MouldingController(IUnitOfWork context, ILogger<MouldingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/moulding
        /// </summary>
        /// <param name="moulding">Moulding object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMoulding)]
        public async Task<IActionResult> CreateMoulding(MouldingCreateDto moulding)
        {
            try
            {
                var mouldingList = moulding.Data?.Select(x => new Moulding()
                {
                    Description = Convert.ToString(x[1]),
                    MouldingTime = Convert.ToInt64(x[0]),
                    PartographId = moulding.PartographId,
                }).ToList() ?? new List<Moulding>();

                foreach (var item in mouldingList)
                {
                    context.MouldingsRepository.UpdateMoulding(item);
                }

                await context.SaveChangesAsync();

                return Ok(mouldingList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateMoulding", "MouldingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}