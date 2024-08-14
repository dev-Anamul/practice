using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Tomas
 * Date created  : 10.04.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTResponse controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTResponseController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<ARTResponseController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTResponseController(IUnitOfWork context, ILogger<ARTResponseController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-response
        /// </summary>
        /// <param name="aRTResponse">ARTResponse object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTResponse)]
        public async Task<IActionResult> CreateARTResponse(ARTResponse artResponse)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ARTResponse, artResponse.EncounterType);
                interaction.EncounterId = artResponse.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = artResponse.CreatedBy;
                interaction.CreatedIn = artResponse.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                artResponse.InteractionId = interactionId;
                artResponse.DateCreated = DateTime.Now;
                artResponse.IsDeleted = false;
                artResponse.IsSynced = false;

                context.ARTResponseRepository.Add(artResponse);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadARTResponseByKey", new { key = artResponse.InteractionId }, artResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTResponse", "ARTResponseController.cs", ex.Message, artResponse.CreatedIn, artResponse.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-responses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTResponses)]
        public async Task<IActionResult> ReadARTResponses()
        {
            try
            {
                var aRTResponseIndb = await context.ARTResponseRepository.GetARTResponses();

                aRTResponseIndb = aRTResponseIndb.OrderByDescending(x => x.DateCreated);

                return Ok(aRTResponseIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTResponses", "ARTResponseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-response/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTResponseByClient)]
        public async Task<IActionResult> ReadARTResponseByClient(Guid clientId)
        {
            try
            {
                var aRTResponseIndb = await context.ARTResponseRepository.GetARTResponseByClient(clientId);

                return Ok(aRTResponseIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTResponseByClient", "ARTResponseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/art-response/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTResponses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTResponseByKey)]
        public async Task<IActionResult> ReadARTResponseByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artResponseIndb = await context.ARTResponseRepository.GetARTResponseByKey(key);

                if (artResponseIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(artResponseIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTResponseByKey", "ARTResponseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/art-response/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTResponses.</param>
        /// <param name="artResponse">ARTResponses to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTResponse)]
        public async Task<IActionResult> UpdateARTResponse(Guid key, ARTResponse artResponse)
        {
            try
            {
                if (key != artResponse.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = artResponse.ModifiedBy;
                interactionInDb.ModifiedIn = artResponse.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                artResponse.DateModified = DateTime.Now;
                artResponse.IsDeleted = false;
                artResponse.IsSynced = false;

                context.ARTResponseRepository.Update(artResponse);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateARTResponse", "ARTResponseController.cs", ex.Message, artResponse.CreatedIn, artResponse.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-response/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTResponses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteARTResponse)]
        public async Task<IActionResult> DeleteARTResponse(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artResponseInDb = await context.ARTResponseRepository.GetARTResponseByKey(key);

                if (artResponseInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                artResponseInDb.DateModified = DateTime.Now;
                artResponseInDb.IsDeleted = true;
                artResponseInDb.IsSynced = false;

                context.ARTResponseRepository.Update(artResponseInDb);
                await context.SaveChangesAsync();

                return Ok(artResponseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteARTResponse", "ARTResponseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}