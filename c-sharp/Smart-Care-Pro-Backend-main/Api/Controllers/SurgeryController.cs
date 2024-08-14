using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Surgery controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SurgeryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SurgeryController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SurgeryController(IUnitOfWork context, ILogger<SurgeryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/surgery
        /// </summary>
        /// <param name="Surgery">Surgery object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSurgery)]
        public async Task<ActionResult<Surgery>> CreateSurgery(Surgery surgery)
        {
            try
            {
               Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.EncounterId = surgery.EncounterId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Surgery, surgery.EncounterType);
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = surgery.CreatedBy;
                interaction.CreatedIn = surgery.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                surgery.InteractionId = interactionId;
                surgery.DateCreated = DateTime.Now;
                surgery.IsDeleted = false;
                surgery.IsSynced = false;                

                var newSurgeryInDb = context.SurgeryRepository.Add(surgery);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(surgery.ClientId);

                if (diagnosisInDb != null && surgery.DiagnosisList != null)
                {
                    foreach (var diagnosis in diagnosisInDb)
                    {
                        for (int listCounterStart = 0; listCounterStart < surgery.DiagnosisList.Length; listCounterStart++)
                        {
                            Guid diagnosisId = surgery.DiagnosisList[listCounterStart];

                            if (diagnosis.InteractionId == diagnosisId)
                            {
                                diagnosis.IsSelectedForSurgery = true;
                                diagnosis.SurgeryId = surgery.InteractionId;
                                context.DiagnosisRepository.Update(diagnosis);
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSurgeryByKey", new { key = newSurgeryInDb.InteractionId }, newSurgeryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSurgery", "SurgeryController.cs", ex.Message, surgery.CreatedIn, surgery.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/surgery/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSurgeryByKey)]
        public async Task<IActionResult> ReadSurgeryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var surgeryInDb = await context.SurgeryRepository.GetSurgeryByKey(key);

                if (surgeryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(surgeryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSurgeryByKey", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/treatment-plan/surgery/{surgeryId}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTreatmentPlanBySurgeryId)]
        public async Task<IActionResult> ReadTreatmentPlanBySurgeryId(Guid surgeryId)
        {
            try
            {
                if (surgeryId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var treatmentPlanInDb = await context.TreatmentPlanRepository.GetTreatmentPlanBySurgeryId(surgeryId);

                if (treatmentPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(treatmentPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTreatmentPlanBySurgeryId", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        //ReadDiagnosisBySurgeryId = "diagnoses-diagnosesBySurgeryId/{surgeryId}"

        /// <summary>
        /// URL: sc-api/surgeries
        /// </summary>
        /// <param name="">Get all row from the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSurgerys)]
        public async Task<IActionResult> ReadSurgeries()
        {
            try
            {
                var surgeryInDb = await context.SurgeryRepository.GetSurgerys();

                return Ok(surgeryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSurgeries", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnoses-diagnoses-latest-by-client/{clientId}"
        /// </summary>
        /// <param name="">Get all row from the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosesLatestBYClient)]
        public async Task<IActionResult> ReadDiagnosesLatestBYClient(Guid clientId)
        {
            try
            {
                var surgeryInDb = await context.DiagnosisRepository.GetLastDayDiagnosisByClient(clientId);

                return Ok(surgeryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosesLatestBYClient", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/diagnoses-diagnoses-last-by-client/{clientId}
        /// </summary>
        /// <param name="">Get all row from the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosesLastBYClient)]
        public async Task<IActionResult> ReadDiagnosesLastBYClient(Guid clientId)
        {
            try
            {
                var surgeryInDb = await context.DiagnosisRepository.GetLastDiagnosisByClient(clientId);

                if (surgeryInDb.Count() <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(surgeryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosesLastBYClient", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/surgery/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Surgery.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSurgeryByClientId)]
        public async Task<IActionResult> ReadSurgeryByClientId(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                if (pageSize == 0)
                {
                    var surgeryInDb = await context.SurgeryRepository.GetSurgeryByClientID(clientId);

                    foreach (Surgery surgery in surgeryInDb)
                    {
                        TimeSpan OperationTime = new TimeSpan(surgery.OperationTime.Hours, surgery.OperationTime.Minutes, surgery.OperationTime.Seconds);
                        DateTime SurgeryOperationTime = DateTime.ParseExact(OperationTime.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                        string OperationTimeStr = SurgeryOperationTime.ToString("hh:mm tt");
                        surgery.OperationTimeStr = OperationTimeStr.ToUpper();
                    }
                    return Ok(surgeryInDb);
                }
                else
                {
                    var surgeryInDb = await context.SurgeryRepository.GetSurgeryByClientID(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    foreach (Surgery surgery in surgeryInDb)
                    {
                        TimeSpan OperationTime = new TimeSpan(surgery.OperationTime.Hours, surgery.OperationTime.Minutes, surgery.OperationTime.Seconds);
                        DateTime SurgeryOperationTime = DateTime.ParseExact(OperationTime.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                        string OperationTimeStr = SurgeryOperationTime.ToString("hh:mm tt");
                        surgery.OperationTimeStr = OperationTimeStr.ToUpper();
                    }

                    PagedResultDto<Surgery> surgeryDto = new PagedResultDto<Surgery>()
                    {
                        Data = surgeryInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.SurgeryRepository.GetSurgeryByClientIDTotalCount(clientId, encounterType)
                    };
                    return Ok(surgeryDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSurgeryByClientId", "SurgeryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/surgery/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <param name="Surgery">Surgery to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSurgery)]
        public async Task<IActionResult> UpdateSurgery(Guid key, Surgery surgery)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = surgery.ModifiedBy;
                interactionInDb.ModifiedIn = surgery.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != surgery.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var surgeryInDb = await context.SurgeryRepository.GetSurgeryByKey(key);

                surgeryInDb.BookingDate = surgery.BookingDate;
                surgeryInDb.BookingTime = surgery.BookingTime;
                surgeryInDb.OperationDate = surgery.OperationDate;
                surgeryInDb.OperationTime = surgery.OperationTime;
                surgeryInDb.OperationType = surgery.OperationType;
                surgeryInDb.WardId = surgery.WardId;
                surgeryInDb.PostOpProcedure = surgery.PostOpProcedure;
                surgeryInDb.Surgeons = surgery.Surgeons;
                surgeryInDb.Team = surgery.Team;

                surgeryInDb.ModifiedBy = surgery.ModifiedBy;
                surgeryInDb.ModifiedIn = surgery.ModifiedIn;
                surgeryInDb.DateModified = DateTime.Now;
                surgeryInDb.IsDeleted = false;
                surgeryInDb.IsSynced = false;

                context.SurgeryRepository.Update(surgeryInDb);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(surgery.ClientId);

                var diagnosisBySurgery = diagnosisInDb.Where(x => x.SurgeryId == surgery.InteractionId).ToList();
                foreach (var item in diagnosisBySurgery)
                {
                    item.IsSelectedForSurgery = false;
                    item.SurgeryId = surgery.InteractionId;
                    context.DiagnosisRepository.Update(item);
                }
                if (diagnosisInDb != null && surgery.DiagnosisList != null)
                {
                    foreach (var diagnosis in diagnosisInDb)
                    {
                        for (int listCounterStart = 0; listCounterStart < surgery.DiagnosisList.Length; listCounterStart++)
                        {
                            Guid diagnosisId = surgery.DiagnosisList[listCounterStart];

                            if (diagnosis.InteractionId == diagnosisId)
                            {
                                diagnosis.IsSelectedForSurgery = true;
                                diagnosis.SurgeryId = surgery.InteractionId;
                                context.DiagnosisRepository.Update(diagnosis);
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSurgery", "SurgeryController.cs", ex.Message, surgery.ModifiedIn, surgery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/surgery/intra-op/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <param name="Surgery">Surgery to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSurgeryIntraOp)]
        public async Task<IActionResult> UpdateSurgeryIntraOp(Guid key, Surgery surgery)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = surgery.ModifiedBy;
                interactionInDb.ModifiedIn = surgery.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != surgery.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var surgeryInDb = await context.SurgeryRepository.GetSurgeryByKey(key);

                surgeryInDb.OperationStartTime = surgery.OperationStartTime;
                surgeryInDb.OperationEndTime = surgery.OperationEndTime;
                surgeryInDb.ModifiedIn = surgery.ModifiedIn;
                surgeryInDb.ModifiedBy = surgery.ModifiedBy;
                surgeryInDb.DateModified = DateTime.Now;
                surgeryInDb.IsDeleted = false;
                surgeryInDb.IsSynced = false;

                context.SurgeryRepository.Update(surgeryInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSurgeryIntraOp", "SurgeryController.cs", ex.Message, surgery.ModifiedIn, surgery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/surgery/post-op/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <param name="Surgery">Surgery to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSurgeryPostOp)]
        public async Task<IActionResult> UpdateSurgeryPostOp(Guid key, Surgery surgery)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = surgery.ModifiedBy;
                interactionInDb.ModifiedIn = surgery.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != surgery.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var surgeryInDb = await context.SurgeryRepository.GetSurgeryByKey(key);

                surgeryInDb.Team = surgery.Team;
                surgeryInDb.SurgeryIndication = surgery.SurgeryIndication;
                surgeryInDb.OperationName = surgery.OperationName;
                surgeryInDb.PostOpProcedure = surgery.PostOpProcedure;
                surgeryInDb.ModifiedBy = surgery.ModifiedBy;
                surgeryInDb.ModifiedIn = surgery.ModifiedIn;
                surgeryInDb.DateModified = DateTime.Now;
                surgeryInDb.IsDeleted = false;
                surgeryInDb.IsSynced = false;

                context.SurgeryRepository.Update(surgeryInDb);

                TreatmentPlan treatmentPlanplan = new TreatmentPlan();

                treatmentPlanplan.InteractionId = Guid.NewGuid();
                treatmentPlanplan.TreatmentPlans = surgery.TreatmentNote;
                treatmentPlanplan.SurgeryId = surgery.InteractionId;
                treatmentPlanplan.ClientId = surgery.ClientId;
                treatmentPlanplan.EncounterId = surgery.EncounterId;
                treatmentPlanplan.CreatedIn = surgery.CreatedIn;
                treatmentPlanplan.CreatedBy = surgery.CreatedBy;
                treatmentPlanplan.DateCreated = DateTime.Now;
                treatmentPlanplan.IsDeleted = false;
                treatmentPlanplan.IsSynced = false;

                context.TreatmentPlanRepository.Add(treatmentPlanplan);

                Interaction interaction = new Interaction();

                interaction.Oid = treatmentPlanplan.InteractionId;
                interaction.EncounterId = surgery.EncounterId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TreatmentPlan, surgery.EncounterType);
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = surgery.CreatedBy;
                interaction.CreatedIn = surgery.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSurgeryPostOp", "SurgeryController.cs", ex.Message, surgery.ModifiedIn, surgery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/surgery/pre-op/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <param name="Surgery">Surgery to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSurgeryPreOp)]
        public async Task<IActionResult> UpdateSurgeryPreOp(Guid key, Surgery surgery)
        {
            try
            {
                if (key != surgery.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var surgeryInDb = await context.SurgeryRepository.GetSurgeryByKey(key);
                surgeryInDb.Surgeons = surgery.Surgeons;
                surgeryInDb.TimePatientWheeledTheater = surgery.TimePatientWheeledTheater;
                surgeryInDb.NursingPreOpPlan = surgery.NursingPreOpPlan;
                surgeryInDb.SurgicalCheckList = surgery.SurgicalCheckList;
                surgeryInDb.Team = surgery.Team;
                surgeryInDb.ModifiedIn = surgery.ModifiedIn;
                surgeryInDb.DateModified = DateTime.Now;
                surgeryInDb.ModifiedBy = surgery.ModifiedBy;
                surgeryInDb.IsSynced = false;
                surgeryInDb.IsDeleted = false;

                context.SurgeryRepository.Update(surgeryInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSurgeryPreOp", "SurgeryController.cs", ex.Message, surgery.ModifiedIn, surgery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}