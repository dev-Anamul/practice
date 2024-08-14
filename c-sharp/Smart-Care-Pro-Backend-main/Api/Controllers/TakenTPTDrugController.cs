using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// TakenTPTDrug Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TakenTPTDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TakenTPTDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TakenTPTDrugController(IUnitOfWork context, ILogger<TakenTPTDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/taken-tpt-drug
        /// </summary>
        /// <param name="takenTPTDrug">TakenTPTDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTakenTPTDrug)]
        public async Task<IActionResult> CreateTakenTPTDrug(TakenTPTDrug takenTPTDrug)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TakenTPTDrug, takenTPTDrug.EncounterType);
                interaction.EncounterId = takenTPTDrug.EncounterId;
                interaction.CreatedBy = takenTPTDrug.CreatedBy;
                interaction.CreatedIn = takenTPTDrug.CreatedIn;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = Guid.Empty;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                takenTPTDrug.InteractionId = interactionId;
                takenTPTDrug.EncounterId = takenTPTDrug.EncounterId;
                takenTPTDrug.TPTHistoryId = takenTPTDrug.TPTHistoryId;
                takenTPTDrug.EncounterType = takenTPTDrug.EncounterType;
                takenTPTDrug.DateCreated = DateTime.Now;
                takenTPTDrug.IsDeleted = false;
                takenTPTDrug.IsSynced = false;

                context.TakenTPTDrugRepository.Add(takenTPTDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTakenTPTDrugByKey", new { key = takenTPTDrug.InteractionId }, takenTPTDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTakenTPTDrug", "TakenTPTDrugController.cs", ex.Message, takenTPTDrug.CreatedIn, takenTPTDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-art-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenTPTDrugs)]
        public async Task<IActionResult> ReadTakenTPTDrugs()
        {
            try
            {
                var takenTPTDrugInDb = await context.TakenTPTDrugRepository.GetTakenTPTDrugs();

                return Ok(takenTPTDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenTPTDrugs", "TakenTPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tpt-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenTPTDrugByKey)]
        public async Task<IActionResult> ReadTakenTPTDrugByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenTPTDrugInDb = await context.TakenTPTDrugRepository.GetTakenTPTDrugByKey(key);

                if (takenTPTDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(takenTPTDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenTPTDrugByKey", "TakenTPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tpt-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
        /// <param name="takenTPTDrug">TakenTPTDrug to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTakenTPTDrug)]
        public async Task<IActionResult> UpdateTakenTPTDrug(Guid key, TakenTPTDrug takenTPTDrug)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = takenTPTDrug.ModifiedBy;
                interactionInDb.ModifiedIn = takenTPTDrug.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != takenTPTDrug.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                takenTPTDrug.DateModified = DateTime.Now;
                takenTPTDrug.IsDeleted = false;
                takenTPTDrug.IsSynced = false;

                context.TakenTPTDrugRepository.Update(takenTPTDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTakenTPTDrug", "TakenTPTDrugController.cs", ex.Message, takenTPTDrug.ModifiedIn, takenTPTDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tpt-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTakenTPTDrug)]
        public async Task<IActionResult> DeleteTakenTPTDrug(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenTPTDrugInDb = await context.TakenTPTDrugRepository.GetTakenTPTDrugByKey(key);

                if (takenTPTDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                takenTPTDrugInDb.DateModified = DateTime.Now;
                takenTPTDrugInDb.IsDeleted = true;
                takenTPTDrugInDb.IsSynced = false;

                context.TakenTPTDrugRepository.Update(takenTPTDrugInDb);
                await context.SaveChangesAsync();

                return Ok(takenTPTDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTakenTPTDrug", "TakenTPTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}