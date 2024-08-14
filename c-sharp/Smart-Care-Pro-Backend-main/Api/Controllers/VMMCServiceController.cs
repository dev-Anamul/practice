using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 11.04.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// VMMC Service Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VMMCServiceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VMMCServiceController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VMMCServiceController(IUnitOfWork context, ILogger<VMMCServiceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/vmmc-service
        /// </summary>
        /// <param name="vMMCService">VMMCService object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVMMCService)]
        public async Task<IActionResult> CreateVMMCService(VMMCService vMMCService)
        {
            try
            {
                if (vMMCService.Oid != Guid.Empty)
                {
                    var vMMCServiceInDb = await context.VMMCServiceRepository.GetVMMCServiceByKey(vMMCService.Oid);

                    if (vMMCServiceInDb != null)
                    {
                        vMMCServiceInDb.MCNumber = vMMCService.MCNumber;
                        vMMCServiceInDb.IsConsentGiven = vMMCService.IsConsentGiven;
                        vMMCServiceInDb.IVAccess = vMMCService.IVAccess;
                        vMMCServiceInDb.PresentedHIVStatus = vMMCService.PresentedHIVStatus;
                        vMMCServiceInDb.HIVStatusEvidencePresented = vMMCService.HIVStatusEvidencePresented;
                        vMMCServiceInDb.IsPreTestCounsellingOffered = vMMCService.IsPreTestCounsellingOffered;
                        vMMCServiceInDb.IsHIVTestingServiceOffered = vMMCService.IsHIVTestingServiceOffered;
                        vMMCServiceInDb.IsPostTestCounsellingOffered = vMMCService.IsPostTestCounsellingOffered;
                        vMMCServiceInDb.DateModified = DateTime.Now;
                        vMMCServiceInDb.IsDeleted = false;
                        vMMCServiceInDb.IsSynced = false;
                        vMMCServiceInDb.ModifiedIn = vMMCService.ModifiedIn;
                        vMMCServiceInDb.ModifiedBy = vMMCService.ModifiedBy;

                        context.VMMCServiceRepository.Update(vMMCServiceInDb);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        vMMCService.DateCreated = DateTime.Now;
                        vMMCService.IsDeleted = false;
                        vMMCService.IsSynced = false;

                        context.VMMCServiceRepository.Add(vMMCService);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    vMMCService.DateCreated = DateTime.Now;
                    vMMCService.IsDeleted = false;
                    vMMCService.IsSynced = false;

                    context.VMMCServiceRepository.Add(vMMCService);
                    await context.SaveChangesAsync();
                }

                if (vMMCService.CircumcisionReasonList != null)
                {
                    foreach (var item in vMMCService.CircumcisionReasonList)
                    {
                        OptedCircumcisionReason circumcisionReason = new OptedCircumcisionReason();

                        circumcisionReason.InteractionId = Guid.NewGuid();
                        circumcisionReason.CircumcisionReasonId = item;
                        circumcisionReason.VMMCServiceId = vMMCService.Oid;
                        circumcisionReason.IsSynced = false;
                        circumcisionReason.IsDeleted = false;

                        context.OptedCircumcisionReasonRepository.Add(circumcisionReason);
                        await context.SaveChangesAsync();
                    }
                }

                if (vMMCService.VMMCCampaignList != null)
                {
                    foreach (var item in vMMCService.VMMCCampaignList)
                    {
                        OptedVMMCCampaign vmmcCampaign = new OptedVMMCCampaign();

                        vmmcCampaign.InteractionId = Guid.NewGuid();
                        vmmcCampaign.VMMCCampaignId = item;
                        vmmcCampaign.VMMCServiceId = vMMCService.Oid;
                        vmmcCampaign.IsDeleted = false;
                        vmmcCampaign.IsSynced = false;

                        context.OptedVMMCCampaignRepository.Add(vmmcCampaign);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadVMMCServiceByKey", new { key = vMMCService.Oid }, vMMCService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVMMCService", "VMMCServiceController.cs", ex.Message, vMMCService.CreatedIn, vMMCService.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-services
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVMMCServices)]
        public async Task<IActionResult> ReadVMMCServices()
        {
            try
            {
                var vmmcIndb = await context.VMMCServiceRepository.GetVMMCServices();
                vmmcIndb = vmmcIndb.OrderByDescending(x => x.DateCreated);
                return Ok(vmmcIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVMMCServices", "VMMCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-service/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCService.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVMMCServiceByKey)]
        public async Task<IActionResult> ReadVMMCServiceByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vMMCServiceInDb = await context.VMMCServiceRepository.GetVMMCServiceByKey(key);

                if (vMMCServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(vMMCServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVMMCServiceByKey", "VMMCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-service/key/{clientId}
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadVMMCServiceByClient)]
        public async Task<IActionResult> ReadVMMCServiceByClient(Guid clientId)
        {
            try
            {
                var vMMCServiceInDb = await context.VMMCServiceRepository.GetVMMCServiceByClient(clientId);

                return Ok(vMMCServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVMMCServiceByClient", "VMMCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCService.</param>
        /// <param name="vMMCService">VMMCService to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVMMCService)]
        public async Task<IActionResult> UpdateVMMCService(Guid key, VMMCService vMMCService)
        {
            try
            {
                if (key != vMMCService.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var vMMCServiceInDb = await context.VMMCServiceRepository.FirstOrDefaultAsync(x => x.Oid == key);

                if (vMMCServiceInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                vMMCServiceInDb.DateModified = DateTime.Now;
                vMMCServiceInDb.IsDeleted = false;
                vMMCServiceInDb.IsSynced = false;
                vMMCServiceInDb.ModifiedIn = vMMCService.ModifiedIn;
                vMMCServiceInDb.ModifiedBy = vMMCService.ModifiedBy;
                vMMCServiceInDb.IsConsentGiven = vMMCService.IsConsentGiven;
                vMMCServiceInDb.ASAClassification = vMMCService.ASAClassification;
                vMMCServiceInDb.AtlantoOccipitalFlexion = vMMCService.AtlantoOccipitalFlexion;
                vMMCServiceInDb.Complications = vMMCService.Complications;
                vMMCServiceInDb.BonyLandmarks = vMMCService.BonyLandmarks;
                vMMCServiceInDb.PresentedHIVStatus = vMMCService.PresentedHIVStatus;
                vMMCServiceInDb.ThyromentalDistance = vMMCService.ThyromentalDistance;
                vMMCServiceInDb.TongueSize = vMMCService.TongueSize;
                vMMCServiceInDb.MCNumber = vMMCService.MCNumber;
                vMMCServiceInDb.HasDentures = vMMCService.HasDentures;
                vMMCServiceInDb.MovementOfHeadNeck = vMMCService.MovementOfHeadNeck;
                vMMCServiceInDb.HasAbnormalitiesOfTheNeck = vMMCService.HasAbnormalitiesOfTheNeck;
                vMMCServiceInDb.HasLooseTeeth = vMMCService.HasLooseTeeth;
                vMMCServiceInDb.HIVStatusEvidencePresented = vMMCService.HIVStatusEvidencePresented;
                vMMCServiceInDb.InterincisorGap = vMMCService.InterincisorGap;
                vMMCServiceInDb.IsHIVTestingServiceOffered = vMMCService.IsHIVTestingServiceOffered;
                vMMCServiceInDb.IsPostTestCounsellingOffered = vMMCService.IsPostTestCounsellingOffered;
                vMMCServiceInDb.IsPreTestCounsellingOffered = vMMCService.IsPreTestCounsellingOffered;
                vMMCServiceInDb.IsReferredToARTIfPositive = vMMCService.IsReferredToARTIfPositive;
                vMMCServiceInDb.IVAccess = vMMCService.IVAccess;

                context.VMMCServiceRepository.Update(vMMCServiceInDb);
                await context.SaveChangesAsync();

                var circumcisionReason = context.OptedCircumcisionReasonRepository.GetCircumcisionReasonByVMMCService(vMMCService.Oid);

                foreach (var item in circumcisionReason.Result.ToList())
                {
                    context.OptedCircumcisionReasonRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                var vmmcCampaign = context.OptedVMMCCampaignRepository.GetVMMCCampaignByVMMCService(vMMCService.Oid);

                foreach (var item in vmmcCampaign.Result.ToList())
                {
                    context.OptedVMMCCampaignRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (vMMCService.CircumcisionReasonList != null)
                {
                    foreach (var item in vMMCService.CircumcisionReasonList)
                    {
                        OptedCircumcisionReason optedCircumcisionReason = new OptedCircumcisionReason();
                        optedCircumcisionReason.InteractionId = Guid.NewGuid();
                        optedCircumcisionReason.CircumcisionReasonId = item;
                        optedCircumcisionReason.VMMCServiceId = vMMCService.Oid;
                        optedCircumcisionReason.IsSynced = false;
                        optedCircumcisionReason.IsDeleted = false;

                        context.OptedCircumcisionReasonRepository.Add(optedCircumcisionReason);
                        await context.SaveChangesAsync();
                    }
                }

                if (vMMCService.VMMCCampaignList != null)
                {
                    foreach (var item in vMMCService.VMMCCampaignList)
                    {
                        OptedVMMCCampaign vMMCCampaign = new OptedVMMCCampaign();

                        vMMCCampaign.InteractionId = Guid.NewGuid();
                        vMMCCampaign.VMMCCampaignId = item;
                        vMMCCampaign.VMMCServiceId = vMMCService.Oid;
                        vMMCCampaign.IsDeleted = false;
                        vMMCCampaign.IsSynced = false;

                        context.OptedVMMCCampaignRepository.Add(vMMCCampaign);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadVMMCServiceByKey", new { key = vMMCService.Oid }, vMMCService);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVMMCService", "VMMCServiceController.cs", ex.Message, vMMCService.ModifiedIn, vMMCService.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vmmc-service/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VMMCService.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteVMMCService)]
        public async Task<IActionResult> DeleteVMMCService(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vMMCServiceInDb = await context.VMMCServiceRepository.GetVMMCServiceByKey(key);

                if (vMMCServiceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.VMMCServiceRepository.Update(vMMCServiceInDb);
                await context.SaveChangesAsync();

                return Ok(vMMCServiceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVMMCService", "VMMCServiceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.CreateAirwayAssessment)]
        public async Task<IActionResult> CreateAirwayAssessment(AirwayAssessmentDto airwayAssessmentDto)
        {
            try
            {
                if (airwayAssessmentDto.Oid != Guid.Empty)
                {
                    var vMMCService = await context.VMMCServiceRepository.GetVMMCServiceByKey(airwayAssessmentDto.Oid);

                    vMMCService.HasDentures = airwayAssessmentDto.HasDentures;
                    vMMCService.HasLooseTeeth = airwayAssessmentDto.HasLooseTeeth;
                    vMMCService.HasAbnormalitiesOfTheNeck = airwayAssessmentDto.HasAbnormalitiesOfTheNeck;
                    vMMCService.TongueSize = airwayAssessmentDto.TongueSize;
                    vMMCService.MandibleSize = airwayAssessmentDto.MandibleSize;
                    vMMCService.DateModified = DateTime.Now;
                    vMMCService.IsDeleted = false;
                    vMMCService.IsSynced = false;

                    context.VMMCServiceRepository.Update(vMMCService);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var vMMCService = new VMMCService
                    {
                        Oid = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false,
                        HasDentures = airwayAssessmentDto.HasDentures,
                        HasLooseTeeth = airwayAssessmentDto.HasLooseTeeth,
                        HasAbnormalitiesOfTheNeck = airwayAssessmentDto.HasAbnormalitiesOfTheNeck,
                        TongueSize = airwayAssessmentDto.TongueSize,
                        MandibleSize = airwayAssessmentDto.MandibleSize
                    };

                    context.VMMCServiceRepository.Add(vMMCService);
                    await context.SaveChangesAsync();

                    airwayAssessmentDto.Oid = vMMCService.Oid;
                }

                return CreatedAtAction("ReadVMMCServiceByKey", new { key = airwayAssessmentDto.Oid }, airwayAssessmentDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAirwayAssessment", "VMMCServiceController.cs", ex.Message, airwayAssessmentDto.CreatedIn, airwayAssessmentDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.CreatePredictiveTest)]
        public async Task<IActionResult> CreatePredictiveTest(PredictiveTestDto predictiveTestDto)
        {
            try
            {
                if (predictiveTestDto.Oid != Guid.Empty)
                {
                    var vMMCService = await context.VMMCServiceRepository.GetVMMCServiceByKey(predictiveTestDto.Oid);

                    vMMCService.InterincisorGap = predictiveTestDto.InterIncisorGap;
                    vMMCService.MovementOfHeadNeck = predictiveTestDto.MovementOfHeadNek;
                    vMMCService.ThyromentalDistance = predictiveTestDto.ThyromentalDistance;
                    vMMCService.AtlantoOccipitalFlexion = predictiveTestDto.AtlantoOcciptalFlexion;
                    vMMCService.MallampatiClass = predictiveTestDto.MallampatiClass;
                    vMMCService.DateModified = DateTime.Now;
                    vMMCService.IsDeleted = false;
                    vMMCService.IsSynced = false;

                    context.VMMCServiceRepository.Update(vMMCService);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var vMMCService = new VMMCService
                    {
                        Oid = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false,
                        InterincisorGap = predictiveTestDto.InterIncisorGap,
                        MovementOfHeadNeck = predictiveTestDto.MovementOfHeadNek,
                        ThyromentalDistance = predictiveTestDto.ThyromentalDistance,
                        AtlantoOccipitalFlexion = predictiveTestDto.AtlantoOcciptalFlexion,
                        MallampatiClass = predictiveTestDto.MallampatiClass,
                        CreatedBy = predictiveTestDto.CreatedBy,
                        CreatedIn = predictiveTestDto.CreatedIn
                    };

                    context.VMMCServiceRepository.Add(vMMCService);
                    await context.SaveChangesAsync();

                    predictiveTestDto.Oid = vMMCService.Oid;
                }

                return CreatedAtAction("ReadVMMCServiceByKey", new { key = predictiveTestDto.Oid }, predictiveTestDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePredictiveTest", "VMMCServiceController.cs", ex.Message, predictiveTestDto.CreatedIn, predictiveTestDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.CreateAssessmentPlan)]
        public async Task<IActionResult> CreateAssessmentPlan(AssessmentPlanDto assessmentPlanDto)
        {
            try
            {
                if (assessmentPlanDto.Oid != Guid.Empty)
                {
                    var vMMCService = await context.VMMCServiceRepository.GetVMMCServiceByKey(assessmentPlanDto.Oid);

                    vMMCService.ASAClassification = assessmentPlanDto.ASAClassification;
                    vMMCService.IVAccess = assessmentPlanDto.IVAccess;
                    vMMCService.BonyLandmarks = assessmentPlanDto.BonyLandmarks;
                    vMMCService.DateModified = DateTime.Now;
                    vMMCService.IsDeleted = false;
                    vMMCService.IsSynced = false;

                    context.VMMCServiceRepository.Update(vMMCService);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var vMMCService = new VMMCService
                    {
                        Oid = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false,
                        ASAClassification = assessmentPlanDto.ASAClassification,
                        IVAccess = assessmentPlanDto.IVAccess,
                        BonyLandmarks = assessmentPlanDto.BonyLandmarks,
                    };

                    context.VMMCServiceRepository.Add(vMMCService);
                    await context.SaveChangesAsync();

                    assessmentPlanDto.Oid = vMMCService.Oid;
                }

                return CreatedAtAction("ReadVMMCServiceByKey", new { key = assessmentPlanDto.Oid }, assessmentPlanDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAssessmentPlan", "VMMCServiceController.cs", ex.Message, assessmentPlanDto.CreatedIn, assessmentPlanDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}