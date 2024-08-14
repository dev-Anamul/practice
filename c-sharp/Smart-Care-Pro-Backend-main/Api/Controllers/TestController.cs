using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    /// <summary>
    /// Province controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TestController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TestController(IUnitOfWork context, ILogger<TestController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/province
        /// </summary>
        /// <param name="province">Province object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTest)]
        public async Task<IActionResult> CreateTest(Test test)
        {
            try
            {
                test.DateCreated = DateTime.Now;
                test.IsDeleted = false;
                test.IsSynced = false;

                context.TestRepository.Add(test);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTestByKey", new { key = test.Oid }, test);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTest", "TestController.cs", ex.Message, test.CreatedIn, test.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/provinces
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTests)]
        public async Task<IActionResult> ReadTests()
        {
            try
            {
                var testIndb = await context.TestRepository.GetTests();

                return Ok(testIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTests", "TestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestByKey)]
        public async Task<IActionResult> ReadTestByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testIndb = await context.TestRepository.GetTestByKey(key);

                if (testIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(testIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestByKey", "TestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/test/testbysubtest/{testsubid}
        /// </summary>
        /// <param name="testsubid">Foreign key of the table SubTest.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.TestBySubTestType)]
        public async Task<IActionResult> GetAllTestBySubTest(int testsubid)
        {
            try
            {
                var userInDb = await context.TestRepository.GetTestBySubTest(testsubid);

                return Ok(userInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetAllTestBySubTest", "TestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/test/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Test.</param>
        /// <param name="test">Test to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTest)]
        public async Task<IActionResult> UpdateTest(int key, Test test)
        {
            try
            {
                if (key != test.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                test.DateModified = DateTime.Now;
                test.IsDeleted = false;
                test.IsSynced = false;

                context.TestRepository.Update(test);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTest", "TestController.cs", ex.Message, test.ModifiedIn, test.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/test/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Test.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTest)]
        public async Task<IActionResult> DeleteTest(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testIndb = await context.TestRepository.GetTestByKey(key);

                if (testIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                testIndb.DateModified = DateTime.Now;
                testIndb.IsDeleted = true;
                testIndb.IsSynced = false;

                context.TestRepository.Update(testIndb);
                await context.SaveChangesAsync();

                return Ok(testIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTest", "TestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}