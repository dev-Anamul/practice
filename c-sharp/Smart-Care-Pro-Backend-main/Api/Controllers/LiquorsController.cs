using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Liquors controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class LiquorsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<LiquorsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public LiquorsController(IUnitOfWork context, ILogger<LiquorsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/liquor
        /// </summary>
        /// <param name="liquor">Liquor object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateLiquor)]
        public async Task<IActionResult> CreateLiquor(LiquorCreateDto liquor)
        {
            try
            {
                var liquorList = liquor.Data?.Select(x => new Liquor()
                {
                    Description = Convert.ToString(x[1]),
                    LiquorTime = Convert.ToInt64(x[0]),
                    PartographId = liquor.PartographId,
                }).ToList() ?? new List<Liquor>();

                foreach (var item in liquorList)
                {
                    context.LiquorsRepository.UpdateLiquor(item);
                }

                await context.SaveChangesAsync();

                return Ok(liquorList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateLiquor", "LiquorsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}