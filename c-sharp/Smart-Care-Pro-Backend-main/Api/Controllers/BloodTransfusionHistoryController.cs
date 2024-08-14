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
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// BloodTransfusionHistory controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BloodTransfusionHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BloodTransfusionHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BloodTransfusionHistoryController(IUnitOfWork context, ILogger<BloodTransfusionHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-history
        /// </summary>
        /// <param name="bloodTransfusionHistory">BloodTransfusionHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBloodTransfusionHistory)]
        public async Task<IActionResult> CreateBloodTransfusionHistory(BloodTransfusionHistory bloodTransfusionHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.BloodTransfusionHistory, bloodTransfusionHistory.EncounterType);
                interaction.EncounterId = bloodTransfusionHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = bloodTransfusionHistory.CreatedBy;
                interaction.CreatedIn = bloodTransfusionHistory.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                bloodTransfusionHistory.InteractionId = interactionId;
                bloodTransfusionHistory.DateCreated = DateTime.Now;
                bloodTransfusionHistory.IsDeleted = false;
                bloodTransfusionHistory.IsSynced = false;

                foreach (var item in bloodTransfusionHistory.IdentifiedPriorSensitizationsList)
                {
                    IdentifiedPriorSensitization identifiedPriorSensitization = new IdentifiedPriorSensitization();

                    identifiedPriorSensitization.InteractionId = Guid.NewGuid();
                    identifiedPriorSensitization.BloodTransfusionId = bloodTransfusionHistory.InteractionId;
                    identifiedPriorSensitization.EncounterId = bloodTransfusionHistory.EncounterId;
                    identifiedPriorSensitization.PriorSensitizationId = item;
                    identifiedPriorSensitization.CreatedIn = bloodTransfusionHistory.CreatedIn;
                    identifiedPriorSensitization.CreatedBy = bloodTransfusionHistory.CreatedBy;
                    identifiedPriorSensitization.DateCreated = DateTime.Now;
                    identifiedPriorSensitization.IsDeleted = false;
                    identifiedPriorSensitization.IsSynced = false;

                    context.IdentifiedPriorSensitizationRepository.Add(identifiedPriorSensitization);
                }

                context.BloodTransfusionHistoryRepository.Add(bloodTransfusionHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadBloodTransfusionHistoryByKey", new { key = bloodTransfusionHistory.InteractionId }, bloodTransfusionHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBloodTransfusionHistory", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBloodTransfusionHistorys)]
        public async Task<IActionResult> ReadBloodTransfusionHistorys()
        {
            try
            {
                var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistorys(); bloodTransfusionHistoryInDb = bloodTransfusionHistoryInDb.OrderByDescending(x => x.DateCreated);

                return Ok(bloodTransfusionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBloodTransfusionHistorys", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-history/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBloodTransfusionHistoryByClient)]
        public async Task<IActionResult> ReadBloodTransfusionHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByClient(clientId);
                    var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByClientLast24Hours(clientId);

                    foreach (var item in bloodTransfusionHistoryInDb)
                    {
                        item.IdentifiedPriorSensitizations = await context.IdentifiedPriorSensitizationRepository.LoadListWithChildAsync<IdentifiedPriorSensitization>(x => x.IsDeleted == false && x.BloodTransfusionId == item.InteractionId, p => p.PriorSensitization);
                    }

                    return Ok(bloodTransfusionHistoryInDb);
                }
                else
                {
                    var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    foreach (var item in bloodTransfusionHistoryInDb)
                    {
                        item.IdentifiedPriorSensitizations = await context.IdentifiedPriorSensitizationRepository.LoadListWithChildAsync<IdentifiedPriorSensitization>(x => x.IsDeleted == false && x.BloodTransfusionId == item.InteractionId, p => p.PriorSensitization);
                    }

                    PagedResultDto<BloodTransfusionHistory> bloodTransfusionHistoryDto = new PagedResultDto<BloodTransfusionHistory>()
                    {
                        Data = bloodTransfusionHistoryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(bloodTransfusionHistoryDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBloodTransfusionHistoryByClient", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-history/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBloodTransfusionHistoryByEncounter)]
        public async Task<IActionResult> ReadBloodTransfusionHistoryByEncounter(Guid EncounterId)
        {
            try
            {
                var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByEncounter(EncounterId);

                return Ok(bloodTransfusionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBloodTransfusionHistoryByEncounter", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BloodTransfusionHistorys.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBloodTransfusionHistoryByKey)]
        public async Task<IActionResult> ReadBloodTransfusionHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByKey(key);

                if (bloodTransfusionHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(bloodTransfusionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBloodTransfusionHistoryByKey", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/blood-transfusion-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BloodTransfusionHistorys.</param>
        /// <param name="bloodTransfusionHistory">BloodTransfusionHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBloodTransfusionHistory)]
        public async Task<IActionResult> UpdateBloodTransfusionHistory(Guid key, BloodTransfusionHistory bloodTransfusionHistory)
        {
            try
            {
                if (key != bloodTransfusionHistory.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = bloodTransfusionHistory.ModifiedBy;
                interactionInDb.ModifiedIn = bloodTransfusionHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetByIdAsync(key);

                if (bloodTransfusionHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                bloodTransfusionHistoryInDb.DateModified = DateTime.Now;
                bloodTransfusionHistoryInDb.ModifiedIn = bloodTransfusionHistory.ModifiedIn;
                bloodTransfusionHistoryInDb.ModifiedBy = bloodTransfusionHistory.ModifiedBy;
                bloodTransfusionHistoryInDb.IsDeleted = false;
                bloodTransfusionHistoryInDb.IsSynced = false;
                bloodTransfusionHistoryInDb.BloodGroup = bloodTransfusionHistory.BloodGroup;
                bloodTransfusionHistoryInDb.NumberOfUnits = bloodTransfusionHistory.NumberOfUnits;
                bloodTransfusionHistoryInDb.KinBloodGroup = bloodTransfusionHistory.KinBloodGroup;
                bloodTransfusionHistoryInDb.RHSensitivity = bloodTransfusionHistory.RHSensitivity;

                context.BloodTransfusionHistoryRepository.Update(bloodTransfusionHistoryInDb);
                await context.SaveChangesAsync();

                var identifiedPriorSensitizationlist = context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByBloodTransfusion(key);

                foreach (var item in identifiedPriorSensitizationlist.Result.ToList())
                {
                    context.IdentifiedPriorSensitizationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (bloodTransfusionHistory.IdentifiedPriorSensitizationsList != null)
                {
                    foreach (var item in bloodTransfusionHistory.IdentifiedPriorSensitizationsList)
                    {
                        IdentifiedPriorSensitization identifiedPriorSensitization = new IdentifiedPriorSensitization();

                        identifiedPriorSensitization.InteractionId = Guid.NewGuid();
                        identifiedPriorSensitization.BloodTransfusionId = bloodTransfusionHistory.InteractionId;
                        identifiedPriorSensitization.EncounterId = bloodTransfusionHistory.EncounterId;
                        identifiedPriorSensitization.PriorSensitizationId = item;
                        identifiedPriorSensitization.CreatedBy = bloodTransfusionHistory.CreatedBy;
                        identifiedPriorSensitization.CreatedIn = bloodTransfusionHistory.CreatedIn;
                        identifiedPriorSensitization.DateCreated = DateTime.Now;
                        identifiedPriorSensitization.IsDeleted = false;
                        identifiedPriorSensitization.IsSynced = false;

                        context.IdentifiedPriorSensitizationRepository.Add(identifiedPriorSensitization);
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBloodTransfusionHistory", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/blood-transfusion-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BloodTransfusionHistorys.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteBloodTransfusionHistory)]
        public async Task<IActionResult> DeleteBloodTransfusionHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var bloodTransfusionHistoryInDb = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByKey(key);

                if (bloodTransfusionHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                bloodTransfusionHistoryInDb.DateModified = DateTime.Now;
                bloodTransfusionHistoryInDb.IsDeleted = true;
                bloodTransfusionHistoryInDb.IsSynced = false;

                context.BloodTransfusionHistoryRepository.Update(bloodTransfusionHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(bloodTransfusionHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteBloodTransfusionHistory", "BloodTransfusionHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}