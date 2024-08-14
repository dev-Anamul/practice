using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;
using Interaction = Domain.Entities.Interaction;

/*
 * Created by    : Lion
 * Date created  : 04.01.2022
 * Modified by   : Bella
 * Last modified : 09.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Diagnosis controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DiagnosisController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DiagnosisController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DiagnosisController(IUnitOfWork context, ILogger<DiagnosisController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region Diagnosis
        /// <summary>
        /// URL: sc-api/diagnosis
        /// </summary>
        /// <param name="diagnosis">Diagnosis object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDiagnosis)]
        public async Task<IActionResult> CreateDiagnosis(Diagnosis diagnosis)
        {

            try
            {
                if (diagnosis.Diagnoses == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.AddCondtions);

                foreach (var item in diagnosis.Diagnoses)
                {
                    Guid interactionId = Guid.NewGuid();
                    Interaction interaction = new Interaction();
                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Diagnosis, diagnosis.EncounterType);
                    interaction.EncounterId = diagnosis.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = diagnosis.CreatedBy;
                    interaction.CreatedIn = diagnosis.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);

                    diagnosis.InteractionId = interactionId;
                    diagnosis.ICDId = item.ICDId;
                    diagnosis.NTGId = item.NTGId;
                    diagnosis.DateCreated = DateTime.Now;
                    diagnosis.IsDeleted = false;
                    diagnosis.IsSynced = false;


                    context.DiagnosisRepository.Add(diagnosis);
                    await context.SaveChangesAsync();
                }

                return CreatedAtAction("ReadDiagnosisByKey", new { key = diagnosis.InteractionId }, diagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.CreatedIn, diagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnoses)]
        public async Task<IActionResult> ReadDiagnoses()
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnoses();
                diagnosisInDb = diagnosisInDb.OrderByDescending(x => x.DateCreated);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnoses", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnosis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosisByKey)]
        public async Task<IActionResult> ReadDiagnosisByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByKey(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosisByKey", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadDiagnosisByClient)]
        public async Task<IActionResult> ReadDiagnosisByClient(Guid clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisDb = await context.DiagnosisRepository.GetLatestDiagnosisByClient(clientId);

                if (diagnosisDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosisByClient", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpGet]
        [Route(RouteConstants.ReadLastEncounterDiagnosisByClient)]
        public async Task<IActionResult> GetLastEncounterDiagnosisByClient(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisDb = await context.DiagnosisRepository.GetLastEncounterDiagnosisByClient(clientId);

                if (diagnosisDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetLastEncounterDiagnosisByClient", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/diagnosis/surgery/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Surgeries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosisBySurgeryId)]
        public async Task<IActionResult> ReadDiagnosisBySurgery(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisBySurgeryId(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosisBySurgery", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosesByOPDVisit)]
        public async Task<IActionResult> ReadDiagnosesByOPDVisitID(Guid encounterId)
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(encounterId);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosesByOPDVisitID", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadDiagnosesBYClient)]
        public async Task<IActionResult> ReadDiagnosesByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                  //  var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(clientId);
                    var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClientLast24Hours(clientId);

                    return Ok(diagnosisInDb);
                }
                else
                {
                    var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<Diagnosis> diagnosisWithPaggingDto = new PagedResultDto<Diagnosis>()
                    {
                        Data = diagnosisInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.DiagnosisRepository.GetDiagnosisByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(diagnosisWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDiagnosesByClient", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnosis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <param name="diagnosis">Diagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDiagnosis)]
        public async Task<IActionResult> UpdateDiagnosis(Guid key, Diagnosis diagnosis)
        {
            try
            {
                if (key != diagnosis.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = diagnosis.ModifiedBy;
                interactionInDb.ModifiedIn = diagnosis.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                diagnosis.DateModified = DateTime.Now;
                diagnosis.IsDeleted = false;
                diagnosis.IsSynced = false;

                context.DiagnosisRepository.Update(diagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.ModifiedIn, diagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnosis/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveDiagnosis)]
        public async Task<IActionResult> RemoveDiagnosis(Guid encounterId, EncounterType? encounterType)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByEncounter(encounterId, encounterType);

                if (diagnosisInDb == null || !diagnosisInDb.Any())
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in diagnosisInDb)
                {
                    context.DiagnosisRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveDiagnosis", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        #endregion

        #region Load TreeView
        /// <summary>
        /// URL: sc-api/diagnosis/loadNTGTree
        /// </summary>  
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.LoadNTGTreeDiagnosis)]
        public async Task<IActionResult> LoadNTGTree()
        {
            try
            {
                var tree = await LoadNTGTreeAsync();

                return Ok(tree);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadNTGTree", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.LoadNTGLevel1Diagnosis)]
        public async Task<IActionResult> LoadNTGLevel1()
        {
            try
            {
                var ntgLevelOneDiagnosisInDb = await context.NTGLevelOneDiagnosisRepository.GetNTGLevelOneDiagnoses();

                return Ok(ntgLevelOneDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadNTGLevel1", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.LoadNTGLevel2Diagnosis)]
        public async Task<IActionResult> LoadNTGLevel2()
        {
            try
            {
                var ntgLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnosisRepository.GetNTGLevelTwoDiagnoses();

                return Ok(ntgLevelTwoDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadNTGLevel2", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.LoadNTGLevel3Diagnosis)]
        public async Task<IActionResult> LoadNTGLevel3()
        {
            try
            {
                var ntgLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnosisRepository.GetNTGLevelThreeDiagnoses();

                return Ok(ntgLevelThreeDiagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadNTGLevel3", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/diagnosis/loadDiagnosisCodesTree
        /// </summary>
        /// <returns>Http status code: Ok.</returns>

        [HttpGet]
        [Route(RouteConstants.LoadDiagnosisCodesTreeDiagnosis)]
        public async Task<IActionResult> LoadDiagnosisCodesTree()
        {
            try
            {
                var tree = await LoadDiagnosisCodesWithTreeAsync();

                return Ok(tree);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoadDiagnosisCodesTree", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        private async Task<List<ICDsDigonosisCodeDto>> LoadDiagnosisCodesWithTreeAsync()
        {
            var icdDiagnosisInDb = await context.ICDDiagnosisRepository.GetICDDiagnoses();

            return ICDsDigonosisCodeDto.BuildTree(icdDiagnosisInDb.ToList());
        }
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

            return NTGTreeDto.BuildTree(ntgLevelOneDiagnosisInDb.ToList());
        }

        #endregion

        #region IPD Diagnosis
        /// <summary>
        /// URL: sc-api/ipd-diagnosis
        /// </summary>
        /// <param name="diagnosis">Diagnosis object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIPDDiagnosis)]
        public async Task<IActionResult> CreateIPDDiagnosis(Diagnosis diagnosis)
        {
            try
            {
                var Diagnose = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(diagnosis.EncounterId);

                if (diagnosis.ICD11 != null)
                {
                    foreach (var item in diagnosis.ICD11)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.ICDId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.EncounterId = diagnosis.EncounterId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.ICDId = item;
                            diagnosis.NTGId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                if (diagnosis.NTG != null)
                {
                    foreach (var item in diagnosis.NTG)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.NTGId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.NTGId = item;
                            diagnosis.ICDId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                return CreatedAtAction("ReadDiagnosisByKey", new { key = diagnosis.InteractionId }, diagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIPDDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.CreatedIn, diagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-diagnosis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <param name="diagnosis">Diagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIPDDiagnosis)]
        public async Task<IActionResult> UpdateIPDDiagnosis(Guid key, Diagnosis diagnosis)
        {
            try
            {
                if (key != diagnosis.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                diagnosis.DateModified = DateTime.Now;
                diagnosis.IsDeleted = false;
                diagnosis.IsSynced = false;

                context.DiagnosisRepository.Update(diagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIPDDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.ModifiedIn, diagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/ipd-diagnosis/remove/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIPDDiagnosis)]
        public async Task<IActionResult> RemoveIPDDiagnosis(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in diagnosisInDb)
                {
                    context.DiagnosisRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIPDDiagnosis", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PEP Diagnosis
        /// <summary>
        /// URL: sc-api/pep-diagnosis
        /// </summary>
        /// <param name="diagnosis">Diagnosis object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePEPDiagnosis)]
        public async Task<IActionResult> CreatePEPDiagnosis(Diagnosis diagnosis)
        {
            try
            {
                var Diagnose = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(diagnosis.EncounterId);

                if (diagnosis.ICD11 != null)
                {
                    foreach (var item in diagnosis.ICD11)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.ICDId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.ICDId = item;
                            diagnosis.NTGId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }
                if (diagnosis.NTG != null)
                {
                    foreach (var item in diagnosis.NTG)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.NTGId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.NTGId = item;
                            diagnosis.ICDId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                return CreatedAtAction("ReadPEPDiagnosisByKey", new { key = diagnosis.InteractionId }, diagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePEPDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.CreatedIn, diagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-diagnosis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPDiagnosisByKey)]
        public async Task<IActionResult> ReadPEPDiagnosisByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByKey(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPDiagnosisByKey", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPDiagnoses)]
        public async Task<IActionResult> ReadPEPDiagnoses()
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnoses();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPDiagnoses", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPDiagnosisByEncounter)]
        public async Task<IActionResult> ReadPEPDiagnosisByEncounter(Guid EncounterID)
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(EncounterID);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPDiagnosisByEncounter", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-diagnoses-byClient/{ClientID}
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadPEPDiagnosisByClient)]
        public async Task<IActionResult> ReadPEPDiagnosisByClient(Guid ClientID)
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(ClientID);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPEPDiagnosisByClient", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pep-diagnosis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <param name="diagnosis">Diagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePEPDiagnosis)]
        public async Task<IActionResult> UpdatePEPDiagnosis(Guid key, Diagnosis diagnosis)
        {
            try
            {
                if (key != diagnosis.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                diagnosis.DateModified = DateTime.Now;
                diagnosis.IsDeleted = false;
                diagnosis.IsSynced = false;

                context.DiagnosisRepository.Update(diagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePEPDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.ModifiedIn, diagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePEPDiagnosis)]
        public async Task<IActionResult> RemovePEPDiagnosis(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in diagnosisInDb)
                {
                    context.DiagnosisRepository.Delete(item);
                }
                await context.SaveChangesAsync();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePEPDiagnosis", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PrEP Diagnosis
        /// <summary>
        /// URL: sc-api/prep-diagnosis
        /// </summary>
        /// <param name="diagnosis">Diagnosis object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrEPDiagnosis)]
        public async Task<IActionResult> CreatePrEPDiagnosis(Diagnosis diagnosis)
        {
            try
            {
                var Diagnose = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(diagnosis.EncounterId);

                if (diagnosis.ICD11 != null)
                {
                    foreach (var item in diagnosis.ICD11)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.ICDId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.ICDId = item;
                            diagnosis.NTGId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }
                if (diagnosis.NTG != null)
                {
                    foreach (var item in diagnosis.NTG)
                    {
                        if (Diagnose != null && !Diagnose.Where(x => x.NTGId == item).Any())
                        {
                            Guid interactionId = Guid.NewGuid();

                            Interaction interaction = new Interaction();

                            interaction.Oid = interactionId;
                            interaction.ServiceCode = Convert.ToString(5);
                            interaction.EncounterId = diagnosis.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = diagnosis.CreatedBy;
                            interaction.CreatedIn = diagnosis.CreatedIn;
                            interaction.IsDeleted = false;
                            interaction.IsSynced = false;

                            context.InteractionRepository.Add(interaction);

                            diagnosis.InteractionId = interactionId;
                            diagnosis.ClientId = diagnosis.ClientId;
                            diagnosis.DateCreated = DateTime.Now;
                            diagnosis.IsDeleted = false;
                            diagnosis.IsSynced = false;
                            diagnosis.NTGId = item;
                            diagnosis.ICDId = null;

                            context.DiagnosisRepository.Add(diagnosis);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                return CreatedAtAction("ReadPrEPDiagnosisByKey", new { key = diagnosis.InteractionId }, diagnosis);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrEPDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.CreatedIn, diagnosis.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-diagnosis/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPDiagnosisByKey)]
        public async Task<IActionResult> ReadPrEPDiagnosisByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByKey(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPDiagnosisByKey", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-diagnoses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPDiagnoses)]
        public async Task<IActionResult> ReadPrEPDiagnoses()
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnoses();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPDiagnoses", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-diagnoses
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPDiagnosisByEncounter)]
        public async Task<IActionResult> ReadPrEPDiagnosisByEncounter(Guid EncounterID)
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(EncounterID);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPDiagnosisByEncounter", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-diagnoses-byClient/{ClientID}
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrEPDiagnosisByClient)]
        public async Task<IActionResult> ReadPrEPDiagnosisByClient(Guid ClientID)
        {
            try
            {
                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByClient(ClientID);

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrEPDiagnosisByClient", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prep-diagnosis/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Diagnoses.</param>
        /// <param name="diagnosis">Diagnosis to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePrEPDiagnosis)]
        public async Task<IActionResult> UpdatePrEPDiagnosis(Guid key, Diagnosis diagnosis)
        {
            try
            {
                if (key != diagnosis.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                diagnosis.DateModified = DateTime.Now;
                diagnosis.IsDeleted = false;
                diagnosis.IsSynced = false;

                context.DiagnosisRepository.Update(diagnosis);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePrEPDiagnosis", "DiagnosisController.cs", ex.Message, diagnosis.ModifiedIn, diagnosis.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePrEPDiagnosis)]
        public async Task<IActionResult> RemovePrEPDiagnosis(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in diagnosisInDb)
                {
                    context.DiagnosisRepository.Delete(item);
                }
                await context.SaveChangesAsync();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePrEPDiagnosis", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region Under5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveUnderFiveDiagnosis)]
        public async Task<IActionResult> RemoveUnderFiveDiagnosis(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var diagnosisInDb = await context.DiagnosisRepository.GetDiagnosisByOPDVisit(key);

                if (diagnosisInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in diagnosisInDb)
                {
                    context.DiagnosisRepository.Delete(item);
                }
                await context.SaveChangesAsync();

                return Ok(diagnosisInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveUnderFiveDiagnosis", "DiagnosisController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}