using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.03.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DSDAssesmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DSDAssesmentController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DSDAssesmentController(IUnitOfWork context, ILogger<DSDAssesmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/dsd-assesment
        /// </summary>
        /// <param name="dSDAssesment">DSDAssesment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDSDAssesment)]
        public async Task<IActionResult> CreateDSDAssesment(DSDAssessment dsdAssesment)
        {
            try
            {

                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DSDAssessment, dsdAssesment.EncounterType);
                interaction.EncounterId = dsdAssesment.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = dsdAssesment.CreatedBy;
                interaction.CreatedIn = dsdAssesment.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                dsdAssesment.InteractionId = interactionId;
                dsdAssesment.DateCreated = DateTime.Now;
                dsdAssesment.IsDeleted = false;
                dsdAssesment.IsSynced = false;

                context.DSDAssesmentRepository.Add(dsdAssesment);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDSDAssesmentByKey", new { key = dsdAssesment.InteractionId }, dsdAssesment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDSDAssesment", "DSDAssesmentController.cs", ex.Message, dsdAssesment.CreatedIn, dsdAssesment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dsd-assesments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDSDAssesments)]
        public async Task<IActionResult> ReadDSDAssesments()
        {
            try
            {
                var dSDAssesmentIndb = await context.DSDAssesmentRepository.GetDSDAssesments();

                return Ok(dSDAssesmentIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDSDAssesments", "DSDAssesmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dsd-assesment/ByClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDSDAssesmentByClient)]
        public async Task<IActionResult> ReadDSDAssesmentByClient(Guid clientId)
        {
            try
            {
                var dSDAssesmentIndb = await context.DSDAssesmentRepository.GetDSDAssesmentByClient(clientId);

                return Ok(dSDAssesmentIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDSDAssesmentByClient", "DSDAssesmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/dsd-assesment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDSDAssesmentByKey)]
        public async Task<IActionResult> ReadDSDAssesmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dsdAssesmentIndb = await context.DSDAssesmentRepository.GetDSDAssesmentByKey(key);

                if (dsdAssesmentIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(dsdAssesmentIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDSDAssesmentByKey", "DSDAssesmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/dsd-assesment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <param name="dSDAssesment">DSDAssesment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDSDAssesment)]
        public async Task<IActionResult> UpdateDSDAssesment(Guid key, DSDAssessment dsdAssesment)
        {
            try
            {
                if (key != dsdAssesment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = dsdAssesment.ModifiedBy;
                interactionInDb.ModifiedIn = dsdAssesment.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                dsdAssesment.DateModified = DateTime.Now;
                dsdAssesment.IsDeleted = false;
                dsdAssesment.IsSynced = false;

                context.DSDAssesmentRepository.Update(dsdAssesment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDSDAssesment", "DSDAssesmentController.cs", ex.Message, dsdAssesment.ModifiedIn, dsdAssesment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dsd-assesment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDSDAssesment)]
        public async Task<IActionResult> DeleteDSDAssesment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dsdAssesmentInDb = await context.DSDAssesmentRepository.GetDSDAssesmentByKey(key);

                if (dsdAssesmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                dsdAssesmentInDb.DateModified = DateTime.Now;
                dsdAssesmentInDb.IsDeleted = true;
                dsdAssesmentInDb.IsSynced = false;

                context.DSDAssesmentRepository.Update(dsdAssesmentInDb);
                await context.SaveChangesAsync();

                return Ok(dsdAssesmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDSDAssesment", "DSDAssesmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}