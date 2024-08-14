using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Brian
 * Date created  : 21.01.2023
 * Modified by   : Bella
 * Last modified : 22.01.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PEPPreventionHistory controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PEPPreventionHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PEPPreventionHistoryController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PEPPreventionHistoryController(IUnitOfWork context, ILogger<PEPPreventionHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pep-prevention-history
        /// </summary>
        /// <param name="pEPPreventionHistory">PEPPreventionHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPPreventionHistory)]
        public async Task<IActionResult> CreatePEPPreventionHistory(HIVPreventionHistory pEPPreventionHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.HIVPreventionHistory, pEPPreventionHistory.EncounterType);
                interaction.EncounterId = pEPPreventionHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = pEPPreventionHistory.CreatedBy;
                interaction.CreatedIn = pEPPreventionHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                pEPPreventionHistory.InteractionId = interactionId;
                pEPPreventionHistory.EncounterId = pEPPreventionHistory.EncounterId;
                pEPPreventionHistory.ClientId = pEPPreventionHistory.ClientId;

                pEPPreventionHistory.DateCreated = DateTime.Now;
                pEPPreventionHistory.IsDeleted = false;
                pEPPreventionHistory.IsSynced = false;

                context.PEPPreventionHistoryRepository.Add(pEPPreventionHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPEPPreventionHistoryByKey", new { key = pEPPreventionHistory.InteractionId }, pEPPreventionHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPPreventionHistory", "PEPPreventionHistoryController.cs", ex.Message, pEPPreventionHistory.CreatedIn, pEPPreventionHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/pep-prevention-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPPreventionHistories)]
        public async Task<IActionResult> ReadPEPPreventionHistories()
        {
            try
            {
                var pEPPreventionHistoryInDb = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistories();

                return Ok(pEPPreventionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPPreventionHistories", "PEPPreventionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-prevention-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPPreventionHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPPreventionHistoryByKey)]
        public async Task<IActionResult> ReadPEPPreventionHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pEPPreventionHistoryInDb = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByKey(key);

                if (pEPPreventionHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pEPPreventionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPPreventionHistoryByKey", "PEPPreventionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadPEPPreventionHistoryByClient)]
        public async Task<IActionResult> ReadPEPPreventionHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var pEPPreventionHistoryInDb = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByClient(clientId); ;

                    return Ok(pEPPreventionHistoryInDb);
                }
                else
                {
                    var pEPPreventionHistoryInDb = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType); ;

                    PagedResultDto<HIVPreventionHistory> hIVPreventionHistoryWIthPaggingDto = new PagedResultDto<HIVPreventionHistory>()
                    {
                        Data = pEPPreventionHistoryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(hIVPreventionHistoryWIthPaggingDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPPreventionHistoryByClient", "PEPPreventionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/pep-prevention-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPPreventionHistories.</param>
        /// <param name="pEPPreventionHistory">PEPPreventionHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPPreventionHistory)]
        public async Task<IActionResult> UpdatePEPPreventionHistory(Guid key, HIVPreventionHistory pEPPreventionHistory)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pEPPreventionHistory.ModifiedBy;
                interactionInDb.ModifiedIn = pEPPreventionHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pEPPreventionHistory.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                pEPPreventionHistory.DateModified = DateTime.Now;
                pEPPreventionHistory.IsDeleted = false;
                pEPPreventionHistory.IsSynced = false;

                context.PEPPreventionHistoryRepository.Update(pEPPreventionHistory);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePEPPreventionHistory", "PEPPreventionHistoryController.cs", ex.Message, pEPPreventionHistory.ModifiedIn, pEPPreventionHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-prevention-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPPreventionHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePEPPreventionHistory)]
        public async Task<IActionResult> DeletePEPPreventionHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pEPPreventionHistoryInDb = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByKey(key);

                if (pEPPreventionHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pEPPreventionHistoryInDb.DateModified = DateTime.Now;
                pEPPreventionHistoryInDb.IsDeleted = true;
                pEPPreventionHistoryInDb.IsSynced = false;

                context.PEPPreventionHistoryRepository.Update(pEPPreventionHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(pEPPreventionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePEPPreventionHistory", "PEPPreventionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}