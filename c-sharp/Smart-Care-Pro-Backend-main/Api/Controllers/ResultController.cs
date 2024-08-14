using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 01.01.2023
 * Modified by   : Stephan
 * Last modified : 06.05.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// District controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ResultController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ResultController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ResultController(IUnitOfWork context, ILogger<ResultController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/result
        /// </summary>
        /// <param name="result">Result object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateResult)]
        public async Task<IActionResult> CreateResult(List<Result> results)
        {
            try
            {
                foreach (var result in results)
                {
                    var investigation = await context.InvestigationRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == result.InvestigationId);

                    if (investigation != null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Result, Enums.EncounterType.Result);
                        interaction.EncounterId = result.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = result.CreatedBy;
                        interaction.CreatedIn = result.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        investigation.IsResultReceived = true;

                        result.InteractionId = interactionId;
                        result.DateCreated = DateTime.Now;
                        result.IsDeleted = false;
                        result.IsSynced = false;

                        context.ResultRepository.Add(result);
                        context.InvestigationRepository.Update(investigation);
                    }
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadResultByKey", new { key = results.Select(x => x.InteractionId).FirstOrDefault() }, results.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateResult", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/results
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadResults)]
        public async Task<IActionResult> ReadResults()
        {
            try
            {
                var resultsIndb = await context.ResultRepository.GetResults();

                return Ok(resultsIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadResults", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadResultByKey)]
        public async Task<IActionResult> ReadResultByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultsIndb = await context.ResultRepository.GetResultByKey(key);

                if (resultsIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(resultsIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadResultByKey", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result/by-client/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationByClient)]
        public async Task<IActionResult> ReadInvestigationByClient(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultsIndb = await context.InvestigationRepository.GetInvestigationByClient(key);

                if (resultsIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(resultsIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationByClient", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLatestResultByClient)]
        public async Task<IActionResult> ReadLatestResultByClient(Guid clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultDb = await context.ResultRepository.GetLatestResultByClient(clientId);

                if (resultDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(resultDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestResultByClient", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <param name="result">District to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateResult)]
        public async Task<IActionResult> UpdateResult(Guid key, Result result)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = result.ModifiedBy;
                interactionInDb.ModifiedIn = result.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != result.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var resultInDb = context.ResultRepository.GetById(key);

                if (resultInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                result.DateModified = DateTime.Now;
                result.IsDeleted = false;
                result.IsSynced = false;
                result.CreatedBy = resultInDb.CreatedBy;
                result.DateCreated = resultInDb.DateCreated;
                result.CreatedIn = resultInDb.CreatedIn;
                result.ModifiedBy = resultInDb.ModifiedBy;
                result.ModifiedIn = resultInDb.ModifiedIn;

                context.ResultRepository.Update(result);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateResult", "ResultController.cs", ex.Message, result.ModifiedIn, result.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/result/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteResult)]
        public async Task<IActionResult> DeleteResult(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var resultsIndb = context.ResultRepository.Get(key);

                if (resultsIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                resultsIndb.DateModified = DateTime.Now;
                resultsIndb.IsDeleted = true;
                resultsIndb.IsSynced = false;

                context.ResultRepository.Update(resultsIndb);
                await context.SaveChangesAsync();

                return Ok(resultsIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteResult", "ResultController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}