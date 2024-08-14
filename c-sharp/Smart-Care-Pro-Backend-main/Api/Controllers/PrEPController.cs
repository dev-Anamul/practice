using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 30.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// PrEP controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PrEPController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PrEPController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PrEPController(IUnitOfWork context, ILogger<PrEPController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/prep
        /// </summary>
        /// <param name="prep">PrEP object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEP)]
        public async Task<IActionResult> CreatePrEP(Plan prep)
        {
            try
            {
                var prepInDb = await context.PrEPRepository.GetPrEPClient(prep.ClientId);
                if (prepInDb != null && prepInDb.Count() > 0)
                {
                    var lastPrep = prepInDb.OrderByDescending(x => x.DateCreated).FirstOrDefault();
                    if (lastPrep != null)
                    {

                        DateTime? fromDate = lastPrep?.StartDate;

                        DateTime? targetDate = fromDate?.AddMonths(3);
                        DateTime currentDate = DateTime.Now;
                        if (lastPrep?.StopDate == null && currentDate < targetDate.Value)
                        {
                            if (prep.Plans == Enums.Plans.Start)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, "You have already started PEP Plan");
                            }
                        }

                        lastPrep.IsUrinalysisNormal = prep.IsUrinalysisNormal;
                        lastPrep.HasAcuteHIVInfectionSymptoms = prep.HasAcuteHIVInfectionSymptoms;
                        lastPrep.IsAbleToAdhereDailyPrEP = prep.IsAbleToAdhereDailyPrEP;
                        lastPrep.HasGreaterFiftyCreatinineClearance = prep.HasGreaterFiftyCreatinineClearance;
                        lastPrep.IsPotentialHIVExposureMoreThanSixWeeksOld = prep.IsPotentialHIVExposureMoreThanSixWeeksOld;
                        lastPrep.StoppingReasonId = prep.StoppingReasonId;
                        lastPrep.Note = prep.Note;
                        lastPrep.Plans = prep.Plans;
                        lastPrep.EncounterType = prep.EncounterType;
                        if (lastPrep.Plans == Enums.Plans.Start)
                            lastPrep.StartDate = DateTime.Now;

                        if (lastPrep.Plans == Enums.Plans.Stop)
                            lastPrep.StopDate = DateTime.Now;
                        else
                            lastPrep.StoppingReasonId = null;

                        lastPrep.DateModified = DateTime.Now;
                        lastPrep.IsDeleted = false;
                        lastPrep.IsSynced = false;

                        context.PrEPRepository.Update(lastPrep);
                        await context.SaveChangesAsync();
                        return CreatedAtAction("ReadPrEPByKey", new { key = lastPrep.InteractionId }, prep);
                    }
                }

                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Plan, prep.EncounterType);
                interaction.EncounterId = prep.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = prep.CreatedBy;
                interaction.CreatedIn = prep.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                prep.InteractionId = interactionId;
                prep.EncounterId = prep.EncounterId;
                prep.ClientId = prep.ClientId;
                prep.DateCreated = DateTime.Now;
                prep.IsDeleted = false;
                prep.IsSynced = false;

                context.PrEPRepository.Add(prep);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPrEPByKey", new { key = prep.InteractionId }, prep);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrEP", "PrEPController.cs", ex.Message, prep.CreatedIn, prep.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/preps
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPs)]
        public async Task<IActionResult> ReadPrEPs()
        {
            try
            {
                var prepInDb = await context.PrEPRepository.GetPrEPs();

                return Ok(prepInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPs", "PrEPController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPByClient)]
        public async Task<IActionResult> ReadPrEPByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var prepInDb = await context.PrEPRepository.GetPrEPClient(clientId);

                    return Ok(prepInDb);
                }
                else
                {
                    var prepInDb = await context.PrEPRepository.GetPrEPClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PlanWithPaggingDto planWithPaggingDto = new PlanWithPaggingDto()
                    {
                        plans = prepInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.PrEPRepository.GetPrEPClientTotalCount(clientId, encounterType)
                    };

                    return Ok(planWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPByClient", "PrEPController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEP.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPByKey)]
        public async Task<IActionResult> ReadPrEPByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var prepInDb = await context.PrEPRepository.GetPrEPByKey(key);

                if (prepInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(prepInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPByKey", "PrEPController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/prep/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEPs.</param>
        /// <param name="prep">PrEP to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>    
        [HttpPut]
        [Route(RouteConstants.UpdatePrEP)]
        public async Task<IActionResult> UpdatePrEP(Guid key, Plan prep)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = prep.ModifiedBy;
                interactionInDb.ModifiedIn = prep.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != prep.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (prep.StartDate != null)
                {
                    prep.StartDate = prep.StartDate;
                }
                else if (prep.StopDate != null)
                {
                    prep.StopDate = prep.StopDate;
                }

                prep.DateModified = DateTime.Now;
                prep.IsDeleted = false;
                prep.IsSynced = false;

                context.PrEPRepository.Update(prep);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePrEP", "PrEPController.cs", ex.Message, prep.ModifiedIn, prep.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PrEPs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePrEP)]
        public async Task<IActionResult> DeletePrEP(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var prepInDb = await context.PrEPRepository.GetPrEPByKey(key);

                if (prepInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                prepInDb.DateModified = DateTime.Now;
                prepInDb.IsDeleted = true;
                prepInDb.IsSynced = false;

                context.PrEPRepository.Update(prepInDb);
                await context.SaveChangesAsync();

                return Ok(prepInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePrEP", "PrEPController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}