using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 28.03.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// FeedingHistory controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FeedingHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FeedingHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FeedingHistoryController(IUnitOfWork context, ILogger<FeedingHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/feeding-history
        /// </summary>
        /// <param name="feedingHistory">FeedingHistory object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFeedingHistory)]
        public async Task<IActionResult> CreateFeedingHistory(FeedingHistory feedingHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FeedingHistory, feedingHistory.EncounterType);
                interaction.EncounterId = feedingHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = feedingHistory.CreatedBy;
                interaction.CreatedIn = feedingHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                feedingHistory.InteractionId = interactionId;
                feedingHistory.EncounterId = feedingHistory.EncounterId;
                feedingHistory.ClientId = feedingHistory.ClientId;
                feedingHistory.DateCreated = DateTime.Now;
                feedingHistory.IsDeleted = false;
                feedingHistory.IsSynced = false;

                context.FeedingHistoryRepository.Add(feedingHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFeedingHistoryByKey", new { key = feedingHistory.InteractionId }, feedingHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFeedingHistory", "FeedingHistoryController.cs", ex.Message, feedingHistory.CreatedIn, feedingHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingHistories)]
        public async Task<IActionResult> ReadFeedingHistories()
        {
            try
            {
                var feedingHistoryInDb = await context.FeedingHistoryRepository.GetFeedingHistories();

                return Ok(feedingHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingHistories", "FeedingHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingHistoryByKey)]
        public async Task<IActionResult> ReadFeedingHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var feedingHistoryInDb = await context.FeedingHistoryRepository.GetFeedingHistoryByKey(key);

                if (feedingHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(feedingHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingHistoryByKey", "FeedingHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-history/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table FeedingHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingHistoryByClient)]
        public async Task<IActionResult> ReadFeedingHistoryByClient(Guid clientId)
        {
            try
            {
                var feedingHistoryInDb = await context.FeedingHistoryRepository.GetFeedingHistoryByClient(clientId);

                return Ok(feedingHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingHistoryByClient", "FeedingHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingHistories.</param>
        /// <param name="feedingHistory">FeedingHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFeedingHistory)]
        public async Task<IActionResult> UpdateFeedingHistory(Guid key, FeedingHistory feedingHistory)
        {
            try
            {
                if (key != feedingHistory.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = feedingHistory.ModifiedBy;
                interactionInDb.ModifiedIn = feedingHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                feedingHistory.DateModified = DateTime.Now;
                feedingHistory.IsDeleted = false;
                feedingHistory.IsSynced = false;

                context.FeedingHistoryRepository.Update(feedingHistory);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFeedingHistory", "FeedingHistoryController.cs", ex.Message, feedingHistory.ModifiedIn, feedingHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFeedingHistory)]
        public async Task<IActionResult> DeleteFeedingHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var feedingHistoryInDb = await context.FeedingHistoryRepository.GetFeedingHistoryByKey(key);

                if (feedingHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.FeedingHistoryRepository.Update(feedingHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(feedingHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFeedingHistory", "FeedingHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}