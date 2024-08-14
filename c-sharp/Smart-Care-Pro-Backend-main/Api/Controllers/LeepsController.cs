using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{

    /// <summary>
    ///leeps Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class LeepsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<LeepsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        /// 
        public LeepsController(IUnitOfWork context, ILogger<LeepsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/leep
        /// </summary>
        /// <param name="screening">leeps object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateLeep)]
        public async Task<IActionResult> CreateLeep(Leeps leeps)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Leep, leeps.EncounterType);
                interaction.EncounterId = leeps.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = leeps.CreatedBy;
                interaction.CreatedIn = leeps.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                leeps.InteractionId = interactionId;
                leeps.DateCreated = DateTime.Now;
                leeps.IsDeleted = false;
                leeps.IsSynced = false;

                context.LeepsRepository.Add(leeps);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadLeepByKey", new { key = leeps.InteractionId }, leeps);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "Screening", "ScreeningController.cs", ex.Message, leeps.CreatedIn, leeps.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLeeps)]
        public async Task<IActionResult> ReadLeeps()
        {
            try
            {
                var screeningDb = await context.LeepsRepository.GetLeeps();

                return Ok(screeningDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadScreening", "ScreeningController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/Leeps/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Leeps.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLeepByKey)]
        public async Task<IActionResult> ReadLeepByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var leepInDb = await context.LeepsRepository.GetLeepsByKey(key);

                if (leepInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(leepInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLeepByKey", "LeepController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// pre-screening-visit-by-client/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadLeepsByClient)]
        public async Task<IActionResult> ReadLeepsByClient(Guid clientId)
        {
            try
            {
                var leepsInDb = await context.LeepsRepository.GetLeepsbyClienId(clientId);

                return Ok(leepsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadleepsByClient", "leepsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/leeps/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLeepsByEncounter)]
        public async Task<IActionResult> ReadLeepsByEncounter(Guid EncounterId)
        {
            try
            {
                var leepsInDb = await context.LeepsRepository.GetLeepsByEncounter(EncounterId);

                return Ok(leepsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadleepsByEncounter", "leepsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/leeps/{key}
        /// </summary>
        /// <param name="key">Primary key of the table leeps.</param>
        /// <param name="leeps">leeps to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateLeep)]
        public async Task<IActionResult> UpdateLeep(Guid key, Leeps leeps)
        {
            try
            {
                if (key != leeps.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var leepsInDb = await context.LeepsRepository.GetLeepsByKey(key);

                if (leepsInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                leeps.DateModified = DateTime.Now;
                leeps.IsDeleted = false;
                leeps.IsSynced = false;

                context.LeepsRepository.Update(leeps);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateleepsVisit", "leepsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/leeps/{key}
        /// </summary>
        /// <param name="key">Primary key of the table leeps.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteLeep)]
        public async Task<IActionResult> DeleteLeep(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var leepsInDb = await context.LeepsRepository.GetLeepsByKey(key);

                if (leepsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                leepsInDb.IsDeleted = true;
                leepsInDb.IsSynced = false;
                leepsInDb.DateModified = DateTime.Now;

                context.LeepsRepository.Update(leepsInDb);
                await context.SaveChangesAsync();

                return Ok(leepsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "Deleteleeps", "leepsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
