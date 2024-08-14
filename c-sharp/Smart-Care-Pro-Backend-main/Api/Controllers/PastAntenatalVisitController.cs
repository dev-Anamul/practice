using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PastAntenatalVisit controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PastAntenatalVisitController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PastAntenatalVisitController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PastAntenatalVisitController(IUnitOfWork context, ILogger<PastAntenatalVisitController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visit
        /// </summary>
        /// <param name="pastAntenatalVisit">PastAntenatalVisit object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePastAntenatalVisit)]
        public async Task<IActionResult> CreatePastAntenatalVisit(PastAntenatalVisit pastAntenatalVisit)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();
                foreach (var item in pastAntenatalVisit.PastAntenatalVisitList)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PastAntenatalVisit, pastAntenatalVisit.EncounterType);
                    interaction.EncounterId = pastAntenatalVisit.EncounterId;
                    interaction.CreatedBy = pastAntenatalVisit.CreatedBy;
                    interaction.CreatedIn = pastAntenatalVisit.CreatedIn;

                    interaction.DateCreated = DateTime.Now;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionId;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.EncounterType = pastAntenatalVisit.EncounterType;
                    item.EncounterId = pastAntenatalVisit.EncounterId;
                    item.ClientId = pastAntenatalVisit.ClientId;
                    item.CreatedBy = pastAntenatalVisit.CreatedBy;
                    item.CreatedIn = pastAntenatalVisit.CreatedIn;
                }

                context.InteractionRepository.AddRange(interactions);
                context.PastAntenatalVisitRepository.AddRange(pastAntenatalVisit.PastAntenatalVisitList);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPastAntenatalVisitByKey", new { key = pastAntenatalVisit.InteractionId }, pastAntenatalVisit);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePastAntenatalVisit", "PastAntenatalVisitController.cs", ex.Message, pastAntenatalVisit.CreatedIn, pastAntenatalVisit.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visits
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastAntenatalVisits)]
        public async Task<IActionResult> ReadPastAntenatalVisits()
        {
            try
            {
                var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisits();

                return Ok(pastAntenatalVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastAntenatalVisits", "PastAntenatalVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visit/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastAntenatalVisitByClient)]
        public async Task<IActionResult> ReadPastAntenatalVisitByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByClient(clientId);

                    return Ok(pastAntenatalVisitInDb);
                }
                else
                {
                    var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<PastAntenatalVisit> pastAntenatalVisitDto = new PagedResultDto<PastAntenatalVisit>()
                    {
                        Data = pastAntenatalVisitInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PastAntenatalVisitRepository.GetPastAntenatalVisitByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(pastAntenatalVisitDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastAntenatalVisitByClient", "PastAntenatalVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visit/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastAntenatalVisitByEncounter)]
        public async Task<IActionResult> ReadPastAntenatalVisitByEncounter(Guid encounterId)
        {
            try
            {
                var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByEncounter(encounterId);

                return Ok(pastAntenatalVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastAntenatalVisitByEncounter", "PastAntenatalVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastAntenatalVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPastAntenatalVisitByKey)]
        public async Task<IActionResult> ReadPastAntenatalVisitByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByKey(key);

                if (pastAntenatalVisitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pastAntenatalVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPastAntenatalVisitByKey", "PastAntenatalVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/past-antenatal-visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastAntenatalVisits.</param>
        /// <param name="pastAntenatalVisit">PastAntenatalVisit to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePastAntenatalVisit)]
        public async Task<IActionResult> UpdatePastAntenatalVisit(Guid key, PastAntenatalVisit pastAntenatalVisit)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pastAntenatalVisit.ModifiedBy;
                interactionInDb.ModifiedIn = pastAntenatalVisit.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pastAntenatalVisit.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var pastAntenatalVisitDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByKey(key);

                pastAntenatalVisitDb.Findings = pastAntenatalVisit.Findings;
                pastAntenatalVisitDb.IsAdmitted = pastAntenatalVisit.IsAdmitted;
                pastAntenatalVisitDb.VisitNo = pastAntenatalVisit.VisitNo;
                pastAntenatalVisitDb.VisitDate = pastAntenatalVisit.VisitDate;

                pastAntenatalVisitDb.ModifiedIn = pastAntenatalVisit.ModifiedIn;
                pastAntenatalVisitDb.ModifiedBy = pastAntenatalVisit.ModifiedBy;
                pastAntenatalVisitDb.DateModified = DateTime.Now;
                pastAntenatalVisitDb.IsDeleted = false;
                pastAntenatalVisitDb.IsSynced = false;

                context.PastAntenatalVisitRepository.Update(pastAntenatalVisitDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePastAntenatalVisit", "PastAntenatalVisitController.cs", ex.Message, pastAntenatalVisit.ModifiedIn, pastAntenatalVisit.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/past-antenatal-visit/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PastAntenatalVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePastAntenatalVisit)]
        public async Task<IActionResult> DeletePastAntenatalVisit(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pastAntenatalVisitInDb = await context.PastAntenatalVisitRepository.GetPastAntenatalVisitByKey(key);

                if (pastAntenatalVisitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pastAntenatalVisitInDb.DateModified = DateTime.Now;
                pastAntenatalVisitInDb.IsDeleted = true;
                pastAntenatalVisitInDb.IsSynced = false;

                context.PastAntenatalVisitRepository.Update(pastAntenatalVisitInDb);
                await context.SaveChangesAsync();

                return Ok(pastAntenatalVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePastAntenatalVisit", "PastAntenatalVisitController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}