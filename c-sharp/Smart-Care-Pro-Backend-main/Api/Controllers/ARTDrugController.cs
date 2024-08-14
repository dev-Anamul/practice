using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTDrug controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ARTDrugController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTDrugController(IUnitOfWork context, ILogger<ARTDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-drug
        /// </summary>
        /// <param name="artDrug">ARTDrug object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTDrug)]
        public async Task<IActionResult> CreateARTDrug(ARTDrug artDrug)
        {
            try
            {
                if (await IsARTDrugDuplicate(artDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                artDrug.DateCreated = DateTime.Now;
                artDrug.IsDeleted = false;
                artDrug.IsSynced = false;

                context.ARTDrugRepository.Add(artDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadARTDrugByKey", new { key = artDrug.Oid }, artDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTDrug", "ARTDrugController.cs", ex.Message, artDrug.CreatedIn, artDrug.CreatedBy);

                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/art-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugs)]
        public async Task<IActionResult> ReadARTDrugs()
        {
            try
            {
                var artDrugInDb = await context.ARTDrugRepository.GetARTDrugs();

                artDrugInDb = artDrugInDb.OrderByDescending(x => x.DateCreated);

                return Ok(artDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugs", "ARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugByKey)]
        public async Task<IActionResult> ReadARTDrugByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artDrugInDb = await context.ARTDrugRepository.GetARTDrugByKey(key);

                if (artDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(artDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugByKey", "ARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-by-art-drug-class/{artDrugClassId}
        /// </summary>
        /// <param name="artDrugClassId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugByARTDrugClass)]
        public async Task<IActionResult> ReadARTDrugByARTDrugClass(int artDrugClassId)
        {
            try
            {
                var artDrugInDb = await context.ARTDrugRepository.GetARTDrugByARTDrugClass(artDrugClassId);

                return Ok(artDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugByARTDrugClass", "ARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugs.</param>
        /// <param name="artDrug">ARTDrug to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTDrug)]
        public async Task<IActionResult> UpdateARTDrug(int key, ARTDrug artDrug)
        {
            try
            {
                if (key != artDrug.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsARTDrugDuplicate(artDrug) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                artDrug.DateModified = DateTime.Now;
                artDrug.IsDeleted = false;
                artDrug.IsSynced = false;

                context.ARTDrugRepository.Update(artDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateARTDrug", "ARTDrugController.cs", ex.Message, artDrug.CreatedIn, artDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteARTDrug)]
        public async Task<IActionResult> DeleteARTDrug(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artDrugInDb = await context.ARTDrugRepository.GetARTDrugByKey(key);

                if (artDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                artDrugInDb.DateModified = DateTime.Now;
                artDrugInDb.IsDeleted = true;
                artDrugInDb.IsSynced = false;

                context.ARTDrugRepository.Update(artDrugInDb);
                await context.SaveChangesAsync();

                return Ok(artDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteARTDrug", "ARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the drug name is duplicate or not. 
        /// </summary>
        /// <param name="artDrug">ARTDrug object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsARTDrugDuplicate(ARTDrug artDrug)
        {
            try
            {
                var artDrugInDb = await context.ARTDrugRepository.GetARTDrugByName(artDrug.Description);

                if (artDrugInDb != null)
                    if (artDrugInDb.Oid != artDrug.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsARTDrugDuplicate", "ARTDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}