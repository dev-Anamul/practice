using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ConstitutionalSymptomController controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ConstitutionalSymptomController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ConstitutionalSymptomController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ConstitutionalSymptomController(IUnitOfWork context, ILogger<ConstitutionalSymptomController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptoms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConstitutionalSymptoms)]
        public async Task<IActionResult> ReadConstitutionalSymptoms()
        {
            try
            {
                var constitutionalSymptomInDb = await context.ConstitutionalSymptomRepository.GetConstitutionalSymptoms();

                constitutionalSymptomInDb = constitutionalSymptomInDb.OrderByDescending(x => x.DateCreated);

                return Ok(constitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConstitutionalSymptoms", "ConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptoms
        /// </summary>
        /// <param name="constitutionalSymptom">ConstitutionalSymptom object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateConstitutionalSymptom)]
        public async Task<IActionResult> CreateConstitutionalSymptom(ConstitutionalSymptom constitutionalSymptom)
        {
            try
            {
                if (await IsConstitutionalSymptomDuplicate(constitutionalSymptom) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                constitutionalSymptom.DateCreated = DateTime.Now;
                constitutionalSymptom.IsDeleted = false;
                constitutionalSymptom.IsSynced = false;

                context.ConstitutionalSymptomRepository.Add(constitutionalSymptom);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadConstitutionalSymptomByKey", new { key = constitutionalSymptom.Oid }, constitutionalSymptom);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateConstitutionalSymptom", "ConstitutionalSymptomController.cs", ex.Message, constitutionalSymptom.CreatedIn, constitutionalSymptom.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConstitutionalSymptomByKey)]
        public async Task<IActionResult> ReadConstitutionalSymptomByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var constitutionalSymptomInDb = await context.ConstitutionalSymptomRepository.GetConstitutionalSymptomByKey(key);

                if (constitutionalSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(constitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConstitutionalSymptomByKey", "ConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/constitutional-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptomes.</param>
        /// <param name="ConstitutionalSymptom">ConstitutionalSymptom to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateConstitutionalSymptom)]
        public async Task<IActionResult> UpdateConstitutionalSymptom(int key, ConstitutionalSymptom constitutionalSymptom)
        {
            try
            {
                if (key != constitutionalSymptom.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsConstitutionalSymptomDuplicate(constitutionalSymptom) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                constitutionalSymptom.DateModified = DateTime.Now;
                constitutionalSymptom.IsDeleted = false;
                constitutionalSymptom.IsSynced = false;

                context.ConstitutionalSymptomRepository.Update(constitutionalSymptom);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateConstitutionalSymptom", "ConstitutionalSymptomController.cs", ex.Message, constitutionalSymptom.ModifiedIn, constitutionalSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/constitutional-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteConstitutionalSymptom)]
        public async Task<IActionResult> DeleteConstitutionalSymptom(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var constitutionalSymptomInDb = await context.ConstitutionalSymptomRepository.GetConstitutionalSymptomByKey(key);

                if (constitutionalSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                constitutionalSymptomInDb.DateModified = DateTime.Now;
                constitutionalSymptomInDb.IsDeleted = true;
                constitutionalSymptomInDb.IsSynced = false;

                context.ConstitutionalSymptomRepository.Update(constitutionalSymptomInDb);
                await context.SaveChangesAsync();

                return Ok(constitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteConstitutionalSymptom", "ConstitutionalSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ConstitutionalSymptom name is duplicate or not.
        /// </summary>
        /// <param name="ConstitutionalSymptom">ConstitutionalSymptom object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsConstitutionalSymptomDuplicate(ConstitutionalSymptom constitutionalSymptom)
        {
            try
            {
                var constitutionalSymptomInDb = await context.ConstitutionalSymptomRepository.GetConstitutionalSymptomByName(constitutionalSymptom.Description);

                if (constitutionalSymptomInDb != null)
                    if (constitutionalSymptomInDb.Oid != constitutionalSymptom.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsConstitutionalSymptomDuplicate", "ConstitutionalSymptomController.cs", ex.Message);
                throw;
            }
        }
    }
}