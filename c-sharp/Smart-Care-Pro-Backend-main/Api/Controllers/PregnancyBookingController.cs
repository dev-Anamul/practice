using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PregnancyBooking controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PregnancyBookingController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PregnancyBookingController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PregnancyBookingController(IUnitOfWork context, ILogger<PregnancyBookingController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pregnancy-booking
        /// </summary>
        /// <param name="pregnancyBooking">PregnancyBooking object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePregnencyBooking)]
        public async Task<IActionResult> CreatePregnencyBooking(PregnancyBooking pregnancyBooking)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PregnancyBooking, pregnancyBooking.EncounterType);
                interaction.EncounterId = pregnancyBooking.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = pregnancyBooking.CreatedBy;
                interaction.CreatedIn = pregnancyBooking.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                pregnancyBooking.InteractionId = interactionId;
                pregnancyBooking.DateCreated = DateTime.Now;
                pregnancyBooking.IsDeleted = false;
                pregnancyBooking.IsSynced = false;

                context.PregnancyBookingRepository.Add(pregnancyBooking);
                await context.SaveChangesAsync();

                if (pregnancyBooking.IdentifiedPregnancyConfirmationList != null)
                {
                    foreach (var item in pregnancyBooking.IdentifiedPregnancyConfirmationList)
                    {
                        IdentifiedPregnancyConfirmation identifiedPregnencyConfirmation = new IdentifiedPregnancyConfirmation();
                        identifiedPregnencyConfirmation.InteractionId = Guid.NewGuid();
                        identifiedPregnencyConfirmation.GynConfirmationId = item;
                        identifiedPregnencyConfirmation.PregnancyBookingId = pregnancyBooking.InteractionId;
                        identifiedPregnencyConfirmation.CreatedIn = pregnancyBooking.CreatedIn;
                        identifiedPregnencyConfirmation.CreatedBy = pregnancyBooking.CreatedBy;
                        identifiedPregnencyConfirmation.DateCreated = DateTime.Now;
                        identifiedPregnencyConfirmation.IsDeleted = false;
                        identifiedPregnencyConfirmation.IsSynced = false;

                        context.IdentifiedPregnancyConfirmationRepository.Add(identifiedPregnencyConfirmation);

                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadPregnancyBookingByKey", new { key = pregnancyBooking.InteractionId }, pregnancyBooking);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePregnencyBooking", "PregnancyBookingController.cs", ex.Message, pregnancyBooking.CreatedIn, pregnancyBooking.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pregnancy-bookings
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPregnancyBookings)]
        public async Task<IActionResult> ReadPregnancyBookings()
        {
            try
            {
                var pregnancyBookingInDb = await context.PregnancyBookingRepository.GetPregnancyBookings();

                return Ok(pregnancyBookingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPregnancyBookings", "PregnancyBookingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pregnancy-booking/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPregnancyBookingByClient)]
        public async Task<IActionResult> ReadPregnancyBookingByClient(Guid clientId)
        {
            try
            {
                var pregnancyBookingInDb = await context.PregnancyBookingRepository.GetPregnancyBookingByClient(clientId);

                foreach (var item in pregnancyBookingInDb)
                {
                    item.IdentifiedPregnencyConfirmations = await context.IdentifiedPregnancyConfirmationRepository.LoadListWithChildAsync<IdentifiedPregnancyConfirmation>(g => g.IsDeleted == false && g.PregnancyBookingId == item.InteractionId, p => p.GynConfirmation);
                }

                return Ok(pregnancyBookingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPregnancyBookingByClient", "PregnancyBookingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pregnancy-booking/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPregnancyBookingByEncounter)]
        public async Task<IActionResult> ReadpregnancyBookingByEncounter(Guid encounterId)
        {
            try
            {
                var pregnancyBookingInDb = await context.PregnancyBookingRepository.GetPregnancyBookingByEncounter(encounterId);

                return Ok(pregnancyBookingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadpregnancyBookingByEncounter", "PregnancyBookingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pregnancy-booking/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PregnancyBookings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPregnancyBookingByKey)]
        public async Task<IActionResult> ReadPregnancyBookingByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pregnancyBookingInDb = await context.PregnancyBookingRepository.GetPregnancyBookingByKey(key);

                pregnancyBookingInDb.IdentifiedPregnencyConfirmations = await context.IdentifiedPregnancyConfirmationRepository.LoadListWithChildAsync<IdentifiedPregnancyConfirmation>(x => x.PregnancyBookingId == pregnancyBookingInDb.InteractionId, p => p.GynConfirmation);

                if (pregnancyBookingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pregnancyBookingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPregnancyBookingByKey", "PregnancyBookingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/pregnancy-booking/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PregnancyBookings.</param>
        /// <param name="pregnancyBooking">PregnancyBooking to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePregnancyBooking)]
        public async Task<IActionResult> UpdatePregnancyBooking(Guid key, PregnancyBooking pregnancyBooking)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pregnancyBooking.ModifiedBy;
                interactionInDb.ModifiedIn = pregnancyBooking.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pregnancyBooking.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                pregnancyBooking.DateModified = DateTime.Now;
                pregnancyBooking.IsDeleted = false;
                pregnancyBooking.IsSynced = false;

                context.PregnancyBookingRepository.Update(pregnancyBooking);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePregnancyBooking", "PregnancyBookingController.cs", ex.Message, pregnancyBooking.ModifiedIn, pregnancyBooking.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pregnancy-booking/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PregnancyBookings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePregnancyBooking)]
        public async Task<IActionResult> DeletePregnancyBooking(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pregnancyBookingInDb = await context.PregnancyBookingRepository.GetPregnancyBookingByKey(key);

                if (pregnancyBookingInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pregnancyBookingInDb.DateModified = DateTime.Now;
                pregnancyBookingInDb.IsDeleted = true;
                pregnancyBookingInDb.IsSynced = false;

                context.PregnancyBookingRepository.Update(pregnancyBookingInDb);

                await context.SaveChangesAsync();

                return Ok(pregnancyBookingInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePregnancyBooking", "PregnancyBookingController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}