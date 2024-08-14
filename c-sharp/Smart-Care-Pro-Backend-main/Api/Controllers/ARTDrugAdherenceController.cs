using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTDrugAdherence controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTDrugAdherenceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ARTDrugAdherenceController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTDrugAdherenceController(IUnitOfWork context, ILogger<ARTDrugAdherenceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-drug-dherence
        /// </summary>
        /// <param name="artDrugAdherence">ARTDrugAdherence object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTDrugAdherence)]
        public async Task<IActionResult> CreateARTDrugAdherence(ARTDrugAdherenceDto artDrugAdherence)
        {
            ARTDrugAdherence artDrugAdherenceList = new ARTDrugAdherence();

            try
            {
                List<ARTDrugAdherence> listArtDrugAdherences = new List<ARTDrugAdherence>();

                foreach (var item in artDrugAdherence.ARTDrugAdherences)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ARTDrugAdherence, artDrugAdherence.EncounterType);
                    interaction.EncounterId = artDrugAdherence.EncounterID;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = artDrugAdherence.CreatedBy;
                    interaction.CreatedIn = artDrugAdherence.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);
                    await context.SaveChangesAsync();

                    item.InteractionId = interactionId;
                    item.HaveTroubleTakingPills = item.HaveTroubleTakingPills;
                    item.HowManyDosesMissed = item.HowManyDosesMissed;
                    item.ReducePharmacyVisitTo = item.ReducePharmacyVisitTo;
                    item.ReferForAdherenceCounselling = item.ReferForAdherenceCounselling;
                    item.Forgot = item.Forgot;
                    item.Illness = item.Illness;
                    item.SideEffect = item.SideEffect;
                    item.MedicineFinished = item.MedicineFinished;
                    item.AwayFromHome = item.AwayFromHome;
                    item.ClinicRunOutOfMedication = item.ClinicRunOutOfMedication;
                    item.OtherMissingReason = item.OtherMissingReason;
                    item.Note = item.Note;
                    item.Nausea = item.Nausea;
                    item.Vomitting = item.Vomitting;
                    item.YellowEyes = item.YellowEyes;
                    item.MouthSores = item.MouthSores;
                    item.Diarrhea = item.Diarrhea;
                    item.Headache = item.Headache;
                    item.Rash = item.Rash;
                    item.Numbness = item.Numbness;
                    item.Insomnia = item.Insomnia;
                    item.Depression = item.Depression;
                    item.WeightGain = item.WeightGain;
                    item.OtherSideEffect = item.OtherSideEffect;
                    item.DrugId = item.DrugId;
                    item.EncounterId = artDrugAdherence.EncounterID;
                    item.ClientId = artDrugAdherence.ClientID;
                    item.EncounterType = artDrugAdherence.EncounterType;

                    item.CreatedBy = artDrugAdherence.CreatedBy;
                    item.CreatedIn = artDrugAdherence.CreatedIn;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.ARTDrugAdherenceRepository.Add(item);
                    await context.SaveChangesAsync();

                    listArtDrugAdherences.Add(item);
                }

                artDrugAdherenceList.ARTDrugAdherences = listArtDrugAdherences;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTDrugAdherence", "ARTDrugAdherenceController.cs", ex.Message, artDrugAdherence.CreatedIn, artDrugAdherence.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return CreatedAtAction("ReadARTDrugAdherenceByKey", new { key = artDrugAdherence.ARTDrugAdherences.Select(x => x.InteractionId) }, artDrugAdherence);
        }

        /// <summary>
        /// URL: sc-api/art-drug-adherences
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugAdherences)]
        public async Task<IActionResult> ReadARTDrugAdherences()
        {
            try
            {
                var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherences();

                drugAdherenceInDb = drugAdherenceInDb.OrderByDescending(x => x.DateCreated);

                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugAdherences", "ARTDrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-adherence/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugAdherenceByClient)]
        public async Task<IActionResult> ReadARTDrugAdherenceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    // var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByClient(clientId);
                    var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByClientLast24Hours(clientId);

                    return Ok(drugAdherenceInDb);
                }
                else
                {
                    var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<ARTDrugAdherence> aRTDrugAdherenceDto = new PagedResultDto<ARTDrugAdherence>()
                    {
                        Data = drugAdherenceInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(aRTDrugAdherenceDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugAdherenceByClient", "ARTDrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-dherence/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugAdherences.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTDrugAdherenceByKey)]
        public async Task<IActionResult> ReadARTDrugAdherenceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByKey(key);

                if (drugAdherenceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTDrugAdherenceByKey", "ARTDrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/art-drug-dherence/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugAdherences.</param>
        /// <param name="artDrugAdherence">ARTDrugAdherence to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTDrugAdherence)]
        public async Task<IActionResult> UpdateARTDrugAdherence(Guid key, ARTDrugAdherence artDrugAdherence)
        {
            try
            {
                if (key != artDrugAdherence.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = artDrugAdherence.ModifiedBy;
                interactionInDb.ModifiedIn = artDrugAdherence.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                artDrugAdherence.DateModified = DateTime.Now;
                artDrugAdherence.IsDeleted = false;
                artDrugAdherence.IsSynced = false;

                context.ARTDrugAdherenceRepository.Update(artDrugAdherence);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateARTDrugAdherence", "ARTDrugAdherenceController.cs", ex.Message, artDrugAdherence.CreatedIn, artDrugAdherence.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-drug-adherence/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugAdherences.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteARTDrugAdherence)]
        public async Task<IActionResult> DeleteARTDrugAdherence(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugAdherenceInDb = await context.ARTDrugAdherenceRepository.GetARTDrugAdherenceByKey(key);

                if (drugAdherenceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugAdherenceInDb.DateModified = DateTime.Now;
                drugAdherenceInDb.IsDeleted = true;
                drugAdherenceInDb.IsSynced = false;

                context.ARTDrugAdherenceRepository.Update(drugAdherenceInDb);
                await context.SaveChangesAsync();

                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteARTDrugAdherence", "ARTDrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}