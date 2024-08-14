using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// QuickExamination controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class QuickExaminationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<QuickExaminationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public QuickExaminationController(IUnitOfWork context, ILogger<QuickExaminationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/quick-examination
        /// </summary>
        /// <param name="quickExamination">QuickExamination object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateQuickExamination)]
        public async Task<IActionResult> CreateQuickExamination(QuickExamination quickExamination)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.QuickExamination, quickExamination.EncounterType);
                interaction.EncounterId = quickExamination.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = quickExamination.CreatedBy;
                interaction.CreatedIn = quickExamination.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                quickExamination.InteractionId = interactionId;
                quickExamination.DateCreated = DateTime.Now;
                quickExamination.IsDeleted = false;
                quickExamination.IsSynced = false;

                context.QuickExaminationRepository.Add(quickExamination);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadQuickExaminationByKey", new { key = quickExamination.InteractionId }, quickExamination);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateQuickExamination", "QuickExaminationController.cs", ex.Message, quickExamination.CreatedIn, quickExamination.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/quick-examinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuickExaminations)]
        public async Task<IActionResult> ReadQuickExaminations()
        {
            try
            {
                var quickExaminationInDb = await context.QuickExaminationRepository.ReadQuickExaminations();

                return Ok(quickExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuickExaminations", "QuickExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/quick-examination/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuickExaminationByClient)]
        public async Task<IActionResult> ReadQuickExaminationByClient(Guid clientId)
        {
            try
            {
                var quickExaminationInDb = await context.QuickExaminationRepository.ReadQuickExaminationByClient(clientId);

                return Ok(quickExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuickExaminationByClient", "QuickExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/quick-examination/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuickExaminationByEncounter)]
        public async Task<IActionResult> ReadQuickExaminationByEncounter(Guid encounterId)
        {
            try
            {
                var quickExaminationInDb = await context.QuickExaminationRepository.ReadQuickExaminationByEncounter(encounterId);

                return Ok(quickExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuickExaminationByEncounter", "QuickExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/quick-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table QuickExamination.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuickExaminationByKey)]
        public async Task<IActionResult> ReadQuickExaminationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var quickExaminationInDb = await context.QuickExaminationRepository.ReadQuickExaminationByKey(key);

                if (quickExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(quickExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuickExaminationByKey", "QuickExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/quick-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table QuickExamination.</param>
        /// <param name="aNCService">QuickExamination to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateQuickExamination)]
        public async Task<IActionResult> UpdateQuickExamination(Guid key, QuickExamination quickExamination)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = quickExamination.ModifiedBy;
                interactionInDb.ModifiedIn = quickExamination.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != quickExamination.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                quickExamination.DateModified = DateTime.Now;
                quickExamination.IsDeleted = false;
                quickExamination.IsSynced = false;

                context.QuickExaminationRepository.Update(quickExamination);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateQuickExamination", "QuickExaminationController.cs", ex.Message, quickExamination.ModifiedIn, quickExamination.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/quick-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table QuickExamination.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteQuickExamination)]
        public async Task<IActionResult> DeleteQuickExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var quickExaminationInDb = await context.QuickExaminationRepository.ReadQuickExaminationByKey(key);

                if (quickExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                quickExaminationInDb.DateModified = DateTime.Now;
                quickExaminationInDb.IsDeleted = true;
                quickExaminationInDb.IsSynced = false;

                context.QuickExaminationRepository.Update(quickExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(quickExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteQuickExamination", "QuickExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}