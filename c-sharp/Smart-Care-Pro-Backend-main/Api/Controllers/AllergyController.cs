using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Brian
 * Last modified : 27.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Allergy controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AllergyController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AllergyController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AllergyController(IUnitOfWork context, ILogger<AllergyController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/allergies
        /// </summary>
        /// <param name="allergy">Allergy object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAllergy)]
        public async Task<IActionResult> CreateAllergy(Allergy allergy)
        {
            try
            {
                if (await IsAllergyDuplicate(allergy) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                allergy.DateCreated = DateTime.Now;
                allergy.IsDeleted = false;
                allergy.IsSynced = false;

                context.AllergyRepository.Add(allergy);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadAllergyByKey", new { key = allergy.Oid }, allergy);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAllergicDrug", "AllergyController.cs", ex.Message, allergy.CreatedIn, allergy.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergies
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAllergies)]
        public async Task<IActionResult> ReadAllergies()
        {
            try
            {
                var allergyInDb = await context.AllergyRepository.GetAllergies();

                allergyInDb = allergyInDb.OrderByDescending(x => x.DateCreated);

                return Ok(allergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllergies", "AllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergy/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAllergyByKey)]
        public async Task<IActionResult> ReadAllergyByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var allergyInDb = await context.AllergyRepository.GetAllergyByKey(key);

                if (allergyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(allergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllergyByKey", "AllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/allergy/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <param name="allergy">Allergy to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAllergy)]
        public async Task<IActionResult> UpdateAllergy(int key, Allergy allergy)
        {
            try
            {
                if (key != allergy.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsAllergyDuplicate(allergy) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                allergy.DateModified = DateTime.Now;
                allergy.IsDeleted = false;
                allergy.IsSynced = false;

                context.AllergyRepository.Update(allergy);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateAllergy", "AllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/allergy/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAllergy)]
        public async Task<IActionResult> DeleteAllergy(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var allergyInDb = await context.AllergyRepository.GetAllergyByKey(key);

                if (allergyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                allergyInDb.DateModified = DateTime.Now;
                allergyInDb.IsDeleted = true;
                allergyInDb.IsSynced = false;

                context.AllergyRepository.Update(allergyInDb);
                await context.SaveChangesAsync();

                return Ok(allergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAllergy", "AllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the allergy name is duplicate or not.
        /// </summary>
        /// <param name="allergy">Allergy object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsAllergyDuplicate(Allergy allergy)
        {
            try
            {
                var allergyInDb = await context.AllergyRepository.GetAllergyByName(allergy.Description);

                if (allergyInDb != null)
                    if (allergyInDb.Oid != allergy.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAllergyDuplicate", "AllergyController.cs", ex.Message);
                throw;
            }
        }
    }
}