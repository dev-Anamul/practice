using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Drug Definition controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugDefinitionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugDefinitionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugDefinitionController(IUnitOfWork context, ILogger<DrugDefinitionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugDefinition
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.DrugDefinition)]
        public async Task<IActionResult> ReadDrugDefinitions()
        {
            try
            {
                var drugDefinitionInDb = await context.DrugDefinitionRepository.GetDrugDefinition();

                return Ok(drugDefinitionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugDefinitions", "DrugDefinitionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}