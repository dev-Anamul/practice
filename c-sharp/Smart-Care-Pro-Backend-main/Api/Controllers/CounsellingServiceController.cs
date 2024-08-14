using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 28.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
    /// <summary>
    /// CounsellingService controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CounsellingServiceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CounsellingServiceController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CounsellingServiceController(IUnitOfWork context, ILogger<CounsellingServiceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/counselling-service
        /// </summary>
        /// <param name="counsellingService">CounsellingService object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCounsellingService)]
        public async Task<IActionResult> CreateCounsellingService(CounsellingService counsellingService)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.CounsellingService, counsellingService.EncounterType);
                interaction.EncounterId = counsellingService.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = counsellingService.CreatedBy;
                interaction.CreatedIn = counsellingService.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                counsellingService.InteractionId = interactionId;
                counsellingService.EncounterId = counsellingService.EncounterId;
                counsellingService.ClientId = counsellingService.ClientId;
                counsellingService.DateCreated = DateTime.Now;
                counsellingService.IsDeleted = false;
                counsellingService.IsSynced = false;

                context.CounsellingServiceRepository.Add(counsellingService);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCounsellingServiceByKey", new { key = counsellingService.InteractionId }, counsellingService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCounsellingService", "CounsellingServiceController.cs", ex.Message, counsellingService.CreatedIn, counsellingService.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/counselling-services
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCounsellingServices)]
        public async Task<IActionResult> ReadCounsellingServices()
        {
            try
            {
                var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServices();
                counsellingServiceInDb = counsellingServiceInDb.OrderByDescending(x => x.DateCreated);
                return Ok(counsellingServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCounsellingServices", "CounsellingServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/counselling-service/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CounsellingServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCounsellingServiceByKey)]
        public async Task<IActionResult> ReadCounsellingServiceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServiceByKey(key);

                if (counsellingServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(counsellingServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCounsellingServiceByKey", "CounsellingServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/counselling-service/byClient/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table CounsellingServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCounsellingServiceByClient)]
        public async Task<IActionResult> ReadCounsellingServiceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                   // var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServiceByClient(clientId);
                    var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServiceByClientLast24Hours(clientId);

                    return Ok(counsellingServiceInDb);
                }
                else
                {
                    var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServiceByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<CounsellingService> counsellingServiceDto = new PagedResultDto<CounsellingService>()
                    {
                        Data = counsellingServiceInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.CounsellingServiceRepository.GetCounsellingServiceByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(counsellingServiceDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCounsellingServiceByClient", "CounsellingServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/counselling-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CounsellingServices.</param>
        /// <param name="counsellingService">CounsellingService to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCounsellingService)]
        public async Task<IActionResult> UpdateCounsellingService(Guid key, CounsellingService counsellingService)
        {
            try
            {
                if (key != counsellingService.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = counsellingService.ModifiedBy;
                interactionInDb.ModifiedIn = counsellingService.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                counsellingService.DateModified = DateTime.Now;
                counsellingService.IsDeleted = false;
                counsellingService.IsSynced = false;
                counsellingService.ClientId = counsellingService.ClientId;

                context.CounsellingServiceRepository.Update(counsellingService);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCounsellingService", "CounsellingServiceController.cs", ex.Message, counsellingService.ModifiedIn, counsellingService.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/counselling-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CounsellingServices.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCounsellingService)]
        public async Task<IActionResult> DeleteCounsellingService(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var counsellingServiceInDb = await context.CounsellingServiceRepository.GetCounsellingServiceByKey(key);

                if (counsellingServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.CounsellingServiceRepository.Update(counsellingServiceInDb);
                await context.SaveChangesAsync();

                return Ok(counsellingServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCounsellingService", "CounsellingServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}