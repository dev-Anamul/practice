using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Strphan
 * Last modified: 12.09.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ANCScreening controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ANCScreeningController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ANCScreeningController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ANCScreeningController(IUnitOfWork context, ILogger<ANCScreeningController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/anc-screening
        /// </summary>
        /// <param name="aNCScreening">ANCScreening object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateANCScreening)]
        public async Task<IActionResult> CreateANCScreening(ANCScreening ancScreening)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ANCScreening, ancScreening.EncounterType);
                interaction.EncounterId = ancScreening.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = ancScreening.CreatedBy;
                interaction.CreatedIn = ancScreening.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                ancScreening.InteractionId = interactionId;

                ancScreening.DateCreated = DateTime.Now;
                ancScreening.IsDeleted = false;
                ancScreening.IsSynced = false;

                context.ANCScreeningRepository.Add(ancScreening);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadANCScreeningByKey", new { key = ancScreening.InteractionId }, ancScreening);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateANCScreening", "ANCScreeningController.cs", ex.Message, ancScreening.CreatedIn, ancScreening.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-screenings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCScreenings)]
        public async Task<IActionResult> ReadANCScreenings()
        {
            try
            {
                var ancScreeningInDb = await context.ANCScreeningRepository.GetANCScreenings();
                ancScreeningInDb = ancScreeningInDb.OrderByDescending(x => x.DateCreated);
                return Ok(ancScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCScreenings", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-screening/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCScreeningByClient)]
        public async Task<IActionResult> ReadANCScreeningByClient(Guid clientId)
        {
            try
            {
             //   var ancScreeningInDb = await context.ANCScreeningRepository.GetANCScreeningByClient(clientId);
               var ancScreeningInDb = await context.ANCScreeningRepository.GetANCScreeningByClientLast24Hours(clientId);

                return Ok(ancScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCScreeningByClient", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-screening/ByEncounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCScreeningByEncounter)]
        public async Task<IActionResult> ReadANCScreeningByEncounter(Guid encounterId)
        {
            try
            {
                var ancScreeningInDb = await context.ANCScreeningRepository.GetANCScreeningByEncounter(encounterId);

                return Ok(ancScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCScreeningByEncounter", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCScreenings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCScreeningByKey)]
        public async Task<IActionResult> ReadANCScreeningByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var ancScreeningInDb = await context.ANCScreeningRepository.GetANCScreeningByKey(key);

                if (ancScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(ancScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCScreeningByKey", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/anc-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCScreenings.</param>
        /// <param name="ancScreening">ANCScreening to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateANCScreening)]
        public async Task<IActionResult> UpdateANCScreening(Guid key, ANCScreening ancScreening)
        {
            try
            {
                if (key != ancScreening.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = ancScreening.ModifiedBy;
                interactionInDb.ModifiedIn = ancScreening.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                ancScreening.DateModified = DateTime.Now;
                ancScreening.ModifiedBy = ancScreening.ModifiedBy;
                ancScreening.ModifiedIn = ancScreening.ModifiedIn;
                ancScreening.IsDeleted = false;
                ancScreening.IsSynced = false;

                context.ANCScreeningRepository.Update(ancScreening);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateANCScreening", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/anc-screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ANCScreenings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteANCScreening)]
        public async Task<IActionResult> DeleteANCScreening(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var aNCScreeningInDb = await context.ANCScreeningRepository.GetANCScreeningByKey(key);

                if (aNCScreeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                aNCScreeningInDb.DateModified = DateTime.Now;
                aNCScreeningInDb.IsDeleted = true;
                aNCScreeningInDb.IsSynced = false;

                context.ANCScreeningRepository.Update(aNCScreeningInDb);
                await context.SaveChangesAsync();

                return Ok(aNCScreeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteANCScreening", "ANCScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}