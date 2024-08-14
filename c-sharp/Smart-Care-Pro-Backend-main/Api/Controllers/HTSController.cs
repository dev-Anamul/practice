using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 26.10.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// HTS controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class HTSController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<HTSController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public HTSController(IUnitOfWork context, ILogger<HTSController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/hts
        /// </summary>
        /// <param name="hts">HTS object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateHTS)]
        public async Task<IActionResult> CreateHTS(HTS hts)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.HTS, hts.EncounterType);
                interaction.EncounterId = hts.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = hts.CreatedBy;
                interaction.CreatedIn = hts.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                hts.InteractionId = interactionId;
                hts.DateCreated = DateTime.Now;
                hts.IsDeleted = false;
                hts.IsSynced = false;

                // Client update By HTS test result
                Client client = context.ClientRepository.GetClientByKey(hts.ClientId).Result;

                if (hts.DetermineTestResult == Enums.TestResult.Reactive && hts.BiolineTestResult == Enums.TestResult.Reactive)
                {
                    client.HIVStatus = Enums.HIVStatus.Positive;

                    context.ClientRepository.Update(client);
                }
                else if (hts.DetermineTestResult == Enums.TestResult.Reactive && hts.BiolineTestResult == Enums.TestResult.NonReactive)
                {
                    client.HIVStatus = Enums.HIVStatus.Indeterminant;

                    context.ClientRepository.Update(client);
                }
                else
                {
                    client.HIVStatus = Enums.HIVStatus.Negative;

                    context.ClientRepository.Update(client);
                }
                // end

                context.HTSRepository.Add(hts);
                await context.SaveChangesAsync();

                if (hts.RiskAssessmentList != null)
                {
                    foreach (var item in hts.RiskAssessmentList)
                    {
                        RiskAssessment riskAssessment = new RiskAssessment();

                        riskAssessment.Oid = Guid.NewGuid();
                        riskAssessment.RiskFactorId = item;
                        riskAssessment.HTSId = hts.InteractionId;
                        riskAssessment.IsDeleted = false;
                        riskAssessment.IsSynced = false;

                        context.RiskAssessmentRepository.Add(riskAssessment);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadHTSByKey", new { key = hts.InteractionId }, hts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateHTS", "HTSController.cs", ex.Message, hts.CreatedIn, hts.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHTS)]
        public async Task<IActionResult> ReadHTS(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var htsInDb = await context.HTSRepository.GetHTS(clientId);
                    var htsInDb = await context.HTSRepository.GetHTSLast24Hours(clientId);

                    return Ok(htsInDb);
                }
                else
                {
                    var htsInDb = await context.HTSRepository.GetHTS(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<HTS> htslDto = new PagedResultDto<HTS>()
                    {
                        Data = htsInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.HTSRepository.GetHTSTotalCount(clientId, encounterType)
                    };

                    return Ok(htslDto);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHTS", "HTSController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HTS.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHTSByKey)]
        public async Task<IActionResult> ReadHTSByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var htsInDb = await context.HTSRepository.GetHTSByKey(key);

                if (htsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                htsInDb.RiskAssessments = await context.RiskAssessmentRepository.LoadListWithChildAsync<RiskAssessment>(x => x.HTSId == htsInDb.InteractionId, p => p.HIVRiskFactor);

                return Ok(htsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHTSByKey", "HTSController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/latest-hts-by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table HTS.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLatestHTSByClient)]
        public async Task<IActionResult> ReadLatestHTSByClient(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var htsInDb = await context.HTSRepository.GetLatestHTSByClient(clientId);

                if (htsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                htsInDb.RiskAssessments = await context.RiskAssessmentRepository.LoadListWithChildAsync<RiskAssessment>(x => x.HTSId == htsInDb.InteractionId, p => p.HIVRiskFactor);

                return Ok(htsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestHTSByClient", "HTSController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/hts-by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Foreign key of the table HTS.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadHTSByClient)]
        public async Task<IActionResult> ReadHTSByClient(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var htsInDb = await context.HTSRepository.GetHTSByClient(clientId);

                if (htsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(htsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadHTSByClient", "HTSController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HTS.</param>
        /// <param name="hts">HTS to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateHTS)]
        public async Task<IActionResult> UpdateHTS(Guid key, HTS hts)
        {
            try
            {
                if (key != hts.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = hts.ModifiedBy;
                interactionInDb.ModifiedIn = hts.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                var htsInDb = await context.HTSRepository.GetHTSByKey(key);

                if (htsInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                htsInDb.PartnerHIVStatus = hts.PartnerHIVStatus;
                htsInDb.PartnerLastTestDate = hts.PartnerLastTestDate;
                htsInDb.RetestDate = hts.RiskAssessmentList == null ? null : hts.RetestDate;
                htsInDb.BiolineTestResult = hts.BiolineTestResult;
                htsInDb.ClientSource = hts.ClientSource;
                htsInDb.ClientType = hts.ClientType;
                htsInDb.ClientTypeId = hts.ClientTypeId;
                htsInDb.ConsentForSMS = hts.ConsentForSMS;
                htsInDb.DetermineTestResult = hts.DetermineTestResult;
                htsInDb.EncounterType = hts.EncounterType;
                htsInDb.HIVType = hts.HIVType;
                htsInDb.HIVNotTestingReasonId = hts.HIVNotTestingReasonId;
                htsInDb.HIVTestingReasonId = hts.HIVTestingReasonId;
                htsInDb.HasCounselled = hts.HasCounselled;
                htsInDb.HasConsented = hts.HasConsented;
                htsInDb.IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected;
                htsInDb.IsResultReceived = hts.IsResultReceived;
                htsInDb.LastTested = hts.LastTested;
                htsInDb.NotTestingReason = hts.NotTestingReason;
                htsInDb.ServicePointId = hts.ServicePointId;
                htsInDb.TestNo = hts.TestNo;
                htsInDb.LastTestResult = hts.LastTestResult;
                htsInDb.VisitTypeId = hts.VisitTypeId;
                htsInDb.SampleCollectionDate = hts.SampleCollectionDate;
                htsInDb.DateModified = DateTime.Now;
                htsInDb.IsDeleted = false;
                htsInDb.IsSynced = false;

                context.HTSRepository.Update(htsInDb);
                await context.SaveChangesAsync();

                // Client update By HTS test result
                Client client = context.ClientRepository.GetById(hts.ClientId);

                if (htsInDb.DetermineTestResult == Enums.TestResult.Reactive && htsInDb.BiolineTestResult == Enums.TestResult.Reactive)
                {
                    client.HIVStatus = Enums.HIVStatus.Positive;

                    context.ClientRepository.Update(client);
                }
                else if (htsInDb.DetermineTestResult == Enums.TestResult.Reactive && htsInDb.BiolineTestResult == Enums.TestResult.NonReactive)
                {
                    client.HIVStatus = Enums.HIVStatus.Indeterminant;

                    context.ClientRepository.Update(client);
                }
                else
                {
                    client.HIVStatus = Enums.HIVStatus.Negative;

                    context.ClientRepository.Update(client);
                }

                await context.SaveChangesAsync();

                // end

                var riskAssessmentByHTS = context.RiskAssessmentRepository.GetRiskAssessmentByHTS(hts.InteractionId);

                foreach (var item in riskAssessmentByHTS.Result.ToList())
                {
                    context.RiskAssessmentRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (hts.RiskAssessmentList != null)
                {
                    foreach (var item in hts.RiskAssessmentList)
                    {
                        RiskAssessment riskAssessment = new RiskAssessment();
                        riskAssessment.Oid = Guid.NewGuid();
                        riskAssessment.RiskFactorId = item;
                        riskAssessment.HTSId = hts.InteractionId;
                        riskAssessment.IsDeleted = false;
                        riskAssessment.IsSynced = false;

                        context.RiskAssessmentRepository.Add(riskAssessment);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadHTSByKey", new { key = hts.InteractionId }, hts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateHTS", "HTSController.cs", ex.Message, hts.ModifiedIn, hts.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/hts/{key}
        /// </summary>
        /// <param name="key">Primary key of the table HTS.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteHTS)]
        public async Task<IActionResult> DeleteHTS(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var htsInDb = await context.HTSRepository.GetHTSByKey(key);

                if (htsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                htsInDb.DateModified = DateTime.Now;
                htsInDb.IsDeleted = true;
                htsInDb.IsSynced = false;

                context.HTSRepository.Update(htsInDb);
                await context.SaveChangesAsync();

                return Ok(htsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteHTS", "HTSController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}