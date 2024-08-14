using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 13.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Acetone controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AcetoneController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AcetoneController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AcetoneController(IUnitOfWork context, ILogger<AcetoneController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/acentone
        /// </summary>
        /// <param name="acetones"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.CreateAcentone)]
        public async Task<IActionResult> CreateAcentone(AcetoneDto acetones)
        {
            try
            {
                var acetonesList = acetones.Data?.Select(x => new Acetone()
                {
                    Description = Convert.ToString(x[1]),
                    AcetoneTime = Convert.ToInt64(x[0]),
                    PartographId = acetones.PartographId,

                }).ToList() ?? new List<Acetone>();

                foreach (var item in acetonesList)
                {
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.DateCreated = DateTime.Now;

                    context.AcetonesRepository.UpdateAcetone(item);
                }

                await context.SaveChangesAsync();

                return Ok(acetonesList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateAcentone", "AcetoneController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}