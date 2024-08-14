using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 11.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PMTCT controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PMTCTController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PMTCTController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PMTCTController(IUnitOfWork context, ILogger<PMTCTController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pmtct
        /// </summary>
        /// <param name="pmtct">PMTCT object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePMTCT)]
        public async Task<IActionResult> CreatePMTCT(PMTCT pmtct)
        {
            try
            {

                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PMTCT, pmtct.EncounterType);
                interaction.EncounterId = pmtct.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = pmtct.CreatedBy;
                interaction.CreatedIn = pmtct.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                pmtct.InteractionId = interactionId;
                pmtct.DateCreated = DateTime.Now;
                pmtct.IsDeleted = false;
                pmtct.IsSynced = false;

                context.PMTCTRepository.Add(pmtct);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadPMTCTByKey", new { key = pmtct.InteractionId }, pmtct);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePMTCT", "PMTCTController.cs", ex.Message, pmtct.CreatedIn, pmtct.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pmtcts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPMTCTs)]
        public async Task<IActionResult> ReadPMTCTs()
        {
            try
            {
                var pmtctInDb = await context.PMTCTRepository.GetPMTCTs();

                return Ok(pmtctInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPMTCTs", "PMTCTController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pmtct/by-client/{clientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPMTCTByClient)]
        public async Task<IActionResult> ReadPMTCTByClient(Guid clientID)
        {
            try
            {
                var pmtctInDb = await context.PMTCTRepository.GetPMTCTByClient(clientID);

                return Ok(pmtctInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPMTCTByClient", "PMTCTController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pmtct/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PMTCTs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPMTCTByKey)]
        public async Task<IActionResult> ReadPMTCTByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pmtctInDb = await context.PMTCTRepository.GetPMTCTByKey(key);

                if (pmtctInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(pmtctInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPMTCTByKey", "PMTCTController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/pmtct/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PMTCTs.</param>
        /// <param name="pmtct">PMTCT to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePMTCT)]
        public async Task<IActionResult> UpdatePMTCT(Guid key, PMTCT pmtct)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pmtct.ModifiedBy;
                interactionInDb.ModifiedIn = pmtct.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pmtct.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                pmtct.DateModified = DateTime.Now;
                pmtct.IsDeleted = false;
                pmtct.IsSynced = false;

                context.PMTCTRepository.Update(pmtct);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePMTCT", "PMTCTController.cs", ex.Message, pmtct.ModifiedIn, pmtct.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pmtct/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PMTCTs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeletePMTCT)]
        public async Task<IActionResult> DeletePMTCT(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pmtctInDb = await context.PMTCTRepository.GetPMTCTByKey(key);

                if (pmtctInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pmtctInDb.DateModified = DateTime.Now;
                pmtctInDb.IsDeleted = true;
                pmtctInDb.IsSynced = false;

                context.PMTCTRepository.Update(pmtctInDb);
                await context.SaveChangesAsync();

                return Ok(pmtctInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePMTCT", "PMTCTController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}