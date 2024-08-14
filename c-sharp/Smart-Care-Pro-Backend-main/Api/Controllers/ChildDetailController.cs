using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChildDetail controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ChildDetailController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ChildDetailController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ChildDetailController(IUnitOfWork context, ILogger<ChildDetailController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/child-detail
        /// </summary>
        /// <param name="childDetail">ChildDetail object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateChildDetail)]
        public async Task<IActionResult> CreateChildDetail(ChildDetail childDetail)
        {
            try
            {

                List<Interaction> interactions = new List<Interaction>();

                foreach (var item in childDetail.ChildDetails)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ChildDetail, childDetail.EncounterType);
                    interaction.EncounterId = childDetail.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = childDetail.CreatedBy;
                    interaction.CreatedIn = childDetail.CreatedIn;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;


                    interactions.Add(interaction);

                    item.InteractionId = interactionId;
                    item.DateCreated = DateTime.Now;
                    item.EncounterId = childDetail.EncounterId;
                    item.ClientId = childDetail.ClientId;
                    item.EncounterType = childDetail.EncounterType;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.CreatedBy = childDetail.CreatedBy;
                    item.CreatedIn = childDetail.CreatedIn;
                }

                context.InteractionRepository.AddRange(interactions);
                context.ChildDetailRepository.AddRange(childDetail.ChildDetails);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadChildDetailByKey", new { key = childDetail.InteractionId }, childDetail);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}",
                    DateTime.Now, "BusinessLayer", "CreateChildDetail", "ChildDetailController.cs", ex.Message,
                    childDetail.CreatedIn, childDetail.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/child-details
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildDetails)]
        public async Task<IActionResult> ReadChildDetails()
        {
            try
            {
                var childDetailInDb = await context.ChildDetailRepository.GetChildDetails();

                childDetailInDb = childDetailInDb.OrderByDescending(x => x.DateCreated);

                return Ok(childDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildDetails", "ChildDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/child-detail/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildDetailByClient)]
        public async Task<IActionResult> ReadChildDetailByClient(Guid clientId)
        {
            try
            {
                var childDetailInDb = await context.ChildDetailRepository.GetChildDetailByClient(clientId);

                return Ok(childDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildDetailByClient", "ChildDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/child-detail/ByEncounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildDetailByEncounter)]
        public async Task<IActionResult> ReadChildDetailByEncounter(Guid encounterId)
        {
            try
            {
                var childDetailInDb = await context.ChildDetailRepository.GetChildDetailByEncounter(encounterId);

                return Ok(childDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildDetailByEncounter", "ChildDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/child-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadChildDetailByKey)]
        public async Task<IActionResult> ReadChildDetailByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var childDetailInDb = await context.ChildDetailRepository.GetChildDetailByKey(key);

                if (childDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(childDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadChildDetailByKey", "ChildDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/child-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildDetails.</param>
        /// <param name="childDetail">ChildDetail to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateChildDetail)]
        public async Task<IActionResult> UpdateChildDetail(Guid key, ChildDetail childDetail)
        {
            try
            {
                if (key != childDetail.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = childDetail.ModifiedBy;
                interactionInDb.ModifiedIn = childDetail.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var childDetailInDb = await context.ChildDetailRepository.GetChildDetailByKey(childDetail.InteractionId);

                childDetailInDb.DateModified = DateTime.Now;
                childDetailInDb.IsDeleted = false;
                childDetailInDb.IsSynced = false;
                childDetailInDb.BirthOutcome = childDetail.BirthOutcome;
                childDetailInDb.BirthWeight = childDetail.BirthWeight;
                childDetailInDb.ChildCareNumber = childDetail.ChildCareNumber;
                childDetailInDb.ChildName = childDetail.ChildName;
                childDetailInDb.ChildSex = childDetail.ChildSex;
                childDetailInDb.DateOfChildTurns18Months = childDetail.DateOfChildTurns18Months;
                childDetailInDb.LastTCResult = childDetail.LastTCResult;
                childDetailInDb.LastTCDate = childDetail.LastTCDate;

                context.ChildDetailRepository.Update(childDetailInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateChildDetail", "ChildDetailController.cs", ex.Message, childDetail.CreatedIn, childDetail.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/child-detail/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ChildDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteChildDetail)]
        public async Task<IActionResult> DeleteChildDetail(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var childDetailInDb = await context.ChildDetailRepository.GetChildDetailByKey(key);

                if (childDetailInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                childDetailInDb.DateModified = DateTime.Now;
                childDetailInDb.IsDeleted = true;
                childDetailInDb.IsSynced = false;

                context.ChildDetailRepository.Update(childDetailInDb);
                await context.SaveChangesAsync();

                return Ok(childDetailInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteChildDetail", "ChildDetailController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}