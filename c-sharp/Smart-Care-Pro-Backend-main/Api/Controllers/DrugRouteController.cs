using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 06.03.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Drug Route controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DrugRouteController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DrugRouteController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DrugRouteController(IUnitOfWork context, ILogger<DrugRouteController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/drugRoutes
        /// </summary>
        /// <param name="drugRoute">DrugRoute object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDrugRoute)]
        public async Task<ActionResult<DrugRoute>> CreateDrugRoute(DrugRoute drugRoute)
        {
            try
            {
                if (await IsDrugRouteDuplicate(drugRoute) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                drugRoute.DateCreated = DateTime.Now;
                drugRoute.IsDeleted = false;
                drugRoute.IsSynced = false;

                context.DrugRouteRepository.Add(drugRoute);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDrugRouteByKey", new { key = drugRoute.Oid }, drugRoute);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugRoute", "DrugRouteController.cs", ex.Message, drugRoute.CreatedIn, drugRoute.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugRoutes
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugRoutes)]
        public async Task<IActionResult> ReadDrugRoutes()
        {
            try
            {
                var routeInDb = await context.DrugRouteRepository.GetDrugRoutes();

                return Ok(routeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugRoutes", "DrugRouteController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugRoutes/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugRoute.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDrugRouteByKey)]
        public async Task<IActionResult> ReadDrugRouteByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugRouteInDb = await context.DrugRouteRepository.GetDrugRouteByKey(key);

                if (drugRouteInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(drugRouteInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugRouteByKey", "DrugRouteController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugRoutes/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugRoutes.</param>
        /// <param name="drugRoute">DrugRoute to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDrugRoute)]
        public async Task<IActionResult> UpdateDrugRoute(int key, DrugRoute drugRoute)
        {
            try
            {
                if (key != drugRoute.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                drugRoute.DateModified = DateTime.Now;
                drugRoute.IsDeleted = false;
                drugRoute.IsSynced = false;

                context.DrugRouteRepository.Update(drugRoute);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugRoute", "DrugRouteController.cs", ex.Message, drugRoute.ModifiedIn, drugRoute.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/drugRoutes/{key}
        /// </summary>
        /// <param name="key">Primary key of the table DrugRoutes.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDrugRoute)]
        public async Task<IActionResult> DeleteDrugRoute(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var drugRouteInDb = await context.DrugRouteRepository.GetDrugRouteByKey(key);

                if (drugRouteInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                drugRouteInDb.DateModified = DateTime.Now;
                drugRouteInDb.IsDeleted = true;
                drugRouteInDb.IsSynced = false;

                context.DrugRouteRepository.Update(drugRouteInDb);
                await context.SaveChangesAsync();

                return Ok(drugRouteInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugRoute", "DrugRouteController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the DrugRoute name is duplicate or not.
        /// </summary>
        /// <param name="DrugRoute">DrugRoute object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDrugRouteDuplicate(DrugRoute drugRoute)
        {
            try
            {
                var drugRouteInDb = await context.DrugRouteRepository.GetDrugRouteByName(drugRoute.Description);

                if (drugRouteInDb != null)
                    if (drugRouteInDb.Oid != drugRoute.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDrugRouteDuplicate", "DrugRouteController.cs", ex.Message);
                throw;
            }
        }
    }
}