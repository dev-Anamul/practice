using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HIVRiskScreeningController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HIVRiskScreeningController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HIVRiskScreeningController(IUnitOfWork context, ILogger<HIVRiskScreeningController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening
        /// </summary>
        /// <param name="hIVRiskScreening">HIVRiskScreening object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHIVRiskScreening)]
        public async Task<IActionResult> CreateHIVRiskScreening(HIVRiskScreening hIVRiskScreening)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.HIVRiskScreening, hIVRiskScreening.EncounterType);
                interaction.EncounterId = hIVRiskScreening.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = hIVRiskScreening.CreatedBy;
                interaction.CreatedIn = hIVRiskScreening.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                hIVRiskScreening.InteractionId = interactionId;
                hIVRiskScreening.ClientId = hIVRiskScreening.ClientId;
                hIVRiskScreening.DateCreated = DateTime.Now;
                hIVRiskScreening.IsDeleted = false;
                hIVRiskScreening.IsSynced = false;

                context.HIVRiskScreeningRepository.Add(hIVRiskScreening);
                await context.SaveChangesAsync();

                return CreatedAtAction("CreateHIVRiskScreening", new { key = hIVRiskScreening.InteractionId }, hIVRiskScreening);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHIVRiskScreening", "HIVRiskScreeningController.cs", ex.Message, hIVRiskScreening.CreatedIn, hIVRiskScreening.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screenings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskScreenings)]
        public async Task<IActionResult> ReadHIVRiskScreenings()
        {
            try
            {
                var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreenings();

                return Ok(hIVRiskScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskScreenings", "HIVRiskScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskScreeningByClient)]
        public async Task<IActionResult> ReadHIVRiskScreeningByClient(Guid clientId)
        {
            try
            {
                var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByClient(clientId); ;

                return Ok(hIVRiskScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskScreeningByClient", "HIVRiskScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskScreeningByEncounter)]
        public async Task<IActionResult> ReadHIVRiskScreeningByEncounter(Guid encounterId, EncounterType? encounterType)
        {
            try
            {
                if (encounterType == null)
                {
                    var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByEncounter(encounterId); ;

                    return Ok(hIVRiskScreeningInDb);
                }
                else
                {
                    var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByEncounterIdEncounterType(encounterId, encounterType.Value);

                    return Ok(hIVRiskScreeningInDb);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskScreeningByEncounter", "HIVRiskScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskScreenings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHIVRiskScreeningByKey)]
        public async Task<IActionResult> ReadHIVRiskScreeningByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByKey(key);

                if (hIVRiskScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(hIVRiskScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHIVRiskScreeningByKey", "HIVRiskScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskScreening.</param>
        /// <param name="hIVRiskScreening">HIVRiskScreening to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHIVRiskScreening)]
        public async Task<IActionResult> UpdateHIVRiskScreening(Guid key, HIVRiskScreening hIVRiskScreening)
        {
            try
            {
                if (key != hIVRiskScreening.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = hIVRiskScreening.ModifiedBy;
                interactionInDb.ModifiedIn = hIVRiskScreening.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                hIVRiskScreening.DateCreated = DateTime.Now;
                hIVRiskScreening.IsDeleted = false;
                hIVRiskScreening.IsSynced = false;

                context.HIVRiskScreeningRepository.Update(hIVRiskScreening);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHIVRiskScreening", "HIVRiskScreeningController.cs", ex.Message, hIVRiskScreening.ModifiedIn, hIVRiskScreening.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hiv-risk-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskScreening.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHIVRiskScreening)]
        public async Task<IActionResult> DeleteHIVRiskScreening(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var hIVRiskScreeningInDb = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByKey(key);

                if (hIVRiskScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.HIVRiskScreeningRepository.Update(hIVRiskScreeningInDb);
                await context.SaveChangesAsync();

                return Ok(hIVRiskScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHIVRiskScreening", "HIVRiskScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}