using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 2.01.2023
 * Modified by   : Bella
 * Last modified : 11.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// TreatmentPlan controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TreatmentPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TreatmentPlanController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TreatmentPlanController(IUnitOfWork context, ILogger<TreatmentPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/treatment-plan
        /// </summary>
        /// <param name="treatmentPlan">CreateTreatmentPlan object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTreatmentPlan)]
        public async Task<IActionResult> CreateTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TreatmentPlan, treatmentPlan.EncounterType);
                interaction.EncounterId = treatmentPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedIn = treatmentPlan.CreatedIn;
                interaction.CreatedBy = treatmentPlan.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                treatmentPlan.InteractionId = interactionId;
                treatmentPlan.ClientId = treatmentPlan.ClientId;
                treatmentPlan.EncounterId = treatmentPlan.EncounterId;
                treatmentPlan.DateCreated = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Add(treatmentPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-treatment-plan
        /// </summary>
        /// <param name="treatmentPlan">CreateTreatmentPlang object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIPDTreatmentPlan)]
        public async Task<IActionResult> CreateIPDTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TreatmentPlan, treatmentPlan.EncounterType);
                interaction.EncounterId = treatmentPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = Guid.Empty;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                treatmentPlan.InteractionId = interactionId;
                treatmentPlan.ClientId = treatmentPlan.ClientId;
                treatmentPlan.EncounterId = treatmentPlan.EncounterId;
                treatmentPlan.DateCreated = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Add(treatmentPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIPDTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan
        /// </summary>
        /// <param name="treatmentPlan">CreateTreatmentPlang object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPTreatmentPlan)]
        public async Task<IActionResult> CreatePEPTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TreatmentPlan, treatmentPlan.EncounterType);
                interaction.EncounterId = treatmentPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = Guid.Empty;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                treatmentPlan.InteractionId = interactionId;
                treatmentPlan.ClientId = treatmentPlan.ClientId;
                treatmentPlan.EncounterId = treatmentPlan.EncounterId;
                treatmentPlan.DateCreated = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Add(treatmentPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPEPTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan
        /// </summary>
        /// <param name="treatmentPlan">CreateTreatmentPlang object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEPTreatmentPlan)]
        public async Task<IActionResult> CreatePrEPTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TreatmentPlan, treatmentPlan.EncounterType);
                interaction.EncounterId = treatmentPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = Guid.Empty;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                treatmentPlan.InteractionId = interactionId;
                treatmentPlan.ClientId = treatmentPlan.ClientId;
                treatmentPlan.EncounterId = treatmentPlan.EncounterId;
                treatmentPlan.DateCreated = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Add(treatmentPlan);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPrEPTreatmentPlanByKey", new { key = treatmentPlan.InteractionId }, treatmentPlan);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.CreatedIn, treatmentPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTreatmentPlans)]
        public async Task<IActionResult> ReadTreatmentPlans()
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlans();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlans", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPTreatmentPlans)]
        public async Task<IActionResult> ReadPEPTreatmentPlans()
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlans();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPTreatmentPlans", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPTreatmentPlans)]
        public async Task<IActionResult> ReadPrEPTreatmentPlans()
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlans();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPTreatmentPlans", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plans-byClient
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTreatmentPlanByClient)]
        public async Task<IActionResult> ReadTreatmentPlansByClient(Guid clientId, int page, int pageSize, EncounterType encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlansClient(clientId);

                    return Ok(treatmentPlansInDb);
                }
                else
                {
                    var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlansClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<TreatmentPlan> treatmentPlanWithPaggingDto = new PagedResultDto<TreatmentPlan>()
                    {
                        Data = treatmentPlansInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.TreatmentPlanRepository.GetTreatmentPlansClientTotalCount(clientId, encounterType)

                    };

                    return Ok(treatmentPlanWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlansByClient", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadLastEncounterTreatmentPlanByClient)]
        public async Task<IActionResult> GetLastEncounterTreatmentPlanByClient(Guid clientId)
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetLastEncounterTreatmentPlanByClient(clientId);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetLastEncounterTreatmentPlanByClient", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/pep-treatment-plan/by-Client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPTreatmentPlanByClient)]
        public async Task<IActionResult> ReadPEPTreatmentPlansByClient(Guid clientId)
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlansClient(clientId);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPTreatmentPlansByClient", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/by-Client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPTreatmentPlanByClient)]
        public async Task<IActionResult> ReadPrEPTreatmentPlansByClient(Guid clientId)
        {
            try
            {
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlansClient(clientId);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPTreatmentPlansByClient", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTreatmentPlanByKey)]
        public async Task<IActionResult> ReadTreatmentPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlanByKey", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPTreatmentPlanByKey)]
        public async Task<IActionResult> ReadPEPTreatmentPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPTreatmentPlanByKey", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPTreatmentPlanByKey)]
        public async Task<IActionResult> ReadPrEPTreatmentPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPTreatmentPlanByKey", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadCompleteTreatmentPlanByEncounterId)]
        public async Task<IActionResult> ReadCompleteTreatmentPlanByEncounterID(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nutrition = await context.NutritionRepository.GetNutritionByEncounter(encounterId);
                var chiefComplaint = await context.ChiefComplaintRepository.GetChiefComplaintsByOpdVisit(encounterId);
                var conditions = await context.ConditionRepository.GetConditionByOPDVisitID(encounterId);
                var treatmentplans = await context.TreatmentPlanRepository.GetTreatmentPlansOPDVisitID(encounterId);
                var identifiedTBSymptoms = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(encounterId);
                var identifiedConstitutionalSymptoms = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(encounterId);
                var allergies = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByEncounterId(encounterId);
                var immunization = await context.ImmunizationRecordRepository.GetImmunizationRecordByEncounter(encounterId);
                var diagnosis = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(encounterId);
                var glasgowComaScales = await context.GlasgowComaScaleRepository.GetGlasgowComaScalesByEncounterId(encounterId);
                var gynoobs = await context.GynObsHistoryRepository.GetGynObsHistoryByEncounterId(encounterId);
                var birthHisory = await context.BirthHistoryRepository.GetBirthHistoryByEncounter(encounterId);
                var reviewOfSystem = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(encounterId);
                var pastMedicalHistory = await context.MedicalHistoryRepository.GetMedicalHistoriesByVisitID(encounterId);
                var childDevlopment = await context.ChildsDevelopmentHistoryRepository.GetChildsDevelopmentHistoryByOpdVisit(encounterId);
                var assesment = await context.AssessmentRepository.GetAssessmentByEncounter(encounterId);
                var systemExamination = await context.SystemExaminationRepository.GetSystemReviewByEncounter(encounterId);
                var drugAdherence = await context.DrugAdherenceRepository.GetDrugAdherenceByEncounter(encounterId);
                var hivPreventionHistory = await context.PEPPreventionHistoryRepository.GetPEPPreventionHistoryByEncounterId(encounterId);
                var pep = await context.PrEPRepository.GetPrEPByEncounter(encounterId);
                var tbService = await context.TBServiceRepository.GetTBServiceByOpdVisit(encounterId);
                var tbHistories = await context.TBHistoryRepository.GetTBHistoryByEncounter(encounterId);
                var tbFindings = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByEncounterId(encounterId);
                var whoConditions = await context.WHOConditionRepository.GetWHOConditionsByEncounterId(encounterId);
                var tbSuspectingReasons = await context.IdentifiedReasonRepository.GetIdentifiedReasonByEncounterId(encounterId);
                var dot = await context.DotRepository.GetDotByEncounter(encounterId);
                var familyMembers = await context.FamilyMemberRepository.GetFamilyMemberByEncounterId(encounterId);
                var referralModules = await context.ReferralModuleRepository.GetReferralModuleByEncounter(encounterId);
                var ancScreening = await context.ANCScreeningRepository.GetANCScreeningByEncounter(encounterId);
                var ancService = await context.ANCServiceRepository.GetANCServiceByEncounter(encounterId);
                var bloodTransfusionHistories = await context.BloodTransfusionHistoryRepository.GetBloodTransfusionHistoryByEncounter(encounterId);
                var surgeries = await context.SurgeryRepository.GetSurgeryByEncounterID(encounterId);
                var vmmcService = await context.VMMCServiceRepository.GetVMMCServiceByEncounterId(encounterId);
                //var painRecords = await context.PainRecordRepository.GetPainRecordByEncounter(encounterId);
                var complications = await context.ComplicationRepository.GetComplicationByEncounter(encounterId);
                var anestheticPlans = await context.AnestheticPlanRepository.GetAnestheticPlanByEncounter(encounterId);
                var skinPreparations = await context.SkinPreparationRepository.GetSkinPreparationByEncounter(encounterId);
                var artRegisters = await context.ARTRegisterRepository.GetARTRegisterByEncounterID(encounterId);
                //var attachedFacilities = await context.AttachedFacilityRepository.GetAttachedFacilityByEncounterID(encounterId);                     
                var dsdAssesments = await context.DSDAssesmentRepository.GetDSDAssesmentByEncounter(encounterId);
                var visitPurposes = await context.VisitPuposeRepository.GetVisitPurposeByEncounter(encounterId);
                var artResponses = await context.ARTResponseRepository.GetARTResponseByEncounterId(encounterId);
                var priorARTExposers = await context.PriorARTExposerRepository.GetPriorARTExposerByEncounter(encounterId);
                var takenTPTDrugs = await context.TakenARTDrugRepository.GetTakenARTDrugByEncounter(encounterId);
                var tptHistories = await context.TPTHistoryRepository.GetTPTHistoryByEncounter(encounterId);
                var artTreatmentPlans = await context.ARTTreatmentPlan.GetARTTreatmentPlanByEncounter(encounterId);
                var clientsDisabilities = await context.ClientsDisabilityRepository.GetClientsDisabilityByEncounter(encounterId);
                var pmtcts = await context.PMTCTRepository.GetPMTCTByEncounter(encounterId);
                var pregnancyBookings = await context.PregnancyBookingRepository.GetPregnancyBookingByEncounter(encounterId);
                var motherDetails = await context.MotherDetailRepository.GetMotherDetailByEncounter(encounterId);
                var ObstericExaminations = await context.ObstetricExaminationRepository.GetObstetricExaminationByEncounter(encounterId);
                var pelvicVaginalExaminations = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByEncounter(encounterId);
                var priorSensitizations = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByEncounter(encounterId);
                var visitDetails = await context.VisitDetailRepository.GetVisitDetailByEncounter(encounterId);
                var identifiedEyesAssessments = await context.IdentifiedEyesAssessmentRepository.GetIdentifiedEyesAssessmentByEncounter(encounterId);
                var identifiedCordStumps = await context.IdentifiedCordStumpRepository.GetIdentifiedCordStumpByEncounter(encounterId);
                var preferredFeedings = await context.IdentifiedPreferredFeedingRepository.GetIdentifiedPreferredFeedingByEncounter(encounterId);
                var reasonOfReferrals = await context.IdentifiedReferralReasonRepository.GetIdentifiedReferralReasonByEncounter(encounterId);
                var motherDeliverySummaries = await context.MotherDeliverySummaryRepository.GetMotherDeliverySummaryByEncounter(encounterId);
                var pPHTreatments = await context.PPHTreatmentRepository.GetPPHTreatmentByEncounter(encounterId);
                var medicalTreatments = await context.MedicalTreatmentRepository.GetMedicalTreatmentByEncounter(encounterId);
                var apgarScores = await context.ApgarScoreRepository.GetApgarScoreByEncounter(encounterId);
                var newBornDetails = await context.NewBornDetailRepository.GetNewBornDetailByEncounter(encounterId);
                var guidedExaminations = await context.GuidedExaminationRepository.ReadGuidedExaminationByEncounter(encounterId);
                var quickExaminations = await context.QuickExaminationRepository.ReadQuickExaminationByEncounter(encounterId);
                var medicalConditions = await context.MedicalConditionRepository.GetMedicalConditionByEncounterID(encounterId);
                var familyPlans = await context.FamilyPlanRepository.GetFamilyPlanByEncounter(encounterId);
                var familyPlanRegister = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByEncounterID(encounterId);
                var insertionAndRemovalProcedures = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedureByEncounter(encounterId);
                var counsellingServices = await context.CounsellingServiceRepository.GetCounsellingServiceByEncounter(encounterId);
                var screeningAndPreventions = await context.ScreeningAndPreventionRepository.GetScreeningAndPreventionByEncounter(encounterId);
                var veginalPositions = await context.VaginalPositionRepository.GetVaginalPositionByEncounter(encounterId);
                var dischargeMetrics = await context.DischargeMetricRepository.GetDischargeMetricByEncounter(encounterId);
                var feedingMethods = await context.FeedingMethodRepository.GetFeedingMethodByEncounter(encounterId);

                var treatmentPlanInDb = await context.TreatmentPlanRepository.GetTreatmentPlansEncounterId(encounterId);
                var planInDb = await context.PrEPRepository.GetPrEPEncounterId(encounterId);

                CompleteTreatMentPlanDto completeTreatMentPlanDto = new CompleteTreatMentPlanDto();

                completeTreatMentPlanDto.nutritions = nutrition.ToList();
                completeTreatMentPlanDto.chiefComplaints = chiefComplaint.ToList();
                completeTreatMentPlanDto.birthHistories = birthHisory.ToList();
                completeTreatMentPlanDto.systemReviews = reviewOfSystem.ToList();
                completeTreatMentPlanDto.childsDevelopmentHistories = childDevlopment.ToList();
                completeTreatMentPlanDto.medicalHistories = pastMedicalHistory.ToList();
                completeTreatMentPlanDto.tbHistories = tbHistories.ToList();
                completeTreatMentPlanDto.drugAdherence = drugAdherence.ToList();
                completeTreatMentPlanDto.preventionHistory = hivPreventionHistory.ToList();
                completeTreatMentPlanDto.PEP = pep.ToList();
                completeTreatMentPlanDto.conditions = conditions.ToList();
                completeTreatMentPlanDto.assessments = assesment.ToList();
                completeTreatMentPlanDto.systemExaminations = systemExamination.ToList();
                completeTreatMentPlanDto.treatmentPlans = treatmentplans.ToList();
                completeTreatMentPlanDto.identifiedTBSymptoms = identifiedTBSymptoms.ToList();
                completeTreatMentPlanDto.identifiedConstitutionalSymptoms = identifiedConstitutionalSymptoms.ToList();
                completeTreatMentPlanDto.allergies = allergies.ToList();
                completeTreatMentPlanDto.immunizationRecords = immunization.ToList();
                completeTreatMentPlanDto.diagnoses = diagnosis.ToList();
                completeTreatMentPlanDto.glasgowComaScales = glasgowComaScales.ToList();
                completeTreatMentPlanDto.identifiedTBFindings = tbFindings.ToList();
                completeTreatMentPlanDto.whoConditions = whoConditions.ToList();
                completeTreatMentPlanDto.identifiedReasons = tbSuspectingReasons.ToList();
                completeTreatMentPlanDto.gynObsHistories = gynoobs.ToList();
                completeTreatMentPlanDto.drugAdherence = drugAdherence.ToList();
                completeTreatMentPlanDto.preventionHistory = hivPreventionHistory.ToList();
                completeTreatMentPlanDto.PEP = pep.ToList();
                completeTreatMentPlanDto.dots = dot.ToList();
                completeTreatMentPlanDto.familyMembers = familyMembers.ToList();
                completeTreatMentPlanDto.referralModules = referralModules.ToList();
                completeTreatMentPlanDto.ancScreenings = ancScreening.ToList();
                completeTreatMentPlanDto.ancServices = ancService.ToList();
                completeTreatMentPlanDto.BloodTransfusionHistories = bloodTransfusionHistories.ToList();
                completeTreatMentPlanDto.Surgeries = surgeries.ToList();
                completeTreatMentPlanDto.vmmcServices = vmmcService.ToList();
                completeTreatMentPlanDto.complications = complications.ToList();
                completeTreatMentPlanDto.anestheticPlans = anestheticPlans.ToList();
                completeTreatMentPlanDto.skinPreparations = skinPreparations.ToList();
                completeTreatMentPlanDto.artServices = artRegisters.ToList();
                completeTreatMentPlanDto.dsdAssesments = dsdAssesments.ToList();
                completeTreatMentPlanDto.visitPurposes = visitPurposes.ToList();
                completeTreatMentPlanDto.artResponses = artResponses.ToList();
                completeTreatMentPlanDto.priorARTExposers = priorARTExposers.ToList();
                completeTreatMentPlanDto.tptHistories = tptHistories.ToList();
                completeTreatMentPlanDto.artTreatmentPlans = artTreatmentPlans.ToList();
                completeTreatMentPlanDto.clientsDisabilities = clientsDisabilities.ToList();
                completeTreatMentPlanDto.pmtcts = pmtcts.ToList();
                completeTreatMentPlanDto.pregnancyBookings = pregnancyBookings.ToList();
                completeTreatMentPlanDto.motherDetails = motherDetails.ToList();
                completeTreatMentPlanDto.ObstericExaminations = ObstericExaminations.ToList();
                completeTreatMentPlanDto.pelvicVaginalExaminations = pelvicVaginalExaminations.ToList();
                completeTreatMentPlanDto.identifiedPriorSensitizations = priorSensitizations.ToList();
                completeTreatMentPlanDto.visitDetails = visitDetails.ToList();
                completeTreatMentPlanDto.identifiedEyesAssessments = identifiedEyesAssessments.ToList();
                completeTreatMentPlanDto.identifiedCordStumps = identifiedCordStumps.ToList();
                completeTreatMentPlanDto.preferredFeedings = preferredFeedings.ToList();
                completeTreatMentPlanDto.reasonOfReferrals = reasonOfReferrals.ToList();
                completeTreatMentPlanDto.motherDeliverySummaries = motherDeliverySummaries.ToList();
                completeTreatMentPlanDto.pPHTreatments = pPHTreatments.ToList();
                completeTreatMentPlanDto.medicalTreatments = medicalTreatments.ToList();
                completeTreatMentPlanDto.apgarScores = apgarScores.ToList();
                completeTreatMentPlanDto.newBornDetails = newBornDetails.ToList();
                completeTreatMentPlanDto.pPHTreatments = pPHTreatments.ToList();
                completeTreatMentPlanDto.guidedExaminations = guidedExaminations.ToList();
                completeTreatMentPlanDto.quickExaminations = quickExaminations.ToList();
                completeTreatMentPlanDto.medicalConditions = medicalConditions.ToList();
                completeTreatMentPlanDto.familyPlans = familyPlans.ToList();
                completeTreatMentPlanDto.familyPlanRegisters = familyPlanRegister.ToList();
                completeTreatMentPlanDto.insertionAndRemovalProcedures = insertionAndRemovalProcedures.ToList();
                completeTreatMentPlanDto.counsellingServices = counsellingServices.ToList();
                completeTreatMentPlanDto.screeningAndPreventions = screeningAndPreventions.ToList();
                completeTreatMentPlanDto.veginalPositions = veginalPositions.ToList();
                completeTreatMentPlanDto.dischargeMetrics = dischargeMetrics.ToList();
                completeTreatMentPlanDto.feedingMethods = feedingMethods.ToList();
               
                var  encounter = context.EncounterRepository.GetById(encounterId);
                
                completeTreatMentPlanDto.EncounterDate= encounter?.OPDVisitDate ?? encounter?.IPDAdmissionDate ?? encounter?.DateCreated;
               
                if (treatmentPlanInDb != null)
                    completeTreatMentPlanDto.CreatedBy = treatmentPlanInDb.CreatedBy;
                else
                    completeTreatMentPlanDto.CreatedBy = planInDb?.CreatedBy;


                if (treatmentPlanInDb != null)
                    completeTreatMentPlanDto.CreatedIn = treatmentPlanInDb.CreatedIn;
                else
                    completeTreatMentPlanDto.CreatedIn = planInDb?.CreatedIn;

                if (treatmentPlanInDb != null)
                    completeTreatMentPlanDto.CreatedBy = treatmentPlanInDb.CreatedBy;
                else
                    completeTreatMentPlanDto.CreatedBy = planInDb?.CreatedBy;


                if (treatmentPlanInDb != null)
                    completeTreatMentPlanDto.CreatedIn = treatmentPlanInDb.CreatedIn;
                else
                    completeTreatMentPlanDto.CreatedIn = planInDb?.CreatedIn;


                return Ok(completeTreatMentPlanDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCompleteTreatmentPlanByEncounterID", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadCompleteIPDHistoryDtoByEncounterId)]
        public async Task<IActionResult> ReadCompleteIPDHistoryDtoByEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaint = await context.ChiefComplaintRepository.GetChiefComplaintsByOpdVisit(encounterId);
                var diagnosis = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(encounterId);
                var treatmentPlan = await context.TreatmentPlanRepository.GetTreatmentPlansOPDVisitID(encounterId);

                CompleteIPDHistoryDto completeIPDHistoryDto = new CompleteIPDHistoryDto();

                completeIPDHistoryDto.chiefComplaint = chiefComplaint.ToList();
                completeIPDHistoryDto.diagnosis = diagnosis.ToList();
                completeIPDHistoryDto.treatmentPlan = treatmentPlan.ToList();

                return Ok(completeIPDHistoryDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCompleteIPDHistoryDtoByEncounterId", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLatestTreatmentPlanByClient)]
        public async Task<IActionResult> ReadLatestTreatmentPlanByClient(Guid clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisDb = await context.TreatmentPlanRepository.GetLatestTreatmentPlanByClient(clientId);

                if (diagnosisDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestTreatmentPlanByClient", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLatestTreatmentPlanByClientForFluid)]
        public async Task<IActionResult> ReadLatestTreatmentPlanByClientForFluid(Guid clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisDb = await context.TreatmentPlanRepository.GetLatestTreatmentPlanByClientForFluid(clientId);

                if (diagnosisDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestTreatmentPlanByClientForFluid", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <param name="treatmentPlan">TreatmentPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTreatmentPlan)]
        public async Task<IActionResult> UpdateTreatmentPlan(Guid key, TreatmentPlan treatmentPlan)
        {
            try
            {
                if (key != treatmentPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var treatmentPlanInDb = await context.TreatmentPlanRepository.GetByIdAsync(treatmentPlan.InteractionId);

                if (treatmentPlanInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                treatmentPlanInDb.SurgeryId = treatmentPlan.SurgeryId;
                treatmentPlanInDb.ClientId = treatmentPlan.ClientId;
                treatmentPlanInDb.TreatmentPlans = treatmentPlan.TreatmentPlans;
                treatmentPlanInDb.ModifiedBy = treatmentPlan.ModifiedBy;
                treatmentPlanInDb.ModifiedIn = treatmentPlan.ModifiedIn;
                treatmentPlanInDb.DateModified = DateTime.Now;
                treatmentPlanInDb.IsDeleted = false;
                treatmentPlanInDb.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlanInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.ModifiedIn, treatmentPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <param name="treatmentPlan">TreatmentPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIPDTreatmentPlan)]
        public async Task<IActionResult> UpdateIPDTreatmentPlan(Guid key, TreatmentPlan treatmentPlan)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = treatmentPlan.ModifiedBy;
                interactionInDb.ModifiedIn = treatmentPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != treatmentPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                treatmentPlan.DateModified = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIPDTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.ModifiedIn, treatmentPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <param name="treatmentPlan">TreatmentPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPTreatmentPlan)]
        public async Task<IActionResult> UpdatePEPTreatmentPlan(Guid key, TreatmentPlan treatmentPlan)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = treatmentPlan.ModifiedBy;
                interactionInDb.ModifiedIn = treatmentPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != treatmentPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                treatmentPlan.DateModified = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.ModifiedIn, treatmentPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlan.</param>
        /// <param name="treatmentPlan">TreatmentPlan to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePrEPTreatmentPlan)]
        public async Task<IActionResult> UpdatePrEPTreatmentPlan(Guid key, TreatmentPlan treatmentPlan)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = treatmentPlan.ModifiedBy;
                interactionInDb.ModifiedIn = treatmentPlan.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != treatmentPlan.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                treatmentPlan.DateModified = DateTime.Now;
                treatmentPlan.IsDeleted = false;
                treatmentPlan.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlan);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePrEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message, treatmentPlan.ModifiedIn, treatmentPlan.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTreatmentPlan)]
        public async Task<IActionResult> DeleteTreatmentPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                treatmentPlansInDb.DateCreated = DateTime.Now;
                treatmentPlansInDb.IsDeleted = true;
                treatmentPlansInDb.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlansInDb);
                await context.SaveChangesAsync();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTreatmentPlan", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIPDTreatmentPlan)]
        public async Task<IActionResult> DeleteIPDTreatmentPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                treatmentPlansInDb.DateCreated = DateTime.Now;
                treatmentPlansInDb.IsDeleted = true;
                treatmentPlansInDb.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlansInDb);
                await context.SaveChangesAsync();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIPDTreatmentPlan", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePEPTreatmentPlan)]
        public async Task<IActionResult> DeletePEPTreatmentPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                treatmentPlansInDb.DateCreated = DateTime.Now;
                treatmentPlansInDb.IsDeleted = true;
                treatmentPlansInDb.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlansInDb);
                await context.SaveChangesAsync();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-treatment-plan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePrEPTreatmentPlan)]
        public async Task<IActionResult> DeletePrEPTreatmentPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetTreatmentPlanByKey(key);

                if (treatmentPlansInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                treatmentPlansInDb.DateCreated = DateTime.Now;
                treatmentPlansInDb.IsDeleted = true;
                treatmentPlansInDb.IsSynced = false;

                context.TreatmentPlanRepository.Update(treatmentPlansInDb);
                await context.SaveChangesAsync();

                return Ok(treatmentPlansInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePrEPTreatmentPlan", "TreatmentPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}