using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 03.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Client controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class DFZRankController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DFZRankController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DFZRankController(IUnitOfWork context, ILogger<DFZRankController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region Create
        /// <summary>
        /// URL: sc-api/defence-rank
        /// </summary>
        /// <param name="defenceRank">DefenceRank object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDefenceRank)]
        public async Task<IActionResult> CreateDefenceRank(DFZRank dfzRank )
        {
            try
            {
                if (await IsdefenceRankDuplicate(dfzRank) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                dfzRank.DateCreated = DateTime.Now;
                dfzRank.IsDeleted = false;
                dfzRank.IsSynced = false;

                context.DFZRankRepository.Add(dfzRank);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDefenceRankByKey", new { key = dfzRank.Oid }, dfzRank);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "CreateDefenceRank", "CreateArmedForceService", "DefenceRankController.cs", ex.Message, dfzRank.CreatedIn, dfzRank.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// URL: sc-api/defence-ranks
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDefenceRanks)]
        public async Task<IActionResult> ReadDefenceRanks()
        {
            try
            {
                var defenceRankInDb = await context.DFZRankRepository.GetDFZRanks();

                return Ok(defenceRankInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDefenceRanks", "DefenceRankController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/defence-rank/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table defenceRank.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDefenceRankByKey)]
        public async Task<IActionResult> ReadDefenceRankByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzRankIndb = await context.DFZRankRepository.GetDFZRankByKey(key);

                if (dfzRankIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(dfzRankIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDefenceRankByKey", "DefenceRankController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/defence-rank-by-patienttype/{patienttypeId}
        /// </summary>
        /// <param name="key">Foreign key of the table defenceRank.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDefenceRankByPatientType)]
        public async Task<IActionResult> ReadDefenceRankByPatientType(int key)
        {
            try
            {
                var dfzRankIndb = await context.DFZRankRepository.GetDFZRankByPatientType(key);

                return Ok(dfzRankIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDefenceRankByPatientType", "DefenceRankController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// URL: sc-api/defence-rank/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DefenceRank.</param>
        /// <param name="defenceRank">DefenceRank to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDefenceRank)]
        public async Task<IActionResult> UpdateDefenceRank(int key, DFZRank dfzRank)
        {
            try
            {
                if (key != dfzRank.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsdefenceRankDuplicate(dfzRank) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                dfzRank.DateModified = DateTime.Now;
                dfzRank.IsDeleted = false;
                dfzRank.IsSynced = false;

                context.DFZRankRepository.Update(dfzRank);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDefenceRank", "DefenceRankController.cs", ex.Message, dfzRank.ModifiedIn, dfzRank.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// URL: sc-api/defence-rank/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DefenceRank.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDefenceRank)]
        public async Task<IActionResult> DeleteDefenceRank(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzRankIndb = await context.DFZRankRepository.GetDFZRankByKey(key);

                if (dfzRankIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                dfzRankIndb.DateModified = DateTime.Now;
                dfzRankIndb.IsDeleted = true;
                dfzRankIndb.IsSynced = false;

                context.DFZRankRepository.Update(dfzRankIndb);
                await context.SaveChangesAsync();

                return Ok(dfzRankIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDefenceRank", "DefenceRankController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Duplicate check
        /// <summary>
        /// Checks whether the ArmedForceService name is duplicate or not. 
        /// </summary>
        /// <param name="country">ArmedForceService object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsdefenceRankDuplicate(DFZRank dFZRank )
        {
            try
            {
                var dfzRankInDb = await context.DFZRankRepository. GetDFZRankByName(dFZRank.Description);

                if (dfzRankInDb != null)
                    if (dfzRankInDb.Oid != dfzRankInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsdefenceRankDuplicate", "DefenceRankController.cs", ex.Message);
                throw;
            }
        }
        #endregion
    }
}