using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 08.04.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Opted Circumcision Reason Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class OptedCircumcisionReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<OptedCircumcisionReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public OptedCircumcisionReasonController(IUnitOfWork context, ILogger<OptedCircumcisionReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reason
        /// </summary>
        /// <param name="optedCircumcisionReason">OptedCircumcisionReason object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateOptedCircumcisionReason)]
        public async Task<IActionResult> CreateOptedCircumcisionReason(OptedCircumcisionReason optedCircumcisionReason)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.EncounterId = optedCircumcisionReason.EncounterId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.OptedCircumcisionReason, optedCircumcisionReason.EncounterType);
                interaction.CreatedIn = optedCircumcisionReason.CreatedIn;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = optedCircumcisionReason.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                optedCircumcisionReason.InteractionId = interactionId;
                optedCircumcisionReason.IsSynced = false;
                optedCircumcisionReason.IsDeleted = false;
                optedCircumcisionReason.DateCreated = DateTime.Now;

                context.OptedCircumcisionReasonRepository.Add(optedCircumcisionReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadOptedCircumcisionReasonByKey", new { key = optedCircumcisionReason.InteractionId }, optedCircumcisionReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateOptedCircumcisionReason", "OptedCircumcisionReasonController.cs", ex.Message, optedCircumcisionReason.CreatedIn, optedCircumcisionReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedCircumcisionReasons)]
        public async Task<IActionResult> ReadOptedCircumcisionReasons()
        {
            try
            {
                var optedCircumcisionReasonInDb = await context.OptedCircumcisionReasonRepository.GetOptedCircumcisionReasons();

                return Ok(optedCircumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedCircumcisionReasons", "OptedCircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reason/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedCircumcisionReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedCircumcisionReasonByKey)]
        public async Task<IActionResult> ReadOptedCircumcisionReasonByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var optedCircumcisionReasonInDb = await context.OptedCircumcisionReasonRepository.GetOptedCircumcisionReasonByKey(key);

                if (optedCircumcisionReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(optedCircumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedCircumcisionReasonByKey", "OptedCircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reason/byCircumcisionReason/{CircumcisionReasonId}
        /// </summary>
        /// <param name="circumcisionReasonId">Foreign key of the table OptedCircumcisionReason primary key of VMMCCampaigns.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedCircumcisionReasonByCircumcisionReason)]
        public async Task<IActionResult> ReadOptedCircumcisionReasonByCircumcisionReason(int circumcisionReasonId)
        {
            try
            {
                var optedCircumcisionReasonInDb = await context.OptedCircumcisionReasonRepository.GetOptedCircumcisionReasonByCircumcisionReason(circumcisionReasonId);

                return Ok(optedCircumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedCircumcisionReasonByCircumcisionReason", "OptedCircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reason/byvmmc-service/{vmmcServiceId}"
        /// </summary>
        /// <param name="vmmcServiceId">Foreign key of the table OptedCircumcisionReason primary key of VMMCServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedCircumcisionReasonByVMMCService)]
        public async Task<IActionResult> ReadOptedCircumcisionReasonByVMMCService(Guid vmmcServiceId)
        {
            try
            {
                var optedCircumcisionReasonInDb = await context.OptedCircumcisionReasonRepository.GetCircumcisionReasonByVMMCService(vmmcServiceId);

                return Ok(optedCircumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedCircumcisionReasonByVMMCService", "OptedCircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-circumcision-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedCircumcisionReason.</param>
        /// <param name="optedCircumcisionReason">OptedCircumcisionReason to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateOptedCircumcisionReason)]
        public async Task<IActionResult> UpdateOptedCircumcisionReason(Guid key, OptedCircumcisionReason optedCircumcisionReason)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = optedCircumcisionReason.ModifiedBy;
                interactionInDb.ModifiedIn = optedCircumcisionReason.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != optedCircumcisionReason.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                optedCircumcisionReason.DateModified = DateTime.Now;
                optedCircumcisionReason.IsDeleted = false;
                optedCircumcisionReason.IsSynced = false;

                context.OptedCircumcisionReasonRepository.Update(optedCircumcisionReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateOptedCircumcisionReason", "OptedCircumcisionReasonController.cs", ex.Message, optedCircumcisionReason.ModifiedIn, optedCircumcisionReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedCircumcisionReason.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteOptedCircumcisionReason)]
        public async Task<IActionResult> DeleteOptedCircumcisionReason(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var optedCircumcisionReasonInDb = await context.OptedCircumcisionReasonRepository.GetOptedCircumcisionReasonByKey(key);

                if (optedCircumcisionReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.OptedCircumcisionReasonRepository.Update(optedCircumcisionReasonInDb);
                await context.SaveChangesAsync();

                return Ok(optedCircumcisionReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteOptedCircumcisionReason", "OptedCircumcisionReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}