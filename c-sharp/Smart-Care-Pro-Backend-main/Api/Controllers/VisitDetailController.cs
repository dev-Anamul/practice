using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// VisitDetail controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VisitDetailController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VisitDetailController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VisitDetailController(IUnitOfWork context, ILogger<VisitDetailController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/visit-detail
        /// </summary>
        /// <param name="visitDetail">VisitDetail object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVisitDetail)]
        public async Task<IActionResult> CreateVisitDetail(VisitDetail visitDetail)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.VisitDetail, visitDetail.EncounterType);
                interaction.EncounterId = visitDetail.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = visitDetail.CreatedBy;
                interaction.CreatedIn = visitDetail.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                visitDetail.InteractionId = interactionId;
                visitDetail.DateCreated = DateTime.Now;
                visitDetail.IsDeleted = false;
                visitDetail.IsSynced = false;

                context.VisitDetailRepository.Add(visitDetail);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVisitDetailByKey", new { key = visitDetail.InteractionId }, visitDetail);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVisitDetail", "VisitDetailController.cs", ex.Message, visitDetail.CreatedIn, visitDetail.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-details
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitDetails)]
        public async Task<IActionResult> ReadVisitDetails()
        {
            try
            {
                var visitDetailInDb = await context.VisitDetailRepository.GetVisitDetails();
                visitDetailInDb = visitDetailInDb.OrderByDescending(x => x.DateCreated);
                return Ok(visitDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitDetails", "VisitDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-detail/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitDetailByKey)]
        public async Task<IActionResult> ReadVisitDetailByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitDetailIndb = await context.VisitDetailRepository.GetVisitDetailByKey(key);

                if (visitDetailIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(visitDetailIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitDetailByKey", "VisitDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-detail/byClient/{ClientID}
        /// </summary>
        /// <param name="ClientID">Primary key of the table VisitDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitDetailByClient)]
        public async Task<IActionResult> ReadVisitDetailByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {

                    var visitDetailInDb = await context.VisitDetailRepository.GetVisitDetailByClient(ClientID);

                    return Ok(visitDetailInDb);
                }
                else
                {
                    var visitDetailInDb = await context.VisitDetailRepository.GetVisitDetailByClient(ClientID, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<VisitDetail> visitDetailDto = new PagedResultDto<VisitDetail>()
                    {
                        Data = visitDetailInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.VisitDetailRepository.GetVisitDetailByClientTotalCount(ClientID, encounterType)
                    };

                    return Ok(visitDetailDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitDetailByClient", "VisitDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitDetails.</param>
        /// <param name="visitDetail">VisitDetail to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVisitDetail)]
        public async Task<IActionResult> UpdateVisitDetail(Guid key, VisitDetail visitDetail)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = visitDetail.ModifiedBy;
                interactionInDb.ModifiedIn = visitDetail.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != visitDetail.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                visitDetail.DateModified = DateTime.Now;
                visitDetail.IsDeleted = false;
                visitDetail.IsSynced = false;

                context.VisitDetailRepository.Update(visitDetail);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVisitDetail", "VisitDetailController.cs", ex.Message, visitDetail.ModifiedIn, visitDetail.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitDetails.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteVisitDetail)]
        public async Task<IActionResult> DeleteVisitDetail(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitDetailInDb = await context.VisitDetailRepository.GetVisitDetailByKey(key);

                if (visitDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.VisitDetailRepository.Update(visitDetailInDb);
                await context.SaveChangesAsync();

                return Ok(visitDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVisitDetail", "VisitDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}