using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    /// MedicalCondition controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicalConditionsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicalConditionsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicalConditionsController(IUnitOfWork context, ILogger<MedicalConditionsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/medical-condition
        /// </summary>
        /// <param name="medicalCondition">MedicalCondition object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicalCondition)]
        public async Task<IActionResult> CreateMedicalCondition(MedicalCondition medicalCondition)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MedicalCondition, medicalCondition.EncounterType);
                interaction.EncounterId = medicalCondition.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = medicalCondition.CreatedBy;
                interaction.CreatedIn = medicalCondition.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                medicalCondition.InteractionId = interactionId;
                medicalCondition.DateCreated = DateTime.Now;
                medicalCondition.IsDeleted = false;
                medicalCondition.IsSynced = false;

                context.MedicalConditionRepository.Add(medicalCondition);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMedicalConditionByKey", new { key = medicalCondition.InteractionId }, medicalCondition);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMedicalCondition", "MedicalConditionsController.cs", ex.Message, medicalCondition.CreatedIn, medicalCondition.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalConditions)]
        public async Task<IActionResult> ReadMedicalConditions()
        {
            try
            {
                var medicalConditionInDb = await context.MedicalConditionRepository.GetMedicalConditions();

                return Ok(medicalConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalConditions", "MedicalConditionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalConditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalConditionByKey)]
        public async Task<IActionResult> ReadMedicalConditionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalConditionInDb = await context.MedicalConditionRepository.GetMedicalConditionByKey(key);

                if (medicalConditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(medicalConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalConditionByKey", "MedicalConditionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-condition/ByClient/{ClientID}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalConditionByClient)]
        public async Task<IActionResult> ReadMedicalConditionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var medicalConditionInDb = await context.MedicalConditionRepository.GetMedicalConditionByClient(clientId);

                    return Ok(medicalConditionInDb);
                }
                else
                {
                    var medicalConditionInDb = await context.MedicalConditionRepository.GetMedicalConditionByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);


                    PagedResultDto<MedicalCondition> medicalConditionDto = new PagedResultDto<MedicalCondition>()
                    {
                        Data = medicalConditionInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.MedicalConditionRepository.GetMedicalConditionByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(medicalConditionDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalConditionByClient", "MedicalConditionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalConditions.</param>
        /// <param name="medicalCondition">MedicalCondition to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMedicalCondition)]
        public async Task<IActionResult> UpdateMedicalCondition(Guid key, MedicalCondition medicalCondition)
        {
            try
            {
                if (key != medicalCondition.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = medicalCondition.ModifiedBy;
                interactionInDb.ModifiedIn = medicalCondition.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                medicalCondition.DateModified = DateTime.Now;
                medicalCondition.IsDeleted = false;
                medicalCondition.IsSynced = false;

                context.MedicalConditionRepository.Update(medicalCondition);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicalCondition", "MedicalConditionsController.cs", ex.Message, medicalCondition.ModifiedIn, medicalCondition.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalConditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMedicalCondition)]
        public async Task<IActionResult> DeleteMedicalCondition(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalConditionInDb = await context.MedicalConditionRepository.GetMedicalConditionByKey(key);

                if (medicalConditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                medicalConditionInDb.DateModified = DateTime.Now;
                medicalConditionInDb.IsDeleted = true;
                medicalConditionInDb.IsSynced = false;

                context.MedicalConditionRepository.Update(medicalConditionInDb);
                await context.SaveChangesAsync();

                return Ok(medicalConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMedicalCondition", "MedicalConditionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}