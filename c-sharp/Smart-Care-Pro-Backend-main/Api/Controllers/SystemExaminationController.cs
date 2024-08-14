using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 02.01.2023
 * Modified by   : Bella
 * Last modified : 05.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SystemExaminationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SystemExaminationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SystemExaminationController(IUnitOfWork context, ILogger<SystemExaminationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region SystemExamination
        /// <summary>
        /// URL: sc-api/system-examination
        /// </summary>
        /// <param name="systemExamination">SystemExamination object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSystemExamination)]
        public async Task<IActionResult> CreateSystemExamination(SystemExaminationDto systemExamination)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();

                foreach (var item in systemExamination.SystemExaminations)
                {
                    Guid interactionId = Guid.NewGuid();

                    Interaction interaction = new Interaction();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.SystemExamination, systemExamination.EncounterType);
                    interaction.EncounterId = systemExamination.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = systemExamination.CreatedBy;
                    interaction.CreatedIn = systemExamination.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionId;
                    item.ClientId = systemExamination.ClientID;
                    item.EncounterId = systemExamination.EncounterId;
                    item.PhysicalSystemId = item.PhysicalSystemId;
                    item.Note = item.Note;
                    item.CreatedBy = systemExamination.CreatedBy;
                    item.CreatedIn = systemExamination.CreatedIn;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.EncounterType = systemExamination.EncounterType;
                }

                context.InteractionRepository.AddRange(interactions);
                context.SystemExaminationRepository.AddRange(systemExamination.SystemExaminations);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSystemExamination", "SystemExaminationController.cs", ex.Message, systemExamination.CreatedIn, systemExamination.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return CreatedAtAction("ReadSystemExaminationByKey", new { key = systemExamination.SystemExaminations.Select(x => x.InteractionId).FirstOrDefault() }, systemExamination);
        }

        /// <summary>
        /// URL: sc-api/system-examinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemExaminations)]
        public async Task<IActionResult> ReadSystemExaminations()
        {
            try
            {
                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminations();

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemExaminations", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemExaminationByClient)]
        public async Task<IActionResult> ReadSystemExaminationsByClient(Guid clientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationsByClientID(clientID);

                    return Ok(systemExaminationInDb);
                }
                else
                {
                    var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationsByClientID(clientID, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<SystemExamination> systemExaminationWithPaggingDto = new PagedResultDto<SystemExamination>()
                    {
                        Data = systemExaminationInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.SystemExaminationRepository.GetSystemExaminationsByClientIDTotalCount(clientID, encounterType)
                    };
                    return Ok(systemExaminationWithPaggingDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemExaminationsByClient", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemExaminationByKey)]
        public async Task<IActionResult> ReadSystemExaminationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemExaminationByKey", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination-by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemExaminationByOPDVisit)]
        public async Task<IActionResult> ReadSystemReviewByOPDVisit(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemReviewInDb = await context.SystemExaminationRepository.GetSystemReviewByEncounter(encounterId);

                if (systemReviewInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(systemReviewInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemReviewByOPDVisit", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <param name="systemExamination">SystemExamination to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSystemExamination)]
        public async Task<IActionResult> UpdateSystemExmination(Guid key, SystemExamination systemExamination)
        {
            try
            {
                if (key != systemExamination.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = systemExamination.ModifiedBy;
                interactionInDb.ModifiedIn = systemExamination.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);


                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                systemExaminationInDb.DateModified = DateTime.Now;
                systemExaminationInDb.IsDeleted = false;
                systemExaminationInDb.IsSynced = false;
                systemExaminationInDb.PhysicalSystemId = systemExamination.PhysicalSystemId;
                systemExaminationInDb.Note = systemExamination.Note;

                context.SystemExaminationRepository.Update(systemExaminationInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSystemExmination", "SystemExaminationController.cs", ex.Message, systemExamination.ModifiedIn, systemExamination.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystemExamination)]
        public async Task<IActionResult> DeleteSystemExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemExaminationInDb.DateModified = DateTime.Now;
                systemExaminationInDb.IsDeleted = true;
                systemExaminationInDb.IsSynced = false;

                context.SystemExaminationRepository.Update(systemExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-examination/remove/{key}
        /// </summary>
        /// <param name="EncounterID">Primary key of the table SystemExamination.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveARTSystemExamination)]
        public async Task<IActionResult> RemoveARTSystemExamination(Guid EncounterID)
        {
            try
            {
                if (EncounterID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemReviewByEncounter(EncounterID);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                //foreach (var item in systemExaminationInDb.Where(e => e.EncounterType == Enums.EncounterType.MedicalEncounter))
                //{
                //    context.SystemExaminationRepository.Delete(item);
                //    await context.SaveChangesAsync();
                //}

                foreach (var item in systemExaminationInDb.ToList())
                {
                    context.SystemExaminationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveARTSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PEPSystemExamination

        /// <summary>
        /// URL: sc-api/pep-system-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePEPSystemExamination)]
        public async Task<IActionResult> DeletePEPSystemExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemExaminationInDb.DateModified = DateTime.Now;
                systemExaminationInDb.IsDeleted = true;
                systemExaminationInDb.IsSynced = false;

                context.SystemExaminationRepository.Update(systemExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePEPSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-system-examination/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePEPSystemExamination)]
        public async Task<IActionResult> RemovePEPSystemExamination(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemReviewByEncounter(encounterId);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (systemExaminationInDb.Where(e => e.EncounterType == Enums.EncounterType.PEP).Count() == 0)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoPEPMatchFoundError);


                foreach (var item in systemExaminationInDb.Where(e => e.EncounterType == Enums.EncounterType.PEP).ToList())
                {
                    item.IsDeleted = true;
                    context.SystemExaminationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePEPSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PrEPSystemExamination

        /// <summary>
        /// URL: sc-api/prep-system-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePrEPSystemExamination)]
        public async Task<IActionResult> DeletePrEPSystemExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemExaminationInDb.DateModified = DateTime.Now;
                systemExaminationInDb.IsDeleted = true;
                systemExaminationInDb.IsSynced = false;

                context.SystemExaminationRepository.Update(systemExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePrEPSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-system-examination/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePrEPSystemExamination)]
        public async Task<IActionResult> RemovePrEPSystemExamination(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemReviewByEncounter(encounterId);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemExaminationInDb.Where(e => e.EncounterType == Enums.EncounterType.PrEP))
                {
                    context.SystemExaminationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePrEPSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Under 5
        /// <summary>
        /// URL: sc-api/under-five-system-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUnderFiveSystemExamination)]
        public async Task<IActionResult> DeleteUnderFiveSystemExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemExaminationByKey(key);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemExaminationInDb.DateModified = DateTime.Now;
                systemExaminationInDb.IsDeleted = true;
                systemExaminationInDb.IsSynced = false;

                context.SystemExaminationRepository.Update(systemExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUnderFiveSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/under-five-system-examination/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table SystemExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveUnderFiveSystemExamination)]
        public async Task<IActionResult> RemoveUnderFiveSystemExamination(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemExaminationInDb = await context.SystemExaminationRepository.GetSystemReviewByEncounter(encounterId);

                if (systemExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemExaminationInDb.Where(e => e.EncounterType == Enums.EncounterType.PrEP))
                {
                    context.SystemExaminationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveUnderFiveSystemExamination", "SystemExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}