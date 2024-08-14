using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Lion
 * Last modified : 21.08.2023
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

    public class ChiefComplaintController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ChiefComplaintController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ChiefComplaintController(IUnitOfWork context, ILogger<ChiefComplaintController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/chief-complaint
        /// </summary>
        /// <param name="chiefComplaint">ChiefComplaint object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateChiefComplaint)]
        public async Task<IActionResult> CreateChiefComplaint(ChiefComplaintDto chiefComplaintDto)
        {
            try
            {

                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ChiefComplaint, chiefComplaintDto.EncounterType);
                interaction.EncounterId = chiefComplaintDto.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedIn = chiefComplaintDto.CreatedIn;
                interaction.CreatedBy = chiefComplaintDto.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                ChiefComplaint chiefComplaint = new ChiefComplaint();
                chiefComplaint.InteractionId = interactionId;
                chiefComplaint.ClientId = chiefComplaintDto.ClientId;
                chiefComplaint.DateCreated = DateTime.Now;
                chiefComplaint.IsDeleted = false;
                chiefComplaint.IsSynced = false;
                chiefComplaint.CreatedBy = chiefComplaintDto.CreatedBy;
                chiefComplaint.CreatedIn = chiefComplaintDto.CreatedIn;
                chiefComplaint.EncounterId = chiefComplaintDto.EncounterId;
                chiefComplaint.EncounterType = chiefComplaintDto.EncounterType;
                chiefComplaint.HIVStatus = chiefComplaintDto.HIVStatus;
                chiefComplaint.HistoryOfChiefComplaint = chiefComplaintDto.HistoryOfChiefComplaint;
                chiefComplaint.ChiefComplaints = chiefComplaintDto.ChiefComplaints;
                chiefComplaint.IsChildGivenARV = chiefComplaintDto.IsChildGivenARV;
                chiefComplaint.IsMotherGivenARV = chiefComplaintDto.IsMotherGivenARV;
                chiefComplaint.LastHIVTestDate = chiefComplaintDto.LastHIVTestDate;
                chiefComplaint.NATResult = chiefComplaintDto.NATResult;
                chiefComplaint.NATTestDate = chiefComplaintDto.NATTestDate;
                chiefComplaint.ExaminationSummary = chiefComplaintDto.ExaminationSummary;
                chiefComplaint.HistorySummary = chiefComplaintDto.HistorySummary;
                chiefComplaint.PotentialHIVExposureDate = chiefComplaintDto.PotentialHIVExposureDate;
                chiefComplaint.RecencyType = chiefComplaintDto.RecencyType;
                chiefComplaint.RecencyTestDate = chiefComplaintDto.RecencyTestDate;
                chiefComplaint.ChildExposureStatus = chiefComplaintDto.ChildExposureStatus;
                chiefComplaint.TBScreenings = chiefComplaintDto.TBScreenings;

                if (chiefComplaintDto.EncounterType == EncounterType.MedicalEncounterIPD)
                {
                    chiefComplaint.ChiefComplaints = "-";
                    chiefComplaint.HistoryOfChiefComplaint = "-";
                    chiefComplaint.HIVStatus = Enums.HIVTestResult.Negative;
                }

                if (chiefComplaintDto.EncounterType == EncounterType.PEP)
                {
                    var htsInDb = await context.HTSRepository.GetLatestHTSByClient(chiefComplaintDto.ClientId);

                    if (htsInDb != null)
                    {
                        htsInDb.RiskAssessments = await context.RiskAssessmentRepository.LoadListWithChildAsync<RiskAssessment>(x => x.HTSId == htsInDb.InteractionId, p => p.HIVRiskFactor);

                        if (htsInDb.LastTested == null)
                        {
                            chiefComplaint.LastHIVTestDate = htsInDb.DateCreated.Value;
                            chiefComplaint.HIVStatus = Enums.HIVTestResult.Negative;
                        }
                        else
                        {
                            chiefComplaint.LastHIVTestDate = htsInDb.LastTested.Value;
                            chiefComplaintDto.HIVStatus = (HIVTestResult)Enum.Parse(typeof(HIVTestResult), htsInDb.LastTestResult.Value.ToString());
                        }

                        chiefComplaintDto.TestingLocation = htsInDb.ServicePoint.Description;
                    }
                }

                if (chiefComplaintDto.EncounterType == EncounterType.PrEP)
                {
                    var htsInDb = await context.HTSRepository.GetLatestHTSByClient(chiefComplaintDto.ClientId);

                    if (htsInDb != null)
                    {
                        htsInDb.RiskAssessments = await context.RiskAssessmentRepository.LoadListWithChildAsync<RiskAssessment>(x => x.HTSId == htsInDb.InteractionId, p => p.HIVRiskFactor);

                        if (htsInDb.LastTested == null)
                        {
                            chiefComplaint.LastHIVTestDate = htsInDb.DateCreated.Value;
                            chiefComplaint.HIVStatus = Enums.HIVTestResult.Negative;
                            chiefComplaint.TestingLocation = htsInDb.ServicePoint.Description;
                        }
                        else
                        {
                            chiefComplaint.LastHIVTestDate = htsInDb.LastTested.Value;
                            chiefComplaint.HIVStatus = (HIVTestResult)Enum.Parse(typeof(HIVTestResult), htsInDb.LastTestResult.Value.ToString());
                            chiefComplaint.TestingLocation = htsInDb.ServicePoint.Description;
                        }

                        chiefComplaint.HIVStatus = Enums.HIVTestResult.Negative;
                    }
                }

                if (chiefComplaintDto.EncounterType == EncounterType.ARTIHPAI || chiefComplaintDto.EncounterType == EncounterType.ARTFollowUp || chiefComplaintDto.EncounterType == EncounterType.ARTStableOnCare
                        || chiefComplaintDto.EncounterType == EncounterType.PediatricIHPAI || chiefComplaintDto.EncounterType == EncounterType.PediatricFollowUp || chiefComplaintDto.EncounterType == EncounterType.PediatricStableOnCare)
                {
                    chiefComplaint.HIVStatus = HIVTestResult.Positive;
                    chiefComplaint.RecencyType = chiefComplaintDto.RecencyType;
                    chiefComplaint.RecencyTestDate = chiefComplaintDto.RecencyTestDate;
                }

                context.ChiefComplaintRepository.Add(chiefComplaint);

                await context.SaveChangesAsync();

                if (chiefComplaintDto.EncounterType == EncounterType.PEP)
                {
                    if (chiefComplaintDto.ExposureList != null)
                    {
                        foreach (var item in chiefComplaintDto.ExposureList)
                        {
                            Exposure Exposure = new Exposure();

                            Exposure.Oid = Guid.NewGuid();
                            Exposure.ExposureTypeId = item;
                            Exposure.ChiefComplaintId = chiefComplaint.InteractionId;
                            Exposure.CreatedBy = chiefComplaint.CreatedBy;
                            Exposure.CreatedIn = chiefComplaint.CreatedIn;
                            Exposure.DateCreated = DateTime.Now;
                            Exposure.IsDeleted = false;
                            Exposure.IsSynced = false;

                            context.ExposureRepository.Add(Exposure);
                        }
                        await context.SaveChangesAsync();
                    }
                }

                if (chiefComplaintDto.KeyPopulations != null)
                {
                    var keyPop = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByEncounter(chiefComplaintDto.EncounterId);
                    keyPop = keyPop.Where(x => x.EncounterType == chiefComplaintDto.EncounterType).ToList();

                    foreach (var item in keyPop)
                    {
                        context.KeyPopulationDemographicRepository.Delete(item);
                    }

                    foreach (var item in chiefComplaintDto.KeyPopulations)
                    {
                        interaction = new Interaction();
                        interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.KeyPopulationDemographic, chiefComplaintDto.EncounterType);
                        interaction.EncounterId = chiefComplaintDto.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = chiefComplaintDto.CreatedBy;
                        interaction.CreatedIn = chiefComplaintDto.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        KeyPopulationDemographic keyPopulationDemographic = new KeyPopulationDemographic();

                        keyPopulationDemographic.InteractionId = interactionId;
                        keyPopulationDemographic.EncounterId = chiefComplaintDto.EncounterId;
                        keyPopulationDemographic.ClientId = chiefComplaintDto.ClientId;
                        keyPopulationDemographic.KeyPopulationId = item;
                        keyPopulationDemographic.CreatedIn = chiefComplaintDto.CreatedIn;
                        keyPopulationDemographic.CreatedBy = chiefComplaintDto.CreatedBy;
                        keyPopulationDemographic.DateCreated = DateTime.Now;
                        keyPopulationDemographic.IsSynced = false;
                        keyPopulationDemographic.IsDeleted = false;
                        keyPopulationDemographic.EncounterType = chiefComplaintDto.EncounterType;

                        context.KeyPopulationDemographicRepository.Add(keyPopulationDemographic);
                    }
                    await context.SaveChangesAsync();
                }

                if (chiefComplaintDto.QuestionsList != null)
                {
                    var hivr = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByEncounter(chiefComplaintDto.EncounterId);

                    hivr = hivr.Where(x => x.EncounterType == chiefComplaintDto.EncounterType).ToList();

                    foreach (var question in hivr)
                    {
                        context.HIVRiskScreeningRepository.Delete(question);
                    }

                    foreach (var item in chiefComplaintDto.QuestionsList)
                    {
                        interaction = new Interaction();
                        interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.HIVRiskScreening, chiefComplaintDto.EncounterType);
                        interaction.EncounterId = chiefComplaintDto.EncounterId;
                        interaction.CreatedBy = chiefComplaintDto.CreatedBy;
                        interaction.CreatedIn = chiefComplaintDto.CreatedIn;
                        interaction.DateCreated = DateTime.Now;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        HIVRiskScreening hIVRiskScreening = new HIVRiskScreening();

                        hIVRiskScreening.InteractionId = interactionId;
                        hIVRiskScreening.ClientId = chiefComplaintDto.ClientId;
                        hIVRiskScreening.EncounterId = chiefComplaintDto.EncounterId;
                        hIVRiskScreening.QuestionId = item.QuestionId;
                        hIVRiskScreening.Answer = item.Answer;
                        hIVRiskScreening.DateCreated = DateTime.Now;
                        hIVRiskScreening.IsDeleted = false;
                        hIVRiskScreening.IsSynced = false;
                        hIVRiskScreening.EncounterType = chiefComplaintDto.EncounterType;

                        context.HIVRiskScreeningRepository.Add(hIVRiskScreening);

                    }

                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadChiefComplaintByKey", new { key = chiefComplaint.InteractionId }, chiefComplaint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateChiefComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaintDto.CreatedIn, chiefComplaintDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint
        /// </summary>
        /// <param name="chiefComplaint">ChiefComplaint object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEPChiefComplaint)]
        public async Task<IActionResult> CreatePrepComplaint(ChiefComplaintDto chiefComplaintDto)
        {
            try
            {
                Interaction interaction = new Interaction();

                if (chiefComplaintDto.KeyPopulations != null)
                {
                    foreach (var item in chiefComplaintDto.KeyPopulations)
                    {
                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.KeyPopulationDemographic, chiefComplaintDto.EncounterType);
                        interaction.EncounterId = chiefComplaintDto.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = chiefComplaintDto.CreatedBy;
                        interaction.CreatedIn = chiefComplaintDto.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        KeyPopulationDemographic keyPopulationDemographic = new KeyPopulationDemographic();

                        keyPopulationDemographic.InteractionId = interactionId;
                        keyPopulationDemographic.EncounterId = chiefComplaintDto.EncounterId;
                        keyPopulationDemographic.ClientId = chiefComplaintDto.ClientId;
                        keyPopulationDemographic.KeyPopulationId = item;
                        keyPopulationDemographic.IsSynced = false;
                        keyPopulationDemographic.IsDeleted = false;
                        keyPopulationDemographic.EncounterType = chiefComplaintDto.EncounterType;

                        context.KeyPopulationDemographicRepository.Add(keyPopulationDemographic);
                    }
                }

                if (chiefComplaintDto.QuestionsList != null)
                {
                    foreach (var item in chiefComplaintDto.QuestionsList)
                    {
                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.HIVRiskScreening, chiefComplaintDto.EncounterType);
                        interaction.EncounterId = chiefComplaintDto.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = chiefComplaintDto.CreatedBy;
                        interaction.CreatedIn = chiefComplaintDto.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        HIVRiskScreening hIVRiskScreening = new HIVRiskScreening();

                        hIVRiskScreening.InteractionId = interactionId;
                        hIVRiskScreening.ClientId = chiefComplaintDto.ClientId;
                        hIVRiskScreening.EncounterId = chiefComplaintDto.EncounterId;
                        hIVRiskScreening.QuestionId = item.QuestionId;
                        hIVRiskScreening.Answer = item.Answer;
                        hIVRiskScreening.DateCreated = DateTime.Now;
                        hIVRiskScreening.IsDeleted = false;
                        hIVRiskScreening.IsSynced = false;
                        hIVRiskScreening.EncounterType = chiefComplaintDto.EncounterType;

                        context.HIVRiskScreeningRepository.Add(hIVRiskScreening);

                    }
                }

                ChiefComplaint chiefComplaint = new ChiefComplaint();

                chiefComplaint.ChiefComplaints = chiefComplaintDto.ChiefComplaints;
                chiefComplaint.HistoryOfChiefComplaint = chiefComplaintDto.HistoryOfChiefComplaint;
                chiefComplaint.EncounterId = chiefComplaintDto.EncounterId;
                chiefComplaint.InteractionId = interaction.Oid;
                chiefComplaint.ClientId = chiefComplaintDto.ClientId;
                chiefComplaint.EncounterType = chiefComplaintDto.EncounterType;
                chiefComplaint.DateCreated = DateTime.Now;
                chiefComplaint.IsDeleted = false;
                chiefComplaint.IsSynced = false;
                chiefComplaint.LastHIVTestDate = chiefComplaintDto.LastHIVTestDate;
                chiefComplaint.TestingLocation = chiefComplaintDto.TestingLocation;
                chiefComplaint.HIVStatus = chiefComplaintDto.HIVStatus;

                context.ChiefComplaintRepository.Add(chiefComplaint);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadChiefComplaintByKey", new { key = chiefComplaint.InteractionId }, chiefComplaint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrepComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaintDto.CreatedIn, chiefComplaintDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-chief-complaint
        /// </summary>
        /// <param name="chiefComplaint">ChiefComplaint object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPChiefComplaint)]
        public async Task<IActionResult> CreatePEPChiefComplaint(ChiefComplaint chiefComplaint)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ChiefComplaint, chiefComplaint.EncounterType);
                interaction.EncounterId = chiefComplaint.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = chiefComplaint.CreatedBy;
                interaction.CreatedIn = chiefComplaint.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                chiefComplaint.InteractionId = interactionId;
                chiefComplaint.ClientId = chiefComplaint.ClientId;
                chiefComplaint.DateCreated = DateTime.Now;
                chiefComplaint.IsDeleted = false;
                chiefComplaint.IsSynced = false;

                context.ChiefComplaintRepository.Add(chiefComplaint);
                await context.SaveChangesAsync();

                if (chiefComplaint.ExposureList != null)
                {
                    foreach (var item in chiefComplaint.ExposureList)
                    {
                        Exposure Exposure = new Exposure();
                        Exposure.Oid = Guid.NewGuid();
                        Exposure.ExposureTypeId = item;
                        Exposure.ChiefComplaintId = chiefComplaint.InteractionId;
                        Exposure.CreatedBy = chiefComplaint.CreatedBy;
                        Exposure.CreatedIn = chiefComplaint.CreatedIn;
                        Exposure.DateCreated = DateTime.Now;
                        Exposure.IsDeleted = false;
                        Exposure.IsSynced = false;

                        context.ExposureRepository.Add(Exposure);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadPEPChiefComplaintByKey", new { key = chiefComplaint.InteractionId }, chiefComplaint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPChiefComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaint.CreatedIn, chiefComplaint.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-chief-complaint
        /// </summary>
        /// <param name="ipdChiefComplaintDto">ChiefComplaint object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIPDChiefComplaint)]
        public async Task<IActionResult> CreateIPDChiefComplaint(ChiefComplaintDto ipdChiefComplaintDto)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ChiefComplaint, ipdChiefComplaintDto.EncounterType);
                interaction.EncounterId = ipdChiefComplaintDto.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = ipdChiefComplaintDto.CreatedBy;
                interaction.CreatedIn = ipdChiefComplaintDto.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                ChiefComplaint chiefComplaint = new ChiefComplaint();

                chiefComplaint.InteractionId = interactionId;
                chiefComplaint.ClientId = ipdChiefComplaintDto.ClientId;
                chiefComplaint.HistorySummary = ipdChiefComplaintDto.HistorySummary;
                chiefComplaint.ExaminationSummary = ipdChiefComplaintDto.ExaminationSummary;
                chiefComplaint.ChiefComplaints = "-";
                chiefComplaint.HistoryOfChiefComplaint = "-";
                chiefComplaint.HIVStatus = Enums.HIVTestResult.Negative;
                chiefComplaint.EncounterId = ipdChiefComplaintDto.EncounterId;
                chiefComplaint.EncounterType = ipdChiefComplaintDto.EncounterType;
                chiefComplaint.CreatedBy = ipdChiefComplaintDto.CreatedBy;
                chiefComplaint.CreatedIn = ipdChiefComplaintDto.CreatedIn;
                chiefComplaint.DateCreated = DateTime.Now;
                chiefComplaint.IsDeleted = false;
                chiefComplaint.IsSynced = false;

                context.ChiefComplaintRepository.Add(chiefComplaint);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadChiefComplaintByKey", new { key = chiefComplaint.InteractionId }, chiefComplaint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIPDChiefComplaint", "ChiefComplaintController.cs", ex.Message, ipdChiefComplaintDto.CreatedIn, ipdChiefComplaintDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaints
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChiefComplaints)]
        public async Task<IActionResult> ReadChiefComplaints()
        {
            try
            {
                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaints();
                chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);
                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChiefComplaints", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-chief-complaints
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPChiefComplaints)]
        public async Task<IActionResult> ReadPEPChiefComplaints()
        {
            try
            {
                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaints();
                chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);
                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPChiefComplaints", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint/by-Client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadChiefComplaintByClient)]
        public async Task<IActionResult> ReadChiefComplaintsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    if (encounterType == null)
                    {
                        var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintsByClient(clientId);

                        chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);

                        foreach (var item in chiefComplaintInDb)
                        {
                            item.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(o => o.IsDeleted == false && o.ChiefComplaintId == item.InteractionId, p => p.ExposureType);
                        }

                        var keyPopulationDemographic = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByClient(clientId);
                        var hIVRiskScreening = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByClient(clientId);

                        foreach (var item in chiefComplaintInDb)
                        {
                            item.keyPopulationDemographics = keyPopulationDemographic.Where(x => x.EncounterId == item.EncounterId && x.EncounterType == encounterType).ToList();
                            item.hIVRiskScreenings = hIVRiskScreening.Where(x => x.EncounterId == item.EncounterId && x.EncounterType == encounterType).ToList();
                        }
                        return Ok(chiefComplaintInDb);
                    }
                    else
                    {
                        var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintsByClient(clientId, encounterType.Value);

                        chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);

                        return Ok(chiefComplaintInDb);

                    }
                }
                else
                {
                    var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintsByClientPagging(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    var chiefComplaintInDbCount = context.ChiefComplaintRepository.GetChiefComplaintsByClientPaggingTotalCount(clientId, encounterType);

                    foreach (var item in chiefComplaintInDb)
                    {
                        item.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(o => o.IsDeleted == false && o.ChiefComplaintId == item.InteractionId, p => p.ExposureType);
                    }
                    //if (encounterType == EncounterType.PrEP || encounterType == EncounterType.ANCService || encounterType == EncounterType.ANCFollowUp)
                    //{
                    var keyPopulationDemographic = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByClient(clientId);

                    var hIVRiskScreening = await context.HIVRiskScreeningRepository.GetHIVRiskScreeningByClient(clientId);

                    foreach (var item in chiefComplaintInDb)
                    {
                        item.keyPopulationDemographics = keyPopulationDemographic.Where(x => x.EncounterId == item.EncounterId && x.EncounterType == encounterType).ToList();
                        item.hIVRiskScreenings = hIVRiskScreening.Where(x => x.EncounterId == item.EncounterId && x.EncounterType == encounterType).ToList();
                    }
                    //  }
                    PagedResultDto<ChiefComplaint> cheifComplaintsWithPagginDto = new PagedResultDto<ChiefComplaint>()
                    {
                        Data = chiefComplaintInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = chiefComplaintInDbCount

                    };
                    return Ok(cheifComplaintsWithPagginDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChiefComplaintsByClient", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL:sc-api/pep-chief-complaint/by-Client/{clientId}
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPChiefComplaintByClient)]
        public async Task<IActionResult> ReadPEPChiefComplaintsByClient(Guid clientId)
        {
            try
            {
                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetPEPChiefComplaintsByClientLast24Hours(clientId);

                foreach (var item in chiefComplaintInDb)
                {
                    item.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(o => o.IsDeleted == false && o.ChiefComplaintId == item.InteractionId, p => p.ExposureType);
                }

                chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);

                return Ok(chiefComplaintInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPChiefComplaintsByClient", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: prep-chief-complaint/by-Client/{clientId}
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPChiefComplaintByClient)]
        public async Task<IActionResult> ReadPrEPChiefComplaintsByClient(Guid clientID)
        {
            try
            {
                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetPrEPChiefComplaintsByClient(clientID);

                chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);

                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPChiefComplaintsByClient", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadANCServiceChiefComplaintByClient)]
        public async Task<IActionResult> ReadANCServiceChiefComplaintsByClient(Guid clientID)
        {
            try
            {
                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintsByClient(clientID);

                chiefComplaintInDb = chiefComplaintInDb.OrderByDescending(x => x.DateCreated);

                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadANCServiceChiefComplaintsByClient", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaints.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChiefComplaintByKey)]
        public async Task<IActionResult> ReadChiefComplaintByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(key);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                chiefComplaintInDb.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(x => x.ChiefComplaintId == chiefComplaintInDb.InteractionId, p => p.ExposureType);
                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChiefComplaintByKey", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadPrEPChiefComplaintByKey)]
        public async Task<IActionResult> ReadPrEPChiefComplaintByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(key);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                chiefComplaintInDb.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(x => x.ChiefComplaintId == chiefComplaintInDb.InteractionId, p => p.ExposureType);
                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPChiefComplaintByKey", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaints.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPChiefComplaintByKey)]
        public async Task<IActionResult> ReadPEPChiefComplaintByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(key);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                chiefComplaintInDb.Exposures = await context.ExposureRepository.LoadListWithChildAsync<Exposure>(x => x.ChiefComplaintId == chiefComplaintInDb.InteractionId, p => p.ExposureType);
                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPChiefComplaintByKey", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaint.</param>
        /// <param name="chiefComplaint">ChiefComplaint to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateChiefComplaint)]
        public async Task<IActionResult> UpdateChiefComplaint(Guid key, ChiefComplaint chiefComplaint)
        {
            try
            {
                if (key != chiefComplaint.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = chiefComplaint.ModifiedBy;
                interactionInDb.ModifiedIn = chiefComplaint.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(chiefComplaint.InteractionId);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (chiefComplaint.EncounterType == EncounterType.MedicalEncounterIPD)
                {
                    chiefComplaintInDb.HistorySummary = chiefComplaint.HistorySummary;
                    chiefComplaintInDb.ExaminationSummary = chiefComplaint.ExaminationSummary;

                    chiefComplaintInDb.ChiefComplaints = "-";
                    chiefComplaintInDb.HistoryOfChiefComplaint = "-";
                    chiefComplaintInDb.HIVStatus = Enums.HIVTestResult.Negative;
                    chiefComplaintInDb.EncounterId = chiefComplaint.EncounterId;

                    chiefComplaintInDb.DateModified = DateTime.Now;
                    chiefComplaintInDb.IsDeleted = false;
                    chiefComplaintInDb.IsSynced = false;

                    context.ChiefComplaintRepository.Update(chiefComplaintInDb);
                    await context.SaveChangesAsync();
                }
                else
                {
                    chiefComplaint.DateCreated = DateTime.Now;
                    chiefComplaint.IsDeleted = false;
                    chiefComplaint.IsSynced = false;

                    context.ChiefComplaintRepository.Update(chiefComplaint);
                    await context.SaveChangesAsync();

                    if (chiefComplaint.EncounterType == EncounterType.PEP)
                    {
                        var exposureResult = context.ExposureRepository.GetExposureByID(chiefComplaint.InteractionId);

                        foreach (var item in exposureResult.Result.ToList())
                        {
                            context.ExposureRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }

                        if (chiefComplaint.ExposureList != null)
                        {
                            foreach (var item in chiefComplaint.ExposureList)
                            {
                                Exposure exposure = new Exposure();
                                exposure.Oid = Guid.NewGuid();
                                exposure.ExposureTypeId = item;
                                exposure.ChiefComplaintId = chiefComplaint.InteractionId;

                                context.ExposureRepository.Add(exposure);
                                await context.SaveChangesAsync();
                            }
                        }
                    }

                    return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateChiefComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaint.ModifiedIn, chiefComplaint.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-chief-complaint/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaint.</param>
        /// <param name="chiefComplaintDto">ChiefComplaint to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIPDChiefComplaint)]
        public async Task<IActionResult> UpdateIPDChiefComplaint(Guid key, ChiefComplaint chiefComplaintDto)
        {
            try
            {
                if (key != chiefComplaintDto.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = chiefComplaintDto.ModifiedBy;
                interactionInDb.ModifiedIn = chiefComplaintDto.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(chiefComplaintDto.InteractionId);

                chiefComplaintInDb.HistorySummary = chiefComplaintDto.HistorySummary;
                chiefComplaintInDb.ExaminationSummary = chiefComplaintDto.ExaminationSummary;

                chiefComplaintInDb.ChiefComplaints = "-";
                chiefComplaintInDb.HistoryOfChiefComplaint = "-";
                chiefComplaintInDb.HIVStatus = Enums.HIVTestResult.Negative;
                chiefComplaintInDb.EncounterId = chiefComplaintDto.EncounterId;

                chiefComplaintInDb.DateModified = DateTime.Now;
                chiefComplaintInDb.IsDeleted = false;
                chiefComplaintInDb.IsSynced = false;

                context.ChiefComplaintRepository.Update(chiefComplaintInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIPDChiefComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaintDto.ModifiedIn, chiefComplaintDto.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/pep-chief-complaint/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <param name="chiefComplaint"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPChiefComplaint)]
        public async Task<IActionResult> UpdatePEPChiefComplaint(Guid key, ChiefComplaint chiefComplaint)
        {
            try
            {
                if (key != chiefComplaint.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = chiefComplaint.ModifiedBy;
                interactionInDb.ModifiedIn = chiefComplaint.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                chiefComplaint.DateCreated = DateTime.Now;
                chiefComplaint.IsDeleted = false;
                chiefComplaint.IsSynced = false;

                context.ChiefComplaintRepository.Update(chiefComplaint);
                await context.SaveChangesAsync();

                var exposureResult = context.ExposureRepository.GetExposureByID(chiefComplaint.InteractionId);

                foreach (var item in exposureResult.Result.ToList())
                {
                    context.ExposureRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (chiefComplaint.ExposureList != null)
                {
                    foreach (var item in chiefComplaint.ExposureList)
                    {
                        Exposure exposure = new Exposure();

                        exposure.Oid = Guid.NewGuid();
                        exposure.ExposureTypeId = item;
                        exposure.ChiefComplaintId = chiefComplaint.InteractionId;

                        context.ExposureRepository.Add(exposure);
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePEPChiefComplaint", "ChiefComplaintController.cs", ex.Message, chiefComplaint.ModifiedIn, chiefComplaint.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaint.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteChiefComplaint)]
        public async Task<IActionResult> DeleteChiefComplaint(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(key);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                chiefComplaintInDb.IsDeleted = true;
                context.ChiefComplaintRepository.Update(chiefComplaintInDb);
                await context.SaveChangesAsync();

                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteChiefComplaint", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-chief-complaint/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaint.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePEPChiefComplaint)]
        public async Task<IActionResult> DeletePEPChiefComplaint(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(key);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                chiefComplaintInDb.IsDeleted = true;

                context.ChiefComplaintRepository.Update(chiefComplaintInDb);
                await context.SaveChangesAsync();

                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePEPChiefComplaint", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/chief-complaint-by-encounter/key/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChiefComplaintByEncounter)]
        public async Task<IActionResult> ReadChiefComplaintByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaintInDb = await context.ChiefComplaintRepository.GetChiefComplaintByOpdVisit(encounterId);

                if (chiefComplaintInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(chiefComplaintInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChiefComplaintByEncounter", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-chief-complaint/remove/{interactionId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePrEPChiefComplaint)]
        public async Task<IActionResult> RemovePrEPChiefComplaint(Guid interactionId)
        {
            try
            {
                if (interactionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                // var chiefComplaintdb = await context.ChiefComplaintRepository.GetChiefComplaintsByOpdVisit(encounterId);
                var chiefComplaintdb = await context.ChiefComplaintRepository.GetChiefComplaintByKey(interactionId);

                if (chiefComplaintdb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var chiefComplaintdbByEncounter = await context.ChiefComplaintRepository.GetChiefComplaintsByOpdVisitEncounterType(chiefComplaintdb.EncounterId, chiefComplaintdb.EncounterType);

                if (chiefComplaintdbByEncounter == null)
                {
                    var keypopulationindb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByEncounterIdEncounterType(chiefComplaintdb.EncounterId, chiefComplaintdb.EncounterType);

                    foreach (var item in keypopulationindb)
                    {
                        context.KeyPopulationDemographicRepository.Delete(item);
                    }

                }

                context.ChiefComplaintRepository.Delete(chiefComplaintdb);

                await context.SaveChangesAsync();

                return Ok(chiefComplaintdb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePrEPChiefComplaint", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-key-population/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePrEPKeyPopulation)]
        public async Task<IActionResult> RemovePrEPKeyPopulation(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var keypopulationindb = await context.KeyPopulationDemographicRepository.GetKeyPopulationDemographicByEncounter(encounterId);

                if (keypopulationindb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in keypopulationindb)
                {
                    context.KeyPopulationDemographicRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(keypopulationindb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePrEPKeyPopulation", "ChiefComplaintController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}