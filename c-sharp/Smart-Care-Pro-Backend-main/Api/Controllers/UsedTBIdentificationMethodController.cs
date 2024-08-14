using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// UsedTBIdentificationMethod controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class UsedTBIdentificationMethodController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<UsedTBIdentificationMethodController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public UsedTBIdentificationMethodController(IUnitOfWork context, ILogger<UsedTBIdentificationMethodController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/use-tb-identification
        /// </summary>
        /// <param name="usedTBIdentificationMethod">UsedTBIdentificationMethod object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateUseTBIdentificationMehtod)]
        public async Task<IActionResult> CreateUseTBIdentificationMehtod(UsedTBIdentificationMethod usedTBIdentificationMethod)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.UsedTBIdentificationMethod, usedTBIdentificationMethod.EncounterType);
                interaction.EncounterId = usedTBIdentificationMethod.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = usedTBIdentificationMethod.CreatedBy;
                interaction.CreatedIn = usedTBIdentificationMethod.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                usedTBIdentificationMethod.InteractionId = interactionId;
                usedTBIdentificationMethod.DateCreated = DateTime.Now;
                usedTBIdentificationMethod.IsDeleted = false;
                usedTBIdentificationMethod.IsSynced = false;


                context.UseTBIdentificationMethodRepository.Add(usedTBIdentificationMethod);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadUseTBIdentificationMethodByKey", new { key = usedTBIdentificationMethod.InteractionId }, usedTBIdentificationMethod);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateUseTBIdentificationMehtod", "UsedTBIdentificationMethodController.cs", ex.Message, usedTBIdentificationMethod.CreatedIn, usedTBIdentificationMethod.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/use-tb-identifications
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUseTBIdentificationMehtods)]
        public async Task<IActionResult> ReadUseTBIdentificationMehtods()
        {
            try
            {
                var userTBIdentificationInDb = await context.UseTBIdentificationMethodRepository.GetUsedTBIdentificationMethods();
                userTBIdentificationInDb = userTBIdentificationInDb.OrderByDescending(x => x.DateCreated);
                return Ok(userTBIdentificationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUseTBIdentificationMehtods", "UsedTBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/use-tb-identification-method/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UsedTBIdentificationMethods.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.UpdateUseTBIdentificationMehtod)]
        public async Task<IActionResult> ReadUseTBIdentificationMethodByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbIdentificationInDb = await context.UseTBIdentificationMethodRepository.GetUsedTBIdentificationMethodByKey(key);

                if (tbIdentificationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(tbIdentificationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUseTBIdentificationMethodByKey", "UsedTBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/use-tb-identification/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UsedTBIdentificationMethods.</param>
        /// <param name="usedTBIdentification">UsedTBIdentificationMethods to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateUseTBIdentificationMehtod)]
        public async Task<IActionResult> UpdateUseTBIdentificationMethod(Guid key, UsedTBIdentificationMethod usedTBIdentification)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = usedTBIdentification.ModifiedBy;
                interactionInDb.ModifiedIn = usedTBIdentification.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != usedTBIdentification.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                usedTBIdentification.DateModified = DateTime.Now;
                usedTBIdentification.IsDeleted = false;
                usedTBIdentification.IsSynced = false;

                context.UseTBIdentificationMethodRepository.Update(usedTBIdentification);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateUseTBIdentificationMethod", "UsedTBIdentificationMethodController.cs", ex.Message, usedTBIdentification.ModifiedIn, usedTBIdentification.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/use-tb-identification/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UsedTBIdentificationMethods.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUseTBIdentificationMehtod)]
        public async Task<IActionResult> DeleteUseTBIdentificationMethod(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var tbIdentificationInDb = await context.UseTBIdentificationMethodRepository.GetUsedTBIdentificationMethodByKey(key);

                if (tbIdentificationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                tbIdentificationInDb.DateModified = DateTime.Now;
                tbIdentificationInDb.IsDeleted = true;
                tbIdentificationInDb.IsSynced = false;

                context.UseTBIdentificationMethodRepository.Update(tbIdentificationInDb);

                await context.SaveChangesAsync();

                return Ok(tbIdentificationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUseTBIdentificationMethod", "UsedTBIdentificationMethodController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            
        }
    }
}