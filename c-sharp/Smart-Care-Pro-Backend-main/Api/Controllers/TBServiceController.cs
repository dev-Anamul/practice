using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// TBService controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TBServiceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TBServiceController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TBServiceController(IUnitOfWork context, ILogger<TBServiceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/tb-Service
        /// </summary>
        /// <param name="tbService">TBService object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTBServices)]
        public async Task<IActionResult> CreateTBServices(TBService tbService)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TBService, tbService.EncounterType);
                interaction.EncounterId = tbService.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = tbService.CreatedBy;
                interaction.CreatedIn = tbService.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                tbService.InteractionId = interactionId;
                tbService.DateCreated = DateTime.Now;
                tbService.IsDeleted = false;
                tbService.IsSynced = false;
                tbService.TreatmentStarted = DateTime.Now;

                context.TBServiceRepository.Add(tbService);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTBServiceByKey", new { key = tbService.InteractionId }, tbService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTBServices", "TBServiceController.cs", ex.Message, tbService.CreatedIn, tbService.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBServices)]
        public async Task<IActionResult> ReadTBServices()
        {
            try
            {
                var tbServiceInDb = await context.TBServiceRepository.GetTBServices();

                return Ok(tbServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBServices", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBServiceByClient)]
        public async Task<IActionResult> ReadTBServiceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var tbServiceInDb = await context.TBServiceRepository.GetTBServiceByClient(clientId);

                    return Ok(tbServiceInDb);
                }
                else
                {
                    var tbServiceInDb = await context.TBServiceRepository.GetTBServiceByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<TBService> tBServiceWithPaggingDto = new PagedResultDto<TBService>()
                    {
                        Data = tbServiceInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.TBServiceRepository.GetTBServiceByClientTotalCount(clientId, encounterType)
                    };
                    return Ok(tBServiceWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBServiceByClient", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadActiveTBServiceByClient)]
        public async Task<IActionResult> ReadActiveTBServiceByClient(Guid clientId)
        {
            try
            {
                var tbServiceInDb = await context.TBServiceRepository.GetActiveTBServiceByClient(clientId);

                return Ok(tbServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadActiveTBServiceByClient", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBServicees.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBServiceByKey)]
        public async Task<IActionResult> ReadTBServiceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbServiceInDb = await context.TBServiceRepository.GetTBServiceByKey(key);

                if (tbServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tbServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBServiceByKey", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBServicees.</param>
        /// <param name="tbService">TBService to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTBService)]
        public async Task<IActionResult> UpdateTBService(Guid key, TBService tbService)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = tbService.ModifiedBy;
                interactionInDb.ModifiedIn = tbService.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != tbService.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                tbService.DateModified = DateTime.Now;
                tbService.IsDeleted = false;
                tbService.IsSynced = false;

                tbService.TreatmentStarted = DateTime.Now;

                context.TBServiceRepository.Update(tbService);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTBService", "TBServiceController.cs", ex.Message, tbService.ModifiedIn, tbService.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBServicees.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTBService)]
        public async Task<IActionResult> DeleteTBService(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbServiceInDb = await context.TBServiceRepository.GetTBServiceByKey(key);

                if (tbServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tbServiceInDb.DateModified = DateTime.Now;
                tbServiceInDb.IsDeleted = true;
                tbServiceInDb.IsSynced = false;

                context.TBServiceRepository.Update(tbServiceInDb);
                await context.SaveChangesAsync();

                return Ok(tbServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTBService", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/tb-Service/key/{encounterId}
        /// </summary>
        /// <param name="OPDVisitID">Primary key of the table TBServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTBPlanByEncounter)]
        public async Task<IActionResult> ReadTBPlanByOPDVisitID(Guid OPDVisitID)
        {
            try
            {
                if (OPDVisitID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbService = await context.TBServiceRepository.GetTBServiceByOpdVisit(OPDVisitID);
                var chiefComplaint = await context.ChiefComplaintRepository.GetChiefComplaintsByOpdVisit(OPDVisitID);
                var identifiedTBSymptoms = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(OPDVisitID);
                var identifiedConstitutionalSymptoms = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(OPDVisitID);
                var reviewOfSystem = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(OPDVisitID);
                var pastMedicalHistory = await context.MedicalHistoryRepository.GetMedicalHistoriesByVisitID(OPDVisitID);
                var tbHistories = await context.TBHistoryRepository.GetTBHistoryByEncounter(OPDVisitID);
                var conditions = await context.ConditionRepository.GetConditionByOPDVisitID(OPDVisitID);
                var allergies = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByEncounterId(OPDVisitID);
                var assesment = await context.AssessmentRepository.GetAssessmentByEncounter(OPDVisitID);
                var glasgowComaScales = await context.GlasgowComaScaleRepository.GetGlasgowComaScalesByEncounterId(OPDVisitID);
                var tbFindings = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByEncounterId(OPDVisitID);
                var whoConditions = await context.WHOConditionRepository.GetWHOConditionsByEncounterId(OPDVisitID);
                var tbSuspectingReasons = await context.IdentifiedReasonRepository.GetIdentifiedReasonByEncounterId(OPDVisitID);
                var diagnosis = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(OPDVisitID);
                var treatmentplans = await context.TreatmentPlanRepository.GetTreatmentPlansOPDVisitID(OPDVisitID);
                var dot = await context.DotRepository.GetDotByEncounter(OPDVisitID);
                var familyMembers = await context.FamilyMemberRepository.GetFamilyMemberByEncounterId(OPDVisitID);

                CompleteTBServiceDto completeTBServices = new CompleteTBServiceDto();
                completeTBServices.tbServices = tbService.ToList();
                completeTBServices.chiefComplaints = chiefComplaint.ToList();
                completeTBServices.identifiedTBSymptoms = identifiedTBSymptoms.ToList();
                completeTBServices.identifiedConstitutionalSymptoms = identifiedConstitutionalSymptoms.ToList();
                completeTBServices.systemReviews = reviewOfSystem.ToList();
                completeTBServices.medicalHistories = pastMedicalHistory.ToList();
                completeTBServices.tbHistories = tbHistories.ToList();
                completeTBServices.conditions = conditions.ToList();
                completeTBServices.allergies = allergies.ToList();
                completeTBServices.assessments = assesment.ToList();
                completeTBServices.glasgowComaScales = glasgowComaScales.ToList();
                completeTBServices.identifiedTBFindings = tbFindings.ToList();
                completeTBServices.whoConditions = whoConditions.ToList();
                completeTBServices.identifiedReasons = tbSuspectingReasons.ToList();
                completeTBServices.diagnoses = diagnosis.ToList();
                completeTBServices.treatmentPlans = treatmentplans.ToList();
                completeTBServices.dots = dot.ToList();
                completeTBServices.familyMembers = familyMembers.ToList();

                return Ok(completeTBServices);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTBPlanByOPDVisitID", "TBServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}