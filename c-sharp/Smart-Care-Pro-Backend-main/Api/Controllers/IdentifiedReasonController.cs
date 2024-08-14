using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 08.04.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedReason controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedReasonController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedReasonController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedReasonController(IUnitOfWork context, ILogger<IdentifiedReasonController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-reason
        /// </summary>
        /// <param name="identifiedReason">IdentifiedReason object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedReason)]
        public async Task<IActionResult> CreateIdentifiedReason(IdentifiedReason identifiedReason)
        {
            try
            {
                foreach (var item in identifiedReason.IdentifiedReasonList)
                {
                    var identifiedTBFindings = await context.IdentifiedReasonRepository.LoadWithChildAsync<IdentifiedReason>(x => x.EncounterId == identifiedReason.EncounterId
                    && x.ClientId == identifiedReason.ClientId
                    && x.TBSuspectingReasonId == item
                    && x.IsDeleted == false);

                    if (identifiedTBFindings == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedReason, identifiedReason.EncounterType);
                        interaction.EncounterId = identifiedReason.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = identifiedReason.CreatedBy;
                        interaction.CreatedIn = identifiedReason.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedReason.InteractionId = interactionId;
                        identifiedReason.ClientId = identifiedReason.ClientId;
                        identifiedReason.TBSuspectingReasonId = item;
                        identifiedReason.DateCreated = DateTime.Now;
                        identifiedReason.IsDeleted = false;
                        identifiedReason.IsSynced = false;

                        context.IdentifiedReasonRepository.Add(identifiedReason);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadIdentifiedReasonByKey", new { key = identifiedReason.InteractionId }, identifiedReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedReason", "IdentifiedReasonController.cs", ex.Message, identifiedReason.CreatedIn, identifiedReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-reasons
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReasons)]
        public async Task<IActionResult> ReadIdentifiedReasons()
        {
            try
            {
                var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasons();

                return Ok(identifiedReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReasons", "IdentifiedReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-reason/byClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReasonByClient)]
        public async Task<IActionResult> ReadIdentifiedReasonByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasonByClient(clientId);

                    return Ok(identifiedReasonInDb);
                }
                else
                {
                    var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasonByClient(clientId);
                    PagedResultDto<IdentifiedReason> identifiedTBFindingWithPaggingDto = new PagedResultDto<IdentifiedReason>
                    {
                        Data = identifiedReasonInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.IdentifiedReasonRepository.GetIdentifiedReasonByClientTotalCount(clientId, encounterType)
                    };
                    return Ok(identifiedTBFindingWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReasonByClient", "IdentifiedReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-reason/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReasonByKey)]
        public async Task<IActionResult> ReadIdentifiedReasonByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasonByKey(key);

                if (identifiedReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReasonByKey", "IdentifiedReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-reason/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedReasonByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedReasonByEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasonByEncounterId(encounterId);

                if (identifiedReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedReasonByEncounterId", "IdentifiedReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedReason)]
        public async Task<IActionResult> UpdateIdentifiedReason(Guid key, IdentifiedReason identifiedReason)
        {
            try
            {
                if (identifiedReason.IdentifiedReasonList?.Count() > 0)
                {

                    var identifiedReasonInDb = await context.IdentifiedReasonRepository.LoadListWithChildAsync<IdentifiedReason>(x => x.EncounterId == identifiedReason.EncounterId
                        && x.ClientId == identifiedReason.ClientId);

                    foreach(var item in identifiedReasonInDb)
                    {
                        context.InteractionRepository.Delete(await context.InteractionRepository.GetInteractionByKey(item.InteractionId));
                        context.IdentifiedReasonRepository.Delete(item);
                        context.SaveChangesAsync();
                    }
                }



                foreach (var item in identifiedReason.IdentifiedReasonList)
                {


                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedReason, identifiedReason.EncounterType);
                    interaction.EncounterId = identifiedReason.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = identifiedReason.CreatedBy;
                    interaction.CreatedIn = identifiedReason.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);

                    identifiedReason.InteractionId = interactionId;
                    identifiedReason.ClientId = identifiedReason.ClientId;
                    identifiedReason.TBSuspectingReasonId = item;
                    identifiedReason.DateCreated = DateTime.Now;
                    identifiedReason.IsDeleted = false;
                    identifiedReason.IsSynced = false;

                    context.IdentifiedReasonRepository.Add(identifiedReason);
                    await context.SaveChangesAsync();
                }


                return CreatedAtAction("ReadIdentifiedReasonByKey", new { key = identifiedReason.InteractionId }, identifiedReason);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedReason", "IdentifiedReasonController.cs", ex.Message, identifiedReason.CreatedIn, identifiedReason.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/identified-reason/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedReason.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedReason)]
        public async Task<IActionResult> RemoveIdentifiedReason(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedReasonInDb = await context.IdentifiedReasonRepository.GetIdentifiedReasonByEncounterId(key);

                if (identifiedReasonInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedReasonInDb)
                {
                    context.IdentifiedReasonRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(identifiedReasonInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedReason", "IdentifiedReasonController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}