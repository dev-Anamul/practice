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
    /// NeonatalBirthOutcome Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NeonatalBirthOutcomeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NeonatalBirthOutcomeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NeonatalBirthOutcomeController(IUnitOfWork context, ILogger<NeonatalBirthOutcomeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/neonatal-birth-outcome
        /// </summary>
        /// <param name="neonatalBirthOutcome">NeonatalBirthOutcome object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNeonatalBirthOutcome)]
        public async Task<ActionResult<NeonatalBirthOutcome>> CreateNeonatalBirthOutcome(NeonatalBirthOutcome neonatalBirthOutcome)
        {
            try
            {
                if (await IsNeonatalBirthOutcomeDuplicate(neonatalBirthOutcome) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                neonatalBirthOutcome.DateCreated = DateTime.Now;
                neonatalBirthOutcome.IsDeleted = false;
                neonatalBirthOutcome.IsSynced = false;

                context.NeonatalBirthOutcomeRepository.Add(neonatalBirthOutcome);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNeonatalBirthOutcomeByKey", new { key = neonatalBirthOutcome.Oid }, neonatalBirthOutcome);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNeonatalBirthOutcome", "NeonatalBirthOutcomeController.cs", ex.Message, neonatalBirthOutcome.CreatedIn, neonatalBirthOutcome.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-birth-outcomes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalBirthOutcomes)]
        public async Task<IActionResult> ReadNeonatalBirthOutcomes()
        {
            try
            {
                var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomeRepository.GetNeonatalBirthOutcomes();

                return Ok(neonatalBirthOutcomeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalBirthOutcomes", "NeonatalBirthOutcomeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-birth-outcome/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalBirthOutcomes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalBirthOutcomeByKey)]
        public async Task<IActionResult> ReadNeonatalBirthOutcomeByKey(int key)
        {
            try
            {
                if (key >= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomeRepository.GetNeonatalBirthOutcomeByKey(key);

                if (neonatalBirthOutcomeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(neonatalBirthOutcomeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalBirthOutcomeByKey", "NeonatalBirthOutcomeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-birth-outcome/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalBirthOutcomes.</param>
        /// <param name="neonatalBirthOutcome">NeonatalBirthOutcome to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNeonatalBirthOutcome)]
        public async Task<IActionResult> UpdateNeonatalBirthOutcome(int key, NeonatalBirthOutcome neonatalBirthOutcome)
        {
            try
            {
                if (key != neonatalBirthOutcome.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                neonatalBirthOutcome.DateModified = DateTime.Now;
                neonatalBirthOutcome.IsDeleted = false;
                neonatalBirthOutcome.IsSynced = false;

                context.NeonatalBirthOutcomeRepository.Update(neonatalBirthOutcome);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNeonatalBirthOutcome", "NeonatalBirthOutcomeController.cs", ex.Message, neonatalBirthOutcome.ModifiedIn, neonatalBirthOutcome.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-birth-outcome/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalBirthOutcomes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteNeonatalBirthOutcome)]
        public async Task<IActionResult> DeleteNeonatalBirthOutcome(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomeRepository.GetNeonatalBirthOutcomeByKey(key);

                if (neonatalBirthOutcomeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                neonatalBirthOutcomeInDb.DateModified = DateTime.Now;
                neonatalBirthOutcomeInDb.IsDeleted = true;
                neonatalBirthOutcomeInDb.IsSynced = false;

                context.NeonatalBirthOutcomeRepository.Update(neonatalBirthOutcomeInDb);
                await context.SaveChangesAsync();

                return Ok(neonatalBirthOutcomeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNeonatalBirthOutcome", "NeonatalBirthOutcomeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the NeonatalBirthOutcome name is duplicate or not.
        /// </summary>
        /// <param name="NeonatalBirthOutcome">NeonatalBirthOutcome object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsNeonatalBirthOutcomeDuplicate(NeonatalBirthOutcome neonatalBirthOutcome)
        {
            try
            {
                var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomeRepository.GetNeonatalBirthOutcomeByName(neonatalBirthOutcome.Description);

                if (neonatalBirthOutcomeInDb != null)
                    if (neonatalBirthOutcomeInDb.Oid != neonatalBirthOutcome.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsNeonatalBirthOutcomeDuplicate", "NeonatalBirthOutcomeController.cs", ex.Message);
                throw;
            }
        }
    }
}