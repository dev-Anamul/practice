using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 07.02.2023
 * Modified by  : Lion
 * Last modified: 20.02.2023
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Covax controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CovaxController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CovaxController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CovaxController(IUnitOfWork context, ILogger<CovaxController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/covax
        /// </summary>
        /// <param name="covax">Covax object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCovax)]
        public async Task<IActionResult> CreateCovax(Covax covax)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Covax, Enums.EncounterType.Covax);
                interaction.EncounterId = covax.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = covax.CreatedBy;
                interaction.CreatedIn = covax.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                covax.InteractionId = interactionId;
                covax.DateCreated = DateTime.Now;
                covax.IsDeleted = false;
                covax.IsSynced = false;

                context.CovaxRepository.Add(covax);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCovaxByKey", new { key = covax.InteractionId }, covax);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCovax", "CovaxController.cs", ex.Message, covax.CreatedIn, covax.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covaxes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovaxes)]
        public async Task<IActionResult> ReadCovaxes()
        {
            try
            {
                var covaxInDb = await context.CovaxRepository.GetCovaxes();

                covaxInDb = covaxInDb.OrderByDescending(x => x.DateCreated);

                return Ok(covaxInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovaxes", "CovaxController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covax/byClient/{ClientID}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovaxByClient)]
        public async Task<IActionResult> ReadCovaxByClient(Guid clientId)
        {
            try
            {
                var covaxInDb = await context.CovaxRepository.GetCovaxByClient(clientId);

                return Ok(covaxInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovaxByClient", "CovaxController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covax/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Covaxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovaxByKey)]
        public async Task<IActionResult> ReadCovaxByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covaxInDb = await context.CovaxRepository.GetCovaxByKey(key);

                if (covaxInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(covaxInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovaxByKey", "CovaxController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covax/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Covaxes.</param>
        /// <param name="covax">Covax to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCovax)]
        public async Task<IActionResult> UpdateCovax(Guid key, Covax covax)
        {
            try
            {
                if (key != covax.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = covax.ModifiedBy;
                interactionInDb.ModifiedIn = covax.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                covax.DateModified = DateTime.Now;
                covax.IsDeleted = false;
                covax.IsSynced = false;

                context.CovaxRepository.Update(covax);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCovax", "CovaxController.cs", ex.Message, covax.ModifiedIn, covax.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covax/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Covaxes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCovax)]
        public async Task<IActionResult> DeleteCovax(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covaxInDb = await context.CovaxRepository.GetCovaxByKey(key);

                if (covaxInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                covaxInDb.DateModified = DateTime.Now;
                covaxInDb.IsDeleted = true;
                covaxInDb.IsSynced = false;

                context.CovaxRepository.Update(covaxInDb);
                await context.SaveChangesAsync();

                return Ok(covaxInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCovax", "CovaxController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}