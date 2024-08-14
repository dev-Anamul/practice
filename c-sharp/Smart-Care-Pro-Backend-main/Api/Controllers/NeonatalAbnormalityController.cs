using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : Stephan
 * Last modified: 21.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// NeonatalAbnormality controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NeonatalAbnormalityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NeonatalAbnormalityController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NeonatalAbnormalityController(IUnitOfWork context, ILogger<NeonatalAbnormalityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormality
        /// </summary>
        /// <param name="neonatalAbnormality">NeonatalAbnormality object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNeonatalAbnormality)]
        public async Task<IActionResult> CreateNeonatalAbnormality(NeonatalAbnormality neonatalAbnormality)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.NeonatalAbnormality, neonatalAbnormality.EncounterType);
                interaction.EncounterId = neonatalAbnormality.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = neonatalAbnormality.CreatedBy;
                interaction.CreatedIn = neonatalAbnormality.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                var newBornDetails = await context.NewBornDetailRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == neonatalAbnormality.NeonatalId);

                if (newBornDetails == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                if (neonatalAbnormality.AbnormalitiesList != null)
                {
                    foreach (var item in neonatalAbnormality.AbnormalitiesList)
                    {
                        NeonatalAbnormality neonatalAbnormalityItem = new NeonatalAbnormality();

                        neonatalAbnormalityItem.NeonatalId = newBornDetails.InteractionId;
                        neonatalAbnormalityItem.EncounterId = newBornDetails.EncounterId;
                        neonatalAbnormalityItem.CreatedBy = neonatalAbnormality.CreatedBy;
                        neonatalAbnormalityItem.CreatedIn = neonatalAbnormality.CreatedIn;
                        neonatalAbnormalityItem.InteractionId = Guid.NewGuid();
                        neonatalAbnormalityItem.Abnormalities = item;
                        neonatalAbnormalityItem.DateCreated = DateTime.Now;
                        neonatalAbnormalityItem.IsDeleted = false;
                        neonatalAbnormalityItem.IsSynced = false;

                        context.NeonatalAbnormalityRepository.Add(neonatalAbnormalityItem);
                    }
                }
                else
                {
                    neonatalAbnormality.NeonatalId = newBornDetails.InteractionId;
                    neonatalAbnormality.EncounterId = newBornDetails.EncounterId;
                    neonatalAbnormality.InteractionId = interactionId;
                    neonatalAbnormality.DateCreated = DateTime.Now;
                    neonatalAbnormality.IsDeleted = false;
                    neonatalAbnormality.IsSynced = false;

                    context.NeonatalAbnormalityRepository.Add(neonatalAbnormality);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNeonatalAbnormalityByKey", new { key = neonatalAbnormality.InteractionId }, neonatalAbnormality);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNeonatalAbnormality", "NeonatalAbnormalityController.cs", ex.Message, neonatalAbnormality.CreatedIn, neonatalAbnormality.CreatedBy);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormalities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalAbnormalities)]
        public async Task<IActionResult> ReadNeonatalAbnormalities()
        {
            try
            {
                var neonatalAbnormalityInDb = await context.NeonatalAbnormalityRepository.GetNeonatalAbnormalities();

                return Ok(neonatalAbnormalityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalAbnormalities", "NeonatalAbnormalityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormality/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalAbnormalities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalAbnormalityByKey)]
        public async Task<IActionResult> ReadNeonatalAbnormalityByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalAbnormalityInDb = await context.NeonatalAbnormalityRepository.GetNeonatalAbnormalityByKey(key);

                if (neonatalAbnormalityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(neonatalAbnormalityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalAbnormalityByKey", "NeonatalAbnormalityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormality/byNeonatal/{NeonatalId}
        /// </summary>
        /// <param name="neonatalId">Primary key of the table NewBornDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalAbnormalityByNeonatal)]
        public async Task<IActionResult> ReadNeonatalAbnormalityByNeonatal(Guid neonatalId)
        {
            try
            {
                var NeonatalInDb = await context.NeonatalAbnormalityRepository.GetNeonatalAbnormalityByNeonatal(neonatalId);

                return Ok(NeonatalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalAbnormalityByNeonatal", "NeonatalAbnormalityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormality/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalAbnormalities.</param>
        /// <param name="neonatalAbnormality">NeonatalAbnormality to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNeonatalAbnormality)]
        public async Task<IActionResult> UpdateNeonatalAbnormality(Guid key, NeonatalAbnormality neonatalAbnormality)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = neonatalAbnormality.ModifiedBy;
                interactionInDb.ModifiedIn = neonatalAbnormality.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != neonatalAbnormality.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                neonatalAbnormality.DateModified = DateTime.Now;
                neonatalAbnormality.IsDeleted = false;
                neonatalAbnormality.IsSynced = false;

                context.NeonatalAbnormalityRepository.Update(neonatalAbnormality);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNeonatalAbnormality", "NeonatalAbnormalityController.cs", ex.Message, neonatalAbnormality.ModifiedIn, neonatalAbnormality.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-abnormality/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalAbnormalities.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteNeonatalAbnormality)]
        public async Task<IActionResult> DeleteNeonatalAbnormality(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalAbnormalityInDb = await context.NeonatalAbnormalityRepository.GetNeonatalAbnormalityByKey(key);

                if (neonatalAbnormalityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                neonatalAbnormalityInDb.IsDeleted = true;
                neonatalAbnormalityInDb.DateModified = DateTime.Now;

                context.NeonatalAbnormalityRepository.Update(neonatalAbnormalityInDb);
                await context.SaveChangesAsync();

                return Ok(neonatalAbnormalityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNeonatalAbnormality", "NeonatalAbnormalityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}