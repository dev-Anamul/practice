using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 22.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Province controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TestTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TestTypeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        /// 
        public TestTypeController(IUnitOfWork context, ILogger<TestTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/test-type
        /// </summary>
        /// <param name="testType">testType object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTestType)]
        public async Task<IActionResult> CreateTestType(TestType testType)
        {
            try
            {
                //if (await IsProvinceDuplicate(province) == true)
                //    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NoMatchFoundError);

                testType.DateCreated = DateTime.Now;
                testType.IsDeleted = false;
                testType.IsSynced = false;

                context.TestTypeRepository.Add(testType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTestTypeByKey", new { key = testType.Oid }, testType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTestType", "TestTypeController.cs", ex.Message, testType.CreatedIn, testType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/test-types
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestTypes)]
        public async Task<IActionResult> ReadTestTypes()
        {
            try
            {
                var testTypeIndb = await context.TestTypeRepository.GetTestTypes();

                return Ok(testTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestTypes", "TestTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/test-type/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestTypeByKey)]
        public async Task<IActionResult> ReadTestTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testTypeIndb = await context.TestTypeRepository.GetTestTypeByKey(key);

                if (testTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(testTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestTypeByKey", "TestTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/test-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TestType.</param>
        /// <param name="TestType">TestType to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTestType)]
        public async Task<IActionResult> UpdateTestType(int key, TestType testType)
        {
            try
            {
                if (key != testType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                testType.DateModified = DateTime.Now;
                testType.IsDeleted = false;
                testType.IsSynced = false;

                context.TestTypeRepository.Update(testType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTestType", "TestTypeController.cs", ex.Message, testType.ModifiedIn, testType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/test-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TestType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTestType)]
        public async Task<IActionResult> DeleteTestType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testTypeIndb = await context.TestTypeRepository.GetTestTypeByKey(key);

                if (testTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                testTypeIndb.DateModified = DateTime.Now;
                testTypeIndb.IsDeleted = true;
                testTypeIndb.IsSynced = false;

                context.TestTypeRepository.Update(testTypeIndb);
                await context.SaveChangesAsync();

                return Ok(testTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTestType", "TestTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


    }
}
