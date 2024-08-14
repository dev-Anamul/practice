using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Brian
 * Date created  : 13.04.2023
 * Modified by   : Bella
 * Last modified : 14.04.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// SkinPreparation controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SkinPreparationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SkinPreparationController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SkinPreparationController(IUnitOfWork context, ILogger<SkinPreparationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/skin-preparation
        /// </summary>
        /// <param name="skinPreparation">SkinPreparation object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSkinPreparation)]
        public async Task<IActionResult> CreateSkinPreparation(SkinPreparation skinPreparation)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.SkinPreparation, skinPreparation.EncounterType);
                interaction.EncounterId = skinPreparation.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = skinPreparation.CreatedBy;
                interaction.CreatedIn = skinPreparation.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                skinPreparation.DateCreated = DateTime.Now;
                skinPreparation.IsDeleted = false;
                skinPreparation.IsSynced = false;

                context.SkinPreparationRepository.Add(skinPreparation);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSkinPreparationByKey", new { key = skinPreparation.InteractionId }, skinPreparation);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSkinPreparation", "SkinPreparationController.cs", ex.Message, skinPreparation.CreatedIn, skinPreparation.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSkinPreparations)]
        public async Task<IActionResult> ReadSkinPreparations()
        {
            try
            {
                var skinPreparationIndb = await context.SkinPreparationRepository.GetSkinPreparations();

                return Ok(skinPreparationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSkinPreparations", "SkinPreparationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SkinPreparations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSkinPreparationByKey)]
        public async Task<IActionResult> ReadSkinPreparationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var skinPreparationIndb = await context.SkinPreparationRepository.GetSkinPreparationByKey(key);

                if (skinPreparationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(skinPreparationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSkinPreparationByKey", "SkinPreparationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparation/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSkinPreparationByEncounter)]
        public async Task<IActionResult> ReadSkinPreparationByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var skinPreparationIndb = await context.SkinPreparationRepository.GetSkinPreparationByEncounter(encounterId);

                if (skinPreparationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(skinPreparationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSkinPreparationByEncounter", "SkinPreparationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparations/by-anesthetic-plan/{anestheticPlanId}
        /// </summary>
        /// <param name="anestheticPlanId">Primary key of the table AnestheticPlan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSkinPreparationListByAnestheticPlan)]
        public async Task<IActionResult> ReadSkinPreparationListByAnestheticPlan(Guid anestheticPlanId)
        {
            try
            {
                if (anestheticPlanId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var skinPreparationIndb = await context.SkinPreparationRepository.GetSkinPreparationListByAnestheticPlan(anestheticPlanId);

                if (skinPreparationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(skinPreparationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSkinPreparationListByAnestheticPlan", "SkinPreparationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SkinPreparations.</param>
        /// <param name="skinPreparation">SkinPreparation to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSkinPreparation)]
        public async Task<IActionResult> UpdateSkinPreparation(Guid key, SkinPreparation skinPreparation)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = skinPreparation.ModifiedBy;
                interactionInDb.ModifiedIn = skinPreparation.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != skinPreparation.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                skinPreparation.DateModified = DateTime.Now;
                skinPreparation.IsDeleted = false;
                skinPreparation.IsSynced = false;

                context.SkinPreparationRepository.Update(skinPreparation);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSkinPreparation", "SkinPreparationController.cs", ex.Message, skinPreparation.ModifiedIn, skinPreparation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/skin-preparation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SkinPreparations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSkinPreparation)]
        public async Task<IActionResult> DeleteSkinPreparation(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var skinPreparationIndb = await context.SkinPreparationRepository.GetSkinPreparationByKey(key);

                if (skinPreparationIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                skinPreparationIndb.DateModified = DateTime.Now;
                skinPreparationIndb.IsDeleted = true;
                skinPreparationIndb.IsSynced = false;

                context.SkinPreparationRepository.Update(skinPreparationIndb);
                await context.SaveChangesAsync();

                return Ok(skinPreparationIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSkinPreparation", "SkinPreparationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}