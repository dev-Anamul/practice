using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 13.04.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedComplication controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedComplicationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedComplicationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedComplicationController(IUnitOfWork context, ILogger<IdentifiedComplicationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-complication
        /// </summary>
        /// <param name="identifiedComplication">IdentifiedComplication object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedComplication)]
        public async Task<IActionResult> CreateIdentifiedComplication(IdentifiedComplication identifiedComplication)
        {
            try
            {
                foreach (var item in identifiedComplication.IdentifiedComplicationList)
                {
                    var identifiedComplications = await context.IdentifiedComplicationRepository.LoadWithChildAsync<IdentifiedComplication>(x => x.EncounterId == identifiedComplication.EncounterId
                    && x.ComplicationId == identifiedComplication.ComplicationId
                    && x.ComplicationTypeId == item
                    && x.IsDeleted == false);

                    if (identifiedComplications == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionID = Guid.NewGuid();

                        interaction.Oid = interactionID;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedComplication, identifiedComplication.EncounterType);
                        interaction.EncounterId = identifiedComplication.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = identifiedComplication.CreatedBy;
                        interaction.CreatedIn = identifiedComplication.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedComplication.InteractionId = interactionID;
                        identifiedComplication.ComplicationId = identifiedComplication.ComplicationId;
                        identifiedComplication.ComplicationTypeId = item;
                        identifiedComplication.DateCreated = DateTime.Now;
                        identifiedComplication.IsDeleted = false;
                        identifiedComplication.IsSynced = false;

                        var identifiedComplicationInDb = context.IdentifiedComplicationRepository.Add(identifiedComplication);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadIdentifiedComplicationByKey", new { key = identifiedComplication.InteractionId }, identifiedComplication);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedComplication", "IdentifiedComplicationController.cs", ex.Message, identifiedComplication.CreatedIn, identifiedComplication.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complications
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedComplications)]
        public async Task<IActionResult> ReadIdentifiedComplications()
        {
            try
            {
                var identifiedComplicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplications();

                return Ok(identifiedComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedComplications", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complication/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedComplications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedComplicationByKey)]
        public async Task<IActionResult> ReadIdentifiedComplicationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedComplicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplicationByKey(key);

                if (identifiedComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedComplicationByKey", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/risk-assessment/risk-assessment-by-hts/{complicationId}
        /// </summary>
        /// <param name="complicationId">Foreign key of the table IdentifiedComplications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedComplicationByComplication)]
        public async Task<IActionResult> ReadIdentifiedComplicationByComplication(Guid complicationId)
        {
            try
            {
                if (complicationId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                //var complicationInDb = await context.IdentifiedComplicationRepository.LoadListWithChildAsync<IEnumerable<IdentifiedComplication>>(x => x.ComplicationId == complicationId, p => p.ComplicationType);
                var complicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplicationByComplication(complicationId);

                if (complicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(complicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedComplicationByComplication", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complication/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedComplicationByEncounter)]
        public async Task<IActionResult> ReadIdentifiedComplicationByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedComplicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplicationByEncounter(encounterId);

                if (identifiedComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedComplicationByEncounter", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedComplications.</param>
        /// <param name="identifiedComplication">IdentifiedComplication to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedComplication)]
        public async Task<IActionResult> UpdateIdentifiedComplication(Guid key, IdentifiedComplication identifiedComplication)
        {
            try
            {
                if (key != identifiedComplication.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedComplication.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedComplication.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedComplication.DateModified = DateTime.Now;
                identifiedComplication.IsDeleted = false;
                identifiedComplication.IsSynced = false;

                context.IdentifiedComplicationRepository.Update(identifiedComplication);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedComplication", "IdentifiedComplicationController.cs", ex.Message, identifiedComplication.ModifiedIn, identifiedComplication.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedComplications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedComplication)]
        public async Task<IActionResult> DeleteIdentifiedComplication(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedComplicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplicationByKey(key);

                if (identifiedComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedComplicationInDb.DateModified = DateTime.Now;
                identifiedComplicationInDb.IsDeleted = true;
                identifiedComplicationInDb.IsSynced = false;

                context.IdentifiedComplicationRepository.Update(identifiedComplicationInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedComplication", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-complication/remove/{EncounterID}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedComplication)]
        public async Task<IActionResult> RemoveIdentifiedComplication(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedComplicationInDb = await context.IdentifiedComplicationRepository.GetIdentifiedComplicationByEncounter(encounterId);

                if (identifiedComplicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedComplicationInDb.ToList())
                {
                    context.IdentifiedComplicationRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(identifiedComplicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedComplication", "IdentifiedComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}