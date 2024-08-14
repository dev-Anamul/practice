using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    ///GynObsHistory Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
   [Authorize]

    public class GynObsHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GynObsHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GynObsHistoryController(IUnitOfWork context, ILogger<GynObsHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history
        /// </summary>
        /// <param name="gynObsHistory">GynObsHistory object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGynObsHistory)]
        public async Task<IActionResult> CreateGynObsHistory(GynObsHistory gynObsHistory)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.GynObsHistory, gynObsHistory.EncounterType);
                interaction.EncounterId = gynObsHistory.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = gynObsHistory.CreatedBy;
                interaction.CreatedIn = gynObsHistory.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                gynObsHistory.InteractionId = interactionId;
                gynObsHistory.DateCreated = DateTime.Now;
                gynObsHistory.ClientId = gynObsHistory.ClientId;
                gynObsHistory.IsDeleted = false;
                gynObsHistory.IsSynced = false;

                context.GynObsHistoryRepository.Add(gynObsHistory);
                await context.SaveChangesAsync();

                if (gynObsHistory.ContraceptiveHistoryList != null)
                {
                    foreach (var item in gynObsHistory.ContraceptiveHistoryList)
                    {
                        ContraceptiveHistory contraceptiveHistory = new ContraceptiveHistory();

                        contraceptiveHistory.InteractionId = Guid.NewGuid();
                        contraceptiveHistory.GynObsHistoryId = interactionId;
                        contraceptiveHistory.DateCreated = DateTime.Now;
                        contraceptiveHistory.ContraceptiveId = item;
                        contraceptiveHistory.EncounterId = gynObsHistory.EncounterId;
                        contraceptiveHistory.IsDeleted = false;
                        contraceptiveHistory.IsSynced = false;

                        context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                        await context.SaveChangesAsync();

                    }
                }

                if(gynObsHistory.ContraceptiveHistory != null)
                {
                    foreach (var item in gynObsHistory.ContraceptiveHistory)
                    {
                        ContraceptiveHistory contraceptiveHistory = new ContraceptiveHistory();

                        contraceptiveHistory.InteractionId = Guid.NewGuid();
                        contraceptiveHistory.GynObsHistoryId = interactionId;
                        contraceptiveHistory.DateCreated = DateTime.Now;
                        contraceptiveHistory.ContraceptiveId = item.ContraceptiveId;
                        contraceptiveHistory.UsedFor = item.UsedFor;
                        contraceptiveHistory.EncounterId = gynObsHistory.EncounterId;
                        contraceptiveHistory.IsDeleted = false;
                        contraceptiveHistory.IsSynced = false;

                        context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadGynObsHistoryByKey", new { key = gynObsHistory.InteractionId }, gynObsHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGynObsHistory", "GynObsHistoryController.cs", ex.Message, gynObsHistory.CreatedIn, gynObsHistory.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynObsHistories)]
        public async Task<IActionResult> ReadGynObsHistories()
        {
            try
            {
                var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistories();

                foreach (var item in gynObsHistoriesInDb)
                {
                    item.ContraceptiveHistories = await context.ContraceptiveHistoryRepository.LoadListWithChildAsync<ContraceptiveHistory>(g => g.IsDeleted == false && g.GynObsHistoryId == item.InteractionId, p => p.Contraceptive);
                }

                return Ok(gynObsHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynObsHistories", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history-by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynObsHistoryByClientId)]
        public async Task<IActionResult> ReadGynObsHistoriesByClientID(Guid clientId, int page, int pageSize, EncounterType encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                   // var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByClientID(clientId);
                    var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByClientIDLast24Hours(clientId);

                    return Ok(gynObsHistoriesInDb);
                }
                else
                {
                    var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByClientID(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<GynObsHistory> gynObsHistoryWithPaggingDto = new PagedResultDto<GynObsHistory>()
                    {
                        Data = gynObsHistoriesInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.GynObsHistoryRepository.GetGynObsHistoryByClientIDTotalCount(clientId, encounterType)
                    };
                    return Ok(gynObsHistoryWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynObsHistoriesByClientID", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/latest-gyn-obs-history-by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLatestGynObsHistoriesByClientId)]
        public async Task<IActionResult> ReadLatestGynObsHistoriesByClientID(Guid clientId)
        {
            try
            {
                var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetLatestGynObsHistoryByClientID(clientId);

                if (gynObsHistoriesInDb != null)
                    gynObsHistoriesInDb.ContraceptiveHistories = await context.ContraceptiveHistoryRepository.LoadListWithChildAsync<ContraceptiveHistory>(g => g.IsDeleted == false && g.GynObsHistoryId == gynObsHistoriesInDb.InteractionId, p => p.Contraceptive);

                return Ok(gynObsHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestGynObsHistoriesByClientID", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynObsHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynObsHistoryByKey)]
        public async Task<IActionResult> ReadGynObsHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByKey(key);

                if (gynObsHistoriesInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(gynObsHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynObsHistoryByKey", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history-by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadGynObsHistoryByEncounterId)]
        public async Task<IActionResult> ReadGynObsHistoryByEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByEncounterId(encounterId);

                foreach (var item in gynObsHistoriesInDb)
                {
                    item.ContraceptiveHistories = await context.ContraceptiveHistoryRepository.LoadListWithChildAsync<ContraceptiveHistory>(g => g.IsDeleted == false && g.InteractionId == item.InteractionId, p => p.Contraceptive);
                }

                if (gynObsHistoriesInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(gynObsHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGynObsHistoryByEncounterId", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynObsHistory.</param>
        /// <param name="gynObsHistory">GynObsHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGynObsHistory)]
        public async Task<IActionResult> UpdateGynObsHistory(Guid key, GynObsHistory gynObsHistory)
        {
            try
            {
                if (key != gynObsHistory.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = gynObsHistory.ModifiedBy;
                interactionInDb.ModifiedIn = gynObsHistory.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                await context.SaveChangesAsync();

                var gynObsHistoryInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByKey(key);


                if(gynObsHistoryInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                gynObsHistoryInDb.DateModified = DateTime.Now;
                gynObsHistoryInDb.IsDeleted = false;
                gynObsHistoryInDb.IsSynced = false;
                gynObsHistoryInDb.EncounterType = gynObsHistory.EncounterType;
                gynObsHistoryInDb.GestationalAge = gynObsHistory.GestationalAge;
                gynObsHistoryInDb.HasCounselled = gynObsHistory.HasCounselled;
                gynObsHistoryInDb.IsCaCxScreened = gynObsHistory.IsCaCxScreened;
                gynObsHistoryInDb.IsClientNeedFP = gynObsHistory.IsClientNeedFP;
                gynObsHistoryInDb.IsClientNeedFP = gynObsHistory.IsClientNeedFP;
                gynObsHistoryInDb.IsDeleted = gynObsHistory.IsDeleted;
                gynObsHistoryInDb.IsPregnant = gynObsHistory.IsPregnant;
                gynObsHistoryInDb.IsSynced = gynObsHistory.IsSynced;
                gynObsHistoryInDb.LNMP = gynObsHistory.LNMP;
                gynObsHistoryInDb.MenstrualHistory = gynObsHistory.MenstrualHistory;
                gynObsHistoryInDb.ModifiedBy = gynObsHistory.ModifiedBy;
                gynObsHistoryInDb.ModifiedIn = gynObsHistory.ModifiedIn;
                gynObsHistoryInDb.ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote;
                gynObsHistoryInDb.PreviousSexualHistory = gynObsHistory.PreviousSexualHistory;
                gynObsHistoryInDb.PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant;
                gynObsHistoryInDb.TotalNumberOfPregnancy =gynObsHistory.TotalNumberOfPregnancy;
                gynObsHistoryInDb.TotalBirthGiven = gynObsHistory.TotalBirthGiven;
                gynObsHistoryInDb.AliveChildren = gynObsHistory.AliveChildren;
                gynObsHistoryInDb.CesareanHistory = gynObsHistory.CesareanHistory;
                gynObsHistoryInDb.RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth;
                gynObsHistoryInDb.DateOfDelivery = gynObsHistory.DateOfDelivery;
                gynObsHistoryInDb.Postpartum = gynObsHistory.Postpartum;
                gynObsHistoryInDb.LastDeliveryTime = gynObsHistory.LastDeliveryTime;
                gynObsHistoryInDb.MiscarriageStatus = gynObsHistory.MiscarriageStatus;
                gynObsHistoryInDb.MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks;
                gynObsHistoryInDb.PostAbortionSepsis = gynObsHistory.PostAbortionSepsis;
                gynObsHistoryInDb.AgeAtMenarche = gynObsHistory.AgeAtMenarche;
                gynObsHistoryInDb.MenstrualBloodFlow = gynObsHistory.MenstrualBloodFlow;
                gynObsHistoryInDb.MenstrualCycleRegularity = gynObsHistory.MenstrualCycleRegularity;
                gynObsHistoryInDb.IsMensAssociatedWithPain = gynObsHistory.IsMensAssociatedWithPain;
                gynObsHistoryInDb.FirstSexualIntercourseAge = gynObsHistory.FirstSexualIntercourseAge;
                gynObsHistoryInDb.NumberOfSexualPartners = gynObsHistory.NumberOfSexualPartners;
                gynObsHistoryInDb.FirstPregnancyAge = gynObsHistory.FirstPregnancyAge;
                gynObsHistoryInDb.IsAnythingUsedToCleanVagina = gynObsHistory.IsAnythingUsedToCleanVagina;
                gynObsHistoryInDb.ItemUsedToCleanVagina =gynObsHistory.ItemUsedToCleanVagina;
                gynObsHistoryInDb.IsBleedingDuringOrAfterCoitus = gynObsHistory.IsBleedingDuringOrAfterCoitus;
                gynObsHistoryInDb.HasFever = gynObsHistory.HasFever;
                gynObsHistoryInDb.HasLowerAbdominalPain = gynObsHistory.HasLowerAbdominalPain;
                gynObsHistoryInDb.HasAbnormalVaginalDischarge = gynObsHistory.HasAbnormalVaginalDischarge;
                gynObsHistoryInDb.OtherConcern = gynObsHistory.OtherConcern;
                gynObsHistoryInDb.Examination   =   gynObsHistory.Examination;
                gynObsHistoryInDb.IntercourseStatusId = gynObsHistory.IntercourseStatusId;
                gynObsHistoryInDb.CaCxScreeningMethod = gynObsHistory.CaCxScreeningMethod;
                gynObsHistoryInDb.ClientId = gynObsHistory.ClientId;
                gynObsHistoryInDb.EncounterId = gynObsHistory.EncounterId;
                gynObsHistoryInDb.CreatedIn = gynObsHistory.CreatedIn;
                gynObsHistoryInDb.DateCreated   = gynObsHistory.DateCreated;
                gynObsHistoryInDb.CreatedBy = gynObsHistory.CreatedBy;
               
                context.GynObsHistoryRepository.Update(gynObsHistoryInDb);
                await context.SaveChangesAsync();

                if (gynObsHistory.ContraceptiveHistoryList != null)
                {
                    var contraceptiveHistoryListInDb = await context.ContraceptiveHistoryRepository.GetContraceptiveHistoryByGynObsHistoryId(key);

                    if(contraceptiveHistoryListInDb != null)
                    {
                        foreach(var item in contraceptiveHistoryListInDb)
                        {
                            context.ContraceptiveHistoryRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }
                    }

                    foreach (var item in gynObsHistory.ContraceptiveHistoryList)
                    {
                        ContraceptiveHistory contraceptiveHistory = new ContraceptiveHistory();

                        contraceptiveHistory.InteractionId = Guid.NewGuid();
                        contraceptiveHistory.GynObsHistoryId = key;
                        contraceptiveHistory.DateCreated = DateTime.Now;
                        contraceptiveHistory.ContraceptiveId = item;
                        contraceptiveHistory.EncounterId = gynObsHistory.EncounterId;
                        contraceptiveHistory.IsDeleted = false;
                        contraceptiveHistory.IsSynced = false;

                        context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                        await context.SaveChangesAsync();
                    }
                }

                if (gynObsHistory.ContraceptiveHistoryList != null)
                {
                    var contraceptiveHistoryListInDb = await context.ContraceptiveHistoryRepository.GetContraceptiveHistoryByGynObsHistoryId(key);

                    if (contraceptiveHistoryListInDb != null)
                    {
                        foreach (var item in contraceptiveHistoryListInDb)
                        {
                            context.ContraceptiveHistoryRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }
                    }

                    foreach (var item in gynObsHistory.ContraceptiveHistoryList)
                    {
                        ContraceptiveHistory contraceptiveHistory = new ContraceptiveHistory();

                        contraceptiveHistory.InteractionId = Guid.NewGuid();
                        contraceptiveHistory.GynObsHistoryId = key;
                        contraceptiveHistory.DateCreated = DateTime.Now;
                        contraceptiveHistory.ContraceptiveId = item;
                        contraceptiveHistory.EncounterId = gynObsHistory.EncounterId;
                        contraceptiveHistory.IsDeleted = false;
                        contraceptiveHistory.IsSynced = false;

                        context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                        await context.SaveChangesAsync();
                    }
                }

                if (gynObsHistory.ContraceptiveHistory != null)
                {
                    var contraceptiveHistoryListInDb = await context.ContraceptiveHistoryRepository.GetContraceptiveHistoryByGynObsHistoryId(key);

                    if (contraceptiveHistoryListInDb != null)
                    {
                        foreach (var item in contraceptiveHistoryListInDb)
                        {
                            context.ContraceptiveHistoryRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }
                    }

                    foreach (var item in gynObsHistory.ContraceptiveHistory)
                    {
                        ContraceptiveHistory contraceptiveHistory = new ContraceptiveHistory();

                        contraceptiveHistory.InteractionId = Guid.NewGuid();
                        contraceptiveHistory.GynObsHistoryId = key;
                        contraceptiveHistory.DateCreated = DateTime.Now;
                        contraceptiveHistory.ContraceptiveId = item.ContraceptiveId;
                        contraceptiveHistory.UsedFor=item.UsedFor;
                        contraceptiveHistory.EncounterId = gynObsHistory.EncounterId;
                        contraceptiveHistory.IsDeleted = false;
                        contraceptiveHistory.IsSynced = false;

                        context.ContraceptiveHistoryRepository.Add(contraceptiveHistory);
                        await context.SaveChangesAsync();
                    }
                }
                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGynObsHistory", "GynObsHistoryController.cs", ex.Message, gynObsHistory.ModifiedIn, gynObsHistory.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GynObsHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGynObsHistory)]
        public async Task<IActionResult> DeleteGynObsHistory(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynObsHistoriesInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByKey(key);

                if (gynObsHistoriesInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                gynObsHistoriesInDb.IsDeleted = true;
                context.GynObsHistoryRepository.Update(gynObsHistoriesInDb);
                await context.SaveChangesAsync();

                return Ok(gynObsHistoriesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGynObsHistory", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/gyn-obs-history/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveGynObsHistory)]
        public async Task<IActionResult> RemoveGynObsHistory(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var gynObsHistoryInDb = await context.GynObsHistoryRepository.GetGynObsHistoryByEncounterId(encounterId);

                if (gynObsHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in gynObsHistoryInDb.ToList())
                {
                    context.GynObsHistoryRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(gynObsHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveGynObsHistory", "GynObsHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}