using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    /// <summary>
    /// GeneralDrugDefination controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class GeneralDrugDefinationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GeneralDrugDefinationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GeneralDrugDefinationController(IUnitOfWork context, ILogger<GeneralDrugDefinationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/general-drugdefination
        /// </summary>
        /// <param name="generalDrugDefinition">GeneralDrugDefination object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGeneralDrugDefination)]
        public async Task<ActionResult<GeneralDrugDefinition>> CreateGeneralDrugDefination(GeneralDrugDefinition generalDrugDefinition)
        {
            try
            {
                if (await IsGeneralDrugDefinitionDuplicate(generalDrugDefinition) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                generalDrugDefinition.DateCreated = DateTime.Now;
                generalDrugDefinition.IsDeleted = false;
                generalDrugDefinition.IsSynced = false;

                context.GeneralDrugDefinationRepository.Add(generalDrugDefinition);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadGeneralDrugDefinationByKey", new { key = generalDrugDefinition.Oid }, generalDrugDefinition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGeneralDrugDefination", "GeneralDrugDefinationController.cs", ex.Message, generalDrugDefinition.CreatedIn, generalDrugDefinition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/general-drugdefinations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralDrugDefination)]
        public async Task<IActionResult> ReadGeneralDrugDefination()
        {
            try
            {
                var generalDrugDefinitionInDb = await context.GeneralDrugDefinationRepository.GetGeneralDrugDefinition();

                return Ok(generalDrugDefinitionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralDrugDefination", "GeneralDrugDefinationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/general-drugdefination/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GeneralDrugDefinition.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralDrugDefinationByKey)]
        public async Task<IActionResult> ReadGeneralDrugDefinationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalDrugDefinitionInDb = await context.GeneralDrugDefinationRepository.GetGeneralDrugDefinitionByKey(key);

                if (generalDrugDefinitionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(generalDrugDefinitionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralDrugDefinationByKey", "GeneralDrugDefinationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/general-drugdefination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GeneralDrugDefinition.</param>
        /// <param name="generalDrugDefinitionInDb">GeneralDrugDefinition to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGeneralDrugDefination)]
        public async Task<IActionResult> UpdateGeneralDrugDefination(int key, GeneralDrugDefinition generalDrugDefinition)
        {
            try
            {
                if (key != generalDrugDefinition.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                generalDrugDefinition.DateModified = DateTime.Now;
                generalDrugDefinition.IsDeleted = false;
                generalDrugDefinition.IsSynced = false;

                context.GeneralDrugDefinationRepository.Update(generalDrugDefinition);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGeneralDrugDefination", "GeneralDrugDefinationController.cs", ex.Message, generalDrugDefinition.ModifiedIn, generalDrugDefinition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/general-drugdefination/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GeneralDrugDefinition.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGeneralDrugDefination)]
        public async Task<IActionResult> DeleteGeneralDrugDefination(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalDrugDefinitionInDb = await context.GeneralDrugDefinationRepository.GetGeneralDrugDefinitionByKey(key);

                if (generalDrugDefinitionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                generalDrugDefinitionInDb.DateModified = DateTime.Now;
                generalDrugDefinitionInDb.IsDeleted = true;
                generalDrugDefinitionInDb.IsSynced = false;

                context.GeneralDrugDefinationRepository.Update(generalDrugDefinitionInDb);
                await context.SaveChangesAsync();

                return Ok(generalDrugDefinitionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGeneralDrugDefination", "GeneralDrugDefinationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the GeneralDrugDefinition name is duplicate or not.
        /// </summary>
        /// <param name="generalDrugDefinition">GeneralDrugDefinition object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsGeneralDrugDefinitionDuplicate(GeneralDrugDefinition generalDrugDefinition)
        {
            try
            {
                var generalDrugDefinitionInDb = await context.GeneralDrugDefinationRepository.GetGeneralDrugDefinitionByName(generalDrugDefinition.Description);

                if (generalDrugDefinitionInDb != null)
                    if (generalDrugDefinitionInDb.Oid != generalDrugDefinitionInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsGeneralDrugDefinitionDuplicate", "GeneralDrugDefinationController.cs", ex.Message);
                throw;
            }
        }
    }
}
