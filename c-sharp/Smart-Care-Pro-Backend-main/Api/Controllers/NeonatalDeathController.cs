using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// NeonatalDeath controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NeonatalDeathController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NeonatalDeathController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NeonatalDeathController(IUnitOfWork context, ILogger<NeonatalDeathController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/neonatal-death
        /// </summary>
        /// <param name="neonatalDeath">NeonatalDeath object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNeonatalDeath)]
        public async Task<IActionResult> CreateNeonatalDeath(NeonatalDeath neonatalDeath)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.NeonatalDeath, neonatalDeath.EncounterType);
                interaction.EncounterId = neonatalDeath.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = neonatalDeath.CreatedBy;
                interaction.CreatedIn = neonatalDeath.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                var newBornDetails = await context.NewBornDetailRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == neonatalDeath.NeonatalId);

                if (newBornDetails == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                neonatalDeath.InteractionId = interactionId;
                neonatalDeath.DateCreated = DateTime.Now;
                neonatalDeath.IsDeleted = false;
                neonatalDeath.IsSynced = false;

                context.NeonatalDeathRepository.Add(neonatalDeath);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNeonatalDeathByKey", new { key = neonatalDeath.InteractionId }, neonatalDeath);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNeonatalDeath", "NeonatalDeathController.cs", ex.Message, neonatalDeath.CreatedIn, neonatalDeath.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-deaths
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalDeaths)]
        public async Task<IActionResult> ReadNeonatalDeaths()
        {
            try
            {
                var neonatalDeathInDb = await context.NeonatalDeathRepository.GetNeonatalDeaths();

                return Ok(neonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalDeaths", "NeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-death/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalDeaths.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalDeathByKey)]
        public async Task<IActionResult> ReadNeonatalDeathByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalDeathInDb = await context.NeonatalDeathRepository.GetNeonatalDeathByKey(key);

                if (neonatalDeathInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(neonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalDeathByKey", "NeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-death/byNeonatal/{NeonatalId}
        /// </summary>
        /// <param name="neonatalId">Primary key of the table NewBornDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalDeathByNeonatal)]
        public async Task<IActionResult> ReadNeonatalDeathByNeonatal(Guid neonatalId)
        {
            try
            {
                var neonatalDeathInDb = await context.NeonatalDeathRepository.GetNeonatalDeathByNeonatal(neonatalId);

                return Ok(neonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalDeathByNeonatal", "NeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-death/byCauseOfNeonatalDeath/{CauseOfNeonatalDeathId}
        /// </summary>
        /// <param name="causeofNeonatalDeathId">Primary key of the table CauseOfNeonatalDeaths.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalDeathByCauseOfNeonatalDeath)]
        public async Task<IActionResult> ReadNeonatalDeathByCauseOfNeonatalDeath(int causeofNeonatalDeathId)
        {
            try
            {
                var neonatalDeathInDb = await context.NeonatalDeathRepository.GetNeonatalDeathByCauseOfNeonatalDeath(causeofNeonatalDeathId);

                return Ok(neonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalDeathByCauseOfNeonatalDeath", "NeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-death/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalDeaths.</param>
        /// <param name="neonatalDeath">NeonatalDeath to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNeonatalDeath)]
        public async Task<IActionResult> UpdateNeonatalDeath(Guid key, NeonatalDeath neonatalDeath)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = neonatalDeath.ModifiedBy;
                interactionInDb.ModifiedIn = neonatalDeath.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                NeonatalDeath neonatalDeathDb = await context.NeonatalDeathRepository.GetNeonatalDeathByKey(key);

                if (neonatalDeathDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                neonatalDeathDb.AgeAtTimeOfDeath = neonatalDeath.AgeAtTimeOfDeath;
                neonatalDeathDb.CauseOfNeonatalDeath = neonatalDeath.CauseOfNeonatalDeath;
                neonatalDeathDb.Other = neonatalDeath.Other;
                neonatalDeathDb.TimeOfDeath = neonatalDeath.TimeOfDeath;
                neonatalDeathDb.TimeUnit = neonatalDeath.TimeUnit;
                neonatalDeathDb.Comments = neonatalDeath.Comments;

                neonatalDeathDb.ModifiedBy = neonatalDeath.ModifiedBy;
                neonatalDeathDb.ModifiedIn = neonatalDeath.ModifiedIn;
                neonatalDeathDb.DateModified = DateTime.Now;
                neonatalDeathDb.IsDeleted = false;
                neonatalDeathDb.IsSynced = false;

                context.NeonatalDeathRepository.Update(neonatalDeathDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNeonatalDeath", "NeonatalDeathController.cs", ex.Message, neonatalDeath.ModifiedIn, neonatalDeath.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-death/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalDeaths.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteNeonatalDeath)]
        public async Task<IActionResult> DeleteNeonatalDeath(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalDeathInDb = await context.NeonatalDeathRepository.GetNeonatalDeathByKey(key);

                if (neonatalDeathInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                neonatalDeathInDb.IsDeleted = true;
                neonatalDeathInDb.DateModified = DateTime.Now;

                context.NeonatalDeathRepository.Update(neonatalDeathInDb);
                await context.SaveChangesAsync();

                return Ok(neonatalDeathInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNeonatalDeath", "NeonatalDeathController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}