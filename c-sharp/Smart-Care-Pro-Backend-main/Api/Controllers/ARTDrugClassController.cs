using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 30.03.2022
 * Modified by   : Brian
 * Last modified : 27.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTDrugClass controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTDrugClassController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ARTDrugClassController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTDrugClassController(IUnitOfWork context, ILogger<ARTDrugClassController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-drug-classes
        /// </summary>
        /// <param name="artDrugClass">ARTDrugClass object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTDrugClass)]
        public async Task<IActionResult> CreateARTDrugClass(ARTDrugClass artDrugClass)
        {
            try
            {
                if (await IsARTDrugClassDuplicate(artDrugClass) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                artDrugClass.DateCreated = DateTime.Now;
                artDrugClass.IsDeleted = false;
                artDrugClass.IsSynced = false;

                context.ARTDrugClassRepository.Add(artDrugClass);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadARTDrugClassByKey", new { key = artDrugClass.Oid }, artDrugClass);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTDrugClass", "ARTDrugClassController.cs", ex.Message, artDrugClass.CreatedIn, artDrugClass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-class
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugClass)]
        public async Task<IActionResult> ReadARTDrugClasses()
        {
            try
            {
                var artDrugClassInDb = await context.ARTDrugClassRepository.GetARTDrugClasses();

                artDrugClassInDb = artDrugClassInDb.OrderByDescending(x => x.DateCreated);

                return Ok(artDrugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugClasses", "ARTDrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-class/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugClass.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugClassByKey)]
        public async Task<IActionResult> ReadARTDrugClassByKey(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artDrugClassInDb = await context.ARTDrugClassRepository.GetARTDrugClassByKey(key);

                if (artDrugClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(artDrugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugClassByKey", "ARTDrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/art-drug-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugClasses.</param>
        /// <param name="artDrugClass">ARTDrugClass to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTDrugClass)]
        public async Task<IActionResult> UpdateARTDrugClass(int key, ARTDrugClass artDrugClass)
        {
            try
            {
                if (key != artDrugClass.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsARTDrugClassDuplicate(artDrugClass) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                artDrugClass.DateModified = DateTime.Now;
                artDrugClass.IsDeleted = false;
                artDrugClass.IsSynced = false;

                context.ARTDrugClassRepository.Update(artDrugClass);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateARTDrugClass", "ARTDrugClassController.cs", ex.Message, artDrugClass.CreatedIn, artDrugClass.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-class/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteARTDrugClass)]
        public async Task<IActionResult> DeleteARTDrugClass(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artDrugClassInDb = await context.ARTDrugClassRepository.GetARTDrugClassByKey(key);

                if (artDrugClassInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                artDrugClassInDb.DateModified = DateTime.Now;
                artDrugClassInDb.IsDeleted = true;
                artDrugClassInDb.IsSynced = false;

                context.ARTDrugClassRepository.Update(artDrugClassInDb);
                await context.SaveChangesAsync();

                return Ok(artDrugClassInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteARTDrugClass", "ARTDrugClassController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the ARTDrugClass name is duplicate or not.
        /// </summary>
        /// <param name="ARTDrugClass">ARTDrugClass object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsARTDrugClassDuplicate(ARTDrugClass artDrugClass)
        {
            try
            {
                var artDrugClassInDb = await context.ARTDrugClassRepository.GetARTDrugClassByName(artDrugClass.Description);

                if (artDrugClassInDb != null)
                    if (artDrugClassInDb.Oid != artDrugClass.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsARTDrugClassDuplicate", "ARTDrugClassController.cs", ex.Message);
                throw;
            }
        }
    }
}