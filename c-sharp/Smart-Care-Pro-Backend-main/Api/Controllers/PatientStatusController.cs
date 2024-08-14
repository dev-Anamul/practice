using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Patient Status controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PatientStatusController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PatientStatusController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PatientStatusController(IUnitOfWork context, ILogger<PatientStatusController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/patient-status
        /// </summary>
        /// <param name="patientStatus">PatientStatus object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePatientStatus)]
        public async Task<IActionResult> CreatePatientStatus(PatientStatus patientStatus)
        {
            try
            {
                patientStatus.StatusDate = DateTime.Now;

                patientStatus.DateCreated = DateTime.Now;
                patientStatus.IsDeleted = false;
                patientStatus.IsSynced = false;

                context.PatientStatusRepository.Add(patientStatus);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPatientStatusByKey", new { key = patientStatus.Oid }, patientStatus);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePatientStatus", "PatientStatusController.cs", ex.Message, patientStatus.CreatedIn, patientStatus.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/patient-statuses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPatientStatuses)]
        public async Task<IActionResult> ReadPatientStatuses(int page, int pageSize)
        {
            try
            {
                if (pageSize == 0)
                {
                    var patientStatusInDb = await context.PatientStatusRepository.PatientStatuses();

                    return Ok(patientStatusInDb);
                }
                else
                {
                    var patientStatusInDb = await context.PatientStatusRepository.PatientStatuses(((page - 1) * (pageSize)), pageSize);
                    foreach (var item in patientStatusInDb)
                    {
                        if (item.ReferringFacilityId != null)
                        {
                            var facility = await context.FacilityRepository.GetFacilityByKey(item.ReferringFacilityId.Value);
                            item.ReferringFacilityName = facility.Description;
                        }
                        if (item.ReferredFacilityId != null)
                        {
                            var facility = await context.FacilityRepository.GetFacilityByKey(item.ReferredFacilityId.Value);
                            item.ReferredFacilityName = facility.Description;
                        }
                    }
                    PagedResultDto<PatientStatus> patientStatusesDto = new PagedResultDto<PatientStatus>()
                    {
                        Data = patientStatusInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PatientStatusRepository.PatientStatusesTotalCount()
                    };
                    return Ok(patientStatusesDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientStatuses", "PatientStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadPatientStatusByArtRegisterId)]
        public async Task<IActionResult> ReadPatientStatusByClient(Guid artRegisterId, int page, int pageSize)
        {
            try
            {
                if (pageSize == 0)
                {
                    var patientStatusInDb = await context.PatientStatusRepository.GetPatientStatusbyArtRegisterId(artRegisterId);
                    return Ok(patientStatusInDb);
                }
                else
                {
                    var patientStatusInDb = await context.PatientStatusRepository.GetPatientStatusbyArtRegisterId(artRegisterId, ((page - 1) * (pageSize)), pageSize);

                    foreach (var item in patientStatusInDb)
                    {
                        if (item.ReferringFacilityId != null)
                        {
                            var facility = await context.FacilityRepository.GetFacilityByKey(item.ReferringFacilityId.Value);
                            item.ReferringFacilityName = facility.Description;
                        }
                        if (item.ReferredFacilityId != null)
                        {
                            var facility = await context.FacilityRepository.GetFacilityByKey(item.ReferredFacilityId.Value);
                            item.ReferredFacilityName = facility.Description;
                        }
                    }

                    PagedResultDto<PatientStatus> patientStatusesDto = new PagedResultDto<PatientStatus>()
                    {
                        Data = patientStatusInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PatientStatusRepository.GetPatientStatusbyArtRegisterIdTotalCount(artRegisterId)
                    };

                    return Ok(patientStatusesDto);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientStatusByClient", "PatientStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/patient-status/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PatientStatus.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPatientStatusByKey)]
        public async Task<IActionResult> ReadPatientStatusByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var patientStatusInDb = await context.PatientStatusRepository.GetPatientStatusByKey(key);

                if (patientStatusInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(patientStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPatientStatusByKey", "PatientStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/patient-status/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PatientStatus.</param>
        /// <param name="patientstatus">PatientStatus to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePatientStatus)]
        public async Task<IActionResult> UpdatePatientStatus(Guid key, PatientStatus patientStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                patientStatus.DateModified = DateTime.Now;
                patientStatus.IsDeleted = false;
                patientStatus.IsSynced = false;

                context.PatientStatusRepository.Update(patientStatus);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePatientStatus", "PatientStatusController.cs", ex.Message, patientStatus.ModifiedIn, patientStatus.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}