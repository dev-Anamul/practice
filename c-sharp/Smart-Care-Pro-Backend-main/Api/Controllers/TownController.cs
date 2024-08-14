using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by: Brian
 * Date created: 12.09.2022
 * Modified by: Brian
 * Last modified: 06.11.2022
 */

namespace Api.Controllers
{
    /// <summary>
    /// Town controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TownController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TownController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TownController(IUnitOfWork context, ILogger<TownController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/town
        /// </summary>
        /// <param name="town">Town object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTown)]
        public async Task<IActionResult> CreateTown(Town town)
        {
            try
            {
                if (await IsTownDuplicate(town) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                town.DateCreated = DateTime.Now;
                town.IsDeleted = false;
                town.IsSynced = false;

                context.TownRepository.Add(town);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTownByKey", new { key = town.Oid }, town);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTown", "TownController.cs", ex.Message, town.CreatedIn, town.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/towns
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTowns)]
        public async Task<IActionResult> ReadTowns()
        {
            try
            {
                var town = await context.TownRepository.GetTowns();

                return Ok(town);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTowns", "TownController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/town/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Towns.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTownByKey)]
        public async Task<IActionResult> ReadTownByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var town = await context.TownRepository.GetTownByKey(key);

                if (town == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(town);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTownByKey", "TownController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/town/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Towns.</param>
        /// <param name="town">Town to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTown)]
        public async Task<IActionResult> UpdateTown(int key, Town town)
        {
            try
            {
                if (key != town.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsTownDuplicate(town) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                town.DateModified = DateTime.Now;
                town.IsDeleted = false;
                town.IsSynced = false;

                context.TownRepository.Update(town);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTown", "TownController.cs", ex.Message, town.ModifiedIn, town.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/town/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Towns.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTown)]
        public async Task<IActionResult> DeleteTown(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var townInDb = context.TownRepository.Get(key);

                if (townInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                townInDb.DateModified = DateTime.Now;
                townInDb.IsDeleted = true;
                townInDb.IsSynced = false;

                context.TownRepository.Update(townInDb);
                await context.SaveChangesAsync();

                return Ok(townInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTown", "TownController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the town name is duplicate or not.
        /// </summary>
        /// <param name="town">Town object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTownDuplicate(Town town)
        {
            try
            {
                var townInDb = await context.TownRepository.GetTownByName(town.Description);

                if (townInDb != null)
                    if (townInDb.Oid != town.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsTownDuplicate", "TownController.cs", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/town/town-by-district/{districtId}
        /// </summary>
        /// <param name="districtId">Foreign key of the table Towns.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.TownByDistrict)]
        public async Task<IActionResult> GetAllTownByDistrict(int districtId)
        {
            try
            {
                var UserInDb = await context.TownRepository.GetTownByDistrict(districtId);
                return Ok(UserInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetAllTownByDistrict", "TownController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}