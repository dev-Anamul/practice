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
    /// IdentifiedPPHTreatment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedPPHTreatmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedPPHTreatmentController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedPPHTreatmentController(IUnitOfWork context, ILogger<IdentifiedPPHTreatmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatment
        /// </summary>
        /// <param name="identifiedPPHTreatment">IdentifiedPPHTreatment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedPPHTreatment)]
        public async Task<IActionResult> CreateIdentifiedPPHTreatment(IdentifiedPPHTreatment identifiedPPHTreatment)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedPPHTreatment, identifiedPPHTreatment.EncounterType);
                interaction.EncounterId = identifiedPPHTreatment.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedPPHTreatment.CreatedBy;
                interaction.CreatedIn = identifiedPPHTreatment.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedPPHTreatment.InteractionId = interactionId;
                identifiedPPHTreatment.DateCreated = DateTime.Now;
                identifiedPPHTreatment.IsDeleted = false;
                identifiedPPHTreatment.IsSynced = false;

                context.IdentifiedPPHTreatmentRepository.Add(identifiedPPHTreatment);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedPPHTreatmentByKey", new { key = identifiedPPHTreatment.InteractionId }, identifiedPPHTreatment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedPPHTreatment", "IdentifiedPPHTreatmentController.cs", ex.Message, identifiedPPHTreatment.CreatedIn, identifiedPPHTreatment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPPHTreatments)]
        public async Task<IActionResult> ReadIdentifiedPPHTreatments()
        {
            try
            {
                var identifiedPPHTreatmentInDb = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatments();

                return Ok(identifiedPPHTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPPHTreatments", "IdentifiedPPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPPHTreatmentByKey)]
        public async Task<IActionResult> ReadIdentifiedPPHTreatmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPPHTreatmentInDb = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByKey(key);

                if (identifiedPPHTreatmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedPPHTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPPHTreatmentByKey", "IdentifiedPPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatment/by-PPHTreatments/{PPHTreatmentsId}
        /// </summary>
        /// <param name="PPHTreatmentsId">Primary key of the table IdentifiedPPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedPPHTreatmentByPPHTreatments)]
        public async Task<IActionResult> ReadIdentifiedPPHTreatmentByPPHTreatments(Guid pphTreatmentsId)
        {
            try
            {
                var identifiedPPHTreatmentInDb = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByPPHTreatments(pphTreatmentsId);

                return Ok(identifiedPPHTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedPPHTreatmentByPPHTreatments", "IdentifiedPPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPPHTreatments.</param>
        /// <param name="identifiedPPHTreatment">IdentifiedPPHTreatment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedPPHTreatment)]
        public async Task<IActionResult> UpdateIdentifiedPPHTreatment(Guid key, IdentifiedPPHTreatment identifiedPPHTreatment)
        {
            try
            {
                if (key != identifiedPPHTreatment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedPPHTreatment.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedPPHTreatment.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedPPHTreatment.DateModified = DateTime.Now;
                identifiedPPHTreatment.IsDeleted = false;
                identifiedPPHTreatment.IsSynced = false;

                context.IdentifiedPPHTreatmentRepository.Update(identifiedPPHTreatment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedPPHTreatment", "IdentifiedPPHTreatmentController.cs", ex.Message, identifiedPPHTreatment.ModifiedIn, identifiedPPHTreatment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-pph-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedPPHTreatment)]
        public async Task<IActionResult> DeleteIdentifiedPPHTreatment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedPPHTreatmentInDb = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByKey(key);

                if (identifiedPPHTreatmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.IdentifiedPPHTreatmentRepository.Update(identifiedPPHTreatmentInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedPPHTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedPPHTreatment", "IdentifiedPPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}