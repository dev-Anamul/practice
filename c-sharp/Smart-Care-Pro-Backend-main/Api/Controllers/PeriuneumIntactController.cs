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
    /// PeriuneumIntact Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PeriuneumIntactController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PeriuneumIntactController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PeriuneumIntactController(IUnitOfWork context, ILogger<PeriuneumIntactController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/periuneum-intact
        /// </summary>
        /// <param name="PeriuneumIntact">PeriuneumIntact object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePeriuneumIntact)]
        public async Task<IActionResult> CreatePeriuneumIntact(PerineumIntact periuneumIntact)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PerineumIntact, periuneumIntact.EncounterType);
                interaction.EncounterId = periuneumIntact.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = periuneumIntact.CreatedBy;
                interaction.CreatedIn = periuneumIntact.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                periuneumIntact.InteractionId = interactionId;

                periuneumIntact.DateCreated = DateTime.Now;
                periuneumIntact.IsDeleted = false;
                periuneumIntact.IsSynced = false;

                context.PeriuneumIntactRepository.Add(periuneumIntact);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPeriuneumIntactByKey", new { key = periuneumIntact.InteractionId }, periuneumIntact);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePeriuneumIntact", "PeriuneumIntactController.cs", ex.Message, periuneumIntact.CreatedIn, periuneumIntact.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/periuneum-intacts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPeriuneumIntacts)]
        public async Task<IActionResult> ReadPeriuneumIntacts()
        {
            try
            {
                var periuneumIntactInDb = await context.PeriuneumIntactRepository.GetPeriuneumIntacts();

                return Ok(periuneumIntactInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPeriuneumIntacts", "PeriuneumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/periuneum-intact/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PeriuneumIntacts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPeriuneumIntactByKey)]
        public async Task<IActionResult> ReadPeriuneumIntactByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var periuneumIntactIndb = await context.PeriuneumIntactRepository.GetPeriuneumIntactByKey(key);

                if (periuneumIntactIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(periuneumIntactIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPeriuneumIntactByKey", "PeriuneumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/by-delivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPeriuneumIntactByDelivery)]
        public async Task<IActionResult> ReadPeriuneumIntactByDelivery(Guid deliveryId)
        {
            try
            {
                var inDb = await context.PeriuneumIntactRepository.GetPeriuneumIntactByDelivery(deliveryId);

                return Ok(inDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPeriuneumIntactByDelivery", "PeriuneumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/periuneum-intact/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PeriuneumIntacts.</param>
        /// <param name="PeriuneumIntact">PeriuneumIntact to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePeriuneumIntact)]
        public async Task<IActionResult> UpdatePeriuneumIntact(Guid key, PerineumIntact periuneumIntact)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = periuneumIntact.ModifiedBy;
                interactionInDb.ModifiedIn = periuneumIntact.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != periuneumIntact.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                PerineumIntact dbPeriuneumIntact = await context.PeriuneumIntactRepository.GetPeriuneumIntactByKey(key);

                dbPeriuneumIntact.IsPerineumIntact = periuneumIntact.IsPerineumIntact;
                dbPeriuneumIntact.MotherDeliveryComment = periuneumIntact.MotherDeliveryComment;
                dbPeriuneumIntact.TearDetails = periuneumIntact.TearDetails;

                dbPeriuneumIntact.DateModified = DateTime.Now;
                dbPeriuneumIntact.IsDeleted = false;
                dbPeriuneumIntact.IsSynced = false;
                dbPeriuneumIntact.ModifiedBy = periuneumIntact.ModifiedBy;
                dbPeriuneumIntact.ModifiedIn = periuneumIntact.ModifiedIn;

                context.PeriuneumIntactRepository.Update(dbPeriuneumIntact);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}",
                    DateTime.Now, "BusinessLayer", "UpdatePeriuneumIntact", "PeriuneumIntactController.cs", ex.Message,
                    periuneumIntact.ModifiedIn, periuneumIntact.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/periuneum-intact/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PeriuneumIntacts.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeletePeriuneumIntact)]
        public async Task<IActionResult> DeletePeriuneumIntact(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var periuneumIntactInDb = await context.PeriuneumIntactRepository.GetPeriuneumIntactByKey(key);

                if (periuneumIntactInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                periuneumIntactInDb.DateModified = DateTime.Now;
                periuneumIntactInDb.IsDeleted = true;
                periuneumIntactInDb.IsSynced = true;

                context.PeriuneumIntactRepository.Update(periuneumIntactInDb);
                await context.SaveChangesAsync();

                return Ok(periuneumIntactInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePeriuneumIntact", "PeriuneumIntactController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}