using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// MotherDetail controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MotherDetailController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MotherDetailController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MotherDetailController(IUnitOfWork context, ILogger<MotherDetailController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/mother-detail
        /// </summary>
        /// <param name="motherDetail">MotherDetail object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMotherDetail)]
        public async Task<IActionResult> CreateMotherDetail(MotherDetail motherDetail)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();
                foreach (var item in motherDetail.MotherDetails)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MotherDetail, motherDetail.EncounterType);
                    interaction.EncounterId = motherDetail.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = motherDetail.CreatedBy;
                    interaction.CreatedIn = motherDetail.CreatedIn;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionId;
                    item.DateCreated = DateTime.Now;
                    item.EncounterId = motherDetail.EncounterId;
                    item.ClientId = motherDetail.ClientId;

                    if (item.EncounterType == 0)
                        item.EncounterType = motherDetail.EncounterType;
                    else
                        item.EncounterType = item.EncounterType;

                    item.IsDeleted = false;
                    item.IsSynced = false;

                }
                context.InteractionRepository.AddRange(interactions);
                context.MotherDetailRepository.AddRange(motherDetail.MotherDetails);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMotherDetailByKey", new { key = motherDetail.InteractionId }, motherDetail);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMotherDetail", "MotherDetailController.cs", ex.Message, motherDetail.CreatedIn, motherDetail.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-details
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDetails)]
        public async Task<IActionResult> ReadMotherDetails()
        {
            try
            {
                var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetails();

                return Ok(motherDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDetails", "MotherDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-detail/ByClient/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDetailByClient)]
        public async Task<IActionResult> ReadMotherDetailByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                if (pageSize == 0)
                {
                    var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetailByClient(clientId);

                    return Ok(motherDetailInDb);
                }
                else
                {
                    var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetailByClient(clientId, ((page - 1) * (pageSize)), pageSize);
                    
                    PagedResultDto<MotherDetail> motherDetailDto = new PagedResultDto<MotherDetail>()
                    {
                        Data = motherDetailInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.MotherDetailRepository.GetMotherDetailByClientTotalCount(clientId)
                    };

                    return Ok(motherDetailDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDetailByClient", "MotherDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-detail/ByEncounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDetailByEncounter)]
        public async Task<IActionResult> ReadMotherDetailByEncounter(Guid encounterId)
        {
            try
            {
                var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetailByEncounter(encounterId);

                return Ok(motherDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDetailByEncounter", "MotherDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MotherDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMotherDetailByKey)]
        public async Task<IActionResult> ReadMotherDetailByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetailByKey(key);

                if (motherDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(motherDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMotherDetailByKey", "MotherDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/mother-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MotherDetails.</param>
        /// <param name="motherDetail">MotherDetail to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMotherDetail)]
        public async Task<IActionResult> UpdateMotherDetail(Guid key, MotherDetail motherDetail)
        {
            try
            {
                if (key != motherDetail.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = motherDetail.ModifiedBy;
                interactionInDb.ModifiedIn = motherDetail.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var motherDetailsDb = await context.MotherDetailRepository.GetMotherDetailByKey(motherDetail.InteractionId);

                motherDetailsDb.DateModified = DateTime.Now;
                motherDetailsDb.IsDeleted = false;
                motherDetailsDb.IsSynced = false;
                motherDetailsDb.DateofDelivary = motherDetail.DateofDelivary;
                motherDetailsDb.PregnancyDuration = motherDetail.PregnancyDuration;
                motherDetailsDb.DeliveryMethod = motherDetail.DeliveryMethod;
                motherDetailsDb.EarlyTerminationReason = motherDetail.EarlyTerminationReason;
                motherDetailsDb.MetarnalComplication = motherDetail.MetarnalComplication;
                motherDetailsDb.Notes = motherDetail.Notes;
                motherDetailsDb.PregnancyNo = motherDetail.PregnancyNo;
                motherDetailsDb.MetarnalOutcome = motherDetail.MetarnalOutcome;
                motherDetailsDb.PueperiumOutcome = motherDetail.PueperiumOutcome;
                motherDetailsDb.PregnancyConclusion = motherDetail.PregnancyConclusion;

                context.MotherDetailRepository.Update(motherDetailsDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMotherDetail", "MotherDetailController.cs", ex.Message, motherDetail.ModifiedIn, motherDetail.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/mother-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MotherDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMotherDetail)]
        public async Task<IActionResult> DeleteMotherDetail(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var motherDetailInDb = await context.MotherDetailRepository.GetMotherDetailByKey(key);

                if (motherDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                motherDetailInDb.DateModified = DateTime.Now;
                motherDetailInDb.IsDeleted = true;
                motherDetailInDb.IsSynced = false;

                context.MotherDetailRepository.Update(motherDetailInDb);
                await context.SaveChangesAsync();

                return Ok(motherDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMotherDetail", "MotherDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}