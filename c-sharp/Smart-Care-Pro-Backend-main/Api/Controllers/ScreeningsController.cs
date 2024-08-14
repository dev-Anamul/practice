using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    /// <summary>
    ///Screenings Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class ScreeningsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ScreeningsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ScreeningsController(IUnitOfWork context, ILogger<ScreeningsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/screening
        /// </summary>
        /// <param name="screening">screening object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateScreening)]
        public async Task<IActionResult> CreateScreening(Screening screening)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Screenings, screening.EncounterType);
                interaction.EncounterId = screening.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = screening.CreatedBy;
                interaction.CreatedIn = screening.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);


                screening.InteractionId = interactionId;
                screening.DateCreated = DateTime.Now;
                screening.IsDeleted = false;
                screening.IsSynced = false;

                context.ScreeningRepository.Add(screening);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadScreeningByKey", new { key = screening.InteractionId }, screening);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "Screening", "ScreeningController.cs", ex.Message, screening.CreatedIn, screening.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/screening
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreenings)]
        public async Task<IActionResult> ReadScreenings()
        {
            try
            {
                var screeningDb = await context.ScreeningRepository.GetScreening();

                return Ok(screeningDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreening", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/pre-screening-Visits/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Screening.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningByKey)]
        public async Task<IActionResult> ReadScreeningByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var screeningVisitInDb = await context.ScreeningRepository.GetScreeningByKey(key);

                if (screeningVisitInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(screeningVisitInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningByKey", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// pre-screening-visit-by-client/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningsByClient)]
        public async Task<IActionResult> ReadScreeningsByClient(Guid clientId)
        {
            try
            {
                var screeningInDb = await context.ScreeningRepository.GetScreeningbyClienId(clientId);

                return Ok(screeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningByClient", "ScreeningClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/screening/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadScreeningByEncounter)]
        public async Task<IActionResult> ReadScreeningByEncounter(Guid EncounterId)
        {
            try
            {
                var screeningInDb = await context.ScreeningRepository.GetScreeningByEncounter(EncounterId);

                return Ok(screeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreeningByEncounter", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Screening.</param>
        /// <param name="Screening">Screening to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateScreening)]
        public async Task<IActionResult> UpdateScreening(Guid key, Screening screening)
        {
            try
            {
                if (key != screening.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var screeningInDb = await context.ScreeningRepository.GetScreeningByKey(key);

                if (screeningInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                screening.DateModified = DateTime.Now;
                screening.IsDeleted = false;
                screening.IsSynced = false;

                context.ScreeningRepository.Update(screening);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateScreeningVisit", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/Screening/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Screening.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteScreening)]
        public async Task<IActionResult> DeleteScreening(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var screeningInDb = await context.ScreeningRepository.GetScreeningByKey(key);

                if (screeningInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                screeningInDb.IsDeleted = true;
                screeningInDb.IsSynced = false;
                screeningInDb.DateModified = DateTime.Now;

                context.ScreeningRepository.Update(screeningInDb);
                await context.SaveChangesAsync();
                
                return Ok(screeningInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteScreening", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
