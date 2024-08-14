using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    /// <summary>
    ///ThermoAblation Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ThermoAblationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ThermoAblationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ThermoAblationController(IUnitOfWork context, ILogger<ThermoAblationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/ThermoAblation
        /// </summary>
        /// <param name="ThermoAblation">ThermoAblation object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateThermoAblation)]
        public async Task<IActionResult> CreateThermoAblation(ThermoAblation thermoAblation)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ThermoAblation, thermoAblation.EncounterType);
                interaction.EncounterId = thermoAblation.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = thermoAblation.CreatedBy;
                interaction.CreatedIn = thermoAblation.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);


                thermoAblation.InteractionId = interactionId;
                thermoAblation.DateCreated = DateTime.Now;
                thermoAblation.IsDeleted = false;
                thermoAblation.IsSynced = false;

                context.ThermoAblationRepository.Add(thermoAblation);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadthermoAblationByKey", new { key = thermoAblation.InteractionId }, thermoAblation);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "ThermoAblation", "ThermoAblationController.cs", ex.Message, thermoAblation.CreatedIn, thermoAblation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/ThermoAblation
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThermoAblations)]
        public async Task<IActionResult> ReadThermoAblation()
        {
            try
            {
                var thermoAblationDb = await context.ThermoAblationRepository.GetThermoAblation();

                return Ok(thermoAblationDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblation", "ThermoAblationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/ThermoAblation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThermoAblation.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThermoAblationByKey)]
        public async Task<IActionResult> ReadThermoAblationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thermoAblationInDb = await context.ThermoAblationRepository.GetThermoAblationByKey(key);

                if (thermoAblationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(thermoAblationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblationByKey", "ThermoAblationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// ThermoAblation-by-client/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadThermoAblationsByClient)]
        public async Task<IActionResult> ReadThermoAblationsByClient(Guid clientId)
        {
            try
            {
                var thermoAblationInDb = await context.ThermoAblationRepository.GetThermoAblationbyClienId(clientId);

                return Ok(thermoAblationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblationByClient", "ThermoAblationClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/thermoAblation/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadThermoAblationsByEncounter)]
        public async Task<IActionResult> ReadThermoAblationsByEncounter(Guid EncounterId)
        {
            try
            {
                var thermoAblationInDb = await context.ThermoAblationRepository.GetThermoAblationByEncounter(EncounterId);

                return Ok(thermoAblationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadThermoAblationsByEncounter", "ThermoAblationsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/ThermoAblations/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ThermoAblations.</param>
        /// <param name="ThermoAblations">ThermoAblations to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateThermoAblation)]
        public async Task<IActionResult> UpdateThermoAblation(Guid key, ThermoAblation thermoAblation)
        {
            try
            {
                if (key != thermoAblation.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var thermoAblationInDb = await context.ThermoAblationRepository.GetThermoAblationByKey(key);

                if (thermoAblationInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                thermoAblation.DateModified = DateTime.Now;
                thermoAblation.IsDeleted = false;
                thermoAblation.IsSynced = false;

                context.ThermoAblationRepository.Update(thermoAblation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateThermoAblation", "ThermoAblationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/ThermoAblation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table thermoAblation.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteThermoAblation)]
        public async Task<IActionResult> DeleteThermoAblation(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var thermoAblationInDb = await context.ThermoAblationRepository.GetThermoAblationByKey(key);

                if (thermoAblationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                thermoAblationInDb.IsDeleted = true;
                thermoAblationInDb.IsSynced = false;
                thermoAblationInDb.DateModified = DateTime.Now;

                context.ThermoAblationRepository.Update(thermoAblationInDb);
                await context.SaveChangesAsync();

                return Ok(thermoAblationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteThermoAblation", "ThermoAblationtController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
