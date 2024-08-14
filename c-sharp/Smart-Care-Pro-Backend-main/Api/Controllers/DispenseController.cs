using Domain.Dto;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Dispense controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DispenseController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DispenseController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DispenseController(IUnitOfWork context, ILogger<DispenseController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/dispense
        /// </summary>
        /// <param name="Dispense">Dispense object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDispense)]
        public async Task<IActionResult> CreateDispense(DispenseDto dispenseDto)
        {
            try
            {

                foreach (var item in dispenseDto.DispenseList)
                {
                    item.DateCreated = DateTime.Now;
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("CreateDispense", new { key = dispenseDto.DispenseList.Select(x => x.InteractionId).FirstOrDefault() }, dispenseDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDispense", "DispenseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}