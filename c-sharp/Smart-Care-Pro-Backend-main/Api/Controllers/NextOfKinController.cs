using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// NextOfKin controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NextOfKinController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NextOfKinController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NextOfKinController(IUnitOfWork context, ILogger<NextOfKinController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/nextofkin
        /// </summary>
        /// <param name="nextofkin">NextOfKin object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNextOfKin)]
        public async Task<IActionResult> CreateNextOfKin(NextOfKin nextofkin)
        {
            try
            {
                Guid id = nextofkin.InteractionId;

                var existingNextOfkin = await context.NextOfKinRepository.GetNextOfKinByKey(id);

                if (nextofkin.CellphoneCountryCode == "260" && nextofkin.Cellphone[0] == '0')
                    nextofkin.Cellphone = nextofkin.Cellphone.Substring(1);

                if (existingNextOfkin != null)
                {
                    existingNextOfkin.CreatedBy = nextofkin.CreatedBy;
                    existingNextOfkin.CreatedIn = nextofkin.CreatedIn;
                    existingNextOfkin.ModifiedBy = nextofkin.ModifiedBy;
                    existingNextOfkin.ModifiedIn = nextofkin.ModifiedIn;
                    existingNextOfkin.DateCreated = nextofkin.DateCreated;
                    existingNextOfkin.FirstName = nextofkin.FirstName;
                    existingNextOfkin.Surname = nextofkin.Surname;
                    existingNextOfkin.HouseNumber = nextofkin.HouseNumber;
                    existingNextOfkin.StreetName = nextofkin.StreetName;
                    existingNextOfkin.OtherNextOfKinType = nextofkin.OtherNextOfKinType;
                    existingNextOfkin.NextOfKinType = nextofkin.NextOfKinType;
                    existingNextOfkin.Township = nextofkin.Township;
                    existingNextOfkin.ChiefName = nextofkin.ChiefName;
                    existingNextOfkin.CellphoneCountryCode = nextofkin.CellphoneCountryCode;
                    existingNextOfkin.Cellphone = nextofkin.Cellphone;
                    existingNextOfkin.OtherCellphoneCountryCode = nextofkin.OtherCellphoneCountryCode;
                    existingNextOfkin.OtherCellphone = nextofkin.OtherCellphone;
                    existingNextOfkin.EmailAddress = nextofkin.EmailAddress;

                    context.NextOfKinRepository.Update(existingNextOfkin);
                }
                else
                {
                    nextofkin.DateCreated = DateTime.Now;
                    nextofkin.IsDeleted = false;
                    nextofkin.IsSynced = false;

                    context.NextOfKinRepository.Add(nextofkin);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNextOfKinByKey", new { key = nextofkin.InteractionId }, nextofkin);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNextOfKin", "NextOfKinController.cs", ex.Message, nextofkin.CreatedIn, nextofkin.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nextofkins
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNextOfKins)]
        public async Task<IActionResult> ReadNextOfKins()
        {
            try
            {
                var nextofkinInDb = await context.NextOfKinRepository.GetNextOfKins();

                return Ok(nextofkinInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNextOfKins", "NextOfKinController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/byClient/{key}
        /// </summary>
        /// <param name="clientId">Primary key of the table NextOfKin.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNextOfKinByClient)]
        public async Task<IActionResult> ReadNextOfKinByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);
                if (pageSize == 0)
                {
                    var incidentStatusInDb = await context.NextOfKinRepository.GetNextOfKinByClient(clientId);

                    if (incidentStatusInDb == null)
                        return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                    return Ok(incidentStatusInDb);
                }
                else
                {
                    var incidentStatusInDb = await context.NextOfKinRepository.GetNextOfKinByClient(clientId, ((page - 1) * (pageSize)), pageSize);
                    PagedResultDto<NextOfKin> nextOfKinDto = new PagedResultDto<NextOfKin>()
                    {
                        Data = incidentStatusInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.NextOfKinRepository.GetNextOfKinByClientTotalCount(clientId)
                    }; 

                    return Ok(nextOfKinDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNextOfKinByClient", "NextOfKinController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NextOfKin.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNextOfKinByKey)]
        public async Task<IActionResult> ReadNextOfKinByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var incidentStatusInDb = await context.NextOfKinRepository.GetNextOfKinByKey(key);

                if (incidentStatusInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(incidentStatusInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNextOfKinByKey", "NextOfKinController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/nextofkin/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NextOfKin.</param>
        /// <param name="nextofkin">NextOfKin to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNextOfKin)]
        public async Task<IActionResult> UpdateNextOfKin(Guid key, NextOfKin nextofkin)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                nextofkin.DateModified = DateTime.Now;
                nextofkin.IsDeleted = false;
                nextofkin.IsSynced = false;

                if (nextofkin.CellphoneCountryCode == "260" && nextofkin.Cellphone[0] == '0')
                    nextofkin.Cellphone = nextofkin.Cellphone.Substring(1);

                context.NextOfKinRepository.Update(nextofkin);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNextOfKin", "NextOfKinController.cs", ex.Message, nextofkin.ModifiedIn, nextofkin.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the next of kin name is duplicate or not.
        /// </summary>
        /// <param name="nextofkin">NextOfKin object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsNextOfKinDuplicate(NextOfKin nextofkin)
        {
            try
            {
                var nextofkinInDb = await context.NextOfKinRepository.GetNextOfKinBySurname(nextofkin.Surname);

                if (nextofkinInDb != null)
                    if (nextofkinInDb.InteractionId != nextofkin.InteractionId)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsNextOfKinDuplicate", "NextOfKinController.cs", ex.Message);
                throw;
            }
        }
    }
}