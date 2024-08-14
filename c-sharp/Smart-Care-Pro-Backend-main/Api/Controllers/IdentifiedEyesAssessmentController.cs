using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedEyesAssessment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedEyesAssessmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedEyesAssessmentController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedEyesAssessmentController(IUnitOfWork context, ILogger<IdentifiedEyesAssessmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment
        /// </summary>
        /// <param name="identifiedEyesAssessment">IdentifiedEyesAssessment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedEyesAssessment)]
        public async Task<IActionResult> CreateIdentifiedEyesAssessment(IdentifiedEyesAssessment identifiedEyesAssessment)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedEyesAssessment, identifiedEyesAssessment.EncounterType);
                interaction.EncounterId = identifiedEyesAssessment.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedEyesAssessment.CreatedBy;
                interaction.CreatedIn = identifiedEyesAssessment.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedEyesAssessment.InteractionId = interactionId;
                identifiedEyesAssessment.DateCreated = DateTime.Now;
                identifiedEyesAssessment.IsDeleted = false;
                identifiedEyesAssessment.IsSynced = false;

                context.IdentifiedEyesAssessmentRepository.Add(identifiedEyesAssessment);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedEyesAssessmentByKey", new { key = identifiedEyesAssessment.InteractionId }, identifiedEyesAssessment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedEyesAssessment", "IdentifiedEyesAssessmentController.cs", ex.Message, identifiedEyesAssessment.CreatedIn, identifiedEyesAssessment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedEyesAssessments)]
        public async Task<IActionResult> ReadIdentifiedEyesAssessments()
        {
            try
            {
                var identifiedEyesAssessmentInDb = await context.IdentifiedEyesAssessmentRepository.GetIdentifiedEyesAssessments();

                return Ok(identifiedEyesAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedEyesAssessments", "IdentifiedEyesAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedEyesAssessmentByKey)]
        public async Task<IActionResult> ReadIdentifiedEyesAssessmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedEyesAssessmentInDb = await context.IdentifiedEyesAssessmentRepository.GetIdentifiedEyesAssessmentByKey(key);

                if (identifiedEyesAssessmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedEyesAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedEyesAssessmentByKey", "IdentifiedEyesAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment/byEncounter/{EncounterID}
        /// </summary>
        /// <param name="encounterId">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedEyesAssessmentByEncounter)]
        public async Task<IActionResult> ReadIdentifiedEyesAssessmentByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedEyesAssessmentInDb = await context.IdentifiedEyesAssessmentRepository.GetIdentifiedEyesAssessmentByEncounter(encounterId);

                return Ok(identifiedEyesAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedEyesAssessmentByEncounter", "IdentifiedEyesAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment/byAssessment/{AssessmentID}
        /// </summary>
        /// <param name="assessmentId">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedEyesAssessmentByAssessment)]
        public async Task<IActionResult> ReadIdentifiedEyesAssessmentByAssessment(Guid assessmentId)
        {
            try
            {
                var identifiedEyesAssessmentInDb = await context.IdentifiedEyesAssessmentRepository.ReadIdentifiedEyesAssessmentByAssessment(assessmentId);

                return Ok(identifiedEyesAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedEyesAssessmentByAssessment", "IdentifiedEyesAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <param name="identifiedEyesAssessment">IdentifiedEyesAssessment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedEyesAssessment)]
        public async Task<IActionResult> UpdateIdentifiedEyesAssessment(Guid key, IdentifiedEyesAssessment identifiedEyesAssessment)
        {
            try
            {
                if (key != identifiedEyesAssessment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedEyesAssessment.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedEyesAssessment.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedEyesAssessment.DateModified = DateTime.Now;
                identifiedEyesAssessment.IsDeleted = false;
                identifiedEyesAssessment.IsSynced = false;

                context.IdentifiedEyesAssessmentRepository.Update(identifiedEyesAssessment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedEyesAssessment", "IdentifiedEyesAssessmentController.cs", ex.Message, identifiedEyesAssessment.ModifiedIn, identifiedEyesAssessment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-eyes-assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedEyesAssessment)]
        public async Task<IActionResult> DeleteIdentifiedEyesAssessment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedEyesAssessmentInDb = await context.IdentifiedEyesAssessmentRepository.GetIdentifiedEyesAssessmentByKey(key);

                if (identifiedEyesAssessmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedEyesAssessmentInDb.IsDeleted = false;

                context.IdentifiedEyesAssessmentRepository.Update(identifiedEyesAssessmentInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedEyesAssessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedEyesAssessment", "IdentifiedEyesAssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}