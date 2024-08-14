using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TurningChartController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TurningChartController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TurningChartController(IUnitOfWork context, ILogger<TurningChartController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/turning-chart
        /// </summary>
        /// <param name="turningChart">TurningChart object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTurningChart)]
        public async Task<IActionResult> CreateTurningChart(TurningChart turningChart)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TurningChart, turningChart.EncounterType);
                interaction.EncounterId = turningChart.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = turningChart.CreatedBy;
                interaction.CreatedIn = turningChart.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                turningChart.InteractionId = interactionID;
                turningChart.EncounterId = turningChart.EncounterId;
                turningChart.ClientId = turningChart.ClientId;
                turningChart.DateCreated = DateTime.Now;
                turningChart.IsDeleted = false;
                turningChart.IsSynced = false;

                context.TurningChartRepository.Add(turningChart);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTurningChartByKey", new { key = turningChart.InteractionId }, turningChart);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTurningChart", "TurningChartController.cs", ex.Message, turningChart.CreatedIn, turningChart.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }


        /// <summary>
        /// URL: sc-api/turning-charts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTurningCharts)]
        public async Task<IActionResult> ReadTurningCharts()
        {
            try
            {
                var turningChartInDb = await context.TurningChartRepository.GetTurningCharts();
                turningChartInDb = turningChartInDb.OrderByDescending(x => x.DateCreated);
                return Ok(turningChartInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTurningCharts", "TurningChartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/turning-chart/ByClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTurningChartByClient)]
        public async Task<IActionResult> ReadTurningChartByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var turningChartInDb = await context.TurningChartRepository.GetTurningChartByClient(ClientID);

                    return Ok(turningChartInDb);
                }
                else
                {
                    var turningChartInDb = await context.TurningChartRepository.GetTurningChartByClient(ClientID, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<TurningChart> turningChartDto = new PagedResultDto<TurningChart>()
                    {
                        Data = turningChartInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.TurningChartRepository.GetTurningChartByClientTotalCount(ClientID, encounterType)
                    };

                    return Ok(turningChartDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTurningChartByClient", "TurningChartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/turning-chart/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTurningChartByKey)]
        public async Task<IActionResult> ReadTurningChartByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var turningChartInDb = await context.TurningChartRepository.GetTurningChartByKey(key);

                if (turningChartInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(turningChartInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTurningChartByKey", "TurningChartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/turning-chart/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlans.</param>
        /// <param name="birthRecord">NursingPlan to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTurningChart)]
        public async Task<IActionResult> UpdateTurningChart(Guid key, TurningChart turningChart)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = turningChart.ModifiedBy;
                interactionInDb.ModifiedIn = turningChart.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != turningChart.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                turningChart.DateModified = DateTime.Now;
                turningChart.IsDeleted = false;
                turningChart.IsSynced = false;


                context.TurningChartRepository.Update(turningChart);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTurningChart", "TurningChartController.cs", ex.Message, turningChart.ModifiedIn, turningChart.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/turning-chart/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTurningChart)]
        public async Task<IActionResult> DeleteTurningChart(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var turningChartInDb = await context.TurningChartRepository.GetTurningChartByKey(key);

                if (turningChartInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                turningChartInDb.DateModified = DateTime.Now;
                turningChartInDb.IsDeleted = true;
                turningChartInDb.IsSynced = false;

                context.TurningChartRepository.Update(turningChartInDb);
                await context.SaveChangesAsync();

                return Ok(turningChartInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTurningChart", "TurningChartController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}
