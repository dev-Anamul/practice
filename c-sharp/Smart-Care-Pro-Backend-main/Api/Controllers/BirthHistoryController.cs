using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 26.12.2022
 * Modified by   : Stephan
 * Last modified : 05.01.2022
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    ///BirthHistory  Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BirthHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BirthHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BirthHistoryController(IUnitOfWork context, ILogger<BirthHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/birth-history
        /// </summary>
        /// <param name="birthHistory">BirthHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBirthHistory)]
        public async Task<IActionResult> CreateBirthHistory(BirthHistory birthHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.BirthHistory, birthHistory.EncounterType);
                interaction.EncounterId = birthHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy =birthHistory.CreatedBy;
                interaction.CreatedIn = birthHistory.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                birthHistory.InteractionId = interactionId;
                birthHistory.EncounterId = birthHistory.EncounterId;
                birthHistory.ClientId = birthHistory.ClientId;
                birthHistory.CreatedBy = birthHistory.CreatedBy;
                birthHistory.DateCreated = DateTime.Now;
                birthHistory.IsDeleted = false;
                birthHistory.IsSynced = false;

                context.BirthHistoryRepository.Add(birthHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadBirthHistoryByKey", new { key = birthHistory.InteractionId }, birthHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBirthHistory", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthHistories)]
        public async Task<IActionResult> ReadBirthHistories()
        {
            try
            {
                var birthHistoryInDb = await context.BirthHistoryRepository.GetBirthHistories();

                birthHistoryInDb = birthHistoryInDb.OrderByDescending(x => x.DateCreated);

                return Ok(birthHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthHistories", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: birth-history/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthHistoryByClient)]
        public async Task<IActionResult> ReadBirthHistoryByClient(Guid clientId)
        {
            try
            {
                var birthHistoryInDb = await context.BirthHistoryRepository.GetBirthHistoryByClient(clientId); 

                return Ok(birthHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthHistoryByClient", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// birth-history/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthHistoryByEncounterId)]
        public async Task<IActionResult> ReadBirthHistoryByEncounterID(Guid encounterId)
        {
            try
            {
                var birthHistoryInDb = await context.BirthHistoryRepository.GetBirthHistoryByEncounter(encounterId);

                return Ok(birthHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthHistoryByEncounterID", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthHistoryByKey)]
        public async Task<IActionResult> ReadBirthHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthHistoryInDb = await context.BirthHistoryRepository.GetBirthHistoryByKey(key);

                if (birthHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(birthHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthHistoryByKey", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/birth-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="birthHistory">BirthHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBirthHistory)]
        public async Task<IActionResult> UpdateBirthHistory(Guid key, BirthHistory birthHistory)
        {
            try
            {
                if (key != birthHistory.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = birthHistory.ModifiedBy;
                interactionInDb.ModifiedIn = birthHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                birthHistory.DateModified = DateTime.Now;
                birthHistory.IsDeleted= false;
                birthHistory.IsSynced = false;

                context.BirthHistoryRepository.Update(birthHistory);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBirthHistory", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteBirthHistory)]
        public async Task<IActionResult> DeleteBirthHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthHistoryInDb = await context.BirthHistoryRepository.GetBirthHistoryByKey(key);

                if (birthHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                birthHistoryInDb.DateModified = DateTime.Now;
                birthHistoryInDb.IsDeleted = true;
                birthHistoryInDb.IsSynced = false;

                context.BirthHistoryRepository.Update(birthHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(birthHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteBirthHistory", "BirthHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}