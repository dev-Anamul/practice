using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    ///BirthHistory  Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class BirthDetailController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<BirthDetailController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public BirthDetailController(IUnitOfWork context, ILogger<BirthDetailController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/birth-detail
        /// </summary>
        /// <param name="birthDetail">BirthHistory object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateBirthDetail)]
        public async Task<IActionResult> CreateBirthDetail(BirthDetail birthDetail)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.BirthDetail, birthDetail.EncounterType); ;
                interaction.EncounterId = birthDetail.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = birthDetail.CreatedBy;
                interaction.CreatedIn = birthDetail.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                birthDetail.InteractionId = interactionId;
                birthDetail.DateCreated = DateTime.Now;
                birthDetail.IsDeleted = false;
                birthDetail.IsSynced = false;

                context.BirthDetailRepository.Add(birthDetail);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadBirthDetailByKey", new { key = birthDetail.InteractionId }, birthDetail);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateBirthDetail", "BirthDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-details
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthDetails)]
        public async Task<IActionResult> ReadBirthDetails()
        {
            try
            {
                var birthDetailsInDb = await context.BirthDetailRepository.GetBirthDetails();

                birthDetailsInDb = birthDetailsInDb.OrderByDescending(x => x.DateCreated);

                return Ok(birthDetailsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthDetails", "BirthDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-detail/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthDetailByKey)]
        public async Task<IActionResult> ReadBirthDetailByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthDetailsInDb = await context.BirthDetailRepository.GetBirthDetailByKey(key);

                if (birthDetailsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(birthDetailsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthDetailByKey", "BirthDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URl: birth-details/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadBirthDetailByEncounter)]
        public async Task<IActionResult> ReadBirthDetailByEncounter(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var birthDetailsInDb = await context.BirthDetailRepository.GetBirthDetailByEncounter(encounterId);

                if (birthDetailsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(birthDetailsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadBirthDetailByEncounter", "BirthDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/birth-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="birthDetail">BirthHistory to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateBirthDetail)]
        public async Task<IActionResult> UpdateBirthDetail(Guid key, BirthDetail birthDetail)
        {
            try
            {
                if (key != birthDetail.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = birthDetail.ModifiedBy;
                interactionInDb.ModifiedIn = birthDetail.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                birthDetail.DateModified = DateTime.Now;
                birthDetail.IsDeleted = false;
                birthDetail.IsSynced = false;

                context.BirthDetailRepository.Update(birthDetail);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateBirthDetail", "BirthDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}