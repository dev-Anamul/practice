using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    /// Opted VMMC Campaign Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class OptedVMMCCampaignController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<OptedVMMCCampaignController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public OptedVMMCCampaignController(IUnitOfWork context, ILogger<OptedVMMCCampaignController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign
        /// </summary>
        /// <param name="optedVmmcCampaign">OptedVmmcCampaign object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateOptedVMMCCampaign)]
        public async Task<IActionResult> CreateOptedVMMCCampaign(OptedVMMCCampaign optedVmmcCampaign)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.EncounterId = optedVmmcCampaign.EncounterId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.OptedVMMCCampaign, optedVmmcCampaign.EncounterType);
                interaction.CreatedIn = optedVmmcCampaign.CreatedIn;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = Guid.Empty;
                interaction.IsDeleted = optedVmmcCampaign.IsDeleted;
                interaction.IsSynced = optedVmmcCampaign.IsSynced;

                context.InteractionRepository.Add(interaction);

                optedVmmcCampaign.InteractionId = interactionId;
                optedVmmcCampaign.IsDeleted = false;
                optedVmmcCampaign.IsSynced = false;
                optedVmmcCampaign.DateCreated = DateTime.Now;

                context.OptedVMMCCampaignRepository.Add(optedVmmcCampaign);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadOptedVMMCCampaignByKey", new { key = optedVmmcCampaign.InteractionId }, optedVmmcCampaign);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateOptedVMMCCampaign", "OptedVMMCCampaignController.cs", ex.Message, optedVmmcCampaign.CreatedIn, optedVmmcCampaign.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaigns
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedVMMCCampaigns)]
        public async Task<IActionResult> ReadOptedVMMCCampaigns()
        {
            try
            {
                var optedVmmcCampaignInDb = await context.OptedVMMCCampaignRepository.GetOptedVMMCCampaigns();

                return Ok(optedVmmcCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedVMMCCampaigns", "OptedVMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedVMMCCampaignByKey)]
        public async Task<IActionResult> ReadOptedVMMCCampaignByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var optedVmmcCampaignInDb = await context.OptedVMMCCampaignRepository.GetOptedVMMCCampaignByKey(key);

                if (optedVmmcCampaignInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(optedVmmcCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedVMMCCampaignByKey", "OptedVMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/byVMMCCampaign/{VMMCCampaignId}
        /// </summary>
        /// <param name="vmmcCampaignId">Foreign key of the table OptedVMMCCampaign primary key of VMMCCampaigns.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedVMMCCampaignByVMMCCampaign)]
        public async Task<IActionResult> ReadOptedVMMCCampaignByVMMCCampaign(int vmmcCampaignId)
        {
            try
            {
                var optedVmmcCampaignInDb = await context.OptedVMMCCampaignRepository.GetOptedVMMCCampaignByVMMCCampaignId(vmmcCampaignId);

                return Ok(optedVmmcCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedVMMCCampaignByVMMCCampaign", "OptedVMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/byVMMCService/{VMMCServiceId}
        /// </summary>
        /// <param name="vmmcServiceId">Foreign key of the table OptedVMMCCampaign primary key of VMMCServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadOptedVMMCCampaignByVMMCService)]
        public async Task<IActionResult> ReadOptedVMMCCampaignByVMMCService(Guid vmmcServiceId)
        {
            try
            {
                var optedVmmcCampaignInDb = await context.OptedVMMCCampaignRepository.GetVMMCCampaignByVMMCService(vmmcServiceId);

                return Ok(optedVmmcCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadOptedVMMCCampaignByVMMCService", "OptedVMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedVMMCCampaign.</param>
        /// <param name="optedVmmcCampaign">OptedVMMCCampaign to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateOptedVMMCCampaign)]
        public async Task<IActionResult> UpdateOptedVMMCCampaign(Guid key, OptedVMMCCampaign optedVmmcCampaign)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = optedVmmcCampaign.ModifiedBy;
                interactionInDb.ModifiedIn = optedVmmcCampaign.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != optedVmmcCampaign.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                optedVmmcCampaign.DateModified = DateTime.Now;
                optedVmmcCampaign.IsDeleted = false;
                optedVmmcCampaign.IsSynced = false;

                context.OptedVMMCCampaignRepository.Update(optedVmmcCampaign);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateOptedVMMCCampaign", "OptedVMMCCampaignController.cs", ex.Message, optedVmmcCampaign.ModifiedIn, optedVmmcCampaign.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/opted-vmmc-campaign/{key}
        /// </summary>
        /// <param name="key">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteOptedVMMCCampaign)]
        public async Task<IActionResult> DeleteOptedVmmcCampaign(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var optedVmmcCampaignInDb = await context.OptedVMMCCampaignRepository.GetOptedVMMCCampaignByKey(key);

                if (optedVmmcCampaignInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                optedVmmcCampaignInDb.IsDeleted = false;

                context.OptedVMMCCampaignRepository.Update(optedVmmcCampaignInDb);
                await context.SaveChangesAsync();

                return Ok(optedVmmcCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteOptedVmmcCampaign", "OptedVMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}