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
    /// FeedingMethod controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FeedingMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FeedingMethodController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FeedingMethodController(IUnitOfWork context, ILogger<FeedingMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/feeding-method
        /// </summary>
        /// <param name="feedingMethod">FeedingMethod object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFeedingMethod)]
        public async Task<IActionResult> CreateFeedingMethod(FeedingMethod feedingMethod)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();

                foreach (var item in feedingMethod.FeedingMethods)
                {
                    var feedingMethodInDb = await context.FeedingMethodRepository.FirstOrDefaultAsync(x => x.ClientId == item.ClientId && x.EncounterId == feedingMethod.EncounterId && x.Methods == feedingMethod.Methods
                    && x.IsSynced == false && x.IsDeleted == false);

                    if (feedingMethodInDb == null)
                    {
                        Guid interactionId = Guid.NewGuid();

                        Interaction interaction = new Interaction();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FeedingMethod, feedingMethod.EncounterType);
                        interaction.EncounterId = feedingMethod.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = item.CreatedBy;
                        interaction.CreatedIn = item.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        interactions.Add(interaction);
                        context.InteractionRepository.Add(interaction);

                        item.InteractionId = interactionId;
                        item.ClientId = feedingMethod.ClientId;
                        item.EncounterId = feedingMethod.EncounterId;
                        item.Methods = item.Methods;
                        item.DateCreated = DateTime.Now;
                        item.CreatedBy = item.CreatedBy;    
                        item.CreatedIn = item.CreatedIn;
                        item.IsDeleted = false;
                        item.IsSynced = false;
                        item.EncounterType = feedingMethod.EncounterType;

                        context.FeedingMethodRepository.Add(item);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadFeedingMethodByKey", new { key = feedingMethod.InteractionId }, feedingMethod);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFeedingMethod", "FeedingMethodController.cs", ex.Message, feedingMethod.CreatedIn, feedingMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-methods
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingMethods)]
        public async Task<IActionResult> ReadFeedingMethods()
        {
            try
            {
                var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethods();

                return Ok(feedingMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingMethods", "FeedingMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-method/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingMethods.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingMethodByKey)]
        public async Task<IActionResult> ReadFeedingMethodByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByKey(key);

                if (feedingMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(feedingMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingMethodByKey", "FeedingMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-method/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table FeedingMethods.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFeedingMethodByClient)]
        public async Task<IActionResult> ReadFeedingMethodByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //  var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByClient(clientId);
                    var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByClientLast24Hours(clientId);

                    return Ok(feedingMethodInDb);
                }
                else
                {
                    var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);


                    PagedResultDto<FeedingMethod> feedingMethodDto = new PagedResultDto<FeedingMethod>()
                    {
                        Data = feedingMethodInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.FeedingMethodRepository.GetFeedingMethodByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(feedingMethodDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFeedingMethodByClient", "FeedingMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-method/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingMethods.</param>
        /// <param name="feedingMethod">FeedingMethod to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFeedingMethod)]
        public async Task<IActionResult> UpdateFeedingMethod(Guid key, FeedingMethod feedingMethod)
        {
            try
            {
                if (key != feedingMethod.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = feedingMethod.ModifiedBy;
                interactionInDb.ModifiedIn = feedingMethod.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                feedingMethod.DateModified = DateTime.Now;
                feedingMethod.IsDeleted = false;
                feedingMethod.IsSynced = false;

                context.FeedingMethodRepository.Update(feedingMethod);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFeedingMethod", "FeedingMethodController.cs", ex.Message, feedingMethod.ModifiedIn, feedingMethod.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/feeding-method/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FeedingMethods.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteFeedingMethod)]
        public async Task<IActionResult> DeleteFeedingMethod(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByKey(key);

                if (feedingMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                feedingMethodInDb.DateModified = DateTime.Now;
                feedingMethodInDb.IsDeleted = true;
                feedingMethodInDb.IsSynced = false;

                context.FeedingMethodRepository.Update(feedingMethodInDb);
                await context.SaveChangesAsync();

                return Ok(feedingMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFeedingMethod", "FeedingMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// feeding-method/remove/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveFeedingMethod)]
        public async Task<IActionResult> RemoveFeedingMethod(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var feedingMethodInDb = await context.FeedingMethodRepository.GetFeedingMethodByEncounter(key);

                if (feedingMethodInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in feedingMethodInDb)
                {
                    context.FeedingMethodRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(feedingMethodInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveFeedingMethod", "FeedingMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}