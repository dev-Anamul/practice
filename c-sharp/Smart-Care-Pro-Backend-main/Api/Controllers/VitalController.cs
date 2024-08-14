using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by: Phoenix(2)
 * Date created: 17.10.2022
 * Modified by: Sphinx(1)
 * Last modified: 07.11.2022
 */
namespace Api.Controllers
{
    /// <summary>
    /// Vital controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VitalController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VitalController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VitalController(IUnitOfWork context, ILogger<VitalController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/vital
        /// </summary>
        /// <param name="vital">Vital object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVital)]
        public async Task<IActionResult> CreateVital(Vital vital)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Vital, vital.EncounterType);
                interaction.EncounterId = vital.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = vital.CreatedBy;
                interaction.CreatedIn = vital.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                if (vital.EncounterType == Enums.EncounterType.IntraTransfusionVital)
                {
                    var latestPreTransfusionVital = await context.VitalRepository.GetLatestVitalByClientAndEncounterType(vital.ClientId, Enums.EncounterType.PreTransfusionVital);
                    if (latestPreTransfusionVital == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.PreTransfusionVital);
                    }
                    else
                    {
                        if (vital.VitalsDate < latestPreTransfusionVital.VitalsDate)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.PreTransfusionVitalDateValidation);
                    }
                }
                if (vital.SystolicIfUnrecordable != 0)
                    vital.Systolic = -1;

                if (vital.DiastolicIfUnrecordable != 0)
                    vital.Diastolic = -1;

                if (vital.BMI == null)
                    vital.BMI = "-1";

                if (vital.MUACScore == null)
                    vital.MUACScore = "-1";

                vital.Oid = interactionId;
                vital.DateCreated = DateTime.Now;
                vital.IsDeleted = false;
                vital.IsSynced = false;

                context.VitalRepository.Add(vital);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVitalByKey", new { key = vital.Oid }, vital);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVital", "VitalController.cs", ex.Message, vital.CreatedIn, vital.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vitals/{clientId}/{pageNo}/{pageSize}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVitals)]
        public async Task<IActionResult> ReadVitals(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var vitalInDb = await context.VitalRepository.GetVitals(clientId);
                    vitalInDb = vitalInDb.OrderByDescending(x => x.DateCreated);

                    return Ok(vitalInDb);
                }
                else
                {
                    var vitalInDb = await context.VitalRepository.GetVitals(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<Vital> vitalDto = new PagedResultDto<Vital>()
                    {
                        Data = vitalInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.VitalRepository.GetVitalsTotalCount(clientId, encounterType)
                    };

                    return Ok(vitalDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVitals", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vital/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVitalByKey)]
        public async Task<IActionResult> ReadVitalByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vitalInDb = await context.VitalRepository.GetVitalByKey(key);

                if (vitalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vitalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVitalByKey", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLatestVitalByClient)]
        public async Task<IActionResult> ReadLatestVitalByClient(Guid clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vitalInDb = await context.VitalRepository.GetLatestVitalByClient(clientId);

                if (vitalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vitalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestVitalByClient", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadLatestVitalByClientAndEncounterType)]
        public async Task<IActionResult> ReadLatestVitalByClientAndEncounterType(Guid clientId, Enums.EncounterType encounterType)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vitalInDb = await context.VitalRepository.GetLatestVitalByClientAndEncounterType(clientId, encounterType);

                if (vitalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vitalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestVitalByClientAndEncounterType", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/vital/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVitalByClient)]
        public async Task<IActionResult> ReadVitalByClient(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vitalInDb = await context.VitalRepository.GetVitalsByClient(clientId);

                if (vitalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vitalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVitalByClient", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/vital/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <param name="vital">Vital to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVital)]
        public async Task<IActionResult> UpdateVital(Guid key, Vital vital)
        {
            try
            {
                if (key != vital.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (vital.EncounterType == Enums.EncounterType.IntraTransfusionVital)
                {
                    var latestPreTransfusionVital = await context.VitalRepository.GetLatestVitalByClientAndEncounterType(vital.ClientId, Enums.EncounterType.PreTransfusionVital);
                    if (latestPreTransfusionVital == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.PreTransfusionVital);
                    }
                    else
                    {
                        if (vital.VitalsDate < latestPreTransfusionVital.VitalsDate)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.PreTransfusionVitalDateValidation);

                    }
                }
                if (vital.SystolicIfUnrecordable != 0)
                    vital.Systolic = -1;

                if (vital.DiastolicIfUnrecordable != 0)
                    vital.Diastolic = -1;

                if (vital.BMI == null)
                    vital.BMI = "-1";

                if (vital.MUACScore == null)
                    vital.MUACScore = "-1";

                vital.DateModified = DateTime.Now;
                vital.IsDeleted = false;
                vital.IsSynced = false;

                context.VitalRepository.Update(vital);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVital", "VitalController.cs", ex.Message, vital.ModifiedIn, vital.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vital/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVital)]
        public async Task<IActionResult> DeleteVital(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vitalInDb = await context.VitalRepository.GetVitalByKey(key);

                if (vitalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                vitalInDb.DateModified = DateTime.Now;
                vitalInDb.IsDeleted = true;
                vitalInDb.IsSynced = false;

                context.VitalRepository.Update(vitalInDb);
                await context.SaveChangesAsync();

                return Ok(vitalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVital", "VitalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}