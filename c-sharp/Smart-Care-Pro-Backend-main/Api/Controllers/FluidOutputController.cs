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

    public class FluidOutputController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FluidOutputController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FluidOutputController(IUnitOfWork context, ILogger<FluidOutputController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/fluid
        /// </summary>
        /// <param name="fluidOutput">FluidOutput object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFluidOutput)]
        public async Task<IActionResult> CreateFluidOutput(FluidOutput fluidOutput)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FluidOutput, fluidOutput.EncounterType);
                interaction.EncounterId = fluidOutput.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = fluidOutput.CreatedBy;
                interaction.CreatedIn = fluidOutput.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                fluidOutput.InteractionId = interactionId;
                fluidOutput.DateCreated = DateTime.Now;
                fluidOutput.IsDeleted = false;
                fluidOutput.IsSynced = false;

                context.FluidOutputRepository.Add(fluidOutput);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFluidOutputByKey", new { key = fluidOutput.InteractionId }, fluidOutput);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFluidOutput", "FluidOutputController.cs", ex.Message, fluidOutput.CreatedIn, fluidOutput.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/fluids
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidOutputs)]
        public async Task<IActionResult> ReadFluidOutputs()
        {
            try
            {
                var fluidOutputInDb = await context.FluidOutputRepository.GetFluidOutputs();

                return Ok(fluidOutputInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidOutputs", "FluidOutputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        // <summary>
        /// URL: sc-api/fluid/{fluidId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidOutputByFluid)]
        public async Task<IActionResult> ReadFluidOutputByFluId(Guid fluidId)
        {
            try
            {
                var fluidOutputInDb = await context.FluidOutputRepository.GetFluidOutputByFluid(fluidId);

                return Ok(fluidOutputInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidOutputByFluId", "FluidOutputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FluidOutputs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidOutputByKey)]
        public async Task<IActionResult> ReadFluidOutputByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidOutputInDb = await context.FluidOutputRepository.GetFluidOutputByKey(key);

                if (fluidOutputInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fluidOutputInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidOutputByKey", "FluidOutputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidOutputByEncounter)]
        public async Task<IActionResult> GetFluidOutputByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidIntakeInDb = await context.FluidOutputRepository.GetFluidOutputByEncounter(encounterId);

                if (fluidIntakeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fluidIntakeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetFluidOutputByEncounter", "FluidOutputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FluidOutputs.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFluOutput)]
        public async Task<IActionResult> DeleteFluidOutput(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidOutputInDb = await context.FluidOutputRepository.GetFluidOutputByKey(key);

                if (fluidOutputInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                fluidOutputInDb.DateModified = DateTime.Now;
                fluidOutputInDb.IsDeleted = true;
                fluidOutputInDb.IsSynced = false;

                context.FluidOutputRepository.Update(fluidOutputInDb);
                await context.SaveChangesAsync();

                return Ok(fluidOutputInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFluidOutput", "FluidOutputController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}