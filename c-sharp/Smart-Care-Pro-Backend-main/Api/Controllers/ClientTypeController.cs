using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

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
    /// ClientType controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ClientTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ClientTypeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ClientTypeController(IUnitOfWork context, ILogger<ClientTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/client-type
        /// </summary>
        /// <param name="clientType">ClientType object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateClientType)]
        public async Task<IActionResult> CreateClientType(ClientType clientType)
        {
            try
            {
                if (await IsClientTypeDuplicate(clientType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                clientType.DateCreated = DateTime.Now;
                clientType.IsDeleted = false;
                clientType.IsSynced = false;

                context.ClientTypeRepository.Add(clientType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadclientTypeByKey", new { key = clientType.Oid }, clientType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClientType", "ClientTypeController.cs", ex.Message, clientType.CreatedIn, clientType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientTypes)]
        public async Task<IActionResult> ReadClientTypes()
        {
            try
            {
                var clientType = await context.ClientTypeRepository.GetClientTypes();

                clientType = clientType.OrderByDescending(x => x.DateCreated);

                return Ok(clientType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientTypes", "ClientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ClientTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientTypeByKey)]
        public async Task<IActionResult> ReadClientTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientType = await context.ClientTypeRepository.GetClientTypeByKey(key);

                if (clientType == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(clientType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientTypeByKey", "ClientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ClientTypes.</param>
        /// <param name="clientType">ClientType to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateClientType)]
        public async Task<IActionResult> UpdateClientType(int key, ClientType clientType)
        {
            try
            {
                if (key != clientType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsClientTypeDuplicate(clientType) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                clientType.DateModified = DateTime.Now;
                clientType.IsDeleted = false;
                clientType.IsSynced = false;

                context.ClientTypeRepository.Update(clientType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateClientType", "ClientTypeController.cs", ex.Message, clientType.ModifiedIn, clientType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ClientTypes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteClientType)]
        public async Task<IActionResult> DeleteClientType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientTypeInDb = await context.ClientTypeRepository.GetClientTypeByKey(key);

                if (clientTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                clientTypeInDb.DateModified = DateTime.Now;
                clientTypeInDb.IsDeleted = true;
                clientTypeInDb.IsSynced = false;

                context.ClientTypeRepository.Update(clientTypeInDb);
                await context.SaveChangesAsync();

                return Ok(clientTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteClientType", "ClientTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the client type name is duplicate or not.
        /// </summary>
        /// <param name="clientType">ClientType object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsClientTypeDuplicate(ClientType clientType)
        {
            try
            {
                var clientTypeInDb = await context.ClientTypeRepository.GetClientTypeByClientTypes(clientType.Description);

                if (clientTypeInDb != null)
                    if (clientTypeInDb.Oid != clientType.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsClientTypeDuplicate", "ClientTypeController.cs", ex.Message);
                throw;
            }
        }
    }
}