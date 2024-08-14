using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class QuestionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<QuestionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public QuestionController(IUnitOfWork context, ILogger<QuestionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/question
        /// </summary>
        /// <param name="question">Question object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateQuestion)]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            try
            {
                if (await IsQuestionDuplicate(question) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                question.DateCreated = DateTime.Now;
                question.IsDeleted = false;
                question.IsSynced = false;

                context.QuestionRepository.Add(question);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadQuestionByKey", new { key = question.Oid }, question);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateQuestion", "QuestionController.cs", ex.Message, question.CreatedIn, question.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/questions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuestions)]
        public async Task<IActionResult> ReadQuestions()
        {
            try
            {
                var questionIndb = await context.QuestionRepository.GetQuestions();

                return Ok(questionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuestions", "QuestionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadQuestionsForForm)]
        public async Task<IActionResult> ReadQuestionsForForm()
        {
            try
            {
                List<QuestionsDto> questionsDto = new List<QuestionsDto>();

                var questionIndb = await context.QuestionRepository.GetQuestions();

                if (questionIndb != null)
                {
                    questionsDto = questionIndb.Select(x => new QuestionsDto
                    {
                        QuestionId = x.Oid,
                        Question = x.Description,
                        Answer = false

                    }).ToList();
                }

                return Ok(questionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuestionsForForm", "QuestionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/question/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Questions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadQuestionByKey)]
        public async Task<IActionResult> ReadQuestionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var questionIndb = await context.QuestionRepository.GetQuestionByKey(key);

                if (questionIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(questionIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadQuestionByKey", "QuestionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/question/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Questions.</param>
        /// <param name="question">Question to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateQuestion)]
        public async Task<IActionResult> UpdateQuestion(int key, Question question)
        {
            try
            {
                if (key != question.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                question.DateModified = DateTime.Now;
                question.IsDeleted = false;
                question.IsSynced = false;

                context.QuestionRepository.Update(question);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateQuestion", "QuestionController.cs", ex.Message, question.ModifiedIn, question.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/question/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Questions.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteQuestion)]
        public async Task<IActionResult> DeleteQuestion(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var questionInDb = await context.QuestionRepository.GetQuestionByKey(key);

                if (questionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                questionInDb.DateModified = DateTime.Now;
                questionInDb.IsDeleted = true;
                questionInDb.IsSynced = false;

                context.QuestionRepository.Update(questionInDb);
                await context.SaveChangesAsync();

                return Ok(questionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteQuestion", "QuestionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the Question name is duplicate or not.
        /// </summary>
        /// <param name="question">Question object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsQuestionDuplicate(Question question)
        {
            try
            {
                var questionInDb = await context.QuestionRepository.GetQuestionByName(question.Description);

                if (questionInDb != null)
                    if (questionInDb.Oid != question.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsQuestionDuplicate", "QuestionController.cs", ex.Message);
                throw;
            }
        }
    }
}