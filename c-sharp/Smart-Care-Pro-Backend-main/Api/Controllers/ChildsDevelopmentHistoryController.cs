using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 26.12.2022
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ChildsDevelopmentHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ChildsDevelopmentHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ChildsDevelopmentHistoryController(IUnitOfWork context, ILogger<ChildsDevelopmentHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/childs-dev-history
        /// </summary>
        /// <param name="childsDevelopmentHistory">ChildsDevelopmentHistory object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateChildsDevelopmentHistory)]
        public async Task<IActionResult> CreateChildsDevelopmentHistory(ChildsDevelopmentHistory childsDevelopmentHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ChildsDevelopmentHistory, childsDevelopmentHistory.EncounterType);
                interaction.EncounterId = childsDevelopmentHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = childsDevelopmentHistory.CreatedBy;
                interaction.CreatedIn = childsDevelopmentHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                childsDevelopmentHistory.InteractionId = interactionId;
                childsDevelopmentHistory.ClientId = childsDevelopmentHistory.ClientId;
                childsDevelopmentHistory.DateCreated = DateTime.Now;
                childsDevelopmentHistory.IsDeleted = false;
                childsDevelopmentHistory.IsSynced = false;

                context.ChildsDevelopmentHistoryRepository.Add(childsDevelopmentHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadChildsDevelopmentHistoryByKey", new { key = childsDevelopmentHistory.InteractionId }, childsDevelopmentHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateChildsDevelopmentHistory", "ChildsDevelopmentHistoryController.cs", ex.Message, childsDevelopmentHistory.CreatedIn, childsDevelopmentHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: childs-dev-history/ByClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildsDevelopmentHistoryByClient)]
        public async Task<IActionResult> ReadChildsDevelopmentHistoryByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByClient(clientId);
                    var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByClientLast24Hours(clientId);

                    childsDevelopmentHistoryInDb = childsDevelopmentHistoryInDb.OrderByDescending(x => x.DateCreated);

                    return Ok(childsDevelopmentHistoryInDb);
                }
                else
                {
                    var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize);

                    childsDevelopmentHistoryInDb = childsDevelopmentHistoryInDb.OrderByDescending(x => x.DateCreated);

                    PagedResultDto<ChildsDevelopmentHistory> diagnosisWithPaggingDto = new PagedResultDto<ChildsDevelopmentHistory>()
                    {
                        Data = childsDevelopmentHistoryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByClientCount(clientId)
                    };
                    return Ok(diagnosisWithPaggingDto);
                }



            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildsDevelopmentHistoryByClient", "ChildsDevelopmentHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/childs-dev-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildsDevelopmentHistories)]
        public async Task<IActionResult> ReadChildsDevelopmentHistories()
        {
            try
            {
                var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistories();

                return Ok(childsDevelopmentHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildsDevelopmentHistories", "ChildsDevelopmentHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/childs-dev-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildsDevelopmentHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildsDevelopmentHistoryByKey)]
        public async Task<IActionResult> ReadChildsDevelopmentHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByKey(key);

                if (childsDevelopmentHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(childsDevelopmentHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildsDevelopmentHistoryByKey", "ChildsDevelopmentHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/childs-dev-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildsDevelopmentHistory.</param>
        /// <param name="ChildsDevelopmentHistory">ChildsDevelopmentHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateChildsDevelopmentHistory)]
        public async Task<IActionResult> UpdateChildsDevelopmentHistory(Guid key, ChildsDevelopmentHistory childsDevelopmentHistory)
        {
            try
            {
                if (key != childsDevelopmentHistory.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = childsDevelopmentHistory.ModifiedBy;
                interactionInDb.ModifiedIn = childsDevelopmentHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                childsDevelopmentHistory.DateModified = DateTime.Now;
                childsDevelopmentHistory.IsDeleted = false;
                childsDevelopmentHistory.IsSynced = false;

                context.ChildsDevelopmentHistoryRepository.Update(childsDevelopmentHistory);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateChildsDevelopmentHistory", "ChildsDevelopmentHistoryController.cs", ex.Message, childsDevelopmentHistory.ModifiedIn, childsDevelopmentHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/childs-dev-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildsDevelopmentHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteChildsDevelopmentHistory)]
        public async Task<IActionResult> DeleteChildsDevelopmentHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var childsDevelopmentHistoryInDb = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByKey(key);

                if (childsDevelopmentHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.ChildsDevelopmentHistoryRepository.Update(childsDevelopmentHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(childsDevelopmentHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteChildsDevelopmentHistory", "ChildsDevelopmentHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}