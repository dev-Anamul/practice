using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 21.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PEPRiskStatus controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PEPRiskStatusController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PEPRiskStatusController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PEPRiskStatusController(IUnitOfWork context, ILogger<PEPRiskStatusController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pep-risk-status
        /// </summary>
        /// <param name="pEPRiskStatus">PEPRiskStatus object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPRiskStatus)]
        public async Task<ActionResult<RiskStatus>> CreatePEPRiskStatus(RiskStatus pEPRiskStatus)
        {
            try
            {
                foreach (var item in pEPRiskStatus.RiskList)
                {
                    var pepRisk = await context.PEPRiskStatusRepository.LoadWithChildAsync<RiskStatus>(x => x.EncounterId == pEPRiskStatus.EncounterId && x.ClientId == pEPRiskStatus.ClientId && x.IsDeleted == false && x.RiskId == item);

                    if (pepRisk == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.RiskStatus, pEPRiskStatus.EncounterType);
                        interaction.EncounterId = pEPRiskStatus.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = pEPRiskStatus.CreatedBy;
                        interaction.CreatedIn = pEPRiskStatus.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        pEPRiskStatus.InteractionId = interactionId;
                        pEPRiskStatus.ClientId = pEPRiskStatus.ClientId;
                        pEPRiskStatus.EncounterType = pEPRiskStatus.EncounterType;
                        pEPRiskStatus.RiskId = item;


                        pEPRiskStatus.CreatedBy = pEPRiskStatus.CreatedBy;
                        pEPRiskStatus.CreatedIn = pEPRiskStatus.CreatedIn;
                        pEPRiskStatus.DateCreated = DateTime.Now;
                        pEPRiskStatus.IsDeleted = false;
                        pEPRiskStatus.IsSynced = false;

                        context.PEPRiskStatusRepository.Add(pEPRiskStatus);
                        await context.SaveChangesAsync();
                    }
                }
                return CreatedAtAction("ReadPEPRiskStatusByKey", new { key = pEPRiskStatus.InteractionId }, pEPRiskStatus);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPRiskStatus", "PEPRiskStatusController.cs", ex.Message, pEPRiskStatus.CreatedIn, pEPRiskStatus.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risk-statuses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRiskStatuses)]
        public async Task<IActionResult> ReadPEPRiskStatuses()
        {
            try
            {
                var pEPRiskStatusInDb = await context.PEPRiskStatusRepository.GetPEPRiskStatuses();

                return Ok(pEPRiskStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPRiskStatuses", "PEPRiskStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-risk-statuses/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRiskStautsByEncounterId)]
        public async Task<IActionResult> ReadPEPRiskStautsByEncounterId(Guid key)
        {
            try
            {
                var pEPRiskStatusInDb = await context.PEPRiskStatusRepository.GetPEPRiskStatusByEncounterID(key);

                return Ok(pEPRiskStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPRiskStautsByEncounterId", "PEPRiskStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/identified-risk-status/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PEPRiskStatus.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRiskStatusByKey)]
        public async Task<IActionResult> ReadPEPRiskStatusByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pepriskStatusInDb = await context.PEPRiskStatusRepository.GetPEPRiskStatusByKey(key);

                if (pepriskStatusInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pepriskStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPRiskStatusByKey", "PEPRiskStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-risk-status/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">clientId of the table PEPRiskStatus.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPRiskStatusByClient)]
        public async Task<IActionResult> ReadPEPStatusByClientID(Guid clientId)
        {
            try
            {
                var pepstatusInDb = await context.PEPRiskStatusRepository.GetPEPRiskStatusByClientID(clientId);

                return Ok(pepstatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPStatusByClientID", "PEPRiskStatusController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}