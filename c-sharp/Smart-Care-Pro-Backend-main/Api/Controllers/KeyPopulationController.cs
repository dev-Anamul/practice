using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// KeyPopulation controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class KeyPopulationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<KeyPopulationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public KeyPopulationController(IUnitOfWork context, ILogger<KeyPopulationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/key-population
        /// </summary>
        /// <param name="keyPopulation">key Population object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateKeyPopulation)]
        public async Task<IActionResult> CreateKeyPopulation(KeyPopulation keyPopulation)
        {
            try
            {
                if (await IsKeyPopulationDuplicate(keyPopulation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                keyPopulation.DateCreated = DateTime.Now;
                keyPopulation.IsDeleted = false;
                keyPopulation.IsSynced = false;

                context.KeyPopulationRepository.Add(keyPopulation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadKeyPopulationByKey", new { key = keyPopulation.Oid }, keyPopulation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateKeyPopulation", "KeyPopulationController.cs", ex.Message, keyPopulation.CreatedIn, keyPopulation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-populations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulations)]
        public async Task<IActionResult> ReadKeyPopulations()
        {
            try
            {
                var keyPopulationInDb = await context.KeyPopulationRepository.GetKeyPopulations();

                return Ok(keyPopulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulations", "KeyPopulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulation.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadKeyPopulationByKey)]
        public async Task<IActionResult> ReadKeyPopulationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var keyPopulationInDb = await context.KeyPopulationRepository.GetKeyPopulationByKey(key);

                if (keyPopulationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(keyPopulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadKeyPopulationByKey", "KeyPopulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <param name="keyPopulation">keyPopulation to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateKeyPopulation)]
        public async Task<IActionResult> UpdateKeyPopulation(int key, KeyPopulation keyPopulation)
        {
            try
            {
                if (key != keyPopulation.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsKeyPopulationDuplicate(keyPopulation) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                keyPopulation.DateModified = DateTime.Now;
                keyPopulation.IsDeleted = false;
                keyPopulation.IsSynced = false;

                context.KeyPopulationRepository.Update(keyPopulation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateKeyPopulation", "KeyPopulationController.cs", ex.Message, keyPopulation.ModifiedIn, keyPopulation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key-population/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteKeyPopulation)]
        public async Task<IActionResult> DeleteKeyPopulation(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var keyPopulationInDb = await context.KeyPopulationRepository.GetKeyPopulationByKey(key);

                if (keyPopulationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                keyPopulationInDb.DateModified = DateTime.Now;
                keyPopulationInDb.IsDeleted = true;
                keyPopulationInDb.IsSynced = false;

                context.KeyPopulationRepository.Update(keyPopulationInDb);
                await context.SaveChangesAsync();

                return Ok(keyPopulationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteKeyPopulation", "KeyPopulationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the keyPopulation name is duplicate or not. 
        /// </summary>
        /// <param name="keyPopulation">keyPopulation object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsKeyPopulationDuplicate(KeyPopulation keyPopulation)
        {
            try
            {
                var keyPopulationInDb = await context.KeyPopulationRepository.GetKeyPopulationByName(keyPopulation.Description);

                if (keyPopulationInDb != null)
                    if (keyPopulationInDb.Oid != keyPopulation.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsKeyPopulationDuplicate", "KeyPopulationController.cs", ex.Message);
                throw;
            }
        }
    }
}