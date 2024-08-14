using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 15.03.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// AttachedFacility controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AttachedFacilityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AttachedFacilityController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AttachedFacilityController(IUnitOfWork context, ILogger<AttachedFacilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/attached-facility
        /// </summary>
        /// <param name="nextofkin">AttachedFacility object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAttachedFacility)]
        public async Task<IActionResult> CreateAttachedFacility(AttachedFacility attachedFacility)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.AttachedFacility, attachedFacility.EncounterType);
                interaction.EncounterId = attachedFacility.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = attachedFacility.CreatedBy;
                interaction.CreatedIn = attachedFacility.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                attachedFacility.InteractionId = interactionId;
                attachedFacility.DateCreated = DateTime.Now;
                attachedFacility.IsDeleted = false;
                attachedFacility.IsSynced = false;

                context.AttachedFacilityRepository.Add(attachedFacility);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAttachedFacilityByKey", new { key = attachedFacility.InteractionId }, attachedFacility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAttachedFacility", "AttachedFacilityController.cs", ex.Message, attachedFacility.CreatedIn, attachedFacility.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/attached-facility
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAttachedFacility)]
        public async Task<IActionResult> ReadAttachedFacility()
        {
            try
            {
                var attachedFacilityInDb = await context.AttachedFacilityRepository.GetAttachedFacility();

                attachedFacilityInDb = attachedFacilityInDb.OrderByDescending(x => x.DateCreated);

                return Ok(attachedFacilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAttachedFacility", "AttachedFacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/attached-facility/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AttachedFacility.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAttachedFacilityByKey)]
        public async Task<IActionResult> ReadAttachedFacilityByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var attachedFacilityInDb = await context.AttachedFacilityRepository.GetAttachedFacilityByKey(key);

                if (attachedFacilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(attachedFacilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAttachedFacilityByKey", "AttachedFacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/attached-facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table AttachedFacility.</param>
        /// <param name="attachedFacility">AttachedFacility to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAttachedFacility)]
        public async Task<IActionResult> UpdateAttachedFacility(Guid key, AttachedFacility attachedFacility)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = attachedFacility.ModifiedBy;
                interactionInDb.ModifiedIn = attachedFacility.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                attachedFacility.DateModified = DateTime.Now;
                attachedFacility.IsDeleted = false;
                attachedFacility.IsSynced = false;

                context.AttachedFacilityRepository.Update(attachedFacility);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateAttachedFacility", "AttachedFacilityController.cs", ex.Message,attachedFacility.CreatedIn,attachedFacility.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadAttachedFacilityByClient)]
        public async Task<IActionResult> ReadAttachedFacilityByClient(Guid clientId)
        {
            try
            {
                var attachedFacilityInDb = await context.AttachedFacilityRepository.GetAttachedFacilityByClient(clientId);

                return Ok(attachedFacilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAttachedFacilityByClient", "AttachedFacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}