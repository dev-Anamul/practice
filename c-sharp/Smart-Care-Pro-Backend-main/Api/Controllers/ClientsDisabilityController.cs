using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 12.09.2022
 * Modified by   : Bella
 * Last modified : 30.01.2023
 * Reviewed by   :
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ClientsDisability controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ClientsDisabilityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ClientsDisabilityController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ClientsDisabilityController(IUnitOfWork context, ILogger<ClientsDisabilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/clients-disability
        /// </summary>
        /// <param name="clientsDisability">ClientsDisability object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateClientsDisability)]
        public async Task<IActionResult> CreateClientsDisability(ClientsDisability clientsDisability)
        {
            try
            { 
                if (clientsDisability.DisabilityList != null && clientsDisability.DisabilityList.Length > 0)
                {
                    foreach (var item in clientsDisability.DisabilityList)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ClientsDisability, clientsDisability.EncounterType);
                        interaction.EncounterId = clientsDisability.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = clientsDisability.CreatedBy;
                        interaction.CreatedIn = clientsDisability.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        clientsDisability.InteractionId = interactionId;
                        clientsDisability.DisabilityId = item;
                        clientsDisability.DateCreated = DateTime.Now;
                        clientsDisability.IsDeleted = false;
                        clientsDisability.IsSynced = false;
                        context.ClientsDisabilityRepository.Add(clientsDisability);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadClientsDisabilityByKey", new { key = clientsDisability.InteractionId }, clientsDisability);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClientsDisability", "ClientsDisabilityController.cs", ex.Message, clientsDisability.CreatedIn, clientsDisability.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients-disabilities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientsDisabilities)]
        public async Task<IActionResult> ReadClientsDisabilities()
        {
            try
            {
                var clientDisabilitiesIndb = await context.ClientsDisabilityRepository.GetClientsDisabilities();

                clientDisabilitiesIndb = clientDisabilitiesIndb.OrderByDescending(x => x.DateCreated);

                return Ok(clientDisabilitiesIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientsDisabilities", "ClientsDisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients-disabilities/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientsDisabilityByClient)]
        public async Task<IActionResult> ReadClientsDisabilityByClient(Guid clientId)
        {
            try
            {
                var clientDisabilitiesIndb = await context.ClientsDisabilityRepository.GetClientsDisabilityByClient(clientId);

                clientDisabilitiesIndb = clientDisabilitiesIndb.OrderByDescending(x => x.DateCreated);

                return Ok(clientDisabilitiesIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientsDisabilityByClient", "ClientsDisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/clients-disability/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientsDisabilityByKey)]
        public async Task<IActionResult> ReadClientsDisabilityByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientDisabilitiesIndb = await context.ClientsDisabilityRepository.GetClientsDisabilityByKey(key);

                if (clientDisabilitiesIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(clientDisabilitiesIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientsDisabilityByKey", "ClientsDisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/clients-disability/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <param name="clientsDisability">ClientsDisability to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateClientsDisability)]
        public async Task<IActionResult> UpdateClientsDisability(Guid key, ClientsDisability clientsDisability)
        {
            try
            {

                var clientsDisabilityInDb = await context.ClientsDisabilityRepository.GetClientsDisabilityByEncounter(key);

                if(clientsDisability.DisabilityList?.Length > 0)
                {
                    foreach(var item in clientsDisabilityInDb)
                    {

                        context.InteractionRepository.Delete(await context.InteractionRepository.GetInteractionByKey(item.InteractionId));
                        context.ClientsDisabilityRepository.Delete(item);
                        await context.SaveChangesAsync();
                    }

                }

                if (clientsDisability.DisabilityList != null && clientsDisability.DisabilityList?.Length > 0)
                {
                    foreach (var item in clientsDisability.DisabilityList)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionId = Guid.NewGuid();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ClientsDisability, clientsDisability.EncounterType);
                        interaction.EncounterId = clientsDisability.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = clientsDisability.CreatedBy;
                        interaction.CreatedIn = clientsDisability.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        clientsDisability.InteractionId = interactionId;
                        clientsDisability.DisabilityId = item;
                        clientsDisability.DateCreated = DateTime.Now;
                        clientsDisability.IsDeleted = false;
                        clientsDisability.IsSynced = false;
                        context.ClientsDisabilityRepository.Add(clientsDisability);
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateClientsDisability", "ClientsDisabilityController.cs", ex.Message, clientsDisability.ModifiedIn, clientsDisability.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients-disability/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteClientsDisability)]
        public async Task<IActionResult> DeleteClientsDisability(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientsDisabilityInDb = await context.ClientsDisabilityRepository.GetClientsDisabilityByEncounter(key);

                if (clientsDisabilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var clientDisability in clientsDisabilityInDb)
                {
                    clientDisability.DateModified = DateTime.Now;
                    clientDisability.IsDeleted = true;
                    clientDisability.IsSynced = false;

                    context.ClientsDisabilityRepository.Update(clientDisability);
                    await context.SaveChangesAsync();
                }

                return Ok(clientsDisabilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteClientsDisability", "ClientsDisabilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}