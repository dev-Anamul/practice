using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 29-01-2023
 * Modified by  : Stephan
 * Last modified: 30-01-2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
   /// <summary>
   /// Ward controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   [Authorize]

   public class WardController : ControllerBase
   {

      private readonly IUnitOfWork context;
      private readonly ILogger<WardController> logger;
      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public WardController(IUnitOfWork context, ILogger<WardController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: sc-api/ward
      /// </summary>
      /// <param name="Ward">Ward object.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpPost]
      [Route(RouteConstants.CreateWard)]
      public async Task<ActionResult<Ward>> CreateWard(Ward ward)
      {
         try
         {
            var WardInDb = await context.WardRepository.GetWardByName(ward.Description);

            if (WardInDb != null && WardInDb.FirmId == ward.FirmId)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

            ward.IsDeleted = false;
            ward.IsSynced = false;
            ward.DateCreated = DateTime.Now;

            var newWardInDb = context.WardRepository.Add(ward);

            await context.SaveChangesAsync();

            return Ok(newWardInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateWard", "WardController.cs", ex.Message, ward.CreatedIn, ward.CreatedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/wards
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadWards)]
      public async Task<IActionResult> ReadWards()
      {
         try
         {
            var WardInDb = await context.WardRepository.GetWards();
            WardInDb = WardInDb.OrderByDescending(x => x.DateCreated);
            return Ok(WardInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWards", "WardController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/wards
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadWardsByFacilityId)]
      public async Task<IActionResult> ReadWardsByFacilityId(int facilityId)
      {
         try
         {
            var wardsByFacilityIdInDb = await context.WardRepository.GetWardsByFacilityId(facilityId);

            return Ok(wardsByFacilityIdInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWardsByFacilityId", "WardController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/ward/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Ward.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadWardByKey)]
      public async Task<IActionResult> ReadWardByKey(int key)
      {
         try
         {
            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var WardInDb = await context.WardRepository.GetWardByKey(key);

            if (WardInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.DuplicateError);

            return Ok(WardInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadWardByKey", "WardController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/ward/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Ward.</param>
      /// <param name="Ward">Ward to be updated.</param>
      /// <returns>Http status code: NoContent.</returns>
      [HttpPut]
      [Route(RouteConstants.UpdateWard)]
      public async Task<IActionResult> UpdateWard(int key, Ward ward)
      {
         try
         {
            if (key != ward.Oid)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);
            var WardInDb = await context.WardRepository.GetWardByName(ward.Description);

            if (WardInDb != null && WardInDb.FirmId == ward.FirmId)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

            WardInDb = await context.WardRepository.GetWardByKey(ward.Oid);

            WardInDb.Description = ward.Description;
            WardInDb.DateModified = ward.DateModified;
            WardInDb.ModifiedBy = ward.ModifiedBy;
            WardInDb.IsSynced = ward.IsSynced;
            WardInDb.IsDeleted = ward.IsDeleted;
            WardInDb.FirmId = ward.FirmId;

            context.WardRepository.Update(WardInDb);
            await context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateWard", "WardController.cs", ex.Message, ward.ModifiedIn, ward.ModifiedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }


      /// <summary>
      /// URL: sc-api/firm/firmByDepartment/{departmentId}
      /// </summary>
      /// <param name="departmentId">Foreign key of the table Firm.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.FirmByDepartment)]
      public async Task<IActionResult> FirmByDepartment(int departmentId)
      {
         try
         {
            var firmsByDepartmenInDb = await context.FirmRepository.GetFirmByDepartment(departmentId);

            return Ok(firmsByDepartmenInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "FirmByDepartment", "WardController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/ward/wardByFirm/{FirmID}
      /// </summary>
      /// <param name="DistrictId">Foreign key of the table Towns.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadWardByFirm)]
      public async Task<IActionResult> GetWardByFirm(int firmID)
      {
         try
         {
            var wardInDb = await context.WardRepository.GetWardByFirm(firmID);

            return Ok(wardInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetWardByFirm", "WardController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }


        /// <summary>
        /// URL: sc-api/ward/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Ward.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteWard)]
        public async Task<IActionResult> DeleteWard(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var wardInDb = await context.WardRepository.GetWardByKey(key);

                if (wardInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                wardInDb.DateModified = DateTime.Now;
                wardInDb.IsDeleted = true;
                wardInDb.IsSynced = false;

                context.WardRepository.Update(wardInDb);
                await context.SaveChangesAsync();

                return Ok(wardInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteWard", "WardController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}