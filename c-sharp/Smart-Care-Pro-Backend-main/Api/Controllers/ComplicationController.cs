using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Lion
 * Date created  : 13.04.2023
* Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */

namespace Api.Controllers
{
    /// <summary>
    /// Complication controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ComplicationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ComplicationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ComplicationController(IUnitOfWork context, ILogger<ComplicationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/complication
        /// </summary>
        /// <param name="complication">Complication object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateComplication)]
        public async Task<IActionResult> CreateComplication(Complication complication)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Complication, complication.EncounterType);
                interaction.EncounterId = complication.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = complication.CreatedBy;
                interaction.CreatedIn = complication.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                complication.InteractionId = interactionId;
                complication.DateCreated = DateTime.Now;
                complication.IsDeleted = false;
                complication.IsSynced = false;
                complication.VMMCServiceId = complication.VMMCServiceId;

                context.ComplicationRepository.Add(complication);
                await context.SaveChangesAsync();

                if (complication.IdentifiedComplicationList != null)
                {
                    foreach (var item in complication.IdentifiedComplicationList)
                    {
                        IdentifiedComplication identifiedComplication = new IdentifiedComplication();

                        identifiedComplication.InteractionId = Guid.NewGuid();
                        identifiedComplication.ComplicationTypeId = item;
                        identifiedComplication.ComplicationId = complication.InteractionId;
                        identifiedComplication.CreatedBy = complication.CreatedBy;
                        identifiedComplication.CreatedIn = complication.CreatedIn;
                        identifiedComplication.DateCreated = DateTime.Now;
                        identifiedComplication.IsSynced = false;
                        identifiedComplication.IsDeleted = false;

                        context.IdentifiedComplicationRepository.Add(identifiedComplication);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadComplicationByKey", new { key = complication.InteractionId }, complication);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateComplication", "ComplicationController.cs", ex.Message, complication.CreatedIn, complication.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplications)]
        public async Task<IActionResult> ReadComplications()
        {
            try
            {
                var complicationIndb = await context.ComplicationRepository.GetComplications();

                complicationIndb = complicationIndb.OrderByDescending(x => x.DateCreated);

                return Ok(complicationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplications", "ComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Complications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplicationByKey)]
        public async Task<IActionResult> ReadComplicationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationIndb = await context.ComplicationRepository.GetComplicationByKey(key);

                if (complicationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                complicationIndb.IdentifiedComplications = await context.IdentifiedComplicationRepository.LoadListWithChildAsync<IdentifiedComplication>(x => x.ComplicationId == complicationIndb.InteractionId, p => p.ComplicationType);

                return Ok(complicationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplicationByKey", "ComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication/ByEncounter/{EncounterID}
        /// </summary>
        /// <param name="EncounterID">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplicationByEncounter)]
        public async Task<IActionResult> ReadComplicationByEncounter(Guid EncounterID)
        {
            try
            {
                if (EncounterID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationInDb = await context.ComplicationRepository.GetComplicationByEncounter(EncounterID);

                if (complicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(complicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplicationByEncounter", "ComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication/by-vmmc-service/{vmmcServiceId}
        /// </summary>
        /// <param name="VMMCServiceID">Foreign key of the table Complications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadComplicationByVMMCService)]
        public async Task<IActionResult> ReadComplicationByVMMCService(Guid vmmcServiceId)
        {
            try
            {
                if (vmmcServiceId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationInDb = await context.ComplicationRepository.GetComplicationByVMMCService(vmmcServiceId);

                if (complicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(complicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadComplicationByVMMCService", "ComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Complications.</param>
        /// <param name="complication">Complication to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateComplication)]
        public async Task<IActionResult> UpdateComplication(Guid key, Complication complication)
        {
            try
            {
                if (key != complication.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                complication.DateModified = DateTime.Now;
                complication.IsDeleted = false;
                complication.IsSynced = false;

                context.ComplicationRepository.Update(complication);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateComplication", "ComplicationController.cs", ex.Message, complication.ModifiedIn, complication.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/complication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Complications.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteComplication)]
        public async Task<IActionResult> DeleteComplication(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var complicationIndb = await context.ComplicationRepository.GetComplicationByKey(encounterId);

                if (complicationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                complicationIndb.DateModified = DateTime.Now;
                complicationIndb.IsDeleted = true;
                complicationIndb.IsSynced = false;

                context.ComplicationRepository.Update(complicationIndb);
                await context.SaveChangesAsync();

                return Ok(complicationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteComplication", "ComplicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}