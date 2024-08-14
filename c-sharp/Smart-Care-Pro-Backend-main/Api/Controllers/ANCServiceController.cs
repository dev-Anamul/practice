using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Stephan
 * Last modified: 16.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ANCService controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ANCServiceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ANCServiceController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ANCServiceController(IUnitOfWork context, ILogger<ANCServiceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/anc-service
        /// </summary>
        /// <param name="ancService">ANCService object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateANCService)]
        public async Task<IActionResult> CreateANCService(ANCService ancService)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ANCService, ancService.EncounterType);
                interaction.EncounterId = ancService.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = ancService.CreatedBy;
                interaction.CreatedIn = ancService.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                ancService.InteractionId = interactionId;
                ancService.DateCreated = DateTime.Now;
                ancService.IsDeleted = false;
                ancService.IsSynced = false;

                context.ANCServiceRepository.Add(ancService);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadANCServiceByKey", new { key = ancService.InteractionId }, ancService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateANCService", "ANCServiceController.cs", ex.Message, ancService.CreatedIn, ancService.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-services
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCServices)]
        public async Task<IActionResult> ReadANCServices()
        {
            try
            {
                var ancServiceInDb = await context.ANCServiceRepository.GetANCServices();

                ancServiceInDb = ancServiceInDb.OrderByDescending(x => x.DateCreated);

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCServices", "ANCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-service/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCServiceByClient)]
        public async Task<IActionResult> ReadANCServiceByClient(Guid clientId)
        {
            try
            {
              //  var ancServiceInDb = await context.ANCServiceRepository.GetANCServiceByClient(clientId);
                var ancServiceInDb = await context.ANCServiceRepository.GetANCServiceByClientLast24Hour(clientId);

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCServiceByClient", "ANCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/latest-anc-service/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLatestANCServiceByClient)]
        public async Task<IActionResult> ReadLatestANCServiceByClient(Guid clientId)
        {
            try
            {
                var ancServiceInDb = await context.ANCServiceRepository.GetLatestANCServiceByClient(clientId);

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestANCServiceByClient", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-service/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCServiceByEncounter)]
        public async Task<IActionResult> ReadANCServiceByEncounter(Guid encounterId)
        {
            try
            {
                var ancServiceInDb = await context.ANCServiceRepository.GetANCServiceByEncounter(encounterId);

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCServiceByEncounter", "ANCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-service/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCServiceByKey)]
        public async Task<IActionResult> ReadANCServiceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ancServiceInDb = await context.ANCServiceRepository.GetANCServiceByKey(key);

                if (ancServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateANCService", "ANCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/anc-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCServices.</param>
        /// <param name="ancService">ANCService to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateANCService)]
        public async Task<IActionResult> UpdateANCService(Guid key, ANCService ancService)
        {
            try
            {
                if (key != ancService.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = ancService.ModifiedBy;
                interactionInDb.ModifiedIn = ancService.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                ancService.DateModified = DateTime.Now;
                ancService.ModifiedBy = ancService.ModifiedBy;
                ancService.ModifiedIn = ancService.ModifiedIn;
                ancService.IsDeleted = false;
                ancService.IsSynced = false;

                context.ANCServiceRepository.Update(ancService);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateANCService", "ANCServiceController.cs", ex.Message, ancService.ModifiedIn, ancService.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteANCService)]
        public async Task<IActionResult> DeleteANCService(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ancServiceInDb = await context.ANCServiceRepository.GetANCServiceByKey(key);

                if (ancServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                ancServiceInDb.DateModified = DateTime.Now;
                ancServiceInDb.IsDeleted = true;
                ancServiceInDb.IsSynced = false;

                context.ANCServiceRepository.Update(ancServiceInDb);
                await context.SaveChangesAsync();

                return Ok(ancServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteANCService", "ANCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}