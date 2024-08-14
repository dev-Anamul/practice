using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// TBSymptomController controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBSymptomController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBSymptomController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBSymptomController(IUnitOfWork context, ILogger<TBSymptomController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-symptoms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBSymptoms)]
        public async Task<IActionResult> ReadTBSymptoms()
        {
            try
            {
                var tbSymptomInDb = await context.TBSymptomRepository.GetTBSymptoms();

                return Ok(tbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBSymptoms", "TBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}