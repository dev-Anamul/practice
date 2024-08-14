using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 05.01.2022
 * Modified by   :  Stephan
 * Last modified: 27.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 * 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Condition controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ConditionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ConditionController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ConditionController(IUnitOfWork context, ILogger<ConditionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region Condition
        /// <summary>
        /// URL: sc-api/condition
        /// </summary>
        /// <param name="conditionDto">Condition object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCondition)]
        public async Task<IActionResult> CreateCondition(ConditionDto conditionDto)
        {
            try
            {

                if (conditionDto is null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.GenericError);

                List<Interaction> interactions = new List<Interaction>();

                if (conditionDto.conditions == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.AddCondtions);

                foreach (var item in conditionDto.conditions)
                {
                    Guid interactionId = Guid.NewGuid();
                    Interaction interaction = new Interaction();
                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Condition, conditionDto.EncounterType);
                    interaction.EncounterId = conditionDto.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = conditionDto.CreatedBy;
                    interaction.CreatedIn = conditionDto.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionId;
                    item.EncounterId = conditionDto.EncounterId;
                    item.EncounterType = conditionDto.EncounterType;
                    item.ClientId = conditionDto.ClientId;
                    item.DateCreated = DateTime.Now;
                    item.CreatedIn = conditionDto.CreatedIn;
                    item.CreatedBy = conditionDto.CreatedBy;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                }

                context.InteractionRepository.AddRange(interactions);
                context.ConditionRepository.AddRange(conditionDto.conditions);

                await context.SaveChangesAsync();
                return CreatedAtAction("CreateCondition", new { byOpdVisitId = conditionDto.conditions.Select(x => x.EncounterId).FirstOrDefault() }, conditionDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCondition", "ConditionController.cs", ex.Message, conditionDto.CreatedIn, conditionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/by-encounter/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table Conditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConditionByEncounterId)]
        public async Task<IActionResult> GetConditionByOPDVisitID(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var conditionInDb = await context.ConditionRepository.GetConditionByOPDVisitID(encounterId);

                conditionInDb = conditionInDb.OrderByDescending(x => x.DateCreated);

                if (conditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetConditionByOPDVisitID", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConditionByKey)]
        public async Task<IActionResult> ReadConditionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var conditionInDb = await context.ConditionRepository.GetConditionByKey(key);

                if (conditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConditionByKey", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConditions)]
        public async Task<IActionResult> ReadConditions()
        {
            try
            {
                var conditionInDb = await context.ConditionRepository.GetConditions();

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConditions", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadConditionByClient)]
        public async Task<IActionResult> ReadConditionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    // var conditionInDb = await context.ConditionRepository.GetConditionByClient(clientId);
                    var conditionInDb = await context.ConditionRepository.GetConditionByClientLast24Hours(clientId);

                    return Ok(conditionInDb);
                }
                else
                {
                    var conditionInDb = await context.ConditionRepository.GetConditionByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<Condition> conditionWithPaggingDto = new PagedResultDto<Condition>()
                    {
                        Data = conditionInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ConditionRepository.GetConditionByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(conditionWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadConditionByClient", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <param name="condition">Conditions to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCondition)]
        public async Task<IActionResult> UpdateCondition(Guid key, Condition condition)
        {
            try
            {
                if (key != condition.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                    var conditionInDb = await context.ConditionRepository.GetConditionByKey(condition.InteractionId);

                    conditionInDb.NTGId = condition.NTGId;
                    conditionInDb.ICDId = condition.ICDId;
                    conditionInDb.Certainty = condition.Certainty;
                    conditionInDb.ConditionType = condition.ConditionType;
                    conditionInDb.Comments = condition.Comments;
                    conditionInDb.DateDiagnosed = condition.DateDiagnosed;
                    conditionInDb.DateResolved = condition.DateResolved;
                    conditionInDb.EncounterType = condition.EncounterType;
                    conditionInDb.CreatedIn = condition.CreatedIn;
                    conditionInDb.CreatedBy = condition.CreatedBy;
                    conditionInDb.DateModified = DateTime.Now;
                    conditionInDb.IsDeleted = false;
                    conditionInDb.IsSynced = false;

                    context.ConditionRepository.Update(conditionInDb);
                
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateCondition", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/remove-condition/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveCondition)]
        public async Task<IActionResult> RemoveCondition(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemInDb = await context.ConditionRepository.GetConditionByOPDVisitID(encounterId);

                if (systemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemInDb)
                {
                    context.ConditionRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveCondition", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Load TreeView
        /// <summary>
        /// URL: sc-api/condition/LoadNTGTree
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.LoadNTGTreeCondition)]
        public async Task<IActionResult> LoadNTGTree()
        {
            try
            {
                var tree = await LoadNTGTreeAsync();

                return Ok(tree);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadNTGTree", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/conditions/LoadDiagnosisCodesTree
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.LoadDiagnosisCodesTreeCondition)]
        public async Task<IActionResult> LoadDiagnosisCodesTree()
        {
            try
            {
                var tree = await LoadDiagnosisCodesWithTreeAsync();

                return Ok(tree);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadDiagnosisCodesTree", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Load Diagnosis Codes with Tree
        /// </summary>
        /// <returns></returns>
        private async Task<List<ICDsDigonosisCodeDto>> LoadDiagnosisCodesWithTreeAsync()
        {
            var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnoses();

            return ICDsDigonosisCodeDto.BuildTree(icdDiagnosisInDb.ToList());
        }

        /// <summary>
        /// Load NTG Tree Diagnosis
        /// </summary>
        /// <returns></returns>
        private async Task<List<NTGTreeDto>> LoadNTGTreeAsync()
        {
            var ntgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnoses();

            foreach (var item in ntgLevelOneDiagnosisInDb)
            {
                foreach (var item2 in item.NTGLevelTwoDiagnoses)
                {
                    item2.NTGLevelThreeDiagnoses = await context.NTGLevelThreeDiagnosisRepository.LoadListWithChildAsync<NTGLevelThreeDiagnosis>(x => x.IsDeleted == false && x.NTGLevelTwoId == item2.Oid);
                }
            }
            //   var nTGLevelOneDiagnoses = await ReadNTGLevelDiagnosis();

            return NTGTreeDto.BuildTree(ntgLevelOneDiagnosisInDb.ToList());
        }
        #endregion

        #region PEPCondition

        /// <summary>
        /// URL: sc-api/pep-condition/byClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPConditionByClient)]
        public async Task<IActionResult> ReadPEPConditionByClient(Guid ClientID)
        {
            try
            {
                var conditionInDb = await context.ConditionRepository.GetConditionByClient(ClientID);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPConditionByClient", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-condition
        /// </summary>
        /// <param name="conditionDto">Condition object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPCondition)]
        public async Task<IActionResult> CreatePEPCondition(ConditionDto conditionDto)
        {
            try
            {

                if (conditionDto is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                List<Interaction> interactions = new List<Interaction>();

                foreach (var item in conditionDto.conditions)
                {
                    Guid interactionID = Guid.NewGuid();

                    Interaction interaction = new Interaction();

                    interaction.Oid = interactionID;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Condition, conditionDto.EncounterType);
                    interaction.EncounterId = conditionDto.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = conditionDto.CreatedBy;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionID;
                    item.EncounterId = conditionDto.EncounterId;
                    item.EncounterType = conditionDto.EncounterType;
                    item.ClientId = conditionDto.ClientId;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                }

                context.InteractionRepository.AddRange(interactions);
                context.ConditionRepository.AddRange(conditionDto.conditions);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetConditionByOPDVisitID", new { byOpdVisitId = conditionDto.conditions.Select(x => x.EncounterId).FirstOrDefault() }, conditionDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPCondition", "ConditionController.cs", ex.Message, conditionDto.CreatedIn, conditionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-condition/byEncounter/{EncounterID}
        /// </summary>
        /// <param name="byOpdVisitId">Primary key of the table Conditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPConditionByEncounter)]
        public async Task<IActionResult> ReadPEPConditionByEncounter(Guid EncounterID)
        {
            try
            {
                if (EncounterID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var conditionInDb = await context.ConditionRepository.GetConditionByOPDVisitID(EncounterID);

                if (conditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPConditionByEncounter", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <param name="condition">Conditions to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPCondition)]
        public async Task<IActionResult> UpdatePEPCondition(Guid key, List<Condition> conditionsList)
        {
            try
            {
                if (key != conditionsList.Select(x => x.EncounterId).FirstOrDefault())
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                foreach (var condition in conditionsList)
                {
                    var conditionInDb = await context.ConditionRepository.GetConditionByKey(condition.InteractionId);

                    conditionInDb.NTGId = condition.NTGId;
                    conditionInDb.ICDId = condition.ICDId;
                    conditionInDb.Certainty = condition.Certainty;
                    conditionInDb.ConditionType = condition.ConditionType;
                    conditionInDb.Comments = condition.Comments;
                    conditionInDb.DateDiagnosed = condition.DateDiagnosed;
                    conditionInDb.DateResolved = condition.DateResolved;

                    conditionInDb.DateModified = DateTime.Now;
                    conditionInDb.IsDeleted = false;
                    conditionInDb.IsSynced = false;

                    context.ConditionRepository.Update(conditionInDb);
                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdatePEPCondition", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PrEPCondition
        /// <summary>
        /// URL: sc-api/prep-condition
        /// </summary>
        /// <param name="conditionDto">Condition object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEPCondition)]
        public async Task<IActionResult> CreatePrEPCondition(ConditionDto conditionDto)
        {
            try
            {

                if (conditionDto is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                List<Interaction> interactions = new List<Interaction>();

                foreach (var item in conditionDto.conditions)
                {
                    Guid interactionID = Guid.NewGuid();

                    Interaction interaction = new Interaction();

                    interaction.Oid = interactionID;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Condition, conditionDto.EncounterType);
                    interaction.EncounterId = conditionDto.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = conditionDto.CreatedBy;
                    interaction.CreatedIn = conditionDto.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    interactions.Add(interaction);

                    item.InteractionId = interactionID;
                    item.EncounterId = conditionDto.EncounterId;
                    item.ClientId = conditionDto.ClientId;
                    item.EncounterType = conditionDto.EncounterType;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                }
                context.InteractionRepository.AddRange(interactions);
                context.ConditionRepository.AddRange(conditionDto.conditions);

                await context.SaveChangesAsync();
                return CreatedAtAction("GetConditionByOPDVisitID", new { byOpdVisitId = conditionDto.conditions.Select(x => x.EncounterId).FirstOrDefault() }, conditionDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrEPCondition", "ConditionController.cs", ex.Message, conditionDto.CreatedIn, conditionDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-condition/byOpdVisitId/{byOpdVisitId}
        /// </summary>
        /// <param name="byOpdVisitId">Primary key of the table Conditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPConditionByEncounter)]
        public async Task<IActionResult> ReadPrEPConditionByEncounter(Guid EncounterID)
        {
            try
            {
                if (EncounterID == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var conditionInDb = await context.ConditionRepository.GetConditionByOPDVisitID(EncounterID);

                if (conditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPConditionByEncounter", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-condition/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPConditionByKey)]
        public async Task<IActionResult> ReadPrEPConditionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var conditionInDb = await context.ConditionRepository.GetConditionByKey(key);

                if (conditionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPConditionByKey", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPConditions)]
        public async Task<IActionResult> ReadPrEPConditions()
        {
            try
            {
                var conditionInDb = await context.ConditionRepository.GetConditions();

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPConditions", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-conditions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPConditionByClient)]
        public async Task<IActionResult> ReadPrEPConditionByClient(Guid ClientID)
        {
            try
            {
                var conditionInDb = await context.ConditionRepository.GetConditionByClient(ClientID);

                return Ok(conditionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPConditionByClient", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/condition/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <param name="ConditionsList">Conditions to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePrEPCondition)]
        public async Task<IActionResult> UpdatePrEPCondition(Guid key, List<Condition> ConditionsList)
        {
            try
            {
                if (key != ConditionsList.Select(x => x.EncounterId).FirstOrDefault())
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                foreach (var condition in ConditionsList)
                {
                    var conditionInDb = await context.ConditionRepository.GetConditionByKey(condition.InteractionId);

                    conditionInDb.NTGId = condition.NTGId;
                    conditionInDb.ICDId = condition.ICDId;
                    conditionInDb.Certainty = condition.Certainty;
                    conditionInDb.ConditionType = condition.ConditionType;
                    conditionInDb.Comments = condition.Comments;
                    conditionInDb.DateDiagnosed = condition.DateDiagnosed;
                    conditionInDb.DateResolved = condition.DateResolved;

                    conditionInDb.DateModified = DateTime.Now;
                    conditionInDb.IsDeleted = false;
                    conditionInDb.IsSynced = false;

                    context.ConditionRepository.Update(conditionInDb);
                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdatePrEPCondition", "ConditionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}