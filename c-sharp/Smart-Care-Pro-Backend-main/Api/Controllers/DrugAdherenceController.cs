using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 01.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugAdherenceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugAdherenceController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugAdherenceController(IUnitOfWork context, ILogger<DrugAdherenceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drug-adherence
        /// </summary>
        /// <param name="drugAdherence">DrugAdherence object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugAdherence)]
        public async Task<IActionResult> CreateDrugAdherence(DrugAdherence drugAdherence)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DrugAdherence, drugAdherence.EncounterType);
                interaction.EncounterId = drugAdherence.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = drugAdherence.CreatedBy;
                interaction.CreatedIn = drugAdherence.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                drugAdherence.InteractionId = interactionId;
                drugAdherence.EncounterId = drugAdherence.EncounterId;
                drugAdherence.ClientId = drugAdherence.ClientId;
                drugAdherence.DateCreated = DateTime.Now;
                drugAdherence.IsDeleted = false;
                drugAdherence.IsSynced = false;

                context.DrugAdherenceRepository.Add(drugAdherence);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDrugAdherenceByKey", new { key = drugAdherence.InteractionId }, drugAdherence);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugAdherence", "DrugAdherenceController.cs", ex.Message, drugAdherence.CreatedIn, drugAdherence.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drug-adherences
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugAdherences)]
        public async Task<IActionResult> ReadDrugAdherences()
        {
            try
            {
                var drugAdherenceInDb = await context.DrugAdherenceRepository.GetDrugAdherences();
                drugAdherenceInDb = drugAdherenceInDb.OrderByDescending(x => x.DateCreated);
                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugAdherences", "DrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drug-adherence/key/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugAdherenceByClient)]
        public async Task<IActionResult> ReadDrugAdherenceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var drugAdherenceInDb = await context.DrugAdherenceRepository.GetDrugAdherenceByClient(clientId);

                    return Ok(drugAdherenceInDb);
                }
                else
                {

                    var drugAdherenceInDb = await context.DrugAdherenceRepository.GetDrugAdherenceByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<DrugAdherence> drugAdherenceWithPaggingDto = new PagedResultDto<DrugAdherence>()
                    {
                        Data = drugAdherenceInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.DrugAdherenceRepository.GetDrugAdherenceByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(drugAdherenceWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugAdherenceByClient", "DrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drug-adherence/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugAdherences.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugAdherenceByKey)]
        public async Task<IActionResult> ReadDrugAdherenceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugAdherenceInDb = await context.DrugAdherenceRepository.GetDrugAdherenceByKey(key);

                if (drugAdherenceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugAdherenceByKey", "DrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/drug-adherence/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugAdherences.</param>
        /// <param name="drugAdherence">DrugAdherence to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugAdherence)]
        public async Task<IActionResult> UpdateDrugAdherence(Guid key, DrugAdherence drugAdherence)
        {
            try
            {
                if (key != drugAdherence.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = drugAdherence.ModifiedBy;
                interactionInDb.ModifiedIn = drugAdherence.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                drugAdherence.DateModified = DateTime.Now;
                drugAdherence.IsDeleted = false;
                drugAdherence.IsSynced = false;

                context.DrugAdherenceRepository.Update(drugAdherence);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugAdherence", "DrugAdherenceController.cs", ex.Message, drugAdherence.ModifiedIn, drugAdherence.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drug-adherence/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugAdherences.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugAdherence)]
        public async Task<IActionResult> DeleteDrugAdherence(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugAdherenceInDb = await context.DrugAdherenceRepository.GetDrugAdherenceByKey(key);

                if (drugAdherenceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugAdherenceInDb.DateModified = DateTime.Now;
                drugAdherenceInDb.IsDeleted = true;
                drugAdherenceInDb.IsSynced = false;

                context.DrugAdherenceRepository.Update(drugAdherenceInDb);
                await context.SaveChangesAsync();

                return Ok(drugAdherenceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugAdherence", "DrugAdherenceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}