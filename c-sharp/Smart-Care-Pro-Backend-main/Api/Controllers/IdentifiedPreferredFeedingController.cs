using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedPreferredFeeding controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPreferredFeedingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPreferredFeedingController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPreferredFeedingController(IUnitOfWork context, ILogger<IdentifiedPreferredFeedingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding
        /// </summary>
        /// <param name="identifiedPreferredFeeding">IdentifiedPreferredFeeding object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPreferredFeeding)]
        public async Task<IActionResult> CreateIdentifiedPreferredFeeding(IdentifiedPreferredFeeding identifiedPreferredFeeding)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPreferredFeeding, identifiedPreferredFeeding.EncounterType);
                interaction.EncounterId = identifiedPreferredFeeding.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPreferredFeeding.CreatedBy;
                interaction.CreatedIn = identifiedPreferredFeeding.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedPreferredFeeding.InteractionId = interactionId;
                identifiedPreferredFeeding.DateCreated = DateTime.Now;
                identifiedPreferredFeeding.IsDeleted = false;
                identifiedPreferredFeeding.IsSynced = false;

                context.IdentifiedPreferredFeedingRepository.Add(identifiedPreferredFeeding);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPreferredFeedingByKey", new { key = identifiedPreferredFeeding.InteractionId }, identifiedPreferredFeeding);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPreferredFeeding", "IdentifiedPreferredFeedingController.cs", ex.Message, identifiedPreferredFeeding.CreatedIn, identifiedPreferredFeeding.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feedings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPreferredFeedings)]
        public async Task<IActionResult> ReadIdentifiedPreferredFeedings()
        {
            try
            {
                var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedings();

                return Ok(identifiedPreferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPreferredFeedings", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPreferredFeedingByKey)]
        public async Task<IActionResult> ReadIdentifiedPreferredFeedingByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByKey(key);

                if (identifiedPreferredFeedingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPreferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPreferredFeedingByKey", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/byClient/{ClientID}
        /// </summary>
        /// <param name="clientId">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPreferredFeedingByClient)]
        public async Task<IActionResult> ReadIdentifiedPreferredFeedingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByClient(clientId);

                    return Ok(identifiedPreferredFeedingInDb);
                }
                else
                {
                    var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<IdentifiedPreferredFeeding> identifiedPreferredFeedingDto = new PagedResultDto<IdentifiedPreferredFeeding>()
                    {
                        Data = identifiedPreferredFeedingInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(identifiedPreferredFeedingDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPreferredFeedingByClient", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/byEncounter/{EncounterID}
        /// </summary>
        /// <param name="EncounterId">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPreferredFeedingByEncounter)]
        public async Task<IActionResult> ReadIdentifiedPreferredFeedingByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByEncounter(encounterId);

                return Ok(identifiedPreferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPreferredFeedingByEncounter", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/by-assessment/{AssessmentID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPreferredFeedingByPreferredFeeding)]
        public async Task<IActionResult> ReadIdentifiedPreferredFeedingByAssessment(int preferredFeedingId)
        {
            try
            {
                var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.ReadIdentifiedPreferredFeedingByPreferredFeeding(preferredFeedingId);

                return Ok(identifiedPreferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPreferredFeedingByAssessment", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <param name="identifiedPreferredFeeding">IdentifiedPreferredFeeding to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPreferredFeeding)]
        public async Task<IActionResult> UpdateIdentifiedPreferredFeeding(Guid key, IdentifiedPreferredFeeding identifiedPreferredFeeding)
        {
            try
            {
                if (key != identifiedPreferredFeeding.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPreferredFeeding.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPreferredFeeding.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPreferredFeeding.DateModified = DateTime.Now;
                identifiedPreferredFeeding.IsDeleted = false;
                identifiedPreferredFeeding.IsSynced = false;

                context.IdentifiedPreferredFeedingRepository.Update(identifiedPreferredFeeding);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPreferredFeeding", "IdentifiedPreferredFeedingController.cs", ex.Message, identifiedPreferredFeeding.ModifiedIn, identifiedPreferredFeeding.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-preferred-feeding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPreferredFeeding)]
        public async Task<IActionResult> DeleteIdentifiedPreferredFeeding(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPreferredFeedingInDb = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByKey(key);

                if (identifiedPreferredFeedingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedPreferredFeedingRepository.Update(identifiedPreferredFeedingInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedPreferredFeedingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPreferredFeeding", "IdentifiedPreferredFeedingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}