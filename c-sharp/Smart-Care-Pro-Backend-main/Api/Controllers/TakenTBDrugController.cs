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
    /// takenTBDrug Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TakenTBDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TakenARTDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TakenTBDrugController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: sc-api/taken-tb-drug
        /// </summary>
        /// <param name="takenTBDrug">takenTBDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTakenTBDrug)]
        public async Task<IActionResult> CreateTakenTBDrug(TakenTBDrug takenTBDrug)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TakenTBDrug, takenTBDrug.EncounterType);
                interaction.EncounterId = takenTBDrug.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = takenTBDrug.CreatedBy;
                interaction.CreatedIn = takenTBDrug.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                takenTBDrug.InteractionId = interactionId;
                takenTBDrug.CreatedIn = takenTBDrug.CreatedIn;
                takenTBDrug.CreatedBy = takenTBDrug.CreatedBy;
                takenTBDrug.DateCreated = DateTime.Now;
                takenTBDrug.IsDeleted = false;
                takenTBDrug.IsSynced = false;

                context.TakenTBDrugRepository.Add(takenTBDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTakenTBDrugByKey", new { key = takenTBDrug.InteractionId }, takenTBDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTakenTBDrug", "TakenTBDrugController.cs", ex.Message, takenTBDrug.CreatedIn, takenTBDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tb-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenTBDrugs)]
        public async Task<IActionResult> ReadTakenTBDrugs()
        {
            try
            {
                var takenTBDrugInDb = await context.TakenTBDrugRepository.GetTakenTBDrugs();

                return Ok(takenTBDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenTBDrugs", "TakenTBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tb-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTBDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenTBDrugByKey)]
        public async Task<IActionResult> ReadTakenTBDrugByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenTBDrugInDb = await context.TakenTBDrugRepository.GetTakenTBDrugByKey(key);

                if (takenTBDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(takenTBDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenTBDrugByKey", "TakenTBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tb-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTBDrugs.</param>
        /// <param name="takenTBDrug">takenTBDrug to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTakenTBDrug)]
        public async Task<IActionResult> UpdateTakenTBDrug(Guid key, TakenTBDrug takenTBDrug)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = takenTBDrug.ModifiedBy;
                interactionInDb.ModifiedIn = takenTBDrug.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != takenTBDrug.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                takenTBDrug.DateModified = DateTime.Now;
                takenTBDrug.IsDeleted = false;
                takenTBDrug.IsSynced = false;

                context.TakenTBDrugRepository.Update(takenTBDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTakenTBDrug", "TakenTBDrugController.cs", ex.Message, takenTBDrug.ModifiedIn, takenTBDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-tb-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenTBDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTakenTBDrug)]
        public async Task<IActionResult> DeleteTakenTBDrug(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenTBDrugInDb = await context.TakenTBDrugRepository.GetTakenTBDrugByKey(key);

                if (takenTBDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                takenTBDrugInDb.DateModified = DateTime.Now;
                takenTBDrugInDb.IsDeleted = true;
                takenTBDrugInDb.IsSynced = false;

                context.TakenTBDrugRepository.Update(takenTBDrugInDb);
                await context.SaveChangesAsync();

                return Ok(takenTBDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTakenTBDrug", "TakenTBDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}