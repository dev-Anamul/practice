using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PlacentaRemoval Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PlacentaRemovalController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PlacentaRemovalController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PlacentaRemovalController(IUnitOfWork context, ILogger<PlacentaRemovalController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/placenta-removal
        /// </summary>
        /// <param name="PlacentaRemoval">PlacentaRemoval object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePlacentaRemoval)]
        public async Task<IActionResult> CreatePlacentaRemoval(PlacentaRemoval placentaRemoval)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PlacentaRemoval, placentaRemoval.EncounterType);
                interaction.EncounterId = placentaRemoval.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = placentaRemoval.CreatedBy;
                interaction.CreatedIn = placentaRemoval.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                placentaRemoval.InteractionId = interactionId;
                placentaRemoval.DateCreated = DateTime.Now;
                placentaRemoval.IsDeleted = false;
                placentaRemoval.IsSynced = false;

                var motherDelivery = await context.MotherDeliverySummaryRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == placentaRemoval.DeliveryId);

                if (motherDelivery == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                context.PlacentaRemovalRepository.Add(placentaRemoval);
                await context.SaveChangesAsync();

                if (placentaRemoval.PlacentaList != null)
                {
                    foreach (var item in placentaRemoval.PlacentaList)
                    {
                        IdentifiedPlacentaRemoval identifiedPlacentaRemoval = new IdentifiedPlacentaRemoval();

                        identifiedPlacentaRemoval.InteractionId = Guid.NewGuid();
                        identifiedPlacentaRemoval.Placenta = item;
                        identifiedPlacentaRemoval.PlacentaRemovalId = placentaRemoval.InteractionId;
                        identifiedPlacentaRemoval.IsSynced = false;
                        identifiedPlacentaRemoval.IsDeleted = false;

                        context.IdentifiedPlacentaRemovalRepository.Add(identifiedPlacentaRemoval);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadPlacentaRemovalByKey", new { key = placentaRemoval.InteractionId }, placentaRemoval);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePlacentaRemoval", "PlacentaRemovalController.cs", ex.Message, placentaRemoval.CreatedIn, placentaRemoval.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/placenta-removals
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPlacentaRemovals)]
        public async Task<IActionResult> ReadPlacentaRemovals()
        {
            try
            {
                var placentaRemovalInDb = await context.PlacentaRemovalRepository.GetPlacentaRemovals();

                return Ok(placentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPlacentaRemovals", "PlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/placenta-removal/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PlacentaRemovals.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPlacentaRemovalByKey)]
        public async Task<IActionResult> ReadPlacentaRemovalByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var placentaRemovalIndb = await context.PlacentaRemovalRepository.GetPlacentaRemovalByKey(key);
                if (placentaRemovalIndb is not null)
                    placentaRemovalIndb.IdentifiedPlacentaRemovals = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovalByPlacentaRemoval(key);

                if (placentaRemovalIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(placentaRemovalIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPlacentaRemovalByKey", "PlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/bydelivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPlacentaRemovalByDelivery)]
        public async Task<IActionResult> ReadPlacentaRemovalByDelivery(Guid deliveryId)
        {
            try
            {
                var placentaRemovalinDb = await context.PlacentaRemovalRepository.GetPlacentaRemovalByDelivery(deliveryId);

                return Ok(placentaRemovalinDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPlacentaRemovalByDelivery", "PlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/placenta-removal/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PlacentaRemovals.</param>
        /// <param name="PlacentaRemoval">PlacentaRemoval to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePlacentaRemoval)]
        public async Task<IActionResult> UpdatePlacentaRemoval(Guid key, PlacentaRemoval placentaRemoval)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = placentaRemoval.ModifiedBy;
                interactionInDb.ModifiedIn = placentaRemoval.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != placentaRemoval.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var dbPlaceRemoval = await context.PlacentaRemovalRepository.GetPlacentaRemovalByKey(key);

                if (dbPlaceRemoval != null)
                {
                    dbPlaceRemoval.IsPlacentaRemovalCompleted = placentaRemoval.IsPlacentaRemovalCompleted;
                    dbPlaceRemoval.Other = placentaRemoval.Other;
                    dbPlaceRemoval.ModifiedIn = placentaRemoval.ModifiedIn;
                    dbPlaceRemoval.ModifiedBy = placentaRemoval.ModifiedBy;
                    dbPlaceRemoval.DateModified = DateTime.Now;
                    dbPlaceRemoval.IsDeleted = false;
                    dbPlaceRemoval.IsSynced = false;

                    context.PlacentaRemovalRepository.Update(dbPlaceRemoval);
                }


                if (placentaRemoval.PlacentaList != null)
                {


                    var dbIdentifiedPlacentaRemoval = await context.IdentifiedPlacentaRemovalRepository.GetIdentifiedPlacentaRemovalByPlacentaRemoval(key);

                    if (dbIdentifiedPlacentaRemoval != null)
                    {
                        foreach (var data in dbIdentifiedPlacentaRemoval)
                        {
                            context.IdentifiedPlacentaRemovalRepository.Delete(data);
                        }
                    }

                    foreach (var item in placentaRemoval.PlacentaList)
                    {
                        IdentifiedPlacentaRemoval identifiedPPHTreatment = new IdentifiedPlacentaRemoval();

                        identifiedPPHTreatment.InteractionId = Guid.NewGuid();
                        identifiedPPHTreatment.Placenta = item;
                        identifiedPPHTreatment.PlacentaRemovalId = placentaRemoval.InteractionId;
                        identifiedPPHTreatment.IsSynced = false;
                        identifiedPPHTreatment.IsDeleted = false;
                        identifiedPPHTreatment.DateCreated = DateTime.UtcNow;
                        identifiedPPHTreatment.CreatedBy = identifiedPPHTreatment.CreatedBy;
                        identifiedPPHTreatment.CreatedIn = identifiedPPHTreatment.CreatedIn;

                        context.IdentifiedPlacentaRemovalRepository.Add(identifiedPPHTreatment);

                    }


                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePlacentaRemoval", "PlacentaRemovalController.cs", ex.Message, placentaRemoval.ModifiedIn, placentaRemoval.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/placenta-removal/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PlacentaRemovals.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeletePlacentaRemoval)]
        public async Task<IActionResult> DeletePlacentaRemoval(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var placentaRemovalInDb = await context.PlacentaRemovalRepository.GetPlacentaRemovalByKey(key);

                if (placentaRemovalInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                placentaRemovalInDb.DateModified = DateTime.UtcNow;
                placentaRemovalInDb.IsDeleted = true;
                placentaRemovalInDb.IsSynced = true;

                context.PlacentaRemovalRepository.Update(placentaRemovalInDb);
                await context.SaveChangesAsync();

                return Ok(placentaRemovalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePlacentaRemoval", "PlacentaRemovalController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}