using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// MedicalHistory controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicalHistoryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicalHistoryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicalHistoryController(IUnitOfWork context, ILogger<MedicalHistoryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region MedicalHistory
        /// <summary>
        /// URL: sc-api/medical-history
        /// </summary>
        /// <param name="medicalHistoryList">MedicalHistory object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicalHistory)]
        public async Task<IActionResult> CreateMedicalHistory(List<MedicalHistory> medicalHistoryList)
        {
            try
            {
                MedicalHistory medicalHistory = new MedicalHistory();

                foreach (var item in medicalHistoryList)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MedicalHistory, item.EncounterType);
                    interaction.EncounterId = item.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = item.CreatedBy;
                    interaction.CreatedIn = item.CreatedIn;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;

                    context.InteractionRepository.Add(interaction);

                    item.InteractionId = interactionId;
                    item.ClientId = item.ClientId;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.MedicalHistoryRepository.Add(item);
                    await context.SaveChangesAsync();

                    medicalHistory = item;
                }
                return CreatedAtAction("ReadMedicalHistoryByKey", new { key = medicalHistory.InteractionId }, medicalHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateMedicalHistory", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.CreateMedicalHistoryForFamilyFoodHistory)]
        public async Task<IActionResult> CreateMedicalHistoryForFamilyFoodHistory(List<MedicalHistory> medicalHistoryList)
        {
            try
            {
                if (!medicalHistoryList.Where(x => x.InformationType == InformationType.FamilyMedicalHistory).Any())
                    return StatusCode(StatusCodes.Status400BadRequest, "Sibbling History is required");

                if (!medicalHistoryList.Where(x => x.InformationType == InformationType.AlcoholandSmokingHabits).Any())
                    return StatusCode(StatusCodes.Status400BadRequest, "Alcohol and SmokingHabits");

                medicalHistoryList = medicalHistoryList.Where(x => x.InformationType == InformationType.FamilyMedicalHistory || x.InformationType == InformationType.AlcoholandSmokingHabits
                || x.InformationType == InformationType.SiblingHistory).ToList();

                MedicalHistory medicalHistory = new MedicalHistory();

                foreach (var item in medicalHistoryList)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MedicalHistory, item.EncounterType);
                    interaction.EncounterId = item.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = item.CreatedBy;
                    interaction.CreatedIn = item.CreatedIn;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;

                    context.InteractionRepository.Add(interaction);

                    item.InteractionId = interactionId;
                    item.ClientId = item.ClientId;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.MedicalHistoryRepository.Add(item);
                    await context.SaveChangesAsync();

                    medicalHistory = item;
                }
                return CreatedAtAction("ReadMedicalHistoryByKey", new { key = medicalHistory.InteractionId }, medicalHistory);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateMedicalHistoryForFamilyFoodHistory", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-histories
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistories)]
        public async Task<IActionResult> ReadMedicalHistories()
        {
            try
            {
                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistories();

                return Ok(medicalHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalHistories", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-histories-byclient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistoriesByClientId)]
        public async Task<IActionResult> ReadMedicalHistoryClient(Guid clientId)
        {
            try
            {

                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoriesByClient(clientId);

                foreach (var item in medicalHistoryInDb)
                {
                    item.PEPRisk = await context.PEPRiskStatusRepository.LoadListWithChildAsync<RiskStatus>(p => p.IsDeleted == false && p.ClientId == item.ClientId && p.EncounterId == p.EncounterId, o => o.Risk);
                }

                medicalHistoryInDb = medicalHistoryInDb.OrderByDescending(x => x.DateCreated);
                return Ok(medicalHistoryInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalHistoryClient", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadPastMedicalHistoryByClient)]
        public async Task<IActionResult> ReadPastMedicalHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {

                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetPastMedicalHistoriesByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                foreach (var item in medicalHistoryInDb)
                {
                    item.PEPRisk = await context.PEPRiskStatusRepository.LoadListWithChildAsync<RiskStatus>(p => p.IsDeleted == false && p.ClientId == item.ClientId && p.EncounterId == p.EncounterId, o => o.Risk);
                }

                PagedResultDto<MedicalHistory> medicalHistoryWIthPaggingDto = new PagedResultDto<MedicalHistory>()
                {
                    Data = medicalHistoryInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = context.MedicalHistoryRepository.GetPastMedicalHistoriesByClientTotalCount(clientId, encounterType)
                };

                return Ok(medicalHistoryWIthPaggingDto);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastMedicalHistoryByClient", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadFamilyFoodHistoryByClient)]
        public async Task<IActionResult> ReadFamilyFoodHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {

                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetFamilyFoodHistoryByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                foreach (var item in medicalHistoryInDb)
                {
                    item.PEPRisk = await context.PEPRiskStatusRepository.LoadListWithChildAsync<RiskStatus>(p => p.IsDeleted == false && p.ClientId == item.ClientId && p.EncounterId == p.EncounterId, o => o.Risk);
                }

                PagedResultDto<MedicalHistory> medicalHistoryWIthPaggingDto = new PagedResultDto<MedicalHistory>()
                {
                    Data = medicalHistoryInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = context.MedicalHistoryRepository.GetFamilyFoodHistoryByClientTotalCount(clientId, encounterType)
                };

                return Ok(medicalHistoryWIthPaggingDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyFoodHistoryByClient", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistoriesByClientIdForFamilyFoodHistories)]
        public async Task<IActionResult> ReadMedicalHistoryClientForFamilyFoodHistories(Guid clientId)
        {
            try
            {
                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoriesByClient(clientId);

                foreach (var item in medicalHistoryInDb)
                {
                    item.PEPRisk = await context.PEPRiskStatusRepository.LoadListWithChildAsync<RiskStatus>(p => p.IsDeleted == false && p.ClientId == item.ClientId && p.EncounterId == p.EncounterId, o => o.Risk);
                }

                return Ok(medicalHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalHistoryClientForFamilyFoodHistories", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/medical-histories-by-visit/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistoriesByEncounterId)]
        public async Task<IActionResult> ReadMedicalHistoriesByEncounterId(Guid encounterId)
        {
            try
            {
                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoriesByVisitID(encounterId);

                return Ok(medicalHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalHistoriesByEncounterId", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-history/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalHistory.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistoryByKey)]
        public async Task<IActionResult> ReadMedicalHistoryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoryByKey(key);

                if (medicalHistoryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(medicalHistoryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalHistoryByKey", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-history/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalHistory.</param>
        /// <param name="medicalHistory">MedicalHistory to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMedicalHistory)]
        public async Task<IActionResult> UpdateMedicalHistory(Guid key, List<MedicalHistory> medicalHistory)
        {
            try
            {
                if (key != medicalHistory.FirstOrDefault()?.EncounterId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                if (medicalHistory.Any())
                {
                    var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoriesByVisitID(key);

                    foreach (var history in medicalHistory)
                    {
                        var historiesToDelete = medicalHistoryInDb.Where(x => x.EncounterId == history.EncounterId && x.InformationType == history.InformationType && x.EncounterType == history.EncounterType).ToList();

                    
                            foreach (var item in historiesToDelete)
                            {
                                context.MedicalHistoryRepository.Delete(item);
                                await context.SaveChangesAsync();
                            }
                        

                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MedicalHistory, history.EncounterType);
                        interaction.EncounterId = history.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = history.CreatedBy;
                        interaction.CreatedIn = history.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        history.InteractionId = interactionId;
                        history.ClientId = history.ClientId;
                        history.DateCreated = DateTime.Now;
                        history.IsDeleted = false;
                        history.IsSynced = false;
                        context.MedicalHistoryRepository.Add(history);
                    }
                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicalHistory", "MedicalHistoryController.cs", ex.Message, medicalHistory.FirstOrDefault()?.ModifiedIn, medicalHistory.FirstOrDefault()?.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-history/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMedicalHistory)]
        public async Task<IActionResult> DeleteMedicalHistory(string key, EncounterType encounterType)
        {
            Guid opdVisitId = new Guid(key);
            var type = encounterType;
            try
            {
                if (opdVisitId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalHistoryInDb = await context.MedicalHistoryRepository.GetMedicalHistoriesByVisitID(opdVisitId);

                var medicalHistoryByType = medicalHistoryInDb.Where(t => t.EncounterType == type);

                if (medicalHistoryByType == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in medicalHistoryByType)
                {
                    if ((item.InformationType == Enums.InformationType.AdmissionHistory || item.InformationType == Enums.InformationType.SurgicalHistory || item.InformationType == Enums.InformationType.PastDrugHistory)
                        && item.EncounterType == type)
                    {
                        item.DateModified = DateTime.Now;
                        item.IsDeleted = true;
                        item.IsSynced = false;

                        context.MedicalHistoryRepository.Delete(item);
                        await context.SaveChangesAsync();
                    }

                    if ((item.InformationType == Enums.InformationType.FamilyMedicalHistory || item.InformationType == Enums.InformationType.SiblingHistory || item.InformationType == Enums.InformationType.AlcoholandSmokingHabits)
                        && item.EncounterType == type)
                    {
                        item.DateModified = DateTime.Now;
                        item.IsDeleted = true;
                        item.IsSynced = false;

                        context.MedicalHistoryRepository.Delete(item);
                        await context.SaveChangesAsync();
                    }

                }

                return Ok(medicalHistoryByType);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMedicalHistory", "MedicalHistoryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}