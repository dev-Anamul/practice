using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 23.02.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ICPC2Description controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ICPC2DescriptionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ICPC2DescriptionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ICPC2DescriptionController(IUnitOfWork context, ILogger<ICPC2DescriptionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description
        /// </summary>
        /// <param name="icpc2">ICPC2Description object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateICPC2Description)]
        public async Task<IActionResult> CreateICPC2Description(ICPC2Description icpc2)
        {
            try
            {
                if (await IsICPC2DescriptionDuplicate(icpc2) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                icpc2.DateCreated = DateTime.Now;
                icpc2.IsDeleted = false;
                icpc2.IsSynced = false;

                context.ICPC2DescriptionRepository.Add(icpc2);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadICPC2DescriptionByKey", new { key = icpc2.Oid }, icpc2);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateICPC2Description", "ICPC2DescriptionController.cs", ex.Message, icpc2.CreatedIn, icpc2.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-descriptions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICPC2Descriptions)]
        public async Task<IActionResult> ReadICPC2Descriptions()
        {
            try
            {
                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2Descriptions();

                return Ok(icpc2DescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICPC2Descriptions", "ICPC2DescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICPC2Descriptions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICPC2DescriptionByKey)]
        public async Task<IActionResult> ReadICPC2DescriptionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2DescriptionByKey(key);

                if (icpc2DescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(icpc2DescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICPC2DescriptionByKey", "ICPC2DescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description/by-anatomic-axis/{anatomicAxisId}
        /// </summary>
        /// <param name="anatomicAxisId">Primary key of the table AnatomicAxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICPC2DescriptionByAnatomicAxis)]
        public async Task<IActionResult> ReadICPC2DescriptionByAnatomicAxis(int anatomicAxisId)
        {
            try
            {
                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2DescriptionByAnatomicAxis(anatomicAxisId);

                return Ok(icpc2DescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICPC2DescriptionByAnatomicAxis", "ICPC2DescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description/by-pathology-axis/{pathologyAxisId}
        /// </summary>
        /// <param name="pathologyAxisId">Primary key of the table PathologyAxis.</param>
        /// <param name="anatomicAxisId">Primary key of the table AnatomicAxis.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadICPC2DescriptionByPathologyAxis)]
        public async Task<IActionResult> ReadICPC2DescriptionByPathologyAxis(int pathologyAxisId, int anatomicAxisId)
        {
            try
            {
                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2DescriptionByPathologyAxis(pathologyAxisId, anatomicAxisId);

                return Ok(icpc2DescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadICPC2DescriptionByPathologyAxis", "ICPC2DescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICPC2Descriptions.</param>
        /// <param name="icpc2">ICPC2Description to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateICPC2Description)]
        public async Task<IActionResult> UpdateICPC2Description(int key, ICPC2Description icpc2)
        {
            try
            {
                if (key != icpc2.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsICPC2DescriptionDuplicate(icpc2) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                icpc2.DateModified = DateTime.Now;
                icpc2.IsDeleted = false;
                icpc2.IsSynced = false;

                context.ICPC2DescriptionRepository.Update(icpc2);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateICPC2Description", "ICPC2DescriptionController.cs", ex.Message, icpc2.ModifiedIn, icpc2.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ICPC2-description/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ICPC2Descriptions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteICPC2Description)]
        public async Task<IActionResult> DeleteICPC2Description(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2DescriptionByKey(key);

                if (icpc2DescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                icpc2DescriptionInDb.DateModified = DateTime.Now;
                icpc2DescriptionInDb.IsDeleted = true;
                icpc2DescriptionInDb.IsSynced = false;

                context.ICPC2DescriptionRepository.Update(icpc2DescriptionInDb);
                await context.SaveChangesAsync();

                return Ok(icpc2DescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteICPC2Description", "ICPC2DescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ICPC2 description is duplicate or not. 
        /// </summary>
        /// <param name="icpc2">ICPC2Description object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsICPC2DescriptionDuplicate(ICPC2Description icpc2)
        {
            try
            {
                var icpc2DescriptionInDb = await context.ICPC2DescriptionRepository.GetICPC2DescriptionByName(icpc2.Description);

                if (icpc2DescriptionInDb != null)
                    if (icpc2DescriptionInDb.Oid != icpc2.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsICPC2DescriptionDuplicate", "ICPC2DescriptionController.cs", ex.Message);
                throw;
            }
        }
    }
}