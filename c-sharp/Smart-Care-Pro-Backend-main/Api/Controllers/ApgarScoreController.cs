using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ApgarScore controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ApgarScoreController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ApgarScoreController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ApgarScoreController(IUnitOfWork context, ILogger<ApgarScoreController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/apgar-score
        /// </summary>
        /// <param name="apgarScore">ApgarScore object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateApgarScore)]
        public async Task<IActionResult> CreateApgarScore(ApgarScore apgarScore)
        {
            try
            {
                foreach (var item in apgarScore.ApgarScores)
                {
                    var apgarInDb = await context.ApgarScoreRepository.FirstOrDefaultAsync(x => x.EncounterId == apgarScore.EncounterId
                    && x.ApgarTimeSpan == item.ApgarTimeSpan && x.NeonatalId == item.NeonatalId && x.IsDeleted == false && x.IsSynced == false);

                    if (apgarInDb == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ApgarScore, apgarScore.EncounterType);
                        interaction.EncounterId = apgarScore.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = apgarScore.CreatedBy;
                        interaction.CreatedIn = apgarScore.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        item.InteractionId = interactionId;
                        item.CreatedIn = apgarScore.CreatedIn;
                        item.CreatedBy = apgarScore.CreatedBy;
                        item.DateCreated = DateTime.Now;
                        item.IsDeleted = false;
                        item.IsSynced = false;
                        item.EncounterId = apgarScore.EncounterId;

                        context.ApgarScoreRepository.Add(item);
                    }
                    else
                    {
                        var interactionInDb = await context.InteractionRepository.GetInteractionByKey(item.InteractionId);

                        if (interactionInDb == null)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                        interactionInDb.ModifiedBy = item.ModifiedBy;
                        interactionInDb.ModifiedIn = item.ModifiedIn;
                        interactionInDb.DateModified = DateTime.Now;
                        interactionInDb.IsDeleted = false;
                        interactionInDb.IsSynced = false;

                        context.InteractionRepository.Update(interactionInDb);

                        apgarInDb.ApgarScores = item.ApgarScores;
                        apgarInDb.ApgarTimeSpan = item.ApgarTimeSpan;
                        apgarInDb.Appearance = item.Appearance;
                        apgarInDb.Pulses = item.Pulses;
                        apgarInDb.Activity = item.Activity;
                        apgarInDb.Respiration = item.Respiration;
                        apgarInDb.IsDeleted = false;
                        apgarInDb.IsSynced = false;
                        apgarInDb.DateModified = DateTime.Now;

                        context.ApgarScoreRepository.Update(apgarInDb);

                    }

                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadApgarScoreByKey", new { key = apgarScore.InteractionId }, apgarScore);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateApgarScore", "ApgarScoreController.cs", ex.Message, apgarScore.CreatedIn, apgarScore.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/apgar-scores
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadApgarScores)]
        public async Task<IActionResult> ReadApgarScores()
        {
            try
            {
                var apgarScoreIndb = await context.ApgarScoreRepository.GetApgarScores();

                apgarScoreIndb = apgarScoreIndb.OrderByDescending(x => x.DateCreated);

                return Ok(apgarScoreIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadApgarScores", "ApgarScoreController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/apgar-score/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadApgarScoreByKey)]
        public async Task<IActionResult> ReadApgarScoreByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var apgarScoreIndb = await context.ApgarScoreRepository.GetApgarScoreByKey(key);

                if (apgarScoreIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(apgarScoreIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadApgarScoreByKey", "ApgarScoreController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/apgar-score/by-neonatal/{neonatalId}
        /// </summary>
        /// <param name="neonatalId">Primary key of the table NewBornDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadApgarScoreByNeonatal)]
        public async Task<IActionResult> ReadApgarScoreByNeonatal(Guid neonatalId)
        {
            try
            {
                var apgarScoreInDb = await context.ApgarScoreRepository.GetApgarScoreByNeonatal(neonatalId);

                return Ok(apgarScoreInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadApgarScoreByNeonatal", "ApgarScoreController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/apgar-score/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ApgarScores.</param>
        /// <param name="apgarScore">ApgarScore to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateApgarScore)]
        public async Task<IActionResult> UpdateApgarScore(Guid key, ApgarScore apgarScore)
        {
            try
            {
                if (key != apgarScore.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = apgarScore.ModifiedBy;
                interactionInDb.ModifiedIn = apgarScore.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var apgarScoreInDb = await context.ApgarScoreRepository.GetApgarScoreByKey(key);

                if (apgarScoreInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                apgarScoreInDb.Activity = apgarScore.Activity;
                apgarScoreInDb.ApgarTimeSpan = apgarScore.ApgarTimeSpan;
                apgarScoreInDb.Pulses = apgarScore.Pulses;
                apgarScoreInDb.Appearance = apgarScore.Appearance;
                apgarScoreInDb.Grimace = apgarScore.Grimace;
                apgarScoreInDb.Respiration = apgarScore.Respiration;
                apgarScoreInDb.TotalScore = apgarScore.TotalScore;

                apgarScoreInDb.DateModified = DateTime.Now;
                apgarScoreInDb.IsDeleted = false;
                apgarScoreInDb.IsSynced = false;

                context.ApgarScoreRepository.Update(apgarScoreInDb);
                await context.SaveChangesAsync();


                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateApgarScore", "ApgarScoreController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/apgar-score/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ApgarScores.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteApgarScore)]
        public async Task<IActionResult> DeleteApgarScore(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var apgarScoreInDb = await context.ApgarScoreRepository.GetApgarScoreByKey(key);

                if (apgarScoreInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                apgarScoreInDb.IsDeleted = true;

                context.ApgarScoreRepository.Update(apgarScoreInDb);
                await context.SaveChangesAsync();

                return Ok(apgarScoreInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteApgarScore", "ApgarScoreController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}