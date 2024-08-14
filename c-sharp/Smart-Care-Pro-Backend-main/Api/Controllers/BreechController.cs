using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Breech Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BreechController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BreechController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BreechController(IUnitOfWork context, ILogger<BreechController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/breechs
        /// </summary>
        /// <param name="breech">Breech object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBreech)]
        public async Task<IActionResult> CreateBreech(Breech breech)
        {
            try
            {
                if (await IsBreechDuplicate(breech) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                breech.DateCreated = DateTime.Now;
                breech.IsDeleted = false;
                breech.IsSynced = false;

                context.BreechRepository.Add(breech);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadBreechByKey", new { key = breech.Oid }, breech);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBreech", "BreechController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/breeches
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBreeches)]
        public async Task<IActionResult> ReadBreechs()
        {
            try
            {
                var breechInDb = await context.BreechRepository.GetBreeches();

                breechInDb = breechInDb.OrderByDescending(x => x.DateCreated);

                return Ok(breechInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBreechs", "BreechController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/breech/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Breeches.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBreechByKey)]
        public async Task<IActionResult> ReadBreechByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var breechIndb = await context.BreechRepository.GetBreechByKey(key);

                if (breechIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(breechIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBreechByKey", "BreechController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

       

        /// <summary>
        ///URL: sc-api/breech/{key}
        /// </summary>
        /// <param name="key">Primary key of the table breech.</param>
        /// <param name="breech">breech to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBreech)]
        public async Task<IActionResult> UpdateBreech(int key, Breech breech)
        {
            try
            {
                if (key != breech.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsBreechDuplicate(breech) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                breech.DateModified = DateTime.Now;
                breech.IsDeleted = false;
                breech.IsSynced = false;

                context.BreechRepository.Update(breech);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBreech", "BreechController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/breech/{key}
        /// </summary>
        /// <param name="key">Primary key of the table breech.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteBreech)]
        public async Task<IActionResult> DeleteBreech(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var breechInDb = await context.BreechRepository.GetBreechByKey(key);

                if (breechInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                breechInDb.DateModified = DateTime.Now;
                breechInDb.IsDeleted = true;
                breechInDb.IsSynced = false;

                context.BreechRepository.Update(breechInDb);
                await context.SaveChangesAsync();

                return Ok(breechInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteBreech", "BreechController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the breech name is duplicate or not.
        /// </summary>
        /// <param name="Breech">breech object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsBreechDuplicate(Breech breech)
        {
            try
            {
                var breechInDb = await context.BreechRepository.GetBreechByName(breech.Description);

                if (breechInDb != null)
                    if (breechInDb.Oid != breech.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsBreechDuplicate", "BreechController.cs", ex.Message);
                throw;
            }
        }
    }
}