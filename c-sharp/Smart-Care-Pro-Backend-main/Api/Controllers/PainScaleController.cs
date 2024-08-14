using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 07-02-2023
 * Modified by  : Brian
 * Last modified: 01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PainScaleController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PainScaleController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PainScaleController(IUnitOfWork context, ILogger<PainScaleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pain-scale
        /// </summary>
        /// <param name="painScale">PainScale object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePainScale)]
        public async Task<IActionResult> CreatePainScale(PainScale painScale)
        {
            try
            {
                if (await IsPainScaleDuplicate(painScale) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                painScale.DateCreated = DateTime.Now;
                painScale.IsDeleted = false;
                painScale.IsSynced = false;

                context.PainScaleRepository.Add(painScale);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPainScaleByKey", new { key = painScale.Oid }, painScale);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePainScale", "PainScaleController.cs", ex.Message, painScale.CreatedIn, painScale.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-scales
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainScales)]
        public async Task<IActionResult> ReadPainScales()
        {
            try
            {
                var painScaleInDb = await context.PainScaleRepository.GetPainScales();

                return Ok(painScaleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainScales", "PainScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-scale/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainScale.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPainScaleByKey)]
        public async Task<IActionResult> ReadPainScaleByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var painScaleInDb = await context.PainScaleRepository.GetPainScaleByKey(key);

                if (painScaleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(painScaleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPainScaleByKey", "PainScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-scale/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainScales.</param>
        /// <param name="question">PainScale to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePainScale)]
        public async Task<IActionResult> UpdatePainScale(int key, PainScale painScale)
        {
            try
            {
                if (key != painScale.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                painScale.DateModified = DateTime.Now;
                painScale.IsDeleted = false;
                painScale.IsSynced = false;

                context.PainScaleRepository.Update(painScale);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePainScale", "PainScaleController.cs", ex.Message, painScale.ModifiedIn, painScale.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pain-scale/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PainScales.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePainScale)]
        public async Task<IActionResult> DeletePainScale(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var painScaleInDb = await context.PainScaleRepository.GetPainScaleByKey(key);

                if (painScaleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                painScaleInDb.DateModified = DateTime.Now;
                painScaleInDb.IsDeleted = true;
                painScaleInDb.IsSynced = false;

                context.PainScaleRepository.Update(painScaleInDb);
                await context.SaveChangesAsync();

                return Ok(painScaleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePainScale", "PainScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the PainScale name is duplicate or not.
        /// </summary>
        /// <param name="PainScale">PainScale object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPainScaleDuplicate(PainScale painScale)
        {
            try
            {
                var painScaleInDb = await context.PainScaleRepository.GetPainScaleByName(painScale.Description);

                if (painScaleInDb != null)
                    if (painScaleInDb.Oid != painScale.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPainScaleDuplicate", "PainScaleController.cs", ex.Message);
                throw;
            }
        }
    }
}