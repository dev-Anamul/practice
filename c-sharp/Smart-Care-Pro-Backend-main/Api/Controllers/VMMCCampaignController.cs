using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brain
 * Date created  : 08.04.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// VMMCCampaign Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VMMCCampaignController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VMMCCampaignController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VMMCCampaignController(IUnitOfWork context, ILogger<VMMCCampaignController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/vmmc-campaign
        /// </summary>
        /// <param name="vmmc-campaign">VMMCCampaign object</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVMMCCampaign)]
        public async Task<IActionResult> CreateVMMCCampaign(VMMCCampaign vMMCCampaign)
        {
            try
            {
                vMMCCampaign.DateCreated = DateTime.Now;
                vMMCCampaign.IsDeleted = false;
                vMMCCampaign.IsSynced = false;

                context.VMMCCampaignRepository.Add(vMMCCampaign);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVMMCCampaignByKey", new { key = vMMCCampaign.Oid }, vMMCCampaign);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVMMCCampaign", "VMMCCampaignController.cs", ex.Message, vMMCCampaign.CreatedIn, vMMCCampaign.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/vmmc-campaigns
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVMMCCampaigns)]
        public async Task<IActionResult> ReadVMMCCampaigns()
        {
            try
            {
                var vMMCCampaignInDb = await context.VMMCCampaignRepository.GetVMMCCampaigns();
                vMMCCampaignInDb = vMMCCampaignInDb.OrderByDescending(x => x.DateCreated);
                return Ok(vMMCCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVMMCCampaigns", "VMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/vmmc-campaign/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCCampaign.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVMMCCampaignByKey)]
        public async Task<IActionResult> ReadVMMCCampaignByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vMMCCampaignInDb = await context.VMMCCampaignRepository.GetVMMCCampaignByKey(key);

                if (vMMCCampaignInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vMMCCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVMMCCampaignByKey", "VMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-campaign/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCCampaign.</param>
        /// <param name="vmmc-campaign">VMMCCampaign to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVMMCCampaign)]
        public async Task<IActionResult> UpdateVMMCCampaign(int key, VMMCCampaign vMMCCampaign)
        {
            try
            {
                if (key != vMMCCampaign.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                vMMCCampaign.DateModified = DateTime.Now;
                vMMCCampaign.IsDeleted = false;
                vMMCCampaign.IsSynced = false;

                context.VMMCCampaignRepository.Update(vMMCCampaign);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVMMCCampaign", "VMMCCampaignController.cs", ex.Message, vMMCCampaign.ModifiedIn, vMMCCampaign.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/vmmc-campaign/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCCampaign.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVMMCCampaign)]
        public async Task<IActionResult> DeleteVMMCCampaign(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vMMCCampaignInDb = await context.VMMCCampaignRepository.GetVMMCCampaignByKey(key);

                if (vMMCCampaignInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                vMMCCampaignInDb.DateModified = DateTime.Now;
                vMMCCampaignInDb.IsDeleted = true;
                vMMCCampaignInDb.IsSynced = false;

                context.VMMCCampaignRepository.Update(vMMCCampaignInDb);
                await context.SaveChangesAsync();

                return Ok(vMMCCampaignInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVMMCCampaign", "VMMCCampaignController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}