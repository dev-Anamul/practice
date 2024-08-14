using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Stephan
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// VeginalPosition controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VaginalPositionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VaginalPositionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VaginalPositionController(IUnitOfWork context, ILogger<VaginalPositionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/veginal-position
        /// </summary>
        /// <param name="veginalPosition">VeginalPosition object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVaginalPosition)]
        public async Task<IActionResult> CreateVaginalPosition(VaginalPosition vaginalPosition)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.VaginalPosition, vaginalPosition.EncounterType);
                interaction.EncounterId = vaginalPosition.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = vaginalPosition.CreatedBy;
                interaction.CreatedIn = vaginalPosition.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                vaginalPosition.InteractionId = interactionId;
                vaginalPosition.DateCreated = DateTime.Now;
                vaginalPosition.IsDeleted = false;
                vaginalPosition.IsSynced = false;

                context.VaginalPositionRepository.Add(vaginalPosition);
                await context.SaveChangesAsync();

                return CreatedAtAction("CreateVaginalPosition", new { key = vaginalPosition.InteractionId }, vaginalPosition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVaginalPosition", "VaginalPositionController.cs", ex.Message, vaginalPosition.CreatedIn, vaginalPosition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/veginal-positions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaginalPositions)]
        public async Task<IActionResult> ReadVaginalPositions()
        {
            try
            {
                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositions();
                veginalPositionInDb = veginalPositionInDb.OrderByDescending(x => x.DateCreated);
                return Ok(veginalPositionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaginalPositions", "VaginalPositionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/veginalPosition/ByClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaginalPositionByClient)]
        public async Task<IActionResult> ReadVeginalPositionByClient(Guid clientId)
        {
            try
            {
                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositionByClient(clientId);

                return Ok(veginalPositionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVeginalPositionByClient", "VaginalPositionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/veginalPosition/ByEncounter/{EncounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaginalPositionByEncounter)]
        public async Task<IActionResult> ReadVeginalPositionByEncounter(Guid encounterId)
        {
            try
            {
                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositionByEncounter(encounterId);

                return Ok(veginalPositionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVeginalPositionByEncounter", "VaginalPositionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/veginal-position/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VeginalPositions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaginalPositionByKey)]
        public async Task<IActionResult> ReadVeginalPositionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositionByKey(key);

                if (veginalPositionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(veginalPositionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVeginalPositionByKey", "VaginalPositionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/veginal-position/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VeginalPositions.</param>
        /// <param name="veginalPosition">VeginalPosition to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVaginalPosition)]
        public async Task<IActionResult> UpdateVeginalPosition(Guid key, VaginalPosition veginalPosition)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = veginalPosition.ModifiedBy;
                interactionInDb.ModifiedIn = veginalPosition.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != veginalPosition.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositionByKey(key);

                if (veginalPositionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                veginalPositionInDb.DateModified = DateTime.Now;
                veginalPositionInDb.ModifiedBy = veginalPosition.ModifiedBy;
                veginalPositionInDb.ModifiedIn = veginalPosition.ModifiedIn;
                veginalPositionInDb.IsDeleted = false;
                veginalPositionInDb.IsSynced = false;
                veginalPositionInDb.Position = veginalPosition.Position;
                veginalPositionInDb.Note = veginalPosition.Note;

                context.VaginalPositionRepository.Update(veginalPositionInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVeginalPosition", "VaginalPositionController.cs", ex.Message, veginalPosition.ModifiedIn, veginalPosition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/veginal-position/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VeginalPositions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVaginalPosition)]
        public async Task<IActionResult> DeleteVeginalPosition(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var veginalPositionInDb = await context.VaginalPositionRepository.GetVaginalPositionByKey(key);

                if (veginalPositionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                veginalPositionInDb.DateModified = DateTime.Now;
                veginalPositionInDb.IsDeleted = true;
                veginalPositionInDb.IsSynced = false;

                context.VaginalPositionRepository.Update(veginalPositionInDb);
                await context.SaveChangesAsync();

                return Ok(veginalPositionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVeginalPosition", "VaginalPositionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}