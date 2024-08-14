using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// identifiedPriorSensitization controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPriorSensitizationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPriorSensitizationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPriorSensitizationController(IUnitOfWork context, ILogger<IdentifiedPriorSensitizationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitization
        /// </summary>
        /// <param name="identifiedPriorSensitization">IdentifiedPriorSensitization object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPriorSensitization)]
        public async Task<IActionResult> CreateIdentifiedPriorSensitization(IdentifiedPriorSensitization identifiedPriorSensitization)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPriorSensitization, identifiedPriorSensitization.EncounterType);
                interaction.EncounterId = identifiedPriorSensitization.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPriorSensitization.CreatedBy;
                interaction.CreatedIn = identifiedPriorSensitization.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                identifiedPriorSensitization.InteractionId = interactionID;
                identifiedPriorSensitization.DateCreated = DateTime.Now;
                identifiedPriorSensitization.IsDeleted = false;
                identifiedPriorSensitization.IsSynced = false;

                context.IdentifiedPriorSensitizationRepository.Add(identifiedPriorSensitization);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPriorSensitizationByKey", new { key = identifiedPriorSensitization.InteractionId }, identifiedPriorSensitization);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPriorSensitization", "IdentifiedPriorSensitizationController.cs", ex.Message, identifiedPriorSensitization.CreatedIn, identifiedPriorSensitization.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitizations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPriorSensitizations)]
        public async Task<IActionResult> ReadIdentifiedPriorSensitizations()
        {
            try
            {
                var identifiedPriorSensitizationInDb = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizations();

                return Ok(identifiedPriorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPriorSensitizations", "IdentifiedPriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitization/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPriorSensitizationByEncounter)]
        public async Task<IActionResult> ReadIdentifiedPriorSensitizationByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedPriorSensitizationInDb = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByEncounter(encounterId);

                return Ok(identifiedPriorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPriorSensitizationByEncounter", "IdentifiedPriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitization/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPriorSensitizationByKey)]
        public async Task<IActionResult> ReadIdentifiedPriorSensitizationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPriorSensitizationInDb = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByKey(key);

                if (identifiedPriorSensitizationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPriorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPriorSensitizationByKey", "IdentifiedPriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitization/{BloodTransfusionID}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPriorSensitizationByBloodTransfusion)]
        public async Task<IActionResult> ReadIdentifiedPriorSensitizationByBloodTransfusion(Guid bloodTransfusionId)
        {
            try
            {
                if (bloodTransfusionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPriorSensitizationInDb = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByBloodTransfusion(bloodTransfusionId);

                if (identifiedPriorSensitizationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPriorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPriorSensitizationByBloodTransfusion", "IdentifiedPriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/identified-prior-sensitization/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
        /// <param name="identifiedPriorSensitization">IdentifiedPriorSensitization to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPriorSensitization)]
        public async Task<IActionResult> UpdateIdentifiedPriorSensitization(Guid key, IdentifiedPriorSensitization identifiedPriorSensitization)
        {
            try
            {
                if (key != identifiedPriorSensitization.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPriorSensitization.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPriorSensitization.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPriorSensitization.DateModified = DateTime.Now;
                identifiedPriorSensitization.IsDeleted = false;
                identifiedPriorSensitization.IsSynced = false;

                context.IdentifiedPriorSensitizationRepository.Update(identifiedPriorSensitization);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPriorSensitization", "IdentifiedPriorSensitizationController.cs", ex.Message, identifiedPriorSensitization.ModifiedIn, identifiedPriorSensitization.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-prior-sensitization/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPriorSensitization)]
        public async Task<IActionResult> DeleteIdentifiedPriorSensitization(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPriorSensitizationInDb = await context.IdentifiedPriorSensitizationRepository.GetIdentifiedPriorSensitizationByKey(key);

                if (identifiedPriorSensitizationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedPriorSensitizationInDb.DateModified = DateTime.Now;
                identifiedPriorSensitizationInDb.IsDeleted = true;
                identifiedPriorSensitizationInDb.IsSynced = false;

                context.IdentifiedPriorSensitizationRepository.Update(identifiedPriorSensitizationInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedPriorSensitizationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPriorSensitization", "IdentifiedPriorSensitizationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}