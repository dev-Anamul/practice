using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 26.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedConstitutionalSymptom controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedConstitutionalSymptomController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedConstitutionalSymptomController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedConstitutionalSymptomController(IUnitOfWork context, ILogger<IdentifiedConstitutionalSymptomController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptom
        /// </summary>
        /// <param name="identifiedConstitutionalSymptom">IdentifiedConstitutionalSymptom object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedConstitutionalSymptom)]
        public async Task<IActionResult> CreateIdentifiedConstitutionalSymptom(IdentifiedConstitutionalSymptom identifiedConstitutionalSymptom)
        {
            try
            {
                foreach (var item in identifiedConstitutionalSymptom.ConstitutionalSymptomTypeList)
                {
                    var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.LoadWithChildAsync<IdentifiedConstitutionalSymptom>(x => x.EncounterId == identifiedConstitutionalSymptom.EncounterId && x.ClientId == identifiedConstitutionalSymptom.ClientId && x.IsDeleted == false && x.ConstitutionalSymptomTypeId == item);

                    if (identifiedConstitutionalSymptomInDb == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedConstitutionalSymptom, identifiedConstitutionalSymptom.EncounterType);
                        interaction.EncounterId = identifiedConstitutionalSymptom.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedIn = identifiedConstitutionalSymptom.CreatedIn;
                        interaction.CreatedBy = identifiedConstitutionalSymptom.CreatedBy;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedConstitutionalSymptom.InteractionId = interactionId;
                        identifiedConstitutionalSymptom.ClientId = identifiedConstitutionalSymptom.ClientId;
                        identifiedConstitutionalSymptom.EncounterId = identifiedConstitutionalSymptom.EncounterId;
                        identifiedConstitutionalSymptom.ConstitutionalSymptomTypeId = item;
                        identifiedConstitutionalSymptom.DateCreated = DateTime.Now;
                        identifiedConstitutionalSymptom.IsDeleted = false;
                        identifiedConstitutionalSymptom.IsSynced = false;

                        context.IdentifiedConstitutionalSymptomRepository.Add(identifiedConstitutionalSymptom);
                        await context.SaveChangesAsync();
                    }
                }
                return CreatedAtAction("ReadIdentifiedConstitutionalSymptomByKey", new { key = identifiedConstitutionalSymptom.InteractionId }, identifiedConstitutionalSymptom);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedConstitutionalSymptom", "IdentifiedConstitutionalSymptomController.cs", ex.Message, identifiedConstitutionalSymptom.CreatedIn, identifiedConstitutionalSymptom.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptoms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedConstitutionalSymptoms)]
        public async Task<IActionResult> ReadIdentifiedConstitutionalSymptoms()
        {
            try
            {
                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptoms();

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedConstitutionalSymptoms", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptoms-by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedConstitutionalSymptomsByClient)]
        public async Task<IActionResult> ReadIdentifiedConstitutionalSymptomsByClientID(Guid clientId)
        {
            try
            {
                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByClientID(clientId);

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedConstitutionalSymptomsByClientID", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/identified-constitutional-symptoms-by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBConstitutionalSymptomsByClient)]
        public async Task<IActionResult> ReadIdentifiedTBConstitutionalSymptomsByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                List<TBConstitutionalSymptomsDto> TBConstitutionalSymptoms = new List<TBConstitutionalSymptomsDto>();
                List<TBConstitutionalSymptomsDto> TBConstitutionalSymptoms2 = new List<TBConstitutionalSymptomsDto>();
                if (pageSize == 0)
                {
                    var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByClientID(clientId);
                    var identifiedTBSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByClient(clientId);

                    TBConstitutionalSymptoms = identifiedConstitutionalSymptomInDb.Select(x => new TBConstitutionalSymptomsDto()
                    {
                        IdentifiedConstitutionalSymptomInteractionID = x.InteractionId,
                        ConstitutionalSymptomTypeID = x.ConstitutionalSymptomTypeId,
                        ConstitutionalSymptomType = x.ConstitutionalSymptomType,
                        OPDVisitID = x.EncounterId,
                        ClientID = x.ClientId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        CreatedBy = x.CreatedBy,
                        CreatedIn = x.CreatedIn,
                        EncounterType = x.EncounterType,
                        EncounterDate=x.EncounterDate,
                        ClinicianName= x.ClinicianName,
                        FacilityName=x.FacilityName,
                    }).ToList();

                    TBConstitutionalSymptoms2 = identifiedTBSymptomInDb.Select(x => new TBConstitutionalSymptomsDto()
                    {
                        IdentifiedTBSymptomInteractionID = x.InteractionId,
                        TBSymptom = x.TBSymptom,
                        TBSymptomID = x.TBSymptomId,    
                        OPDVisitID = x.EncounterId,
                        ClientID = x.ClientId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        CreatedIn = x.CreatedIn,
                        CreatedBy = x.CreatedBy,
                        EncounterType = x.EncounterType,
                        EncounterDate=x.EncounterDate,
                        FacilityName=x.FacilityName,
                        ClinicianName=x.ClinicianName
                    }).ToList();

                    var symptomsInDb = TBConstitutionalSymptoms.Concat(TBConstitutionalSymptoms2).ToList();

                    return Ok(symptomsInDb);
                }
                else
                {
                    var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByClientID(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    int identifiedConstitutionalSymptomInDbCount = context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByClientIDTotalCount(clientId, encounterType);
                    var identifiedTBSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    int identifiedTBSymptomInDbCount = context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByClientTotalCount(clientId, encounterType);

                    TBConstitutionalSymptoms = identifiedConstitutionalSymptomInDb.Select(x => new TBConstitutionalSymptomsDto()
                    {
                        IdentifiedConstitutionalSymptomInteractionID = x.InteractionId,
                        ConstitutionalSymptomTypeID = x.ConstitutionalSymptomTypeId,
                        ConstitutionalSymptomType = x.ConstitutionalSymptomType,
                        OPDVisitID = x.EncounterId,
                        ClientID = x.ClientId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        CreatedBy = x.CreatedBy,
                        CreatedIn = x.CreatedIn,
                        EncounterType = x.EncounterType,
                        EncounterDate=x.EncounterDate
                    }).ToList();

                    TBConstitutionalSymptoms2 = identifiedTBSymptomInDb.Select(x => new TBConstitutionalSymptomsDto()
                    {
                        IdentifiedTBSymptomInteractionID = x.InteractionId,
                        TBSymptom = x.TBSymptom,
                        TBSymptomID = x.TBSymptomId,
                        OPDVisitID = x.EncounterId,
                        ClientID = x.ClientId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        CreatedIn = x.CreatedIn,
                        CreatedBy = x.CreatedBy,
                        EncounterType = x.EncounterType,
                        EncounterDate=x.EncounterDate
                    }).ToList();

                    var symptomsInDb = TBConstitutionalSymptoms.Concat(TBConstitutionalSymptoms2).OrderByDescending(x => x.DateCreated).ToList();
                    PagedResultDto<TBConstitutionalSymptomsDto> tbSymptomsDto = new PagedResultDto<TBConstitutionalSymptomsDto>()
                    {
                        Data = symptomsInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = (identifiedConstitutionalSymptomInDbCount > identifiedTBSymptomInDbCount) ? identifiedConstitutionalSymptomInDbCount : identifiedTBSymptomInDbCount
                    };
                    return Ok(tbSymptomsDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBConstitutionalSymptomsByClientID", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/identified-constitutional-symptoms-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounters.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedConstitutionalSymptomsByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedConstitutionalSymptomsByEncounterId(Guid encounterId)
        {
            try
            {
                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(encounterId);

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedConstitutionalSymptomsByEncounterId", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptom/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedConstitutionalSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedConstitutionalSymptomByKey)]
        public async Task<IActionResult> ReadIdentifiedConstitutionalSymptomByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomByKey(key);

                if (identifiedConstitutionalSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedConstitutionalSymptomByKey", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedConstitutionalSymptom.</param>
        /// <param name="identifiedConstitutionalSymptom">IdentifiedConstitutionalSymptom to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedConstitutionalSymptom)]
        public async Task<IActionResult> UpdateIdentifiedConstitutionalSymptom(Guid key, IdentifiedConstitutionalSymptom identifiedConstitutionalSymptom)
        {
            try
            {
                if (key != identifiedConstitutionalSymptom.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedConstitutionalSymptom.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedConstitutionalSymptom.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedConstitutionalSymptom.DateModified = DateTime.Now;
                identifiedConstitutionalSymptom.IsDeleted = false;
                identifiedConstitutionalSymptom.IsSynced = false;

                context.IdentifiedConstitutionalSymptomRepository.Update(identifiedConstitutionalSymptom);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedConstitutionalSymptom", "IdentifiedConstitutionalSymptomController.cs", ex.Message, identifiedConstitutionalSymptom.ModifiedIn, identifiedConstitutionalSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedConstitutionalSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedConstitutionalSymptom)]
        public async Task<IActionResult> DeleteIdentifiedConstitutionalSymptom(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomByKey(key);

                if (identifiedConstitutionalSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedConstitutionalSymptomInDb.DateModified = DateTime.Now;
                identifiedConstitutionalSymptomInDb.IsDeleted = true;
                identifiedConstitutionalSymptomInDb.IsSynced = false;

                context.IdentifiedConstitutionalSymptomRepository.Update(identifiedConstitutionalSymptomInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedConstitutionalSymptom", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-constitutional-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedConstitutionalSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedConstitutionalSymptom)]
        public async Task<IActionResult> RemoveIdentifiedConstitutionalSymptom(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(key);

                if (identifiedConstitutionalSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedConstitutionalSymptomInDb)
                {
                    context.IdentifiedConstitutionalSymptomRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedConstitutionalSymptom", "IdentifiedConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}