using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// OPDVisit controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class OPDVisitController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<OPDVisitController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public OPDVisitController(IUnitOfWork context, ILogger<OPDVisitController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/opd-visit
        /// </summary>
        /// <param name="encounter">CreateVisit object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatOPDVisit)]
        public async Task<IActionResult> CreateOPDVisit(Encounter encounter)
        {
            try
            {
                var ipdVisit = await context.EncounterRepository.GetIPDAdmissionByClient(encounter.ClientId);

                if (ipdVisit != null && ipdVisit?.Client?.IsAdmitted == true)
                    return CreatedAtAction("ReadOPDVisitByKey", new { key = ipdVisit?.Oid }, ipdVisit);

                //double validHours = -24;

                //var latest = DateTime.Now.AddHours(validHours);

                var today = DateTime.Now.Date;

                var opdVisit = await context.EncounterRepository.LoadWithChildAsync<Encounter>(x => (x.OPDVisitDate != null && x.OPDVisitDate.Value.Date == today.Date) && x.ClientId == encounter.ClientId && x.IsDeleted == false
                , h => h.Client,
                  h => h.Client.HomeLanguage,
                    o => o.Client.Occupation,
                    e => e.Client.EducationLevel,
                    c => c.Client.Country,
                    d => d.Client.District);

                if (opdVisit != null)
                    return CreatedAtAction("ReadOPDVisitByKey", new { key = opdVisit.Oid }, opdVisit);

                encounter.IsDeleted = false;
                encounter.IsSynced = false;
                encounter.Oid = Guid.NewGuid();
                encounter.OPDVisitDate = DateTime.Now;
                encounter.DateCreated = DateTime.Now;

                var visitInDb = context.EncounterRepository.Add(encounter);
                await context.SaveChangesAsync();

                var client = await context.ClientRepository.LoadWithChildAsync<Client>(x => x.Oid == visitInDb.ClientId,
              h => h.HomeLanguage,
                o => o.Occupation,
                e => e.EducationLevel,
                c => c.Country,
                d => d.District);
                visitInDb.Client = client;

                return CreatedAtAction("ReadOPDVisitByKey", new { key = encounter.Oid }, encounter);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateOPDVisit", "OPDVisitController.cs", ex.Message, encounter.CreatedIn, encounter.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opd-visit
        /// </summary>
        /// <param name="encounter">CreateVisit object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatOPDVisitHistory)]
        public async Task<IActionResult> CreatOPDVisitHistory(Encounter encounter)
        {
            try
            {
                var historicalVisitInDb = await context.EncounterRepository.GetEncounterByDate(encounter.OPDVisitDate);

                if (historicalVisitInDb != null)
                {
                    var encounterInDb = await context.EncounterRepository.GetEncounterByKey(historicalVisitInDb.Oid);
                    return Ok(encounterInDb);
                }

                encounter.DateCreated = DateTime.Now;
                encounter.IsDeleted = false;
                encounter.IsSynced = false;
                encounter.Oid = Guid.NewGuid();
                encounter.OPDVisitDate = encounter.OPDVisitDate;
                encounter.DateCreated = DateTime.Now;

                var visitInDb = context.EncounterRepository.Add(encounter);

                await context.SaveChangesAsync();

                var client = await context.ClientRepository.LoadWithChildAsync<Client>(x => x.Oid == visitInDb.ClientId,
              h => h.HomeLanguage,
                o => o.Occupation,
                e => e.EducationLevel,
                c => c.Country,
                d => d.District);
                visitInDb.Client = client;

                return CreatedAtAction("ReadOPDVisitByKey", new { key = encounter.Oid }, encounter);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateOPDVisit", "OPDVisitController.cs", ex.Message, encounter.CreatedIn, encounter.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visits
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadeOPDVisits)]
        public async Task<IActionResult> ReadVisits()
        {
            try
            {
                var visitInDb = await context.EncounterRepository.GetEncounters();

                return Ok(visitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisits", "OPDVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadeOPDVisitsByDate)]
        public async Task<IActionResult> ReadeOPDVisitsByDate(DateTime historicalDate)
        {
            try
            {
                var visitInDb = await context.EncounterRepository.GetEncounterByDate(historicalDate);

                return Ok(visitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisits", "OPDVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Visits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOPDVisitByKey)]
        public async Task<IActionResult> ReadOPDVisitByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitInDb = await context.EncounterRepository.GetEncounterByKey(key);

                if (visitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(visitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOPDVisitByKey", "OPDVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadMedicalHistory)]
        public async Task<IActionResult> ReadCompleteTreatmentPlanByOPDVisitId(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var chiefComplaint = await context.ChiefComplaintRepository.GetChiefComplaintsByClient(clientId);
                var diagnosis = await context.DiagnosisRepository.GetDiagnosisByClient(clientId);

                ChiefComplaintsDto chiefComplaintsDto = new ChiefComplaintsDto
                {
                    ChiefComplaints = chiefComplaint.OrderByDescending(x => x.DateCreated).ToList(),
                    Diagnoses = diagnosis.OrderByDescending(x => x.DateCreated).ToList()
                };

                return Ok(chiefComplaintsDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCompleteTreatmentPlanByOPDVisitId", "OPDVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Visits.</param>
        /// <param name="encounter">Object to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateOPDVisit)]
        public async Task<IActionResult> UpdateMedicalEncounter(Guid key, Encounter encounter)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                encounter.DateModified = DateTime.Now;
                encounter.IsDeleted = false;
                encounter.IsSynced = false;

                context.EncounterRepository.Update(encounter);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicalEncounter", "OPDVisitController.cs", ex.Message, encounter.ModifiedIn, encounter.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table visit.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteOPDVisit)]
        public async Task<IActionResult> DeleteOPDVisit(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitInDb = await context.EncounterRepository.GetEncounterByKey(key);

                if (visitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                visitInDb.DateModified = DateTime.Now;
                visitInDb.IsDeleted = true;
                visitInDb.IsSynced = false;

                context.EncounterRepository.Update(visitInDb);
                await context.SaveChangesAsync();

                return Ok(visitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteOPDVisit", "OPDVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}