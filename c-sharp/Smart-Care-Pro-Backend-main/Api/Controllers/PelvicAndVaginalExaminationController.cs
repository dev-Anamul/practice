using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PelvicAndVaginalExamination controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PelvicAndVaginalExaminationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PelvicAndVaginalExaminationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PelvicAndVaginalExaminationController(IUnitOfWork context, ILogger<PelvicAndVaginalExaminationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination
        /// </summary>
        /// <param name="pelvicAndVaginalExamination">PelvicAndVaginalExamination object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePelvicAndVaginalExamination)]
        public async Task<IActionResult> CreatePelvicAndVaginalExamination(PelvicAndVaginalExamination pelvicAndVaginalExamination)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PelvicAndVaginalExamination, pelvicAndVaginalExamination.EncounterType);
                interaction.EncounterId = pelvicAndVaginalExamination.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = pelvicAndVaginalExamination.CreatedBy;
                interaction.CreatedIn = pelvicAndVaginalExamination.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                pelvicAndVaginalExamination.InteractionId = interactionId;

                pelvicAndVaginalExamination.DateCreated = DateTime.Now;
                pelvicAndVaginalExamination.IsDeleted = false;
                pelvicAndVaginalExamination.IsSynced = false;

                context.PelvicAndVaginalExaminationRepository.Add(pelvicAndVaginalExamination);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPelvicAndVaginalExaminationByKey", new { key = pelvicAndVaginalExamination.InteractionId }, pelvicAndVaginalExamination);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePelvicAndVaginalExamination", "PelvicAndVaginalExaminationController.cs", ex.Message, pelvicAndVaginalExamination.CreatedIn, pelvicAndVaginalExamination.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPelvicAndVaginalExaminations)]
        public async Task<IActionResult> ReadPelvicAndVaginalExaminations()
        {
            try
            {
                var pelvicAndVaginalExaminationInDb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminations();

                return Ok(pelvicAndVaginalExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPelvicAndVaginalExaminations", "PelvicAndVaginalExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPelvicAndVaginalExaminationByKey)]
        public async Task<IActionResult> ReadPelvicAndVaginalExaminationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pelvicAndVaginalExaminationIndb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByKey(key);

                if (pelvicAndVaginalExaminationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pelvicAndVaginalExaminationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPelvicAndVaginalExaminationByKey", "PelvicAndVaginalExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPelvicAndVaginalExaminationByClient)]
        public async Task<IActionResult> ReadPelvicAndVaginalExaminationByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {

                    var pelvicAndVaginalExaminationInDb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByClient(clientId);

                    return Ok(pelvicAndVaginalExaminationInDb);
                }
                else
                {
                    var pelvicAndVaginalExaminationInDb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<PelvicAndVaginalExamination> pelvicAndVaginalExaminationDto = new PagedResultDto<PelvicAndVaginalExamination>()
                    {
                        Data = pelvicAndVaginalExaminationInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(pelvicAndVaginalExaminationDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPelvicAndVaginalExaminationByClient", "PelvicAndVaginalExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination/byencounter/{encounterid}
        /// </summary>
        /// <param name="EncounterId">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPelvicAndVaginalExaminationByEncounter)]
        public async Task<IActionResult> ReadPelvicAndVaginalExaminationByEncounter(Guid encounterId)
        {
            try
            {
                var pelvicAndVaginalExaminationInDb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByEncounter(encounterId);

                return Ok(pelvicAndVaginalExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPelvicAndVaginalExaminationByEncounter", "PelvicAndVaginalExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <param name="pelvicAndVaginalExamination">PelvicAndVaginalExamination to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePelvicAndVaginalExamination)]
        public async Task<IActionResult> UpdatePelvicAndVaginalExamination(Guid key, PelvicAndVaginalExamination pelvicAndVaginalExamination)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pelvicAndVaginalExamination.ModifiedBy;
                interactionInDb.ModifiedIn = pelvicAndVaginalExamination.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pelvicAndVaginalExamination.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                pelvicAndVaginalExamination.DateModified = DateTime.Now;
                pelvicAndVaginalExamination.IsDeleted = false;
                pelvicAndVaginalExamination.IsSynced = false;

                context.PelvicAndVaginalExaminationRepository.Update(pelvicAndVaginalExamination);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePelvicAndVaginalExamination", "PelvicAndVaginalExaminationController.cs", ex.Message, pelvicAndVaginalExamination.ModifiedIn, pelvicAndVaginalExamination.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pelvic-and-vaginal-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeletePelvicAndVaginalExamination)]
        public async Task<IActionResult> DeletePelvicAndVaginalExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pelvicAndVaginalExaminationInDb = await context.PelvicAndVaginalExaminationRepository.GetPelvicAndVaginalExaminationByKey(key);

                if (pelvicAndVaginalExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pelvicAndVaginalExaminationInDb.DateModified = DateTime.Now;
                pelvicAndVaginalExaminationInDb.IsDeleted = true;
                pelvicAndVaginalExaminationInDb.IsSynced = false;

                context.PelvicAndVaginalExaminationRepository.Update(pelvicAndVaginalExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(pelvicAndVaginalExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePelvicAndVaginalExamination", "PelvicAndVaginalExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}