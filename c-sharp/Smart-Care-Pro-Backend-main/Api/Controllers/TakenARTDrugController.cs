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
    /// TakenARTDrug Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TakenARTDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TakenARTDrugController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TakenARTDrugController(IUnitOfWork context, ILogger<TakenARTDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/taken-art-drug
        /// </summary>
        /// <param name="takenARTDrug">TakenARTDrug object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTakenARTDrug)]
        public async Task<IActionResult> CreateTakenARTDrug(TakenARTDrug takenARTDrug)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.TakenARTDrug, takenARTDrug.EncounterType);
                interaction.EncounterId = takenARTDrug.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = takenARTDrug.CreatedBy;
                interaction.CreatedIn = takenARTDrug.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                takenARTDrug.InteractionId = interactionId;
                takenARTDrug.CreatedBy = takenARTDrug.CreatedBy;
                takenARTDrug.CreatedIn = takenARTDrug.CreatedIn;
                takenARTDrug.DateCreated = DateTime.Now;
                takenARTDrug.IsDeleted = false;
                takenARTDrug.IsSynced = false;

                context.TakenARTDrugRepository.Add(takenARTDrug);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTakenARTDrugByKey", new { key = takenARTDrug.InteractionId }, takenARTDrug);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTakenARTDrug", "TakenARTDrugController.cs", ex.Message, takenARTDrug.CreatedIn, takenARTDrug.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-art-drugs
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenARTDrugs)]
        public async Task<IActionResult> ReadTakenARTDrugs()
        {
            try
            {
                var takenArtDrugInDb = await context.TakenARTDrugRepository.GetTakenARTDrugs();

                return Ok(takenArtDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenARTDrugs", "TakenARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-art-drug/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenARTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTakenARTDrugByKey)]
        public async Task<IActionResult> ReadTakenARTDrugByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenArtDrugInDb = await context.TakenARTDrugRepository.GetTakenARTDrugByKey(key);

                if (takenArtDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(takenArtDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTakenARTDrugByKey", "TakenARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-art-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenARTDrugs.</param>
        /// <param name="takenARTDrug">TakenARTDrug to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTakenARTDrug)]
        public async Task<IActionResult> UpdateTakenARTDrug(Guid key, TakenARTDrug takenARTDrug)
        {
            try
            {

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = takenARTDrug.ModifiedBy;
                interactionInDb.ModifiedIn = takenARTDrug.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != takenARTDrug.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                takenARTDrug.DateModified = DateTime.Now;
                takenARTDrug.IsDeleted = false;
                takenARTDrug.IsSynced = false;

                context.TakenARTDrugRepository.Update(takenARTDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTakenARTDrug", "TakenARTDrugController.cs", ex.Message, takenARTDrug.ModifiedIn, takenARTDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/taken-art-drug/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TakenARTDrugs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTakenARTDrug)]
        public async Task<IActionResult> DeleteTakenARTDrug(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var takenArtDrugInDb = await context.TakenARTDrugRepository.GetTakenARTDrugByKey(key);

                if (takenArtDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                takenArtDrugInDb.DateModified = DateTime.Now;
                takenArtDrugInDb.IsDeleted = true;
                takenArtDrugInDb.IsSynced = false;

                context.TakenARTDrugRepository.Update(takenArtDrugInDb);
                await context.SaveChangesAsync();

                return Ok(takenArtDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTakenARTDrug", "TakenARTDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}