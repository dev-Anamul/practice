using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Tomas
 * Date created  : 28.03.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Nutrition controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NutritionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NutritionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NutritionController(IUnitOfWork context, ILogger<NutritionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/nutrition
        /// </summary>
        /// <param name="nutrition">Nutrition object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNutrition)]
        public async Task<IActionResult> CreateNutrition(Nutrition nutrition)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Nutrition, nutrition.EncounterType);
                interaction.EncounterId = nutrition.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = nutrition.CreatedBy;
                interaction.CreatedIn = nutrition.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                nutrition.InteractionId = interactionId;
                nutrition.DateCreated = DateTime.Now;
                nutrition.IsDeleted = false;
                nutrition.IsSynced = false;

                context.NutritionRepository.Add(nutrition);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNutritionByKey", new { key = nutrition.InteractionId }, nutrition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNutrition", "NutritionController.cs", ex.Message, nutrition.CreatedIn, nutrition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nutritions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNutritions)]
        public async Task<IActionResult> ReadNutritions()
        {
            try
            {
                var nutritionInDb = await context.NutritionRepository.GetNutritions();

                return Ok(nutritionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNutritions", "NutritionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nutrition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Nutritions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNutritionByKey)]
        public async Task<IActionResult> ReadNutritionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nutritionInDb = await context.NutritionRepository.GetNutritionByKey(key);

                if (nutritionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(nutritionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNutritionByKey", "NutritionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nutrition/byClient/{ClientID}
        /// </summary>
        /// <param name="clientId">Primary key of the table Nutritions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNutritionByClient)]
        public async Task<IActionResult> ReadNutritionByClient(Guid clientId)
        {
            try
            {
                var nutritionInDb = await context.NutritionRepository.GetNutritionByClient(clientId);

                return Ok(nutritionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNutritionByClient", "NutritionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nutrition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Nutrition.</param>
        /// <param name="nutrition">Nutrition to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNutrition)]
        public async Task<IActionResult> UpdateNutrition(Guid key, Nutrition nutrition)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = nutrition.ModifiedBy;
                interactionInDb.ModifiedIn = nutrition.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != nutrition.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nutrition.DateModified = DateTime.Now;
                nutrition.IsDeleted = false;
                nutrition.IsSynced = false;

                context.NutritionRepository.Update(nutrition);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNutrition", "NutritionController.cs", ex.Message, nutrition.ModifiedIn, nutrition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nutrition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Nutritions.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteNutrition)]
        public async Task<IActionResult> DeleteNutrition(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var nutritionInDb = await context.NutritionRepository.GetNutritionByKey(key);

                if (nutritionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.NutritionRepository.Update(nutritionInDb);
                await context.SaveChangesAsync();

                return Ok(nutritionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNutrition", "NutritionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}