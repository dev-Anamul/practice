using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    /// KeyPopulationDemographic Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class KeyPopulationDemographicController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<KeyPopulationDemographicController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public KeyPopulationDemographicController(IUnitOfWork context, ILogger<KeyPopulationDemographicController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/key-population-demographic
        /// </summary>
        /// <param name="populationDemographic">KeyPopulationDemographic object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateKeyPopulationDemographic)]
        public async Task<IActionResult> CreateKeyPopulationDemographic(KeyPopulationDemographic populationDemographic)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.KeyPopulationDemographic, populationDemographic.EncounterType);
                interaction.EncounterId = populationDemographic.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = populationDemographic.CreatedBy;
                interaction.CreatedIn = populationDemographic.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                populationDemographic.InteractionId = interactionId;
                populationDemographic.DateCreated = DateTime.Now;
                populationDemographic.IsDeleted = false;
                populationDemographic.IsSynced = false;

                context.KeyPopulationDemographicRepository.Add(populationDemographic);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadKeyPopulationDemographicByKey", new { key = populationDemographic.InteractionId }, populationDemographic);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateKeyPopulationDemographic", "KeyPopulationDemographicController.cs", ex.Message, populationDemographic.CreatedIn, populationDemographic.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population-demographics
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulationDemographics)]
        public async Task<IActionResult> ReadKeyPopulationDemographics()
        {
            try
            {
                var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographics();

                return Ok(keyPopulationDemographicInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulationDemographics", "KeyPopulationDemographicController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population-demographic/ByClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulationDemographicByClient)]
        public async Task<IActionResult> ReadKeyPopulationDemographicByClient(Guid clientId)
        {
            try
            {
                var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByClient(clientId);

                return Ok(keyPopulationDemographicInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulationDemographicByClient", "KeyPopulationDemographicController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population-demographic/ByClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulationDemographicByEncounter)]
        public async Task<IActionResult> ReadKeyPopulationDemographicByEncounterId(Guid encounterId, EncounterType? encounterType)
        {
            try
            {
                if (encounterType == null)
                {
                    var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByEncounter(encounterId);

                    return Ok(keyPopulationDemographicInDb);
                }
                else
                {
                    var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByEncounterIdEncounterType(encounterId, encounterType.Value);

                    return Ok(keyPopulationDemographicInDb);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulationDemographicByEncounterId", "KeyPopulationDemographicController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population-demographic/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulationDemographic.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulationDemographicByKey)]
        public async Task<IActionResult> ReadKeyPopulationDemographicByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByKey(key);

                if (keyPopulationDemographicInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(keyPopulationDemographicInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulationDemographicByKey", "KeyPopulationDemographicController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/key-population-demographic/{key}
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulationDemographics.</param>
        /// <param name="populationDemographic">KeyPopulationDemographic to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateKeyPopulationDemographic)]
        public async Task<IActionResult> UpdateKeyPopulationDemographic(Guid key, KeyPopulationDemographic populationDemographic)
        {
            try
            {
                if (key != populationDemographic.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = populationDemographic.ModifiedBy;
                interactionInDb.ModifiedIn = populationDemographic.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                populationDemographic.DateModified = DateTime.Now;
                populationDemographic.IsDeleted = false;
                populationDemographic.IsSynced = false;

                context.KeyPopulationDemographicRepository.Update(populationDemographic);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateKeyPopulationDemographic", "KeyPopulationDemographicController.cs", ex.Message, populationDemographic.ModifiedIn, populationDemographic.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population-demographic/{key}
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulationDemographics.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteKeyPopulationDemographic)]
        public async Task<IActionResult> DeleteKeyPopulationDemographic(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var keyPopulationDemographicInDb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByKey(key);

                if (keyPopulationDemographicInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                keyPopulationDemographicInDb.DateModified = DateTime.Now;
                keyPopulationDemographicInDb.IsDeleted = true;
                keyPopulationDemographicInDb.IsSynced = false;

                context.KeyPopulationDemographicRepository.Update(keyPopulationDemographicInDb);
                await context.SaveChangesAsync();

                return Ok(keyPopulationDemographicInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteKeyPopulationDemographic", "KeyPopulationDemographicController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}