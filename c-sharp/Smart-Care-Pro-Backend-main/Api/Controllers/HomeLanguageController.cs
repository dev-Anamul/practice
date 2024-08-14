using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// HomeLanguage controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HomeLanguageController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HomeLanguageController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HomeLanguageController(IUnitOfWork context, ILogger<HomeLanguageController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/home-language
        /// </summary>
        /// <param name="homeLanguage">HomeLanguage object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHomeLanguage)]
        public async Task<IActionResult> CreateHomeLanguage(HomeLanguage homeLanguage)
        {
            try
            {
                if (await IsHomeLanguageDuplicate(homeLanguage) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                homeLanguage.DateCreated = DateTime.Now;
                homeLanguage.IsDeleted = false;
                homeLanguage.IsSynced = false;

                context.HomeLanguageRepository.Add(homeLanguage);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadHomeLanguageByKey", new { key = homeLanguage.Oid }, homeLanguage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHomeLanguage", "HomeLanguageController.cs", ex.Message, homeLanguage.CreatedIn, homeLanguage.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/home-languages
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHomeLanguages)]
        public async Task<IActionResult> ReadHomeLanguages()
        {
            try
            {
                var homeLanguageInDb = await context.HomeLanguageRepository.GetHomeLanguages();

                return Ok(homeLanguageInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHomeLanguages", "HomeLanguageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/home-language/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HomeLanguages.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHomeLanguageByKey)]
        public async Task<IActionResult> ReadHomeLanguageByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var homeLanguageInDb = await context.HomeLanguageRepository.GetHomeLanguageByKey(key);

                if (homeLanguageInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(homeLanguageInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHomeLanguageByKey", "HomeLanguageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/home-language/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HomeLanguages.</param>
        /// <param name="homeLanguage">HomeLanguage to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHomeLanguage)]
        public async Task<IActionResult> UpdateHomeLanguage(int key, HomeLanguage homeLanguage)
        {
            try
            {
                if (key != homeLanguage.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsHomeLanguageDuplicate(homeLanguage) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                homeLanguage.DateModified = DateTime.Now;
                homeLanguage.IsDeleted = false;
                homeLanguage.IsSynced = false;

                context.HomeLanguageRepository.Update(homeLanguage);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHomeLanguage", "HomeLanguageController.cs", ex.Message, homeLanguage.ModifiedIn, homeLanguage.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/home-language/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HomeLanguages.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHomeLanguage)]
        public async Task<IActionResult> DeleteHomeLanguage(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var homeLanguageInDb = await context.HomeLanguageRepository.GetHomeLanguageByKey(key);

                if (homeLanguageInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                homeLanguageInDb.DateModified = DateTime.Now;
                homeLanguageInDb.IsDeleted = true;
                homeLanguageInDb.IsSynced = false;

                context.HomeLanguageRepository.Update(homeLanguageInDb);
                await context.SaveChangesAsync();

                return Ok(homeLanguageInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHomeLanguage", "HomeLanguageController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the home language is duplicate or not. 
        /// </summary>
        /// <param name="homeLanguage">HomeLanguage object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsHomeLanguageDuplicate(HomeLanguage homeLanguage)
        {
            try
            {
                var homeLanguageInDb = await context.HomeLanguageRepository.GetHomeLanguageByName(homeLanguage.Description);

                if (homeLanguageInDb != null)
                    if (homeLanguageInDb.Oid != homeLanguage.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsHomeLanguageDuplicate", "HomeLanguageController.cs", ex.Message);
                throw;
            }
        }
    }
}