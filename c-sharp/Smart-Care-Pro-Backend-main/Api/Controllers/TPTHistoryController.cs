using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 04.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TPTHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TPTHistoryController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TPTHistoryController(IUnitOfWork context, ILogger<TPTHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-history
        /// </summary>
        /// <param name="tptHistory">TPTHistory object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTPTHistory)]
        public async Task<IActionResult> CreateTPTHistory(TPTHistory tptHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TPTHistory, tptHistory.EncounterType);
                interaction.EncounterId = tptHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = tptHistory.CreatedBy;
                interaction.CreatedIn = tptHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                tptHistory.InteractionId = interactionId;
                tptHistory.DateCreated = DateTime.Now;
                tptHistory.IsDeleted = false;
                tptHistory.IsSynced = false;

                context.TPTHistoryRepository.Add(tptHistory);
                await context.SaveChangesAsync();

                if(tptHistory.TPTTakenDrugList != null)
                {
                    foreach(var item in tptHistory.TPTTakenDrugList)
                    {
                        TakenTPTDrug takenTPTDrug = new TakenTPTDrug();
                        takenTPTDrug.InteractionId =  Guid.NewGuid();
                        takenTPTDrug.TPTDrugId = item;
                        takenTPTDrug.TPTHistoryId = tptHistory.InteractionId;
                        takenTPTDrug.EncounterId = tptHistory.EncounterId;
                        takenTPTDrug.EncounterDate = tptHistory.EncounterDate;
                        takenTPTDrug.EncounterType = tptHistory.EncounterType;
                        takenTPTDrug.CreatedBy = tptHistory.CreatedBy;
                        takenTPTDrug.DateCreated = tptHistory.DateCreated;
                        takenTPTDrug.CreatedIn = tptHistory.CreatedIn;
                        takenTPTDrug.DateModified = tptHistory.DateModified;
                        takenTPTDrug.ModifiedBy = tptHistory.ModifiedBy;
                        takenTPTDrug.ModifiedIn = tptHistory.ModifiedIn;
                        takenTPTDrug.IsDeleted = tptHistory.IsDeleted;
                        takenTPTDrug.IsSynced = tptHistory.IsSynced;
                        takenTPTDrug.ClinicianName = tptHistory.ClinicianName;

                        context.TakenTPTDrugRepository.Add(takenTPTDrug);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadTPTHistoryByKey", new { key = tptHistory.InteractionId }, tptHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTPTHistory", "TPTHistoryController.cs", ex.Message, tptHistory.CreatedIn, tptHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTPTHistories)]
        public async Task<IActionResult> ReadTPTHistories()
        {
            try
            {
                var tPTHistoriesInDb = await context.TPTHistoryRepository.GetTPTHistories();

                return Ok(tPTHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTPTHistories", "TPTHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTPTHistoryByKey)]
        public async Task<IActionResult> ReadTPTHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tPTHistoryInDb = await context.TPTHistoryRepository.GetTPTHistoryByKey(key);

                if (tPTHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tPTHistoryInDb.TakenTPTDrugs = await context.TakenTPTDrugRepository.LoadListWithChildAsync<TakenTPTDrug>(x => x.TPTHistoryId == tPTHistoryInDb.InteractionId, p => p.TPTDrug);

                return Ok(tPTHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTPTHistoryByKey", "TPTHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-history/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table TPTHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTPTHistoryByClient)]
        public async Task<IActionResult> ReadTPTHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var tPTHistoryInDb = await context.TPTHistoryRepository.GetTPTHistoryByClient(clientId);
                    foreach (var item in tPTHistoryInDb)
                    {
                        item.TakenTPTDrugs = await context.TakenTPTDrugRepository.LoadListWithChildAsync<TakenTPTDrug>(x => x.TPTHistoryId == item.InteractionId, p => p.TPTDrug);
                    }
                    return Ok(tPTHistoryInDb);
                }
                else
                {
                    var tPTHistoryInDb = await context.TPTHistoryRepository.GetTPTHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    tPTHistoryInDb = tPTHistoryInDb.ToList();
                    foreach (var data in tPTHistoryInDb)
                    {
                        data.TakenTPTDrugs = await context.TakenTPTDrugRepository.LoadListWithChildAsync<TakenTPTDrug>(x => x.TPTHistoryId == data.InteractionId, p => p.TPTDrug);
                    }

                    PagedResultDto<TPTHistory> tPTHistoryDto = new PagedResultDto<TPTHistory>()
                    {
                        Data = tPTHistoryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.TPTHistoryRepository.GetTPTHistoryByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(tPTHistoryDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTPTHistoryByClient", "TPTHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTHistory.</param>
        /// <param name="tPTHistory">TPTHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTPTHistory)]
        public async Task<IActionResult> UpdateTPTHistory(Guid key, TPTHistory tPTHistory)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = tPTHistory.ModifiedBy;
                interactionInDb.ModifiedIn = tPTHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != tPTHistory.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tPTHistory.DateModified = DateTime.Now;
                tPTHistory.IsDeleted = false;
                tPTHistory.IsSynced = false;

                context.TPTHistoryRepository.Update(tPTHistory);
                await context.SaveChangesAsync();

                var tPTHistoryInDb = await context.TPTHistoryRepository.GetTPTHistoryByKey(key);

                tPTHistoryInDb.TakenTPTDrugs = await context.TakenTPTDrugRepository.LoadListWithChildAsync<TakenTPTDrug>(x => x.TPTHistoryId == tPTHistoryInDb.InteractionId, p => p.TPTDrug);

                foreach (var item in tPTHistoryInDb.TakenTPTDrugs)
                {
                    context.TakenTPTDrugRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadTPTHistoryByKey", new { key = tPTHistory.InteractionId }, tPTHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTPTHistory", "TPTHistoryController.cs", ex.Message, tPTHistory.ModifiedIn, tPTHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tpt-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TPTHistory.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteTPTHistory)]
        public async Task<IActionResult> DeleteTBHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tPTHistoryInDb = await context.TPTHistoryRepository.GetTPTHistoryByKey(key);

                if (tPTHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);


                tPTHistoryInDb.IsDeleted = true;
                context.TPTHistoryRepository.Update(tPTHistoryInDb);
                await context.SaveChangesAsync();

                return Ok(tPTHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBHistory", "TPTHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}