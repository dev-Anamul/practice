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

    public class TestSubTypeController : ControllerBase
    {

        private readonly IUnitOfWork context;
        private readonly ILogger<TestSubTypeController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TestSubTypeController(IUnitOfWork context, ILogger<TestSubTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/test-subtype
        /// </summary>
        /// <param name="testSubType">testSubType object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTestSubtype)]
        public async Task<IActionResult> CreateTestSubtype(TestSubtype testSubType)
        {
            try
            {
                testSubType.DateCreated = DateTime.Now;
                testSubType.IsDeleted = false;
                testSubType.IsSynced = false;

                context.TestSubtypeRepository.Add(testSubType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTestSubtypeByKey", new { key = testSubType.Oid }, testSubType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTestSubtype", "TestSubTypeController.cs", ex.Message, testSubType.CreatedIn, testSubType.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/test-subtypes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestSubtypes)]
        public async Task<IActionResult> ReadTestSubtypes()
        {
            try
            {
                var testSubTypeIndb = await context.TestSubtypeRepository.GetTestSubtypes();

                return Ok(testSubTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestSubtypes", "TestSubTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/test-subtype/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestSubtypeByKey)]
        public async Task<IActionResult> ReadTestSubtypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testSubTypeIndb = await context.TestSubtypeRepository.GetTestSubtypeByKey(key);

                if (testSubTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(testSubTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestSubtypeByKey", "TestSubTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/test-subtype/by-test-type/{testTypeId}
        /// </summary>
        /// <param name="testTypeId">Foreign key of the table TestType.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.SubTypesByTestType)]
        public async Task<IActionResult> GetAllSubTestByTestType(int testTypeId)
        {
            try
            {
                var userInDb = await context.TestSubtypeRepository.GetSubTestByTestType(testTypeId);

                return Ok(userInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetAllSubTestByTestType", "TestSubTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/test-subtype/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TeatSubType.</param>
        /// <param name="testSubType">testSubType to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTestSubtype)]
        public async Task<IActionResult> UpdateTestSubtype(int key, TestSubtype testSubType)
        {
            try
            {
                if (key != testSubType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                testSubType.DateModified = DateTime.Now;
                testSubType.IsDeleted = false;
                testSubType.IsSynced = false;

                context.TestSubtypeRepository.Update(testSubType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTestSubtype", "TestSubTypeController.cs", ex.Message, testSubType.ModifiedIn, testSubType.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/tB-Finding/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TBFindings.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTestSubtype)]
        public async Task<IActionResult> DeleteTestSubtype(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testSubTypeIndb = await context.TestSubtypeRepository.GetTestSubtypeByKey(key);

                if (testSubTypeIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                testSubTypeIndb.DateModified = DateTime.Now;
                testSubTypeIndb.IsDeleted = true;
                testSubTypeIndb.IsSynced = false;

                context.TestSubtypeRepository.Update(testSubTypeIndb);
                await context.SaveChangesAsync();

                return Ok(testSubTypeIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTestSubtype", "TestSubTypeController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


       

    }
}