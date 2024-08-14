using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Stephan
 * Last modified : 21.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    ///Contraceptive History Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ContraceptiveHistoryController : ControllerBase
    {
        private ContraceptiveHistory contraceptiveHistory;
        private readonly ILogger<ContraceptiveHistoryController> logger;
        private readonly IUnitOfWork context;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ContraceptiveHistoryController(IUnitOfWork context, ILogger<ContraceptiveHistoryController> logger)
        {
            contraceptiveHistory = new ContraceptiveHistory();

            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/contraceptive-history
        /// </summary>
        /// <param name="ContraceptiveHistory"> ContraceptiveHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateContraceptiveHistory)]
        public async Task<ActionResult<ContraceptiveHistory>> CreateContraceptiveHistory(ContraceptiveHistory contraceptiveHistory)
        {
            try
            {
                contraceptiveHistory.InteractionId = contraceptiveHistory.EncounterId;
                contraceptiveHistory.DateCreated = DateTime.Now;
                contraceptiveHistory.IsDeleted = false;
                contraceptiveHistory.IsSynced = false;

                context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                await context.SaveChangesAsync();

                return Ok(contraceptiveHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateContraceptiveHistory", "ContraceptiveHistoryController.cs", ex.Message, contraceptiveHistory.CreatedIn, contraceptiveHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ContraceptiveHistorys
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadContraceptiveHistories)]
        public async Task<IActionResult> ReadContraceptiveHistories()
        {
            try
            {
                var contraceptiveHistoryInDb = await context.ContraceptiveHistoryRepository.GetContraceptiveHistories();

                contraceptiveHistoryInDb = contraceptiveHistoryInDb.OrderByDescending(x => x.DateCreated);

                return Ok(contraceptiveHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadContraceptiveHistories", "ContraceptiveHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}