using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// DischargeMetric controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DischargeMetricController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DischargeMetricController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DischargeMetricController(IUnitOfWork context, ILogger<DischargeMetricController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/discharge-metric
        /// </summary>
        /// <param name="dischargeMetric">DischargeMetric object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDischargeMetric)]
        public async Task<IActionResult> CreateDischargeMetric(DischargeMetric dischargeMetric)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DischargeMetric, dischargeMetric.EncounterType); ;
                interaction.EncounterId = dischargeMetric.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = dischargeMetric.CreatedBy;
                interaction.CreatedIn = dischargeMetric.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                dischargeMetric.InteractionId = interactionId;
                dischargeMetric.DateCreated = DateTime.Now;
                dischargeMetric.IsDeleted = false;
                dischargeMetric.IsSynced = false;

                context.DischargeMetricRepository.Add(dischargeMetric);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDischargeMetricByKey", new { key = dischargeMetric.InteractionId }, dischargeMetric);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDischargeMetric", "DischargeMetricController.cs", ex.Message, dischargeMetric.CreatedIn, dischargeMetric.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/discharge-metrics
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDischargeMetrics)]
        public async Task<IActionResult> ReadDischargeMetrics()
        {
            try
            {
                var dischargeMetricInDb = await context.DischargeMetricRepository.GetDischargeMetrics();

                return Ok(dischargeMetricInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDischargeMetrics", "DischargeMetricController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/discharge-metric/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DischargeMetrics.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDischargeMetricByKey)]
        public async Task<IActionResult> ReadDischargeMetricByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dischargeMetricIndb = await context.DischargeMetricRepository.GetDischargeMetricByKey(key);

                if (dischargeMetricIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(dischargeMetricIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDischargeMetricByKey", "DischargeMetricController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/discharge-metric/byClient/{ClientID}
        /// </summary>
        /// <param name="ClientID">Primary key of the table DischargeMetrics.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDischargeMetricByClient)]
        public async Task<IActionResult> ReadDischargeMetricByClient(Guid clientId)
        {
            try
            {
                var dischargeMetricInDb = await context.DischargeMetricRepository.GetDischargeMetricByClient(clientId);

                return Ok(dischargeMetricInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDischargeMetricByClient", "DischargeMetricController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/discharge-metric/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DischargeMetrics.</param>
        /// <param name="dischargeMetric">DischargeMetric to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDischargeMetric)]
        public async Task<IActionResult> UpdateDischargeMetric(Guid key, DischargeMetric dischargeMetric)
        {
            try
            {
                if (key != dischargeMetric.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = dischargeMetric.ModifiedBy;
                interactionInDb.ModifiedIn = dischargeMetric.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                dischargeMetric.DateModified = DateTime.Now;
                dischargeMetric.IsDeleted = false;
                dischargeMetric.IsSynced = false;

                context.DischargeMetricRepository.Update(dischargeMetric);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDischargeMetric", "DischargeMetricController.cs", ex.Message, dischargeMetric.ModifiedIn, dischargeMetric.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/discharge-metric/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DischargeMetrics.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteDischargeMetric)]
        public async Task<IActionResult> DeleteDischargeMetric(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dischargeMetricInDb = await context.DischargeMetricRepository.GetDischargeMetricByKey(key);

                if (dischargeMetricInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.DischargeMetricRepository.Update(dischargeMetricInDb);
                await context.SaveChangesAsync();

                return Ok(dischargeMetricInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDischargeMetric", "DischargeMetricController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}