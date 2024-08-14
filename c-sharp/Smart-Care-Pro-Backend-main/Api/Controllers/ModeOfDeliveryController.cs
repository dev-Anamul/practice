using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ModeOfDelivery Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ModeOfDeliveryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ModeOfDeliveryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ModeOfDeliveryController(IUnitOfWork context, ILogger<ModeOfDeliveryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/mode-of-delivery
        /// </summary>
        /// <param name="modeOfDelivery">ModeOfDelivery object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateModeOfDelivery)]
        public async Task<ActionResult<ModeOfDelivery>> CreateModeOfDelivery(ModeOfDelivery modeOfDelivery)
        {
            try
            {
                if (await IsModeOfDeliveryDuplicate(modeOfDelivery) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                modeOfDelivery.DateCreated = DateTime.Now;
                modeOfDelivery.IsDeleted = false;
                modeOfDelivery.IsSynced = false;

                context.ModeOfDeliveryRepository.Add(modeOfDelivery);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadModeOfDeliveryByKey", new { key = modeOfDelivery.Oid }, modeOfDelivery);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateModeOfDelivery", "ModeOfDeliveryController.cs", ex.Message, modeOfDelivery.CreatedIn, modeOfDelivery.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mode-of-deliveries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModeOfDeliveries)]
        public async Task<IActionResult> ReadModeOfDeliveries()
        {
            try
            {
                var modeOfDeliveryInDb = await context.ModeOfDeliveryRepository.GetModeOfDeliveries();

                return Ok(modeOfDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModeOfDeliveries", "ModeOfDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mode-of-delivery/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ModeOfDeliveries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModeOfDeliveryByKey)]
        public async Task<IActionResult> ReadModeOfDeliveryByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var modeOfDeliveryInDb = await context.ModeOfDeliveryRepository.GetModeOfDeliveryByKey(key);

                if (modeOfDeliveryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(modeOfDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModeOfDeliveryByKey", "ModeOfDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mode-of-delivery/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ModeOfDeliverys.</param>
        /// <param name="modeOfDelivery">ModeOfDelivery to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateModeOfDelivery)]
        public async Task<IActionResult> UpdateModeOfDelivery(int key, ModeOfDelivery modeOfDelivery)
        {
            try
            {
                if (key != modeOfDelivery.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                modeOfDelivery.DateModified = DateTime.Now;
                modeOfDelivery.IsDeleted = false;
                modeOfDelivery.IsSynced = false;

                context.ModeOfDeliveryRepository.Update(modeOfDelivery);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateModeOfDelivery", "ModeOfDeliveryController.cs", ex.Message, modeOfDelivery.ModifiedIn, modeOfDelivery.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mode-of-delivery/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ModeOfDeliverys.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteModeOfDelivery)]
        public async Task<IActionResult> DeleteModeOfDelivery(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var modeOfDeliveryInDb = await context.ModeOfDeliveryRepository.GetModeOfDeliveryByKey(key);

                if (modeOfDeliveryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                modeOfDeliveryInDb.DateModified = DateTime.Now;
                modeOfDeliveryInDb.IsDeleted = true;
                modeOfDeliveryInDb.IsSynced = false;

                context.ModeOfDeliveryRepository.Update(modeOfDeliveryInDb);
                await context.SaveChangesAsync();

                return Ok(modeOfDeliveryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteModeOfDelivery", "ModeOfDeliveryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ModeOfDelivery name is duplicate or not.
        /// </summary>
        /// <param name="modeofDelivery">ModeOfDelivery object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsModeOfDeliveryDuplicate(ModeOfDelivery modeofDelivery)
        {
            try
            {
                var modeOfDeliveryInDb = await context.ModeOfDeliveryRepository.GetModeOfDeliveryByName(modeofDelivery.Description);

                if (modeOfDeliveryInDb != null)
                    if (modeOfDeliveryInDb.Oid != modeofDelivery.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsModeOfDeliveryDuplicate", "ModeOfDeliveryController.cs", ex.Message);
                throw;
            }
        }
    }
}