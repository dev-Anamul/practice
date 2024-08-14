using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 08.04.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedReferralReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedReferralReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedReferralReasonController(IUnitOfWork context, ILogger<IdentifiedReferralReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-referral
        /// </summary>
        /// <param name="identifiedReferralReason">IdentifiedReferralReason object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedReferral)]
        public async Task<IActionResult> CreateIdentifiedReferral(IdentifiedReferralReason identifiedReferralReason)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedReferralReason, identifiedReferralReason.EncounterType);
                interaction.EncounterId = identifiedReferralReason.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = identifiedReferralReason.CreatedBy;
                interaction.CreatedIn = identifiedReferralReason.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                identifiedReferralReason.InteractionId = interactionId;
                identifiedReferralReason.EncounterId = identifiedReferralReason.EncounterId;
                identifiedReferralReason.DateCreated = DateTime.Now;
                identifiedReferralReason.IsDeleted = false;
                identifiedReferralReason.IsSynced = false;

                context.IdentifiedReferralReasonRepository.Add(identifiedReferralReason);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIdentifiedReferralByKey", new { key = identifiedReferralReason.InteractionId }, identifiedReferralReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedReferral", "IdentifiedReferralReasonController.cs", ex.Message, identifiedReferralReason.CreatedIn, identifiedReferralReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-referrals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReferrals)]
        public async Task<IActionResult> ReadIdentifiedReferrals()
        {
            try
            {
                var identifiedReferralInDb = await context.IdentifiedReferralReasonRepository.GetIdentifiedReferralReasons();

                return Ok(identifiedReferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReferrals", "IdentifiedReferralReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/identified-referral-by-encounterId{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounters.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReferralByEncounter)]
        public async Task<IActionResult> ReadIdentifiedReferralByEncounter(Guid encounterId)
        {
            try
            {
                var identifiedReferralInDb = await context.IdentifiedReferralReasonRepository.GetIdentifiedReferralReasonByEncounter(encounterId);

                return Ok(identifiedReferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReferralByEncounter", "IdentifiedReferralReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-referral/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReferralByKey)]
        public async Task<IActionResult> ReadIdentifiedReferralByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedReferralInDb = await context.IdentifiedReferralReasonRepository.GetIdentifiedReferralReasonByKey(key);

                if (identifiedReferralInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedReferralInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReferralByKey", "IdentifiedReferralReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/identified-referral/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="identifiedReferralReason">ReferralModule to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedReferral)]
        public async Task<IActionResult> UpdateIdentifiedReferral(Guid key, IdentifiedReferralReason identifiedReferralReason)
        {
            try
            {
                if (key != identifiedReferralReason.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedReferralReason.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedReferralReason.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedReferralReason.DateModified = DateTime.Now;
                identifiedReferralReason.IsDeleted = false;
                identifiedReferralReason.IsSynced = false;

                context.IdentifiedReferralReasonRepository.Update(identifiedReferralReason);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedReferral", "IdentifiedReferralReasonController.cs", ex.Message, identifiedReferralReason.ModifiedIn, identifiedReferralReason.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-referral/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedReferral)]
        public async Task<IActionResult> DeleteIdentifiedReferral(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedReferralReasonInDb = await context.IdentifiedReferralReasonRepository.GetIdentifiedReferralReasonByKey(key);

                if (identifiedReferralReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedReferralReasonInDb.DateModified = DateTime.Now;
                identifiedReferralReasonInDb.IsDeleted = true;
                identifiedReferralReasonInDb.IsSynced = false;

                context.IdentifiedReferralReasonRepository.Update(identifiedReferralReasonInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedReferralReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedReferral", "IdentifiedReferralReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}