using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FluidInputController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FluidInputController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FluidInputController(IUnitOfWork context, ILogger<FluidInputController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/fluid
        /// </summary>
        /// <param name="fluidIntake">FluidIntake object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFluidInput)]
        public async Task<IActionResult> CreateFluidInput(FluidIntake fluidIntake)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FluidIntake, fluidIntake.EncounterType);
                interaction.EncounterId = fluidIntake.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = fluidIntake.CreatedBy;
                interaction.CreatedIn = fluidIntake.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                fluidIntake.InteractionId = interactionId;
                fluidIntake.DateCreated = DateTime.Now;
                fluidIntake.IsDeleted = false;
                fluidIntake.IsSynced = false;

                context.FluidInputRepository.Add(fluidIntake);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFluidInputByKey", new { key = fluidIntake.InteractionId }, fluidIntake);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFluidInput", "FluidInputController.cs", ex.Message, fluidIntake.CreatedIn, fluidIntake.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluids
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidInputs)]
        public async Task<IActionResult> ReadFluidInputs()
        {
            try
            {
                var fluidIntakeInDb = await context.FluidInputRepository.GetFluidInputs();

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidInputs", "FluidInputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        // <summary>
        /// URL: sc-api/fluid/{fluidId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidInputByFluid)]
        public async Task<IActionResult> ReadFluidInputByFluid(Guid fluidId)
        {
            try
            {
                var fluidIntakeInDb = await context.FluidInputRepository.GetFluidInputByFluid(fluidId);

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidInputByFluid", "FluidInputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidInputByKey)]
        public async Task<IActionResult> ReadFluidInputByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidIntakeInDb = await context.FluidInputRepository.GetFluidInputByKey(key);

                if (fluidIntakeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidInputByKey", "FluidInputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidInputByEncounter)]
        public async Task<IActionResult> GetFluidInputByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidIntakeInDb = await context.FluidInputRepository.GetFluidInputByEncounter(encounterId);

                if (fluidIntakeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetFluidInputByEncounter", "FluidInputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/fluid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FluidIntakes.</param>
        /// <param name="fluidIntake">FluidIntake to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFluidInput)]
        public async Task<IActionResult> UpdateFluidInput(Guid key, FluidIntake fluidIntake)
        {
            try
            {
                if (key != fluidIntake.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = fluidIntake.ModifiedBy;
                interactionInDb.ModifiedIn = fluidIntake.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                fluidIntake.DateModified = DateTime.Now;
                fluidIntake.IsDeleted = false;
                fluidIntake.IsSynced = false;

                context.FluidInputRepository.Update(fluidIntake);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFluidInput", "FluidInputController.cs", ex.Message, fluidIntake.ModifiedIn, fluidIntake.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFluidInput)]
        public async Task<IActionResult> DeleteFluidInput(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidIntakeInDb = await context.FluidInputRepository.GetFluidInputByKey(key);

                if (fluidIntakeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                fluidIntakeInDb.DateModified = DateTime.Now;
                fluidIntakeInDb.IsDeleted = true;
                fluidIntakeInDb.IsSynced = false;

                context.FluidInputRepository.Update(fluidIntakeInDb);
                await context.SaveChangesAsync();

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFluidInput", "FluidInputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}