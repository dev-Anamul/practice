using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Firm controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class EncounterController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<EncounterController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public EncounterController(IUnitOfWork context, ILogger<EncounterController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/firm
        /// </summary>
        /// <param name="Encounter">Encounter object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEncounter)]
        public async Task<ActionResult<Encounter>> CreateEncounter(AdmissionDto admissionDto)
        {
            try
            {
                var bed = await context.EncounterRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.BedId == admissionDto.BedID && x.IPDDischargeDate == null);
                
                if (bed != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.BedAlreadyTaken);

                Encounter encounter = new Encounter();

                encounter.Oid = admissionDto.AdmissionID;
                encounter.IPDAdmissionDate = admissionDto.AdmissionDate;
                encounter.BedId = admissionDto.BedID;
                encounter.AdmissionNote = admissionDto.AdmissionNote;
                encounter.ClientId = admissionDto.ClientID;
                encounter.CreatedBy = admissionDto.CreatedBy;
                encounter.CreatedIn = admissionDto.CreatedIn;

                encounter.DateCreated = DateTime.Now;
                encounter.IsDeleted = false;
                encounter.IsSynced = false;

                var encounterInDb = context.EncounterRepository.Add(encounter);

                var clientInDb = await context.ClientRepository.GetClientByKey(admissionDto.ClientID);

                if (clientInDb != null)
                {
                    clientInDb.IsAdmitted = true;

                    context.ClientRepository.Update(clientInDb);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.GenericError);
                }

                return Ok(encounterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateEncounter", "EncounterController.cs", ex.Message, admissionDto.CreatedIn, admissionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/encounter/{key}
        /// </summary>
        /// <param name="key">primary key of Encounter table</param>
        /// <param name="bed"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.UpdateEncounter)]
        public async Task<IActionResult> UpdateEncounter(Guid key, DischargeDto dischargeDto)
        {
            try
            {
                if (key != dischargeDto.AdmissionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var encounterInDb = await context.EncounterRepository.GetEncounterByKey(dischargeDto.AdmissionId);

                if (encounterInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NoMatchFoundError);

                if (encounterInDb.IPDAdmissionDate > dischargeDto.IPDDischargeDate)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DischargeDate);

                encounterInDb.ClientId = dischargeDto.ClientId;
                encounterInDb.IPDDischargeDate = dischargeDto.IPDDischargeDate;
                encounterInDb.DischargeNote = dischargeDto.DischargeNote;
                encounterInDb.ModifiedIn = dischargeDto.ModifiedIn;
                encounterInDb.ModifiedBy = dischargeDto.ModifiedBy;
                encounterInDb.DateModified = DateTime.Now;

                if (encounterInDb.Client != null)
                {
                    encounterInDb.Client.IsAdmitted = false;
                    context.ClientRepository.Update(encounterInDb.Client);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.GenericError);
                }

                context.EncounterRepository.Update(encounterInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateEncounter", "EncounterController.cs", ex.Message, dischargeDto.ModifiedIn, dischargeDto.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/encounter/admission/{key}
        /// </summary>
        /// <param name="key">primary key of Encounter table</param>
        /// <param name="bed"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAdmission)]
        public async Task<IActionResult> UpdateAdmission(Guid key, AdmissionDto admissionDto)
        {
            try
            {
                if (key != admissionDto.AdmissionID)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var bed = await context.EncounterRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.BedId == admissionDto.BedID && x.Oid != admissionDto.AdmissionID && x.IPDDischargeDate == null);
                
                if (bed != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.BedAlreadyTaken);

                var encounterInDb = await context.EncounterRepository.GetEncounterByKey(admissionDto.AdmissionID);

                encounterInDb.IPDAdmissionDate = admissionDto.AdmissionDate;
                encounterInDb.AdmissionNote = admissionDto.AdmissionNote;
                encounterInDb.ModifiedBy = admissionDto.ModifiedBy;
                encounterInDb.ModifiedIn = admissionDto.ModifiedIn;
                encounterInDb.BedId = admissionDto.BedID;
                encounterInDb.DateModified = DateTime.Now;

                context.EncounterRepository.Update(encounterInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateAdmission", "EncounterController.cs", ex.Message, admissionDto.ModifiedIn, admissionDto.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/encounter/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEncounterByKey)]
        public async Task<IActionResult> ReadEncounterByKey(Guid key)
        {
            try
            {
                var admissionInDb = await context.EncounterRepository.GetEncounterByKey(key);

                return Ok(admissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEncounterByKey", "EncounterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/encounter/client/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>

        [HttpGet]
        [Route(RouteConstants.ReadEncounterByClient)]
        public async Task<IActionResult> ReadEncounterByClient(Guid key)
        {
            try
            {
                var admissionInDb = await context.EncounterRepository.GetEncounterByClient(key);

                return Ok(admissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEncounterByClient", "EncounterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadEncounterListByClient)]
        public async Task<IActionResult> ReadEncounterListByClient(Guid key)
        {
            try
            {
                var admissionInDb = await context.EncounterRepository.ReadEncounterListByClient(key);

                return Ok(admissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEncounterListByClient", "EncounterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadAdmissionsByClient)]
        public async Task<IActionResult> ReadAdmissionsByClient(Guid key)
        {
            try
            {
                var admissionInDb = await context.EncounterRepository.ReadAdmissionsByClient(key);

                return Ok(admissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEncounterListByClient", "EncounterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadIPDAdmissionByClient)]
        public async Task<IActionResult> ReadIPDAdmissionByClient(Guid key)
        {
            try
            {
                var admissionInDb = await context.EncounterRepository.GetIPDAdmissionByClient(key);

                return Ok(admissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEncounterByClient", "EncounterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}