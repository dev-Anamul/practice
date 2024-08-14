using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// GroupItem controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class TestItemController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<TestItemController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public TestItemController(IUnitOfWork context, ILogger<TestItemController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/group-item
        /// </summary>
        /// <param name="groupItem">GroupItem object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTestItem)]
        public async Task<ActionResult<TestItem>> CreateTestItem(TestItem testItem)
        {
            try
            {
                testItem.DateCreated = DateTime.Now;
                testItem.IsDeleted = false;
                testItem.IsSynced = false;

                context.TestItemRepository.Add(testItem);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTestItemByKey", new { key = testItem.Oid }, testItem);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateTestItem", "TestItemController.cs", ex.Message, testItem.CreatedIn, testItem.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/group-item
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestItems)]
        public async Task<IActionResult> ReadTestItems()
        {
            try
            {
                var testItemInDb = await context.TestItemRepository.GetTestItems();

                return Ok(testItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestItems", "TestItemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/group-item/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GroupItem.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTestItemByKey)]
        public async Task<IActionResult> ReadTestItemByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testItemInDb = await context.TestItemRepository.GetTestItemByKey(key);

                if (testItemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(testItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadTestItemByKey", "TestItemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/group-item/{key}
        /// </summary>
        /// <param name="key">Primary key of the table groupItem.</param>
        /// <param name="groupItem">GroupItem to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTestItem)]
        public async Task<IActionResult> UpdateTestItem(int key, TestItem testItem)
        {
            try
            {
                if (key != testItem.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                testItem.DateModified = DateTime.Now;
                testItem.IsDeleted = false;
                testItem.IsSynced = false;

                context.TestItemRepository.Update(testItem);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateTestItem", "TestItemController.cs", ex.Message, testItem.ModifiedIn, testItem.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/group-item/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GroupItems.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTestItem)]
        public async Task<IActionResult> DeleteTestItem(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var testItemInDb = await context.TestItemRepository.GetTestItemByKey(key);

                if (testItemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                testItemInDb.DateModified = DateTime.Now;
                testItemInDb.IsDeleted = true;
                testItemInDb.IsSynced = false;

                context.TestItemRepository.Update(testItemInDb);
                await context.SaveChangesAsync();

                return Ok(testItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteTestItem", "TestItemController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}