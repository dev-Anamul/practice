using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 20.02.2024
 * Modified by   : Stephan
 * Last modified : 22.02.2024
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class PreScreeningVisitController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PreScreeningVisitController> logger;
        public PreScreeningVisitController(IUnitOfWork context, ILogger<PreScreeningVisitController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pre-screening-visit
        /// </summary>
        /// <param name="pre-screening-visit">pre-screening-visit object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePreScreeningVisit)]
        public async Task<IActionResult> CreatePreScreeningVisit(PreScreeningVisit preScreeningVisit)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PreScreeningVisit, preScreeningVisit.EncounterType);
                interaction.EncounterId = preScreeningVisit.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = preScreeningVisit.CreatedBy;
                interaction.CreatedIn = preScreeningVisit.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                preScreeningVisit.InteractionId = interactionId;
                preScreeningVisit.DateCreated = DateTime.Now;
                preScreeningVisit.IsDeleted = false;
                preScreeningVisit.IsSynced = false;

                context.PreScreeningVisitRepository.Add(preScreeningVisit);

                if(preScreeningVisit.IsOnART != null)
                {
                    var getClient = await context.ClientRepository.GetClientByKey(preScreeningVisit.ClientId);

                    if (getClient != null)
                    {
                        if (getClient.IsOnART == false && preScreeningVisit.IsOnART == false)
                            getClient.IsOnART = false;
                        else if (getClient.IsOnART == false && preScreeningVisit.IsOnART == true)
                            getClient.IsOnART = true;
                        else
                            getClient.IsOnART = true;

                        context.ClientRepository.Update(getClient);
                    }
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPreScreeningVisitByKey", new { key = preScreeningVisit.InteractionId }, preScreeningVisit);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "PreScreeningVisit", "PreScreeningVisitController.cs", ex.Message, preScreeningVisit.CreatedIn, preScreeningVisit.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pre-screening-Visits
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreScreeningVisits)]
        public async Task<IActionResult> ReadPreScreeningVisits()
        {
            try
            {
                var preScreeningVisitsInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisit();
 
                return Ok(preScreeningVisitsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreScreeningVisit", "PreScreeningVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pre-screening-Visits/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreScreeningVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreScreeningVisitByKey)]
        public async Task<IActionResult> ReadPreScreeningVisitByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var preScreeningVisitInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisitByKey(key);

                var getArtStatus = await context.ClientRepository.GetClientByKey(preScreeningVisitInDb.ClientId);

                preScreeningVisitInDb.IsOnART = getArtStatus.IsOnART;

                if (preScreeningVisitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(preScreeningVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreScreeningVisitsByKey", "PreScreeningVisitsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// pre-screening-visit-by-client/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreScreeningVisitByClient)]
        public async Task<IActionResult> ReadPreScreeningVisitByClient(Guid clientId)
        {
            try
            {
                var preScreeningVisitInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisitbyClienId(clientId);

                return Ok(preScreeningVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreScreeningVisitByClient", "PreScreeningVisitClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/pre-screening-visit/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPreScreeningVisitByEncounter)]
        public async Task<IActionResult> ReadPreScreeningVisitByEncounter(Guid encounterId)
        {
            try
            {
                var preScreeningVisitInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisitsByEncounter(encounterId);

                return Ok(preScreeningVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPreScreeningVisitByEncounter", "PreScreeningVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/pre-screening-visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreScreeningVisit.</param>
        /// <param name="preScreeningVisit">PreScreening to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePreScreeningVisit)]
        public async Task<IActionResult> UpdatePreScreeningVisit(Guid key, PreScreeningVisit preScreeningVisit)
        {
            try
            {
                if (key != preScreeningVisit.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var preScreeningVisitInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisitByKey(key);

                if (preScreeningVisitInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                preScreeningVisit.DateModified = DateTime.Now;
                preScreeningVisit.IsDeleted = false;
                preScreeningVisit.IsSynced = false;

                context.PreScreeningVisitRepository.Update(preScreeningVisit);

                if (preScreeningVisit.IsOnART != null)
                {
                    var getClient = await context.ClientRepository.GetClientByKey(preScreeningVisit.ClientId);

                    if (getClient != null)
                    {
                        if (getClient.IsOnART == false && preScreeningVisit.IsOnART == false)
                            getClient.IsOnART = false;
                        else if (getClient.IsOnART == false && preScreeningVisit.IsOnART == true)
                            getClient.IsOnART = true;
                        else
                            getClient.IsOnART = true;

                        context.ClientRepository.Update(getClient);
                    }
                }

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdatePreScreeningVisit", "PreScreeningVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/pre-Screening-Visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PreScreeningVisit.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePreScreeningVisit)]
        public async Task<IActionResult> DeletePreScreeningVisit(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var preScreeningVisitInDb = await context.PreScreeningVisitRepository.GetPreScreeningVisitByKey(key);

                if (preScreeningVisitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                preScreeningVisitInDb.IsDeleted = true;
                preScreeningVisitInDb.IsSynced = false;
                preScreeningVisitInDb.DateModified = DateTime.Now;

                context.PreScreeningVisitRepository.Update(preScreeningVisitInDb);
                await context.SaveChangesAsync();
                
                return Ok(preScreeningVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePreScreeningVisit", "PreScreeningVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}