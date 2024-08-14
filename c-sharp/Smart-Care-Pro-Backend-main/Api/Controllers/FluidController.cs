using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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

    public class FluidController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<FluidController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FluidController(IUnitOfWork context, ILogger<FluidController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/fluid
        /// </summary>
        /// <param name="fluid">Fluid object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFluid)]
        public async Task<IActionResult> CreateFluid(Fluid fluid)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Fluid, fluid.EncounterType);
                interaction.EncounterId = fluid.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = fluid.CreatedBy;
                interaction.CreatedIn = fluid.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                fluid.InteractionId = interactionId;
                fluid.DateCreated = DateTime.Now;
                fluid.IsDeleted = false;
                fluid.IsSynced = false;

                context.FluidRepository.Add(fluid);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFluidByKey", new { key = fluid.InteractionId }, fluid);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFluid", "FluidController.cs", ex.Message, fluid.CreatedIn, fluid.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluids
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluids)]
        public async Task<IActionResult> ReadFluids()
        {
            try
            {
                var fluidInDb = await context.FluidRepository.GetFluids();

                return Ok(fluidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluids", "FluidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidByClient)]
        public async Task<IActionResult> ReadFluidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //   var fluidInDb = await context.FluidRepository.GetFluidByClient(clientId);
                    var fluidInDb = await context.FluidRepository.GetFluidByClientLast24Hours(clientId);

                    return Ok(fluidInDb);
                }
                else
                {
                    var fluidInDb = await context.FluidRepository.GetFluidByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);


                    PagedResultDto<Fluid> fluidChartDto = new PagedResultDto<Fluid>()
                    {
                        Data = fluidInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.FluidRepository.GetFluidByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(fluidChartDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidByClient", "FluidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFluidByKey)]
        public async Task<IActionResult> ReadFluidByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidInDb = await context.FluidRepository.GetFluidByKey(key);

                if (fluidInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fluidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFluidByKey", "FluidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/fluid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <param name="fluid">Fluid to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFluid)]
        public async Task<IActionResult> UpdateFluid(Guid key, Fluid fluid)
        {
            try
            {
                if (key != fluid.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = fluid.ModifiedBy;
                interactionInDb.ModifiedIn = fluid.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                fluid.DateModified = DateTime.Now;
                fluid.IsDeleted = false;
                fluid.IsSynced = false;

                context.FluidRepository.Update(fluid);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFluid", "FluidController.cs", ex.Message, fluid.ModifiedIn, fluid.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/fluid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFluid)]
        public async Task<IActionResult> DeleteFluid(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fluidInDb = await context.FluidRepository.GetFluidByKey(key);

                if (fluidInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                fluidInDb.DateModified = DateTime.Now;
                fluidInDb.IsDeleted = true;
                fluidInDb.IsSynced = false;

                context.FluidRepository.Update(fluidInDb);
                await context.SaveChangesAsync();

                return Ok(fluidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFluid", "FluidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}