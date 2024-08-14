using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 08.04.2023
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

    public class TBSuspectingReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBSuspectingReasonController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBSuspectingReasonController(IUnitOfWork context, ILogger<TBSuspectingReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-suspecting-Reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBSuspectingReasons)]
        public async Task<IActionResult> ReadTBSuspectingReasons()
        {
            try
            {
                var tbSuspectingReasonInDb = await context.TBSuspectingReasonRepository.GetTBSuspectingReasons();

                return Ok(tbSuspectingReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBSuspectingReasons", "TBSuspectingReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}