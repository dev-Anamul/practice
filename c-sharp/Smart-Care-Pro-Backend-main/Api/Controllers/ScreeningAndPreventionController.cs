using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
    /// <summary>
    /// ScreeningAndPrevention controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ScreeningAndPreventionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ScreeningAndPreventionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ScreeningAndPreventionController(IUnitOfWork context, ILogger<ScreeningAndPreventionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/screening-and-prevention
        /// </summary>
        /// <param name="screeningAndPrevention">ScreeningAndPrevention object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateScreeningAndPrevention)]
        public async Task<IActionResult> CreateScreeningAndPrevention(ScreeningAndPrevention screeningAndPrevention)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ScreeningAndPrevention, screeningAndPrevention.EncounterType);
                interaction.EncounterId = screeningAndPrevention.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = screeningAndPrevention.CreatedBy;
                interaction.CreatedIn = screeningAndPrevention.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                screeningAndPrevention.InteractionId = interactionId;
                screeningAndPrevention.DateCreated = DateTime.Now;
                screeningAndPrevention.IsDeleted = false;
                screeningAndPrevention.IsSynced = false;

                context.ScreeningAndPreventionRepository.Add(screeningAndPrevention);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadScreeningAndPreventionByKey", new { key = screeningAndPrevention.InteractionId }, screeningAndPrevention);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateScreeningAndPrevention", "ScreeningAndPreventionController.cs", ex.Message, screeningAndPrevention.CreatedIn, screeningAndPrevention.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening-and-preventions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningAndPreventions)]
        public async Task<IActionResult> ReadScreeningAndPreventions()
        {
            try
            {
                var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventions();

                return Ok(screeningAndPreventionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningAndPreventions", "ScreeningAndPreventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening-and-prevention/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningAndPreventionByClient)]
        public async Task<IActionResult> ReadScreeningAndPreventionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByClient(clientId);

                    return Ok(screeningAndPreventionInDb);
                }
                else
                {
                    var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<ScreeningAndPrevention> screeningAndPreventionDto = new PagedResultDto<ScreeningAndPrevention>()
                    {
                        Data = screeningAndPreventionInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(screeningAndPreventionDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningAndPreventionByClient", "ScreeningAndPreventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening-and-prevention/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningAndPreventionByEncounter)]
        public async Task<IActionResult> ReadScreeningAndPreventionByEncounter(Guid encounterId)
        {
            try
            {
                var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByEncounter(encounterId);

                return Ok(screeningAndPreventionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningAndPreventionByEncounter", "ScreeningAndPreventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening-and-prevention/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ScreeningAndPreventions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningAndPreventionByKey)]
        public async Task<IActionResult> ReadScreeningAndPreventionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByKey(key);

                if (screeningAndPreventionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(screeningAndPreventionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningAndPreventionByKey", "ScreeningAndPreventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/screening-and-prevention/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ScreeningAndPreventions.</param>
        /// <param name="screeningAndPrevention">ScreeningAndPrevention to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateScreeningAndPrevention)]
        public async Task<IActionResult> UpdateScreeningAndPrevention(Guid key, ScreeningAndPrevention screeningAndPrevention)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = screeningAndPrevention.ModifiedBy;
                interactionInDb.ModifiedIn = screeningAndPrevention.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != screeningAndPrevention.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                screeningAndPrevention.DateModified = DateTime.Now;
                screeningAndPrevention.IsDeleted = false;
                screeningAndPrevention.IsSynced = false;

                context.ScreeningAndPreventionRepository.Update(screeningAndPrevention);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateScreeningAndPrevention", "ScreeningAndPreventionController.cs", ex.Message, screeningAndPrevention.ModifiedIn, screeningAndPrevention.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening-and-prevention/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ScreeningAndPreventions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteScreeningAndPrevention)]
        public async Task<IActionResult> DeleteScreeningAndPrevention(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var screeningAndPreventionInDb = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByKey(key);

                if (screeningAndPreventionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                screeningAndPreventionInDb.DateModified = DateTime.Now;
                screeningAndPreventionInDb.IsDeleted = true;
                screeningAndPreventionInDb.IsSynced = false;

                context.ScreeningAndPreventionRepository.Update(screeningAndPreventionInDb);
                await context.SaveChangesAsync();

                return Ok(screeningAndPreventionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteScreeningAndPrevention", "ScreeningAndPreventionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}