using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 01.04.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBHistoryController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBHistoryController(IUnitOfWork context, ILogger<TBHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-history
        /// </summary>
        /// <param name="tBHistory">TBHistory object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTBHistory)]
        public async Task<IActionResult> CreateTBHistory(TBHistory tBHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TBHistory, tBHistory.EncounterType);
                interaction.EncounterId = tBHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = tBHistory.CreatedBy;
                interaction.CreatedIn = tBHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                tBHistory.InteractionId = interactionId;
                tBHistory.EncounterType = tBHistory.EncounterType;
                tBHistory.EncounterId = tBHistory.EncounterId;
                tBHistory.CreatedBy = tBHistory.CreatedBy;
                tBHistory.CreatedIn = tBHistory.CreatedIn;
                tBHistory.DateCreated = DateTime.Now;
                tBHistory.IsDeleted = false;
                tBHistory.IsSynced = false;

                if (tBHistory.TBIdentificationMethodList != null && tBHistory.TBIdentificationMethodList.Length > 0)
                {
                  

                    foreach (var itemId in tBHistory.TBIdentificationMethodList)
                    {
                        UsedTBIdentificationMethod identifiedTBMethods = new UsedTBIdentificationMethod();
                        identifiedTBMethods.EncounterId = tBHistory.EncounterId;
                        identifiedTBMethods.CreatedBy = tBHistory.CreatedBy;
                        identifiedTBMethods.CreatedIn = tBHistory.CreatedIn;
                        identifiedTBMethods.EncounterType = tBHistory.EncounterType;
                        identifiedTBMethods.TBIdentificationMethodId = itemId;
                        identifiedTBMethods.TBHistoryId = tBHistory.InteractionId;
                        identifiedTBMethods.DateCreated = DateTime.Now;
                        identifiedTBMethods.IsDeleted = false;
                        identifiedTBMethods.IsSynced = false;

                        context.UseTBIdentificationMethodRepository.Add(identifiedTBMethods);
                    }

                }

                if (tBHistory.TBTakenDrugList != null && tBHistory.TBTakenDrugList.Length > 0)
                {
                    

                    foreach (var itemId in tBHistory.TBTakenDrugList)
                    {
                        TakenTBDrug takenTBDrugs = new TakenTBDrug();
                        takenTBDrugs.EncounterId = tBHistory.EncounterId;
                        takenTBDrugs.CreatedBy = tBHistory.CreatedBy;
                        takenTBDrugs.CreatedIn = tBHistory.CreatedIn;
                        takenTBDrugs.EncounterType = tBHistory.EncounterType;
                        takenTBDrugs.TBDrugId = itemId;
                        takenTBDrugs.TBHistoryId = tBHistory.InteractionId;
                        takenTBDrugs.DateCreated = DateTime.Now;
                        takenTBDrugs.IsDeleted = false;
                        takenTBDrugs.IsDeleted = false;

                        context.TakenTBDrugRepository.Add(takenTBDrugs);
                    }
                }

                context.TBHistoryRepository.Add(tBHistory);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTBHistoryByKey", new { key = tBHistory.InteractionId }, tBHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTBHistory", "TBHistoryController.cs", ex.Message, tBHistory.CreatedIn, tBHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBHistories)]
        public async Task<IActionResult> ReadTBHistories()
        {
            try
            {
                var tBHistoriesInDb = await context.TBHistoryRepository.GetTBHistories();

                return Ok(tBHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBHistories", "TBHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBHistoryByKey)]
        public async Task<IActionResult> ReadTBHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBHistoryInDb = await context.TBHistoryRepository.GetTBHistoryByKey(key);

                if (tBHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tBHistoryInDb.UsedTBIdentificationMethods = await context.UseTBIdentificationMethodRepository.LoadListWithChildAsync<UsedTBIdentificationMethod>(x => x.TBHistoryId == tBHistoryInDb.InteractionId, p => p.TBIdentificationMethod);

                tBHistoryInDb.TakenTBDrugs = await context.TakenTBDrugRepository.LoadListWithChildAsync<TakenTBDrug>(x => x.TBHistoryId == tBHistoryInDb.InteractionId, p => p.TBDrug);

                return Ok(tBHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBHistoryByKey", "TBHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-history/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table TBHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBHistoryByClient)]
        public async Task<IActionResult> ReadTBHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var tBHistoryInDb = await context.TBHistoryRepository.GetTBHistoryByClient(clientId);

                    return Ok(tBHistoryInDb);
                }
                else
                {
                    var tBHistoryInDb = await context.TBHistoryRepository.GetTBHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<TBHistory> tBHistoryWithPaggingDto = new PagedResultDto<TBHistory>()
                    {
                        Data = tBHistoryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.TBHistoryRepository.GetTBHistoryByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(tBHistoryWithPaggingDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBHistoryByClient", "TBHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBHistory.</param>
        /// <param name="tBHistory">TBHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTBHistory)]
        public async Task<IActionResult> UpdateTBHistory(Guid key, TBHistory tBHistory)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = tBHistory.ModifiedBy;
                interactionInDb.ModifiedIn = tBHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != tBHistory.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tBHistory.DateModified = DateTime.Now;
                tBHistory.IsDeleted = false;
                tBHistory.IsSynced = false;

                context.TBHistoryRepository.Update(tBHistory);
                await context.SaveChangesAsync();

                var tBHistoryInDb = await context.TBHistoryRepository.GetTBHistoryByKey(key);

                tBHistoryInDb.UsedTBIdentificationMethods = await context.UseTBIdentificationMethodRepository.LoadListWithChildAsync<UsedTBIdentificationMethod>(x => x.TBHistoryId == tBHistoryInDb.InteractionId, p => p.TBIdentificationMethod);

                foreach (var item in tBHistoryInDb.UsedTBIdentificationMethods)
                {
                    context.UseTBIdentificationMethodRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (tBHistory.TBIdentificationMethodList != null && tBHistory.TBIdentificationMethodList.Length > 0)
                {
                   
                    foreach (var itemId in tBHistory.TBIdentificationMethodList)
                    {
                        UsedTBIdentificationMethod identifiedTBMethods = new UsedTBIdentificationMethod();

                        identifiedTBMethods.EncounterId = tBHistory.EncounterId;
                        identifiedTBMethods.CreatedBy = tBHistory.CreatedBy;
                        identifiedTBMethods.CreatedIn = tBHistory.CreatedIn;
                        identifiedTBMethods.EncounterType = tBHistory.EncounterType;
                        identifiedTBMethods.TBIdentificationMethodId = itemId;
                        identifiedTBMethods.TBHistoryId = tBHistory.InteractionId;

                        context.UseTBIdentificationMethodRepository.Add(identifiedTBMethods);
                        await context.SaveChangesAsync();
                    }

                }

                tBHistoryInDb.TakenTBDrugs = await context.TakenTBDrugRepository.LoadListWithChildAsync<TakenTBDrug>(x => x.TBHistoryId == tBHistoryInDb.InteractionId, p => p.TBDrug);

                foreach (var item in tBHistoryInDb.TakenTBDrugs)
                {
                    context.TakenTBDrugRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (tBHistory.TBTakenDrugList != null && tBHistory.TBTakenDrugList.Length > 0)
                {
                   
                    foreach (var itemId in tBHistory.TBTakenDrugList)
                    {
                        TakenTBDrug takenTBDrugs = new TakenTBDrug();

                        takenTBDrugs.InteractionId = new Guid();
                        takenTBDrugs.EncounterId = tBHistory.EncounterId;
                        takenTBDrugs.CreatedBy = tBHistory.CreatedBy;
                        takenTBDrugs.CreatedIn = tBHistory.CreatedIn;
                        takenTBDrugs.EncounterType = tBHistory.EncounterType;
                        takenTBDrugs.TBDrugId = itemId;
                        takenTBDrugs.TBHistoryId = tBHistory.InteractionId;

                        context.TakenTBDrugRepository.Add(takenTBDrugs);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadTBHistoryByKey", new { key = tBHistory.InteractionId }, tBHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTBHistory", "TBHistoryController.cs", ex.Message, tBHistory.ModifiedIn, tBHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBHistory.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteTBHistory)]
        public async Task<IActionResult> DeleteTBHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tBHistoryInDb = await context.TBHistoryRepository.GetTBHistoryByKey(key);

                if (tBHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tBHistoryInDb.DateModified = DateTime.Now;
                tBHistoryInDb.IsDeleted = true;
                tBHistoryInDb.IsSynced = false;

                context.TBHistoryRepository.Update(tBHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(tBHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBHistory", "TBHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
