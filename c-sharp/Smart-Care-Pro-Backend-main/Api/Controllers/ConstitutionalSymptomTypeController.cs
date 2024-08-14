using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella 
 * Date created  : 25.12.2022
 * Modified by   : Brian
 * Last modified : 30.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ConstitutionalSymptomTypeController controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ConstitutionalSymptomTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ConstitutionalSymptomTypeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ConstitutionalSymptomTypeController(IUnitOfWork context, ILogger<ConstitutionalSymptomTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConstitutionalSymptomTypes)]
        public async Task<IActionResult> ReadConstitutionalSymptomTypes()
        {
            try
            {
                var constitutionalSymptomTypeInDb = await context.ConstitutionalSymptomTypeRepository.GetConstitutionalSymptomTypes(); constitutionalSymptomTypeInDb = constitutionalSymptomTypeInDb.OrderByDescending(x => x.DateCreated);

                return Ok(constitutionalSymptomTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConstitutionalSymptomTypes", "ConstitutionalSymptomTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom-types-by-symptom/{constitutionalSymptomId}
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet] 
        [Route(RouteConstants.ReadConstitutionalSymptomTypesByConstituationalSymptoms)]
        public async Task<IActionResult> ReadConstitutionalSymptomTypesByConstituationalSymtom(int constitutionalSymptomId)
        {
            try
            {
                var constitutionalSymptomTypeInDb = await context.ConstitutionalSymptomTypeRepository.GetConstitutionalSymptomTypesByConstitutionalSymtom(constitutionalSymptomId);

                return Ok(constitutionalSymptomTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConstitutionalSymptomTypesByConstituationalSymtom", "ConstitutionalSymptomTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom-types
        /// </summary>
        /// <param name="constitutionalSymptomType">ConstitutionalSymptomType object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateConstitutionalSymptomType)]
        public async Task<IActionResult> CreateConstitutionalSymptomType(ConstitutionalSymptomType constitutionalSymptomType)
        {
            try
            {
                if (await IsConstitutionalSymptomTypeDuplicate(constitutionalSymptomType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                constitutionalSymptomType.DateCreated = DateTime.Now;
                constitutionalSymptomType.IsDeleted = false;
                constitutionalSymptomType.IsSynced = false;

                context.ConstitutionalSymptomTypeRepository.Add(constitutionalSymptomType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadConstitutionalSymptomTypeByKey", new { key = constitutionalSymptomType.Oid }, constitutionalSymptomType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateConstitutionalSymptomType", "ConstitutionalSymptomTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptomType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConstitutionalSymptomTypeByKey)]
        public async Task<IActionResult> ReadConstitutionalSymptomTypeByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var constitutionalSymptomTypeInDb = await context.ConstitutionalSymptomTypeRepository.GetConstitutionalSymptomTypeByKey(key);

                if (constitutionalSymptomTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(constitutionalSymptomTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConstitutionalSymptomTypeByKey", "ConstitutionalSymptomTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/constitutional-symptom-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptomTypees.</param>
        /// <param name="constitutionalSymptomType">ConstitutionalSymptomType to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateConstitutionalSymptomType)]
        public async Task<IActionResult> UpdateConstitutionalSymptomType(int key, ConstitutionalSymptomType constitutionalSymptomType)
        {
            try
            {
                if (key != constitutionalSymptomType.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsConstitutionalSymptomTypeDuplicate(constitutionalSymptomType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                constitutionalSymptomType.DateModified = DateTime.Now;
                constitutionalSymptomType.IsDeleted = false;
                constitutionalSymptomType.IsSynced = false;

                context.ConstitutionalSymptomTypeRepository.Update(constitutionalSymptomType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateConstitutionalSymptomType", "ConstitutionalSymptomTypeController.cs", ex.Message, constitutionalSymptomType.ModifiedIn, constitutionalSymptomType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteConstitutionalSymptomType)]
        public async Task<IActionResult> DeleteConstitutionalSymptomType(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var constitutionalSymptomTypeInDb = await context.ConstitutionalSymptomTypeRepository.GetConstitutionalSymptomTypeByKey(key);

                if (constitutionalSymptomTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                constitutionalSymptomTypeInDb.DateModified = DateTime.Now;
                constitutionalSymptomTypeInDb.IsDeleted = true;
                constitutionalSymptomTypeInDb.IsSynced = false;

                context.ConstitutionalSymptomTypeRepository.Update(constitutionalSymptomTypeInDb);
                await context.SaveChangesAsync();

                return Ok(constitutionalSymptomTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteConstitutionalSymptomType", "ConstitutionalSymptomTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ConstitutionalSymptomType name is duplicate or not.
        /// </summary>
        /// <param name="constitutionalSymptomType">ConstitutionalSymptomType object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsConstitutionalSymptomTypeDuplicate(ConstitutionalSymptomType constitutionalSymptomType)
        {
            try
            {
                var constitutionalSymptomTypeInDb = await context.ConstitutionalSymptomTypeRepository.GetConstitutionalSymptomTypeByName(constitutionalSymptomType.Description);

                if (constitutionalSymptomTypeInDb != null)
                    if (constitutionalSymptomTypeInDb.Oid != constitutionalSymptomType.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsConstitutionalSymptomTypeDuplicate", "ConstitutionalSymptomTypeController.cs", ex.Message);
                throw;
            }
        }
    }
}