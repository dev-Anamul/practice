using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 12.09.2022
 * Modified by   : Stephan
 * Last modified : 06.11.2022
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Country controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CountryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CountryController(IUnitOfWork context, ILogger<CountryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/country
        /// </summary>
        /// <param name="country">Country object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCountry)]
        public async Task<IActionResult> CreateCountry(Country country)
        {
            try
            {
                if (await IsCountryDuplicate(country) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                country.DateCreated = DateTime.Now;
                country.IsDeleted = false;
                country.IsSynced = false;

                context.CountryRepository.Add(country);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCountryByKey", new { key = country.Oid }, country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCountry", "CountryController.cs", ex.Message, country.CreatedIn, country.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/countries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountries)]
        public async Task<IActionResult> ReadCountries()
        {
            try
            {
                var countryIndb = await context.CountryRepository.GetCountries();

                return Ok(countryIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountries", "CountryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/country/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountryByKey)]
        public async Task<IActionResult> ReadCountryByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var countryIndb = await context.CountryRepository.GetCountryByKey(key);

                if (countryIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(countryIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountryByKey", "CountryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <param name="country">Country to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCountry)]
        public async Task<IActionResult> UpdateCountry(int key, Country country)
        {
            try
            {
                if (key != country.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCountryDuplicate(country) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                country.DateModified = DateTime.Now;
                country.IsDeleted = false;
                country.IsSynced = false;

                context.CountryRepository.Update(country);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCountry", "CountryController.cs", ex.Message, country.ModifiedIn, country.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCountry)]
        public async Task<IActionResult> DeleteCountry(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var countryInDb = await context.CountryRepository.GetCountryByKey(key);

                if (countryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                countryInDb.DateModified = DateTime.Now;
                countryInDb.IsDeleted = true;
                countryInDb.IsSynced = false;

                context.CountryRepository.Update(countryInDb);
                await context.SaveChangesAsync();

                return Ok(countryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCountry", "CountryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the country name is duplicate or not. 
        /// </summary>
        /// <param name="country">Country object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCountryDuplicate(Country country)
        {
            try
            {
                var countryInDb = await context.CountryRepository.GetCountryByName(country.Description);

                if (countryInDb != null)
                    if (countryInDb.Oid != country.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCountryDuplicate", "CountryController.cs", ex.Message);
                throw;
            }
        }
    }
}