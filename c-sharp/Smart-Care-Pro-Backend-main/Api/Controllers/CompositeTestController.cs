using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : Brian
 * Last modified: 27.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// TestGroup controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CompositeTestController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CompositeTestController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CompositeTestController(IUnitOfWork context, ILogger<CompositeTestController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/composite-test
        /// </summary>
        /// <param name="compositeTest">GroupItem object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCompositeTest)]
        public async Task<ActionResult<TestItem>> CreateCompositeTest(CompositeTest compositeTest)
        {
            try
            {
                if (await IsCompositeTestDuplicate(compositeTest) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                compositeTest.DateCreated = DateTime.Now;
                compositeTest.IsDeleted = false;
                compositeTest.IsSynced = false;

                context.CompositeTestRepository.Add(compositeTest);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCompositeTestByKey", new { key = compositeTest.Oid }, compositeTest);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCompositeTest", "CompositeTestController.cs", ex.Message, compositeTest.CreatedIn, compositeTest.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/composite-test
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCompositeTests)]
        public async Task<IActionResult> ReadCompositeTests()
        {
            try
            {
                var compositeTestInDb = await context.CompositeTestRepository.GetCompositeTests();

                compositeTestInDb = compositeTestInDb.OrderByDescending(x => x.DateCreated);

                return Ok(compositeTestInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCompositeTests", "CompositeTestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/composite-test/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TestGroup.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCompositeTestByKey)]
        public async Task<IActionResult> ReadCompositeTestByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var compositeTestInDb = await context.CompositeTestRepository.GetCompositeTestByKey(key);

                if (compositeTestInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(compositeTestInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCompositeTestByKey", "CompositeTestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/composite-test/{key}
        /// </summary>
        /// <param name="key">Primary key of the table testGroup.</param>
        /// <param name="testGroup">TestGroup to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCompositeTest)]
        public async Task<IActionResult> UpdateCompositeTest(int key, CompositeTest compositeTest)
        {
            try
            {
                if (key != compositeTest.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                compositeTest.DateModified = DateTime.Now;
                compositeTest.IsDeleted = false;
                compositeTest.IsSynced = false;

                context.CompositeTestRepository.Update(compositeTest);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCompositeTest", "CompositeTestController.cs", ex.Message, compositeTest.ModifiedIn, compositeTest.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/test-group/{key}
        /// </summary>
        /// <param name="key">Primary key of the table TestGroups.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCompositeTest)]
        public async Task<IActionResult> DeleteTestGroup(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var compositeTestInDb = await context.CompositeTestRepository.GetCompositeTestByKey(key);

                if (compositeTestInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                compositeTestInDb.DateModified = DateTime.Now;
                compositeTestInDb.IsDeleted = true;
                compositeTestInDb.IsSynced = false;

                context.CompositeTestRepository.Update(compositeTestInDb);
                await context.SaveChangesAsync();

                return Ok(compositeTestInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTestGroup", "CompositeTestController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the CompositeTest name is duplicate or not.
        /// </summary>
        /// <param name="CompositeTest">CompositeTest object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCompositeTestDuplicate(CompositeTest compositeTest)
        {
            try
            {
                var compositeTestInDb = await context.CompositeTestRepository.GetCompositeTestByName(compositeTest.Description);

                if (compositeTestInDb != null)
                    if (compositeTestInDb.Oid != compositeTest.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCompositeTestDuplicate", "CompositeTestController.cs", ex.Message);
                throw;
            }
        }
    }
}