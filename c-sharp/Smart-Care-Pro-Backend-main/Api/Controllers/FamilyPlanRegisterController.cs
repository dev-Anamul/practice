using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 03.05.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Assessment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FamilyPlanRegisterController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FamilyPlanRegisterController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FamilyPlanRegisterController(IUnitOfWork context, ILogger<FamilyPlanRegisterController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/family-plan-register
        /// </summary>
        /// <param name="familyPlanRegister">FamilyPlanRegister object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFamilyPlanRegister)]
        public async Task<IActionResult> CreateFamilyPlanRegister(FamilyPlanRegister familyPlanRegister)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FamilyPlanRegister, familyPlanRegister.EncounterType);
                interaction.EncounterId = familyPlanRegister.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = familyPlanRegister.CreatedBy;
                interaction.CreatedIn = familyPlanRegister.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                familyPlanRegister.InteractionId = interactionId;
                familyPlanRegister.EncounterId = familyPlanRegister.EncounterId;
                familyPlanRegister.ClientId = familyPlanRegister.ClientId;
                familyPlanRegister.DateCreated = DateTime.Now;
                familyPlanRegister.IsDeleted = false;
                familyPlanRegister.IsSynced = false;

                context.FamilyPlanRegisterRepository.Add(familyPlanRegister);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFamilyPlanRegisterByKey", new { key = familyPlanRegister.InteractionId }, familyPlanRegister);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFamilyPlanRegister", "FamilyPlanRegisterController.cs", ex.Message, familyPlanRegister.CreatedIn, familyPlanRegister.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan-registers
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanRegisters)]
        public async Task<IActionResult> ReadFamilyPlanRegisters()
        {
            try
            {
                var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisters();

                return Ok(familyPlanRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanRegisters", "FamilyPlanRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan-register/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanRegisters.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanRegisterByKey)]
        public async Task<IActionResult> ReadFamilyPlanRegisterByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByKey(key);

                if (familyPlanRegisterInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(familyPlanRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanRegisterByKey", "FamilyPlanRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan-register/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyPlanRegisterByClient)]
        public async Task<IActionResult> ReadFamilyPlanRegisterByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                   // var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByClient(clientId);
                    var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByClientLast24Hours(clientId);

                    return Ok(familyPlanRegisterInDb);
                }
                else
                {
                    var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<FamilyPlanRegister> familyPlanRegisterDto = new PagedResultDto<FamilyPlanRegister>()
                    {
                        Data = familyPlanRegisterInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(familyPlanRegisterDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyPlanRegisterByClient", "FamilyPlanRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan-register/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanRegisters.</param>
        /// <param name="familyPlanRegister">FamilyPlanRegister to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFamilyPlanRegister)]
        public async Task<IActionResult> UpdateFamilyPlanRegister(Guid key, FamilyPlanRegister familyPlanRegister)
        {
            try
            {
                if (key != familyPlanRegister.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = familyPlanRegister.ModifiedBy;
                interactionInDb.ModifiedIn = familyPlanRegister.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                familyPlanRegister.DateModified = DateTime.Now;
                familyPlanRegister.IsDeleted = false;
                familyPlanRegister.IsSynced = false;

                context.FamilyPlanRegisterRepository.Update(familyPlanRegister);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFamilyPlanRegister", "FamilyPlanRegisterController.cs", ex.Message, familyPlanRegister.ModifiedIn, familyPlanRegister.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-plan-register/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanRegisters.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFamilyPlanRegister)]
        public async Task<IActionResult> DeleteFamilyPlanRegister(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyPlanRegisterInDb = await context.FamilyPlanRegisterRepository.GetFamilyPlanRegisterByKey(key);

                if (familyPlanRegisterInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                familyPlanRegisterInDb.DateModified = DateTime.Now;
                familyPlanRegisterInDb.IsDeleted = true;
                familyPlanRegisterInDb.IsSynced = false;

                context.FamilyPlanRegisterRepository.Update(familyPlanRegisterInDb);
                await context.SaveChangesAsync();

                return Ok(familyPlanRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFamilyPlanRegister", "FamilyPlanRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}