using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 09.03.2023
 * Modified by  : Brian
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
   /// <summary>
   /// Drug Regimen controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   [Authorize]

   public class DrugRegimenController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<DrugRegimenController> logger;

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public DrugRegimenController(IUnitOfWork context, ILogger<DrugRegimenController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: sc-api/drugRegimens
      /// </summary>
      /// <param name="drugRegimen">DrugRegimen object.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpPost]
      [Route(RouteConstants.CreateDrugRegimen)]
      public async Task<ActionResult<DrugRegimen>> CreateDrugRegimen(DrugRegimen drugRegimen)
      {
         try
         {
            if (await IsDrugRegimenDuplicate(drugRegimen) == true)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

            drugRegimen.DateCreated = DateTime.Now;
            drugRegimen.IsDeleted = false;
            drugRegimen.IsSynced = false;

            context.DrugRegimenRepository.Add(drugRegimen);
            await context.SaveChangesAsync();

            return CreatedAtAction("ReadDrugRegimenByKey", new { key = drugRegimen.Oid }, drugRegimen);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDrugRegimen", "DrugRegimenController.cs", ex.Message, drugRegimen.CreatedIn, drugRegimen.CreatedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/drugRegimens
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.DrugRegimens)]
      public async Task<IActionResult> ReadDrugRegimens()
      {
         try
         {
            var routeInDb = await context.DrugRegimenRepository.GetDrugRegimens();

            return Ok(routeInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugRegimens", "DrugRegimenController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/drugRegimens/drugRegimenByRegimenFor/{regimenFor}
      /// </summary>
      /// <param name="regimenFor">regimenId of the table DrugRegimen.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadDrugRegimensByRegimen)]
      public async Task<IActionResult> ReadDrugRegimensByRegimenFor(int regimenFor)
      {
         try
         {
            if (regimenFor <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var drugRegimensInDb = await context.DrugRegimenRepository.GetDrugRegimensByRegimenFor(regimenFor);

            if (drugRegimensInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            return Ok(drugRegimensInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugRegimensByRegimenFor", "DrugRegimenController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }


      /// <summary>
      /// URL: sc-api/drugRegimens/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table DrugRegimen.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadDrugRegimenByKey)]
      public async Task<IActionResult> ReadDrugRegimenByKey(int key)
      {
         try
         {
            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var drugRegimenInDb = await context.DrugRegimenRepository.GetDrugRegimenByKey(key);

            if (drugRegimenInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            return Ok(drugRegimenInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDrugRegimenByKey", "DrugRegimenController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/drugRegimens/{key}
      /// </summary>
      /// <param name="key">Primary key of the table DrugRegimens.</param>
      /// <param name="drugRegimen">DrugRegimen to be updated.</param>
      /// <returns>Http Status Code: NoContent.</returns>
      [HttpPut]
      [Route(RouteConstants.UpdateDrugRegimen)]
      public async Task<IActionResult> UpdateDrugRegimen(int key, DrugRegimen drugRegimen)
      {
         try
         {
            if (key != drugRegimen.Oid)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

            drugRegimen.DateModified = DateTime.Now;
            drugRegimen.IsDeleted = false;
            drugRegimen.IsSynced = false;

            context.DrugRegimenRepository.Update(drugRegimen);
            await context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDrugRegimen", "DrugRegimenController.cs", ex.Message, drugRegimen.ModifiedIn, drugRegimen.ModifiedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/drugRegimens/{key}
      /// </summary>
      /// <param name="key">Primary key of the table DrugRegimens.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpDelete]
      [Route(RouteConstants.DeleteDrugRegimen)]
      public async Task<IActionResult> DeleteDrugRegimen(int key)
      {
         try
         {
            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var drugRegimenInDb = await context.DrugRegimenRepository.GetDrugRegimenByKey(key);

            if (drugRegimenInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            drugRegimenInDb.DateModified = DateTime.Now;
            drugRegimenInDb.IsDeleted = true;
            drugRegimenInDb.IsSynced = false;

            context.DrugRegimenRepository.Update(drugRegimenInDb);
            await context.SaveChangesAsync();

            return Ok(drugRegimenInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDrugRegimen", "DrugRegimenController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// Checks whether the DrugRegimen name is duplicate or not.
      /// </summary>
      /// <param name="drugRegimen">DrugRegimen object.</param>
      /// <returns>Boolean</returns>
      private async Task<bool> IsDrugRegimenDuplicate(DrugRegimen drugRegimen)
      {
         try
         {
            var drugRegimenInDb = await context.DrugRegimenRepository.GetDrugRegimenByName(drugRegimen.Description);

            if (drugRegimenInDb != null)
               if (drugRegimenInDb.Oid != drugRegimen.Oid)
                  return true;

            return false;
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDrugRegimenDuplicate", "DrugRegimenController.cs", ex.Message);
            throw;
         }
      }
   }
}