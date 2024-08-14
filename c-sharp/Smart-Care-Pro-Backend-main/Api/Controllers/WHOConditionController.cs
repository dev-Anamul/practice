using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class WHOConditionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<WHOConditionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public WHOConditionController(IUnitOfWork context, ILogger<WHOConditionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/who-condition
        /// </summary>
        /// <param name="client">Client object.</param>
        /// <returns>Http status code: Created At</returns>
        [HttpPost]
        [Route(RouteConstants.CreateWHOConditions)]
        public async Task<IActionResult> CreateWHOConditions(WhoStagesConditionDto whoStagesConditionDto)
        {
            try
            {
                List<Interaction> interactions = new List<Interaction>();
                List<WHOCondition> whoCondions = new List<WHOCondition>();

                foreach (var item in whoStagesConditionDto.WhoStagesConditionID)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.WHOCondition, whoStagesConditionDto.EncounterType);
                    interaction.EncounterId = whoStagesConditionDto.EncounterID;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = whoStagesConditionDto.CreatedBy;
                    interaction.CreatedIn = whoStagesConditionDto.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    WHOCondition wHOCondition = new WHOCondition();

                    wHOCondition.InteractionId = interactionId;
                    wHOCondition.EncounterId = whoStagesConditionDto.EncounterID;
                    wHOCondition.ClientId = whoStagesConditionDto.ClientId;
                    wHOCondition.EncounterType = whoStagesConditionDto.EncounterType;
                    wHOCondition.WHOStagesConditionId = item;
                    wHOCondition.DateCreated = DateTime.Now;
                    wHOCondition.CreatedIn = whoStagesConditionDto.CreatedIn;
                    wHOCondition.CreatedBy = whoStagesConditionDto.CreatedBy;
                    wHOCondition.IsDeleted = false;
                    wHOCondition.IsSynced = false;

                    whoCondions.Add(wHOCondition);
                }

                context.InteractionRepository.AddRange(interactions);
                context.WHOConditionRepository.AddRange(whoCondions);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadWHOConditionByKey", new { key = whoCondions.Select(x => x.InteractionId).FirstOrDefault() }, whoCondions.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateWHOConditions", "WHOConditionController.cs", ex.Message, whoStagesConditionDto.CreatedIn, whoStagesConditionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadWHOConditions)]
        public async Task<IActionResult> ReadWHOConditions()
        {
            try
            {
                var whoConditionIndb = await context.WHOConditionRepository.GetWHOConditions();

                return Ok(whoConditionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOConditions", "WHOConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table WHOConditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadWHOConditionByKey)]
        public async Task<IActionResult> ReadWHOConditionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var whoConditionIndb = await context.WHOConditionRepository.GetWHOConditionByKey(key);

                if (whoConditionIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(whoConditionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOConditionByKey", "WHOConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadWHOConditionByClient)]
        public async Task<IActionResult> ReadWHOConditionByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var whoConditionIndb = await context.WHOConditionRepository.GetWHOConditionsByClient(ClientID);
                    whoConditionIndb = whoConditionIndb.OrderByDescending(x => x.DateCreated);
                    return Ok(whoConditionIndb);
                }
                else
                {
                    var whoConditionIndb = await context.WHOConditionRepository.GetWHOConditionsByClient(ClientID, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<WHOCondition> whoStagesConditionDto = new PagedResultDto<WHOCondition>()
                    {
                        Data = whoConditionIndb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.WHOConditionRepository.GetWHOConditionsByClientTotalCount(ClientID, encounterType)
                    };

                    return Ok(whoStagesConditionDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWHOConditionByClient", "WHOConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <param name="client">Client to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateWHOCondition)]
        public async Task<IActionResult> UpdateWHOCondition(Guid key, WhoStagesConditionDto whoStagesConditionDto)
        {
            try
            {
                if (whoStagesConditionDto.WhoStagesConditionID?.Length > 0)
                {
                    var whoStagesConditionInDb =await context.WHOConditionRepository.GetWHOConditionsByEncounterId(key);

                    foreach (var item in whoStagesConditionInDb)
                    {
                        var interactionInDb = await context.InteractionRepository.GetByIdAsync(item.InteractionId);
                        context.WHOConditionRepository.Delete(item);
                        context.InteractionRepository.Delete(interactionInDb);
                        await context.SaveChangesAsync();
                    }
                }
               
                
                

                List<Interaction> interactions = new List<Interaction>();
                List<WHOCondition> whoCondions = new List<WHOCondition>();

                foreach (var item in whoStagesConditionDto.WhoStagesConditionID)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.WHOCondition, whoStagesConditionDto.EncounterType);
                    interaction.EncounterId = whoStagesConditionDto.EncounterID;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = whoStagesConditionDto.CreatedBy;
                    interaction.CreatedIn = whoStagesConditionDto.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    WHOCondition wHOCondition = new WHOCondition();

                    wHOCondition.InteractionId = interactionId;
                    wHOCondition.EncounterId = whoStagesConditionDto.EncounterID;
                    wHOCondition.ClientId = whoStagesConditionDto.ClientId;
                    wHOCondition.EncounterType = whoStagesConditionDto.EncounterType;
                    wHOCondition.WHOStagesConditionId = item;
                    wHOCondition.DateCreated = DateTime.Now;
                    wHOCondition.CreatedIn = whoStagesConditionDto.CreatedIn;
                    wHOCondition.CreatedBy = whoStagesConditionDto.CreatedBy;
                    wHOCondition.IsDeleted = false;
                    wHOCondition.IsSynced = false;

                    whoCondions.Add(wHOCondition);
                }

                context.InteractionRepository.AddRange(interactions);
                context.WHOConditionRepository.AddRange(whoCondions);
                await context.SaveChangesAsync();


                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateWHOCondition", "WHOConditionController.cs", ex.Message, whoStagesConditionDto.CreatedIn, whoStagesConditionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/who-condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteWHOCondition)]
        public async Task<IActionResult> DeleteWHOCondition(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var wHOConditionInDb = await context.WHOConditionRepository.GetWHOConditionsByEncounterId(key);

                if (wHOConditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in wHOConditionInDb)
                {
                    item.DateModified = DateTime.Now;
                    item.IsDeleted = true;
                    item.IsSynced = false;

                    context.WHOConditionRepository.Update(item);
                    await context.SaveChangesAsync();
                }

                return Ok(wHOConditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteWHOCondition", "WHOConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}