using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VisitPuposeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VisitPuposeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VisitPuposeController(IUnitOfWork context, ILogger<VisitPuposeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/visit-purpose
        /// </summary>
        /// <param name="visitPurpose">VisitPurpose object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVisitPurpose)]
        public async Task<IActionResult> CreateVisitPurpose(VisitPurpose visitPurpose)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.VisitPurpose, visitPurpose.EncounterType);
                interaction.EncounterId = visitPurpose.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = visitPurpose.CreatedBy;
                interaction.CreatedIn = visitPurpose.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                visitPurpose.InteractionId = interactionId;
                visitPurpose.EncounterId = visitPurpose.EncounterId;
                visitPurpose.ClientId = visitPurpose.ClientId;
                visitPurpose.DateCreated = DateTime.Now;
                visitPurpose.IsDeleted = false;
                visitPurpose.IsSynced = false;

                context.VisitPuposeRepository.Add(visitPurpose);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVisitPurposeByKey", new { key = visitPurpose.InteractionId }, visitPurpose);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVisitPurpose", "VisitPuposeController.cs", ex.Message, visitPurpose.CreatedIn, visitPurpose.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/visit-purpose
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVisitPurposes)]
        public async Task<IActionResult> ReadVisitPurposes()
        {
            try
            {
                var visitPurposeInDb = await context.VisitPuposeRepository.GetVisitPurposes();
                visitPurposeInDb = visitPurposeInDb.OrderByDescending(x => x.DateCreated);
                return Ok(visitPurposeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitPurposes", "VisitPuposeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadVisitPurposeByClient)]
        public async Task<IActionResult> ReadVisitPurposeByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var visitPurposeInDb = await context.VisitPuposeRepository.GetVisitPurposeByClient(clientId);

                    return Ok(visitPurposeInDb);
                }
                else
                {
                    var visitPurposeInDb = await context.VisitPuposeRepository.GetVisitPurposeByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<VisitPurpose> visitPurposeDto = new PagedResultDto<VisitPurpose>()
                    {
                        Data = visitPurposeInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.VisitPuposeRepository.GetVisitPurposeByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(visitPurposeDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitPurposeByClient", "VisitPuposeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

      /// <summary>
      /// URL: sc-api/visit-purpose/key/{clientId}
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadLatestVisitPurposesByClientId)]
      public async Task<IActionResult> ReadLatestVisitPurposesByClientId(Guid clientId)
      {
         try
         {
            var visitPurposeInDb = await context.VisitPuposeRepository.GetLatestVisitPurposeByClientID(clientId);

            if (visitPurposeInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            return Ok(visitPurposeInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestVisitPurposesByClientId", "VisitPuposeController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/visit-purpose/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table VisitPurposes.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
        [Route(RouteConstants.ReadVisitPurposeByKey)]
        public async Task<IActionResult> ReadVisitPurposeByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitPurposeInDb = await context.VisitPuposeRepository.GetVisitPurposeByKey(key);

                if (visitPurposeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(visitPurposeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVisitPurposeByKey", "VisitPuposeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/visit-purpose/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitPurposes.</param>
        /// <param name="visitPurpose">VisitPurpose to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVisitPurpose)]
        public async Task<IActionResult> UpdateVisitPurpose(Guid key, VisitPurpose visitPurpose)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = visitPurpose.ModifiedBy;
                interactionInDb.ModifiedIn = visitPurpose.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != visitPurpose.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                visitPurpose.DateModified = DateTime.Now;
                visitPurpose.IsDeleted = false;
                visitPurpose.IsSynced = false;

                context.VisitPuposeRepository.Update(visitPurpose);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVisitPurpose", "VisitPuposeController.cs", ex.Message, visitPurpose.ModifiedIn, visitPurpose.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/visit-purpose/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VisitPurposes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVisitPurpose)]
        public async Task<IActionResult> DeleteVisitPurpose(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var visitPurposeInDb = await context.VisitPuposeRepository.GetVisitPurposeByKey(key);

                if (visitPurposeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                visitPurposeInDb.DateModified = DateTime.Now;
                visitPurposeInDb.IsDeleted = true;
                visitPurposeInDb.IsSynced = false;

                context.VisitPuposeRepository.Update(visitPurposeInDb);
                await context.SaveChangesAsync();

                return Ok(visitPurposeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVisitPurpose", "VisitPuposeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}