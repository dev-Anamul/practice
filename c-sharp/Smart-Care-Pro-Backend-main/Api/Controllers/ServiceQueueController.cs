using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.04.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ServiceQueueController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ServiceQueueController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ServiceQueueController(IUnitOfWork context, ILogger<ServiceQueueController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/service-queue
        /// </summary>
        /// <param name="serviceQueue">ServiceQueue object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateServiceQueue)]
        public async Task<IActionResult> ServiceQueue(ServiceQueue serviceQueue)
        {
            try
            {
                if (await CheckClientalreadyInService(serviceQueue) != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.ServiceQueueDuplicate);

                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.EncounterId = serviceQueue.EncounterId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ServiceQueue, serviceQueue.EncounterType);
                interaction.CreatedIn = serviceQueue.CreatedIn;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = serviceQueue.CreatedBy;
                interaction.ModifiedIn = serviceQueue.ModifiedIn;
                interaction.DateModified = DateTime.Now;
                interaction.ModifiedBy = Guid.Empty;
                interaction.IsDeleted = serviceQueue.IsDeleted;
                interaction.IsSynced = serviceQueue.IsSynced;

                context.InteractionRepository.Add(interaction);

                serviceQueue.InteractionId = interactionId;
                serviceQueue.DateCreated = DateTime.Now;
                serviceQueue.IsDeleted = false;
                serviceQueue.IsSynced = false;

                context.ServiceQueueRepository.Add(serviceQueue);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadServiceQueueByKey", new { key = serviceQueue.InteractionId }, serviceQueue);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "ServiceQueue", "ServiceQueueController.cs", ex.Message, serviceQueue.CreatedIn, serviceQueue.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/service-queues
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadServiceQueues)]
        public async Task<IActionResult> ReadServiceQueues()
        {
            try
            {
                var serviceQueuesInDb = await context.ServiceQueueRepository.GetServiceQueues();

                return Ok(serviceQueuesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadServiceQueues", "ServiceQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadLatestServiceQueues)]
        public async Task<IActionResult> ReadLatestServiceQueues(int servicePointId)
        {
            try
            {
                var serviceQueuesInDb = await context.ServiceQueueRepository.GetServiceQueueByServicePoint(servicePointId);

                return Ok(serviceQueuesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestServiceQueues", "ServiceQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/service-queue/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ServiceQueue.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadServiceQueueByKey)]
        public async Task<IActionResult> ReadServiceQueueByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var serviceQueueInDb = await context.ServiceQueueRepository.GetServiceQueueByKey(key);

                if (serviceQueueInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(serviceQueueInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadServiceQueueByKey", "ServiceQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/service-queue/by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId">Foreign key of the table ServiceQueue primary key of Encounter.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadServiceQueueByEncounterId)]
        public async Task<IActionResult> ReadServiceQueueByEncounter(Guid encounterId)
        {
            try
            {
                var serviceQueueInDb = await context.ServiceQueueRepository.GetServiceQueueByEncounter(encounterId);

                return Ok(serviceQueueInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadServiceQueueByEncounter", "ServiceQueueController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/service-queue/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ServiceQueue.</param>
        /// <param name="serviceQueue">ServiceQueue to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateServiceQueue)]
        public async Task<IActionResult> UpdateServiceQueue(Guid key, ServiceQueue serviceQueue)
        {
            try
            {

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = serviceQueue.ModifiedBy;
                interactionInDb.ModifiedIn = serviceQueue.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != serviceQueue.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                serviceQueue.DateModified = DateTime.Now;
                serviceQueue.IsDeleted = false;
                serviceQueue.IsSynced = false;

                context.ServiceQueueRepository.Update(serviceQueue);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateServiceQueue", "ServiceQueueController.cs", ex.Message, serviceQueue.ModifiedIn, serviceQueue.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpPut]
        [Route(RouteConstants.UpdateServiceQueueByClientId)]
        public async Task<IActionResult> UpdateServiceQueueByClientId(Guid clientId, int facilityId)
        {
            try
            {
                var serviceQueue = await context.ServiceQueueRepository.LoadWithChildAsync<ServiceQueue>(x => x.ClientId == clientId && x.DateQueued.Date == DateTime.Today && x.IsCompleted == false && x.FacilityQueue.FacilityId == facilityId, f => f.FacilityQueue); ;

                if (serviceQueue == null)
                    return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);

                if (await CheckClientalreadynInFacilityQueue(serviceQueue) != null)
                    return StatusCode(StatusCodes.Status400BadRequest, "You can not add more Service Queue");

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(serviceQueue.InteractionId);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = serviceQueue.ModifiedBy;
                interactionInDb.ModifiedIn = serviceQueue.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                serviceQueue.DateModified = DateTime.Now;
                serviceQueue.IsDeleted = false;
                serviceQueue.IsSynced = false;
                serviceQueue.IsCompleted = true;

                context.ServiceQueueRepository.Update(serviceQueue);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateServiceQueue", "ServiceQueueController.cs", ex.Message, "0", "0");
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        private async Task<ServiceQueue> CheckClientalreadynInFacilityQueue(ServiceQueue serviceQueue)
        {
            List<int> facilityQueuesId = new List<int>();
            var facilityId = await context.FacilityQueueRepository.getFacilityIdByFacilityQueueId(serviceQueue.FacilityQueueId);
            DateTime today = DateTime.Today;

            if (facilityId != null)
                facilityQueuesId = await context.FacilityQueueRepository.getFacilityQueuesIdByFacility(facilityId);

            return await context.ServiceQueueRepository.FirstOrDefaultAsync(x => x.ClientId == serviceQueue.ClientId && x.DateQueued.Date == today && x.InteractionId != serviceQueue.InteractionId && x.IsCompleted == false && facilityQueuesId.Contains(x.FacilityQueueId)); ;
        }

        private async Task<ServiceQueue> CheckClientalreadyInService(ServiceQueue serviceQueue)
        {
            DateTime today = DateTime.Today;

            return await context.ServiceQueueRepository.FirstOrDefaultAsync(x => x.ClientId == serviceQueue.ClientId && x.DateQueued.Date == today && x.InteractionId != serviceQueue.InteractionId && x.IsCompleted == false && x.CreatedIn==serviceQueue.CreatedIn && x.FacilityQueueId == serviceQueue.FacilityQueueId); ;
        }
    }
}