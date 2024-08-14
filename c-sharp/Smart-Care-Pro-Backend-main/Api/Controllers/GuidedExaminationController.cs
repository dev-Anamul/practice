using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// GuidedExamination controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class GuidedExaminationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GuidedExaminationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GuidedExaminationController(IUnitOfWork context, ILogger<GuidedExaminationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/guided-examination
        /// </summary>
        /// <param name="guidedExamination">GuidedExamination object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGuidedExamination)]
        public async Task<IActionResult> CreateGuidedExamination(GuidedExamination guidedExamination)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.GuidedExamination, guidedExamination.EncounterType);
                interaction.EncounterId = guidedExamination.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = guidedExamination.CreatedBy;
                interaction.CreatedIn = guidedExamination.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                guidedExamination.InteractionId = interactionId;
                guidedExamination.DateCreated = DateTime.Now;
                guidedExamination.IsDeleted = false;
                guidedExamination.IsSynced = false;

                context.GuidedExaminationRepository.Add(guidedExamination);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadGuidedExaminationByKey", new { key = guidedExamination.InteractionId }, guidedExamination);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGuidedExamination", "GuidedExaminationController.cs", ex.Message, guidedExamination.CreatedIn, guidedExamination.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/guided-examinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGuidedExaminations)]
        public async Task<IActionResult> ReadGuidedExaminations()
        {
            try
            {
                var guidedExaminationInDb = await context.GuidedExaminationRepository.ReadGuidedExaminations();

                return Ok(guidedExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGuidedExaminations", "GuidedExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/guided-examination/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGuidedExaminationByClient)]
        public async Task<IActionResult> ReadGuidedExaminationByClient(Guid clientId)
        {
            try
            {
                var guidedExaminationInDb = await context.GuidedExaminationRepository.ReadGuidedExaminationByClient(clientId);

                return Ok(guidedExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGuidedExaminationByClient", "GuidedExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/guided-examination/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGuidedExaminationByEncounter)]
        public async Task<IActionResult> ReadGuidedExaminationByEncounter(Guid encounterId)
        {
            try
            {
                var guidedExaminationInDb = await context.GuidedExaminationRepository.ReadGuidedExaminationByEncounter(encounterId);

                return Ok(guidedExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGuidedExaminationByEncounter", "GuidedExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/guided-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GuidedExamination.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGuidedExaminationByKey)]
        public async Task<IActionResult> ReadGuidedExaminationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var guidedExaminationInDb = await context.GuidedExaminationRepository.ReadGuidedExaminationByKey(key);

                if (guidedExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(guidedExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGuidedExaminationByKey", "GuidedExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/guided-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GuidedExamination.</param>
        /// <param name="guidedExamination">GuidedExamination to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGuidedExamination)]
        public async Task<IActionResult> UpdateGuidedExamination(Guid key, GuidedExamination guidedExamination)
        {
            try
            {
                if (key != guidedExamination.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = guidedExamination.ModifiedBy;
                interactionInDb.ModifiedIn = guidedExamination.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                guidedExamination.DateModified = DateTime.Now;
                guidedExamination.IsDeleted = false;
                guidedExamination.IsSynced = false;

                context.GuidedExaminationRepository.Update(guidedExamination);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGuidedExamination", "GuidedExaminationController.cs", ex.Message, guidedExamination.ModifiedIn, guidedExamination.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/guided-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GuidedExamination.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGuidedExamination)]
        public async Task<IActionResult> DeleteGuidedExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var guidedExaminationInDb = await context.GuidedExaminationRepository.ReadGuidedExaminationByKey(key);

                if (guidedExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                guidedExaminationInDb.DateModified = DateTime.Now;
                guidedExaminationInDb.IsDeleted = true;
                guidedExaminationInDb.IsSynced = false;

                context.GuidedExaminationRepository.Update(guidedExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(guidedExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGuidedExamination", "GuidedExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}