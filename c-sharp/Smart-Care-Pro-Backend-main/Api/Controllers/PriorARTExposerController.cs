using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 30.03.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PriorARTExposer controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PriorARTExposerController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PriorARTExposerController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PriorARTExposerController(IUnitOfWork context, ILogger<PriorARTExposerController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposer
        /// </summary>
        /// <param name="priorARTExposer">PriorARTExposer object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePriorARTExposer)]
        public async Task<IActionResult> CreatePriorARTExposer(PriorARTExposure priorARTExposer)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PriorARTExposure, priorARTExposer.EncounterType);
                interaction.EncounterId = priorARTExposer.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedIn = priorARTExposer.CreatedIn;
                interaction.CreatedBy = priorARTExposer.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                priorARTExposer.InteractionId = interactionId;
                priorARTExposer.DateCreated = DateTime.Now;
                priorARTExposer.IsDeleted = false;
                priorARTExposer.IsSynced = false;

                if (priorARTExposer.ARTTakenDrugList != null && priorARTExposer.ARTTakenDrugList.Length > 0)
                {


                    foreach (var itemId in priorARTExposer.ARTTakenDrugList)
                    {
                        TakenARTDrug takenARTDrugs = new TakenARTDrug();
                        takenARTDrugs.EncounterId = priorARTExposer.EncounterId;
                        takenARTDrugs.CreatedBy = priorARTExposer.CreatedBy;
                        takenARTDrugs.CreatedIn = priorARTExposer.CreatedIn;
                        takenARTDrugs.EncounterType = priorARTExposer.EncounterType;
                        takenARTDrugs.ARTDrugId = itemId;
                        takenARTDrugs.PriorExposureId = priorARTExposer.InteractionId;
                        takenARTDrugs.StartDate = priorARTExposer.DateTaken;
                        takenARTDrugs.EndDate = priorARTExposer.DateEnd;
                        takenARTDrugs.StoppingReason = priorARTExposer.StoppingReason;

                        takenARTDrugs.CreatedIn = priorARTExposer.CreatedIn;
                        takenARTDrugs.CreatedBy = priorARTExposer.CreatedBy;
                        takenARTDrugs.DateCreated = DateTime.Now;
                        takenARTDrugs.IsDeleted = false;
                        takenARTDrugs.IsSynced = false;

                        context.TakenARTDrugRepository.Add(takenARTDrugs);
                    }
                }

                context.PriorARTExposerRepository.Add(priorARTExposer);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPriorARTExposerByKey", new { key = priorARTExposer.InteractionId }, priorARTExposer);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePriorARTExposer", "PriorARTExposerController.cs", ex.Message, priorARTExposer.CreatedIn, priorARTExposer.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposers
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPriorARTExposers)]
        public async Task<IActionResult> ReadPriorARTExposers()
        {
            try
            {
                var priorARTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposers();

                return Ok(priorARTExposerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPriorARTExposers", "PriorARTExposerController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposer/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorARTExposers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPriorARTExposerByKey)]
        public async Task<IActionResult> ReadPriorARTExposerByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var aRTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposerByKey(key);

                aRTExposerInDb.TakenARTDrugs = await context.TakenARTDrugRepository.LoadListWithChildAsync<TakenARTDrug>(x => x.PriorExposureId == aRTExposerInDb.InteractionId, p => p.ARTDrug);

                aRTExposerInDb.DateTaken = aRTExposerInDb.TakenARTDrugs.Select(s => s.StartDate).FirstOrDefault();
                aRTExposerInDb.DateEnd = aRTExposerInDb.TakenARTDrugs.Select(s => s.EndDate).FirstOrDefault();
                aRTExposerInDb.StoppingReason = aRTExposerInDb.TakenARTDrugs.Select(s => s.StoppingReason).FirstOrDefault();

                if (aRTExposerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(aRTExposerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPriorARTExposerByKey", "PriorARTExposerController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposer/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table PriorARTExposers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPriorARTExposerByClient)]
        public async Task<IActionResult> ReadPriorARTExposerByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var priorARTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposerByClient(clientId);

                    return Ok(priorARTExposerInDb);
                }
                else
                {
                    var priorARTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposerByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);


                    PagedResultDto<PriorARTExposure> priorARTExposureDto = new PagedResultDto<PriorARTExposure>()
                    {
                        Data = priorARTExposerInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PriorARTExposerRepository.GetPriorARTExposerByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(priorARTExposureDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPriorARTExposerByClient", "PriorARTExposerController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposer/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorARTExposers.</param>
        /// <param name="priorARTExposer">PriorARTExposer to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePriorARTExposer)]
        public async Task<IActionResult> UpdatePriorARTExposer(Guid key, PriorARTExposure priorARTExposer)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = priorARTExposer.ModifiedBy;
                interactionInDb.ModifiedIn = priorARTExposer.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != priorARTExposer.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                priorARTExposer.DateModified = DateTime.Now;
                priorARTExposer.IsDeleted = false;
                priorARTExposer.IsSynced = false;

                context.PriorARTExposerRepository.Update(priorARTExposer);
                await context.SaveChangesAsync();

                var aRTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposerByKey(key);

                aRTExposerInDb.TakenARTDrugs = await context.TakenARTDrugRepository.LoadListWithChildAsync<TakenARTDrug>(x => x.PriorExposureId == aRTExposerInDb.InteractionId, p => p.ARTDrug);

                foreach (var item in aRTExposerInDb.TakenARTDrugs)
                {
                    context.TakenARTDrugRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (priorARTExposer.ARTTakenDrugList != null && priorARTExposer.ARTTakenDrugList.Length > 0)
                {
                    TakenARTDrug takenARTDrugs = new TakenARTDrug();

                    foreach (var itemId in priorARTExposer.ARTTakenDrugList)
                    {
                        takenARTDrugs.InteractionId = new Guid();
                        takenARTDrugs.EncounterId = priorARTExposer.EncounterId;
                        takenARTDrugs.DateModified = priorARTExposer.DateModified;
                        takenARTDrugs.EncounterType = priorARTExposer.EncounterType;
                        takenARTDrugs.ARTDrugId = itemId;
                        takenARTDrugs.PriorExposureId = priorARTExposer.InteractionId;
                        takenARTDrugs.StartDate = priorARTExposer.DateTaken;
                        takenARTDrugs.EndDate = priorARTExposer.DateEnd;
                        takenARTDrugs.StoppingReason = priorARTExposer.StoppingReason;

                        takenARTDrugs.ModifiedIn = priorARTExposer.ModifiedIn;
                        takenARTDrugs.ModifiedBy = priorARTExposer.ModifiedBy;
                        takenARTDrugs.DateModified = DateTime.Now;
                        takenARTDrugs.IsDeleted = false;
                        takenARTDrugs.IsSynced = false;

                        context.TakenARTDrugRepository.Add(takenARTDrugs);
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePriorARTExposer", "PriorARTExposerController.cs", ex.Message, priorARTExposer.ModifiedIn, priorARTExposer.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prior-art-exposer/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PriorARTExposers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePriorARTExposer)]
        public async Task<IActionResult> DeletePriorARTExposer(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var priorARTExposerInDb = await context.PriorARTExposerRepository.GetPriorARTExposerByKey(key);

                if (priorARTExposerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                priorARTExposerInDb.DateModified = DateTime.Now;
                priorARTExposerInDb.IsDeleted = true;
                priorARTExposerInDb.IsSynced = true;

                context.PriorARTExposerRepository.Update(priorARTExposerInDb);
                await context.SaveChangesAsync();

                return Ok(priorARTExposerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePriorARTExposer", "PriorARTExposerController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}