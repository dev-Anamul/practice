using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 01.01.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Assessment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class AssessmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<AssessmentController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public AssessmentController(IUnitOfWork context, ILogger<AssessmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region Assessment
        /// <summary>
        /// URL: sc-api/assessment
        /// </summary>
        /// <param name="assessment">Assessment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateAssessment)]
        public async Task<IActionResult> CreateAssessment(Assessment assessment)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Assessment, assessment.EncounterType);
                interaction.EncounterId = assessment.EncounterId;
                interaction.DateCreated = assessment.DateCreated;
                interaction.CreatedIn = assessment.CreatedIn;
                interaction.CreatedBy = assessment.CreatedBy;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                assessment.InteractionId = interactionId;
                assessment.DateCreated = DateTime.Now;
                assessment.IsDeleted = false;
                assessment.IsSynced = false;

                context.AssessmentRepository.Add(assessment);
                await context.SaveChangesAsync();

                if (assessment.EyesConditionList != null)
                {
                    foreach (var item in assessment.EyesConditionList)
                    {
                        IdentifiedEyesAssessment identifiedEyesAssessment = new IdentifiedEyesAssessment();

                        identifiedEyesAssessment.InteractionId = Guid.NewGuid();
                        identifiedEyesAssessment.EyesCondition = item;
                        identifiedEyesAssessment.AssessmentId = assessment.InteractionId;
                        identifiedEyesAssessment.CreatedBy = assessment.CreatedBy;
                        identifiedEyesAssessment.CreatedIn = assessment.CreatedIn;
                        identifiedEyesAssessment.DateCreated = DateTime.Now;
                        identifiedEyesAssessment.IsDeleted = false;
                        identifiedEyesAssessment.IsSynced = false;

                        context.IdentifiedEyesAssessmentRepository.Add(identifiedEyesAssessment);
                        await context.SaveChangesAsync();
                    }
                }

                if (assessment.CordStumpConditionList != null)
                {
                    foreach (var item in assessment.CordStumpConditionList)
                    {
                        IdentifiedCordStump identifiedCordStump = new IdentifiedCordStump();

                        identifiedCordStump.InteractionId = Guid.NewGuid();
                        identifiedCordStump.CordStumpCondition = item;
                        identifiedCordStump.AssessmentId = assessment.InteractionId;
                        identifiedCordStump.CreatedBy = assessment.CreatedBy;
                        identifiedCordStump.CreatedIn = assessment.CreatedIn;
                        identifiedCordStump.DateCreated = DateTime.Now;
                        identifiedCordStump.IsDeleted = false;
                        identifiedCordStump.IsSynced = false;

                        context.IdentifiedCordStumpRepository.Add(identifiedCordStump);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadAssessmentByKey", new { key = assessment.InteractionId }, assessment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateAssessment", "AssessmentController.cs", ex.Message, assessment.CreatedIn, assessment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/assessments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAssessments)]
        public async Task<IActionResult> ReadAssessments()
        {
            try
            {
                var assessmentInDb = await context.AssessmentRepository.GetAssessments();

                assessmentInDb = assessmentInDb.OrderByDescending(x => x.DateCreated);

                return Ok(assessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAssessments", "AssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/assessment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Assessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAssessmentByKey)]
        public async Task<IActionResult> ReadAssessmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var assessmentInDb = await context.AssessmentRepository.GetAssessmentByKey(key);

                if (assessmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(assessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAssessmentByKey", "AssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/assessment/ByClient/{clientId}/{pageNo}/{pageSize}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadAssessmentByClient)]
        public async Task<IActionResult> ReadAssessmentByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    //var assessmentInDb = await context.AssessmentRepository.GetAssessmentsByClient(clientId);
                    var assessmentInDb = await context.AssessmentRepository.GetAssessmentsByClientLast24Hours(clientId);

                    return Ok(assessmentInDb);
                }
                else
                {
                    var assessmentInDb = await context.AssessmentRepository.GetAssessmentsByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<Assessment> assessmentWithPaggingDto = new PagedResultDto<Assessment>()
                    {
                        Data = assessmentInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.AssessmentRepository.GetAssessmentsByClientTotalCount(clientId, encounterType),
                    };
                    return Ok(assessmentWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAssessmentByClient", "AssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Assessments.</param>
        /// <param name="assessment">Assessment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateAssessment)]
        public async Task<IActionResult> UpdateAssessment(Guid key, Assessment assessment)
        {
            try
            {
                if (key != assessment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = assessment.ModifiedBy;
                interactionInDb.ModifiedIn = assessment.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                assessment.DateModified = DateTime.Now;
                assessment.IsDeleted = false;
                assessment.IsSynced = false;

                context.AssessmentRepository.Update(assessment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateAssessment", "AssessmentController.cs", ex.Message, assessment.CreatedIn, assessment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/assessment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Assessments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteAssessment)]
        public async Task<IActionResult> DeleteAssessment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var assessmentInDb = await context.AssessmentRepository.GetAssessmentByKey(key);

                if (assessmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                assessmentInDb.DateModified = DateTime.Now;
                assessmentInDb.IsDeleted = true;
                assessmentInDb.IsSynced = false;

                context.AssessmentRepository.Update(assessmentInDb);
                await context.SaveChangesAsync();

                return Ok(assessmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteAssessment", "AssessmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}