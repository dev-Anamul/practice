using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// NewBornDetail controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NewBornDetailController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NewBornDetailController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NewBornDetailController(IUnitOfWork context, ILogger<NewBornDetailController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/new-born-detail
        /// </summary>
        /// <param name="newBornDetail">NewBornDetail object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNewBornDetail)]
        public async Task<IActionResult> CreateNewBornDetail(NewBornDetail newBornDetail)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.NewBornDetail, newBornDetail.EncounterType);
                interaction.EncounterId = newBornDetail.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = newBornDetail.CreatedBy;
                interaction.CreatedIn = newBornDetail.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                newBornDetail.InteractionId = interactionId;
                newBornDetail.DateCreated = DateTime.Now;
                newBornDetail.IsDeleted = false;
                newBornDetail.IsSynced = false;

                context.NewBornDetailRepository.Add(newBornDetail);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNewBornDetailByKey", new { key = newBornDetail.InteractionId }, newBornDetail);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNewBornDetail", "NewBornDetailController.cs", ex.Message, newBornDetail.CreatedIn, newBornDetail.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/byDelivery/{DeliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNewBornDetailByDelivery)]
        public async Task<IActionResult> ReadNewBornDetailByDelivery(Guid deliveryId)
        {
            try
            {
                var newBornDetailInDb = await context.NewBornDetailRepository.GetNewBornDetailByDelivery(deliveryId);

                return Ok(newBornDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNewBornDetailByDelivery", "NewBornDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadNewBornDetailByDeliveryHistory)]
        public async Task<IActionResult> ReadNewBornDetailByDeliveryHistory(Guid encounterId)
        {
            try
            {
                NewBornDetailHistoryDto newBornDetailHistoryDto = new()
                {
                    ApgarScores = context.ApgarScoreRepository.GetApgarScoreByEncounter(encounterId).Result.ToList(),
                    NeonatalDeaths = context.NeonatalDeathRepository.GetNeonatalDeathByEncounter(encounterId).Result.ToList(),
                    NeonatalAbnormalities = context.NeonatalAbnormalityRepository.GetNeonatalAbnormalityByEncounter(encounterId).Result.ToList(),
                    NeonatalInjuries = context.NeonatalInjuryRepository.GetNeonatalInjuryByEncounter(encounterId).Result.ToList()
                };

                return Ok(newBornDetailHistoryDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNewBornDetailByDeliveryHistory", "NewBornDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/new-born-details
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNewBornDetails)]
        public async Task<IActionResult> ReadNewBornDetails()
        {
            try
            {
                var newBornDetailInDb = await context.NewBornDetailRepository.GetNewBornDetails();

                return Ok(newBornDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNewBornDetails", "NewBornDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/new-born-detail/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNewBornDetailByKey)]
        public async Task<IActionResult> ReadNewBornDetailByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var newBornDetailInDb = await context.NewBornDetailRepository.GetNewBornDetailByKey(key);

                if (newBornDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(newBornDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNewBornDetailByKey", "NewBornDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/new-born-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NewBornDetails.</param>
        /// <param name="newBornDetail">NewBornDetail to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNewBornDetail)]
        public async Task<IActionResult> UpdateNewBornDetail(Guid key, NewBornDetail newBornDetail)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = newBornDetail.ModifiedBy;
                interactionInDb.ModifiedIn = newBornDetail.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != newBornDetail.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                newBornDetail.DateModified = DateTime.Now;
                newBornDetail.IsDeleted = false;
                newBornDetail.IsSynced = false;

                context.NewBornDetailRepository.Update(newBornDetail);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNewBornDetail", "NewBornDetailController.cs", ex.Message, newBornDetail.ModifiedIn, newBornDetail.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/new-born-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NewBornDetails.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteNewBornDetail)]
        public async Task<IActionResult> DeleteNewBornDetail(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var newBornDetailInDb = await context.NewBornDetailRepository.GetNewBornDetailByKey(key);

                if (newBornDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.NewBornDetailRepository.Update(newBornDetailInDb);
                await context.SaveChangesAsync();

                return Ok(newBornDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNewBornDetail", "NewBornDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
