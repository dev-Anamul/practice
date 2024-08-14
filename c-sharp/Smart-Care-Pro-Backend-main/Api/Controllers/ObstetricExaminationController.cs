using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ObstetricExamination controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ObstetricExaminationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ObstetricExaminationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ObstetricExaminationController(IUnitOfWork context, ILogger<ObstetricExaminationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/obstetric-examination
        /// </summary>
        /// <param name="obstetricExamination">ObstetricExamination object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateObstetricExamination)]
        public async Task<IActionResult> CreateObstetricExamination(ObstetricExamination obstetricExamination)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ObstetricExamination, obstetricExamination.EncounterType);
                interaction.EncounterId = obstetricExamination.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = obstetricExamination.CreatedBy;
                interaction.CreatedIn = obstetricExamination.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                obstetricExamination.InteractionId = interactionId;
                obstetricExamination.DateCreated = DateTime.Now;
                obstetricExamination.IsDeleted = false;
                obstetricExamination.IsSynced = false;

                context.ObstetricExaminationRepository.Add(obstetricExamination);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadObstetricExaminationByKey", new { key = obstetricExamination.InteractionId }, obstetricExamination);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateObstetricExamination", "ObstetricExaminationController.cs", ex.Message, obstetricExamination.CreatedIn, obstetricExamination.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/obstetric-examinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadObstetricExaminations)]
        public async Task<IActionResult> ReadObstetricExaminations()
        {
            try
            {
                var obstetricExaminationInDb = await context.ObstetricExaminationRepository.GetObstetricExaminations();

                return Ok(obstetricExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadObstetricExaminations", "ObstetricExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/obstetric-examination/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadObstetricExaminationByClient)]
        public async Task<IActionResult> ReadObstetricExaminationByClient(Guid clientId)
        {
            try
            {
                var obstetricExaminationInDb = await context.ObstetricExaminationRepository.GetObstetricExaminationByClient(clientId);

                return Ok(obstetricExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadObstetricExaminationByClient", "ObstetricExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/obstetric-examination/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadObstetricExaminationByEncounter)]
        public async Task<IActionResult> ReadObstetricExaminationByEncounter(Guid encounterId)
        {
            try
            {
                var obstetricExaminationInDb = await context.ObstetricExaminationRepository.GetObstetricExaminationByEncounter(encounterId);

                return Ok(obstetricExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadObstetricExaminationByEncounter", "ObstetricExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/obstetric-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ObstetricExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadObstetricExaminationByKey)]
        public async Task<IActionResult> ReadObstetricExaminationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var obstetricExaminationInDb = await context.ObstetricExaminationRepository.GetObstetricExaminationByKey(key);

                if (obstetricExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(obstetricExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadObstetricExaminationByKey", "ObstetricExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/obstetric-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ObstetricExaminations.</param>
        /// <param name="obstetricExamination">ObstetricExamination to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateObstetricExamination)]
        public async Task<IActionResult> UpdateObstetricExamination(Guid key, ObstetricExamination obstetricExamination)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = obstetricExamination.ModifiedBy;
                interactionInDb.ModifiedIn = obstetricExamination.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != obstetricExamination.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                obstetricExamination.DateModified = DateTime.Now;
                obstetricExamination.IsDeleted = false;
                obstetricExamination.IsSynced = false;

                context.ObstetricExaminationRepository.Update(obstetricExamination);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateObstetricExamination", "ObstetricExaminationController.cs", ex.Message, obstetricExamination.ModifiedIn, obstetricExamination.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/obstetric-examination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ObstetricExaminations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteObstetricExamination)]
        public async Task<IActionResult> DeleteObstetricExamination(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var obstetricExaminationInDb = await context.ObstetricExaminationRepository.GetObstetricExaminationByKey(key);

                if (obstetricExaminationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                obstetricExaminationInDb.DateModified = DateTime.Now;
                obstetricExaminationInDb.IsDeleted = true;
                obstetricExaminationInDb.IsSynced = false;

                context.ObstetricExaminationRepository.Update(obstetricExaminationInDb);
                await context.SaveChangesAsync();

                return Ok(obstetricExaminationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteObstetricExamination", "ObstetricExaminationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
