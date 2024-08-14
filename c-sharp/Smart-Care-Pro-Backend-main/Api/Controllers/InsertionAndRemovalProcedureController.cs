using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// InsertionAndRemovalProcedure controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class InsertionAndRemovalProcedureController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<InsertionAndRemovalProcedureController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public InsertionAndRemovalProcedureController(IUnitOfWork context, ILogger<InsertionAndRemovalProcedureController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure
        /// </summary>
        /// <param name="insertionAndRemovalProcedure">InsertionAndRemovalProcedure object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateInsertionAndRemovalProcedure)]
        public async Task<IActionResult> CreateInsertionAndRemovalProcedure(InsertionAndRemovalProcedure insertionAndRemovalProcedure)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionID = Guid.NewGuid();

                interaction.Oid = interactionID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.InsertionAndRemovalProcedure, insertionAndRemovalProcedure.EncounterType);
                interaction.EncounterId = insertionAndRemovalProcedure.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = insertionAndRemovalProcedure.CreatedBy;
                interaction.CreatedIn = insertionAndRemovalProcedure.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                insertionAndRemovalProcedure.InteractionId = interactionID;
                insertionAndRemovalProcedure.DateCreated = DateTime.Now;
                insertionAndRemovalProcedure.IsDeleted = false;
                insertionAndRemovalProcedure.IsSynced = false;

                context.InsertionAndRemovalProcedureRepository.Add(insertionAndRemovalProcedure);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadInsertionAndRemovalProcedureByKey", new { key = insertionAndRemovalProcedure.InteractionId }, insertionAndRemovalProcedure);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateInsertionAndRemovalProcedure", "InsertionAndRemovalProcedureController.cs", ex.Message, insertionAndRemovalProcedure.CreatedIn, insertionAndRemovalProcedure.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedures
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInsertionAndRemovalProcedures)]
        public async Task<IActionResult> ReadInsertionAndRemovalProcedures()
        {
            try
            {
                var insertionAndRemovalProcedureInDb = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedures();

                return Ok(insertionAndRemovalProcedureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInsertionAndRemovalProcedures", "InsertionAndRemovalProcedureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInsertionAndRemovalProcedureByKey)]
        public async Task<IActionResult> ReadInsertionAndRemovalProcedureByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var insertionAndRemovalProcedureIndb = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedureByKey(key);

                if (insertionAndRemovalProcedureIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(insertionAndRemovalProcedureIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInsertionAndRemovalProcedureByKey", "InsertionAndRemovalProcedureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInsertionAndRemovalProcedureByClient)]
        public async Task<IActionResult> ReadInsertionAndRemovalProcedureByClient(Guid clientId)
        {
            try
            {
                var insertionAndRemovalProcedureInDb = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedureByClient(clientId);

                return Ok(insertionAndRemovalProcedureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInsertionAndRemovalProcedureByClient", "InsertionAndRemovalProcedureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure/byEncounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInsertionAndRemovalProcedureByEncounter)]
        public async Task<IActionResult> ReadInsertionAndRemovalProcedureByEncounter(Guid encounterId)
        {
            try
            {
                var insertionAndRemovalProcedureInDb = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedureByEncounter(encounterId);

                return Ok(insertionAndRemovalProcedureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInsertionAndRemovalProcedureByEncounter", "InsertionAndRemovalProcedureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure/{key}
        /// </summary>
        /// <param name="key">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <param name="insertionAndRemovalProcedure">InsertionAndRemovalProcedure to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateInsertionAndRemovalProcedure)]
        public async Task<IActionResult> UpdateInsertionAndRemovalProcedure(Guid key, InsertionAndRemovalProcedure insertionAndRemovalProcedure)
        {
            try
            {
                if (key != insertionAndRemovalProcedure.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = insertionAndRemovalProcedure.ModifiedBy;
                interactionInDb.ModifiedIn = insertionAndRemovalProcedure.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                insertionAndRemovalProcedure.DateModified = DateTime.Now;
                insertionAndRemovalProcedure.IsDeleted = false;
                insertionAndRemovalProcedure.IsSynced = false;

                context.InsertionAndRemovalProcedureRepository.Update(insertionAndRemovalProcedure);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateInsertionAndRemovalProcedure", "InsertionAndRemovalProcedureController.cs", ex.Message, insertionAndRemovalProcedure.ModifiedIn, insertionAndRemovalProcedure.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/insertion-and-removal-procedure/{key}
        /// </summary>
        /// <param name="key">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteInsertionAndRemovalProcedure)]
        public async Task<IActionResult> DeleteInsertionAndRemovalProcedure(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var insertionAndRemovalProcedureInDb = await context.InsertionAndRemovalProcedureRepository.GetInsertionAndRemovalProcedureByKey(key);

                if (insertionAndRemovalProcedureInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.InsertionAndRemovalProcedureRepository.Update(insertionAndRemovalProcedureInDb);
                await context.SaveChangesAsync();

                return Ok(insertionAndRemovalProcedureInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteInsertionAndRemovalProcedure", "InsertionAndRemovalProcedureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}