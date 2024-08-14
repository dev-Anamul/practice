using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 08-04-2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedTBFinding controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    //[Authorize]

    public class IdentifiedTBFindingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedTBFindingController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedTBFindingController(IUnitOfWork context, ILogger<IdentifiedTBFindingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-tb-finding
        /// </summary>
        /// <param name="identifiedTBFinding">IdentifiedTBFinding object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedTBFinding)]
        public async Task<IActionResult> CreateIdentifiedTBFinding(IdentifiedTBFinding identifiedTBFinding)
        {
            try
            {
                foreach (var item in identifiedTBFinding.IdentifiedTBFindingList)
                {
                    var identifiedTBFindings = await context.IdentifiedTBFindingRepository.LoadWithChildAsync<IdentifiedTBFinding>(x => x.EncounterId == identifiedTBFinding.EncounterId
                    && x.ClientId == identifiedTBFinding.ClientId
                    && x.TBFindingId == item
                    && x.IsDeleted == false);

                    if (identifiedTBFindings == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedTBFinding, identifiedTBFinding.EncounterType);
                        interaction.EncounterId = identifiedTBFinding.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = identifiedTBFinding.CreatedBy;
                        interaction.CreatedIn = identifiedTBFinding.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedTBFinding.InteractionId = interactionId;
                        identifiedTBFinding.ClientId = identifiedTBFinding.ClientId;
                        identifiedTBFinding.TBFindingId = item;
                        identifiedTBFinding.DateCreated = DateTime.Now;
                        identifiedTBFinding.IsDeleted = false;
                        identifiedTBFinding.IsSynced = false;

                        context.IdentifiedTBFindingRepository.Add(identifiedTBFinding);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadIdentifiedTBFindingByKey", new { key = identifiedTBFinding.InteractionId }, identifiedTBFinding);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedTBFinding", "IdentifiedTBFindingController.cs", ex.Message, identifiedTBFinding.CreatedIn, identifiedTBFinding.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-findings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBFindings)]
        public async Task<IActionResult> ReadIdentifiedTBFindings()
        {
            try
            {
                var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindings();

                return Ok(identifiedTBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBFindings", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tbFinding/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBFindingByClient)]
        public async Task<IActionResult> ReadIdentifiedTBFindingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
              //      var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByClient(clientId);
                    var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByClientLast24Hours(clientId);

                    return Ok(identifiedTBFindingInDb);
                }
                else
                {
                    var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<IdentifiedTBFinding> identifiedTBFindingWithPaggingDto = new PagedResultDto<IdentifiedTBFinding>()
                    {
                        Data = identifiedTBFindingInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByClientTotalCount(clientId, encounterType)
                    };


                    return Ok(identifiedTBFindingWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBFindingByClient", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-finding/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBFinding.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBFindingByKey)]
        public async Task<IActionResult> ReadIdentifiedTBFindingByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByKey(key);

                if (identifiedTBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedTBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBFindingByKey", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tbFinding-by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBFindingByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedTBFindingEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByEncounterId(encounterId);

                if (identifiedTBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedTBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBFindingEncounterId", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-finding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBFinding.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedTBFinding)]
        public async Task<IActionResult> DeleteIdentifiedTBFinding(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByKey(key);

                if (identifiedTBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedTBFindingInDb.DateModified = DateTime.Now;
                identifiedTBFindingInDb.IsDeleted = true;
                identifiedTBFindingInDb.IsSynced = false;

                context.IdentifiedTBFindingRepository.Update(identifiedTBFindingInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedTBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedTBFinding", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/update-identified-tbfinding/{key}
        /// </summary>
        /// <param name="identifiedTBFinding">IdentifiedTBFinding object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedTBFinding)]
        public async Task<IActionResult> UpdateIdentifiedTBFinding(Guid key, IdentifiedTBFinding identifiedTBFinding)
        {
            try
            {

                if (identifiedTBFinding.IdentifiedTBFindingList?.Length > 0)
                {
                    var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.LoadListWithChildAsync<IdentifiedTBFinding>(x => x.EncounterId == identifiedTBFinding.EncounterId && x.ClientId == identifiedTBFinding.ClientId);

                    foreach(var item in identifiedTBFindingInDb)
                    {
                        context.InteractionRepository.Delete(await context.InteractionRepository.GetInteractionByKey(item.InteractionId));
                        context.IdentifiedTBFindingRepository.Delete(item);
                        await context.SaveChangesAsync();
                    }
                }


                foreach (var item in identifiedTBFinding.IdentifiedTBFindingList)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedTBFinding, identifiedTBFinding.EncounterType);
                    interaction.EncounterId = identifiedTBFinding.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = identifiedTBFinding.CreatedBy;
                    interaction.CreatedIn = identifiedTBFinding.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);

                    identifiedTBFinding.InteractionId = interactionId;
                    identifiedTBFinding.ClientId = identifiedTBFinding.ClientId;
                    identifiedTBFinding.TBFindingId = item;
                    identifiedTBFinding.DateCreated = DateTime.Now;
                    identifiedTBFinding.IsDeleted = false;
                    identifiedTBFinding.IsSynced = false;

                    context.IdentifiedTBFindingRepository.Add(identifiedTBFinding);
                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadIdentifiedTBFindingByKey", new { key = identifiedTBFinding.InteractionId }, identifiedTBFinding);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedTBFinding", "IdentifiedTBFindingController.cs", ex.Message, identifiedTBFinding.CreatedIn, identifiedTBFinding.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tbFinding/{key}
        /// </summary>
        /// <param name="encounterId">Primary key of the table IdentifiedTBFinding.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedTBFinding)]
        public async Task<IActionResult> RemoveIdentifiedTBFinding(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTBFindingInDb = await context.IdentifiedTBFindingRepository.GetIdentifiedTBFindingByEncounterId(encounterId);

                if (identifiedTBFindingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedTBFindingInDb.ToList())
                {
                    context.IdentifiedTBFindingRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(identifiedTBFindingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedTBFinding", "IdentifiedTBFindingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}