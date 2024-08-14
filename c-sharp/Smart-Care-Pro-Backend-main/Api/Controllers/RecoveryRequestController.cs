using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by: Brian
 * Date created: 12.09.2022
 * Modified by: Brian
 * Last modified: 06.11.2022
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
   /// <summary>
   /// Caregiver controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   [Authorize]

   public class RecoveryRequestController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<RecoveryRequestController> logger;
      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public RecoveryRequestController(IUnitOfWork context, ILogger<RecoveryRequestController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: sc-api/recovery-request
      /// </summary>
      /// <param name="request">RecoveryRequest object.</param>
      /// <returns>Http status code: CreatedAt.</returns>
      //[HttpPost]
      //[Route(RouteConstants.CreateRecoveryRequest)]
      //public async Task<IActionResult> CreateRecoveryRequest(RecoveryRequestDto recoveryRequestDto)
      //{
      //   try
      //   {
      //      if (recoveryRequestDto.Username == null && recoveryRequestDto.Cellphone == null)
      //         return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongUsernameAndCellphoneInputError);

      //      var userInDb = new UserAccount();

      //      string Username = string.Empty;
      //      string Cellphone = string.Empty;

      //      if (recoveryRequestDto.Username != null)
      //      {


      //         userInDb = await context.UserAccountRepository.GetUserAccountByUsername(recoveryRequestDto.Username);

      //         if (userInDb == null)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongUsernameInputError);

      //         if (recoveryRequestDto.CountryCode != null && recoveryRequestDto.Cellphone == null)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CellPhoneAndCountryCodeRequired);

      //         if (recoveryRequestDto.CountryCode == null && recoveryRequestDto.Cellphone != null)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CellPhoneAndCountryCodeRequired);

      //         if (recoveryRequestDto.CountryCode != null && recoveryRequestDto.Cellphone != null)
      //         {
      //            if (recoveryRequestDto.CountryCode != userInDb.CountryCode || recoveryRequestDto.Cellphone != userInDb.Cellphone)
      //               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongCellphoneInputError);
      //         }

      //         if (await IsRecoveryRequestDuplicateByUsername(recoveryRequestDto.Username) != true)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.RecoveryRequestDuplicate);

      //         if (userInDb != null)
      //            Username = userInDb.Username;
      //      }

      //      if (recoveryRequestDto.Cellphone != null || recoveryRequestDto.Username == null || userInDb == null)
      //      {
      //         if (await IsRecoveryRequestDuplicateByCellphone(recoveryRequestDto.Cellphone) != true)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.RecoveryRequestDuplicate);

      //         userInDb = await context.UserAccountRepository.GetUserAccountByCellphone(recoveryRequestDto.Cellphone);

      //         if (userInDb == null)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongCellphoneInputError);

      //         if (userInDb != null)
      //            Cellphone = userInDb.Cellphone;

      //         if (userInDb.CountryCode != recoveryRequestDto.CountryCode)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CountryCodeError);
      //      }

      //      if (Username == recoveryRequestDto.Username && Cellphone == recoveryRequestDto.Cellphone)
      //      {
      //         userInDb = await context.UserAccountRepository.GetUserAccountByCellphone(recoveryRequestDto.Cellphone);

      //         if (userInDb.CountryCode != recoveryRequestDto.CountryCode)
      //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CountryCodeError);
      //      }

      //      if (userInDb == null)
      //         return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

      //      if (userInDb.FacilityAccesses == null)
      //         return StatusCode(StatusCodes.Status401Unauthorized, MessageConstants.InvalidFacilityLogin);

      //      RecoveryRequest request = new RecoveryRequest();

      //      request.Oid = Guid.NewGuid();
      //      request.Username = userInDb.Username;
      //      request.Cellphone = userInDb.Cellphone;
      //      request.CountryCode = userInDb.CountryCode;
      //      request.DateCreated = DateTime.Now;
      //      request.CreatedIn = 0;
      //      request.CreatedBy = Guid.Empty;
      //      request.IsSynced = false;
      //      request.IsDeleted = false;
      //      request.DateRequested = DateTime.Now;

      //      context.RecoveryRequestRepository.Add(request);

      //      FacilityAccess facilityAccess = new FacilityAccess();

      //      facilityAccess.DateCreated = DateTime.Now;
      //      facilityAccess.IsDeleted = false;
      //      facilityAccess.IsSynced = false;
      //      facilityAccess.DateRequested = DateTime.Now;
      //      facilityAccess.ForgotPassword = true;
      //      facilityAccess.FacilityId = userInDb.FacilityAccesses.Select(x => x.FacilityId).FirstOrDefault();
      //      facilityAccess.UserAccountId = userInDb.Oid;

      //      context.FacilityAccessRepository.Add(facilityAccess);
      //      await context.SaveChangesAsync();

      //      return CreatedAtAction("ReadRecoveryRequestByKey", new { key = facilityAccess.Oid }, facilityAccess);
      //   }
      //   catch (Exception ex)
      //   {
      //      logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateRecoveryRequest", "RecoveryRequestController.cs", ex.Message);
      //      return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
      //   }
      //}

      [HttpPost]
      [Route(RouteConstants.CreateRecoveryRequest)]
      public async Task<IActionResult> CreateRecoveryRequest(RecoveryRequestDto recoveryRequestDto)
      {
         try
         {
            if (recoveryRequestDto.Username == null && recoveryRequestDto.Cellphone == null)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongUsernameAndCellphoneInputError);

            var userInDb = new UserAccount();

            string Username = string.Empty;
            string Cellphone = string.Empty;

            if (recoveryRequestDto.Username != null)
            {
               userInDb = await context.UserAccountRepository.GetUserAccountByUsername(recoveryRequestDto.Username);

               if (userInDb == null)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongUsernameInputError);

               if (recoveryRequestDto.CountryCode != null && recoveryRequestDto.Cellphone == null)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CellPhoneAndCountryCodeRequired);

               if (recoveryRequestDto.CountryCode == null && recoveryRequestDto.Cellphone != null)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CellPhoneAndCountryCodeRequired);

               if (recoveryRequestDto.CountryCode != null && recoveryRequestDto.Cellphone != null)
               {
                  if (recoveryRequestDto.CountryCode != userInDb.CountryCode || recoveryRequestDto.Cellphone != userInDb.Cellphone)
                     return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongCellphoneInputError);
               }

               if (await IsRecoveryRequestDuplicateByUsername(recoveryRequestDto.Username) != true)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.RecoveryRequestDuplicate);

               if (userInDb != null)
                  Username = userInDb.Username;
            }

            if (recoveryRequestDto.Cellphone != null || recoveryRequestDto.Username == null || userInDb == null)
            {
               if (await IsRecoveryRequestDuplicateByCellphone(recoveryRequestDto.Cellphone) != true)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.RecoveryRequestDuplicate);

               userInDb = await context.UserAccountRepository.GetUserAccountByCellphone(recoveryRequestDto.Cellphone);

               if (userInDb == null)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.WrongCellphoneInputError);

               if (userInDb != null)
                  Cellphone = userInDb.Cellphone;

               if (userInDb.CountryCode != recoveryRequestDto.CountryCode)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CountryCodeError);
            }

            if (Username == recoveryRequestDto.Username && Cellphone == recoveryRequestDto.Cellphone)
            {
               userInDb = await context.UserAccountRepository.GetUserAccountByCellphone(recoveryRequestDto.Cellphone);

               if (userInDb.CountryCode != recoveryRequestDto.CountryCode)
                  return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.CountryCodeError);
            }

            if (userInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            if (userInDb.FacilityAccesses == null)
               return StatusCode(StatusCodes.Status401Unauthorized, MessageConstants.InvalidFacilityLogin);

            RecoveryRequest request = new RecoveryRequest();

            request.Oid = Guid.NewGuid();
            request.Username = userInDb.Username;
            request.Cellphone = userInDb.Cellphone;
            request.CountryCode = userInDb.CountryCode;
            request.DateCreated = DateTime.Now;
            request.CreatedIn = 0;
            request.CreatedBy = Guid.Empty;
            request.IsSynced = false;
            request.IsDeleted = false;
            request.DateRequested = DateTime.Now;
            request.IsRequestOpen = true;

            context.RecoveryRequestRepository.Add(request);

            foreach (var facilityAccess in userInDb.FacilityAccesses.Where(x => x.IsIgnored == false).ToList())
            {
               FacilityAccess facilityAccessInDb = new FacilityAccess();

               facilityAccessInDb.DateCreated = DateTime.Now;
               facilityAccessInDb.IsDeleted = false;
               facilityAccessInDb.IsSynced = false;
               facilityAccessInDb.DateRequested = DateTime.Now;
               facilityAccessInDb.ForgotPassword = true;
               facilityAccessInDb.FacilityId = facilityAccess.FacilityId;

               facilityAccessInDb.UserAccountId = userInDb.Oid;

               context.FacilityAccessRepository.Add(facilityAccessInDb);
            }
            await context.SaveChangesAsync();

            return Ok();
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateRecoveryRequest", "RecoveryRequestController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// URl: sc-api/recovery-requests
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadRecoveryRequests)]
      public async Task<IActionResult> ReadRecoveryRequests()
      {
         try
         {
            var request = await context.RecoveryRequestRepository.GetRecoveryRequests();

            return Ok(request);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequests", "RecoveryRequestController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// URL : sc-api/recovery-request/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table RecoveryRequests.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadRecoveryRequestByKey)]
      public async Task<IActionResult> ReadRecoveryRequestByKey(Guid key)
      {
         try
         {
            if (key == Guid.Empty)
               return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Message = MessageConstants.InvalidParameterError });

            var request = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

            if (request == null)
               return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = MessageConstants.NoMatchFoundError });

            return Ok(request);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequestByKey", "RecoveryRequestController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// URL : sc-api/recovery-request/key/{key}
      /// </summary>
      /// <param name="adminRecoveryRequestDto">Primary key of the table Recovery Request.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpPost]
      [Route(RouteConstants.ReadRecoveryRequestByDate)]
      public async Task<IActionResult> ReadRecoveryRequestByDates(AdminRecoveryRequestDto adminRecoveryRequestDto)
      {
         try
         {
            if (adminRecoveryRequestDto == null)
               return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Message = MessageConstants.InvalidParameterError });

            var request = await context.RecoveryRequestRepository.GetRecoveryRequestByDate(adminRecoveryRequestDto);

            if (request == null)
               return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = MessageConstants.NoMatchFoundError });

            return Ok(request);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequestByDates", "RecoveryRequestController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// URL: sc-api/recovery-request/{key}
      /// </summary>
      /// <param name="key">Primary key of the table RecoveryRequests.</param>
      /// <param name="request">RecoveryRequest to be updated.</param>
      /// <returns>Http Status Code: NoContent.</returns>
      [HttpPut]
      [Route(RouteConstants.UpdateRecoveryRequest)]
      public async Task<IActionResult> UpdateRecoveryRequest(Guid key, RecoveryRequest request)
      {
         try
         {
            if (key != request.Oid)
               return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Message = MessageConstants.UnauthorizedAttemptOfRecordUpdateError });

            request.DateModified = DateTime.Now;
            request.IsDeleted = false;
            request.IsSynced = false;

            context.RecoveryRequestRepository.Update(request);
            await context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateRecoveryRequest", "RecoveryRequestController.cs", ex.Message, request.ModifiedIn, request.ModifiedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// URL: sc-api/recovery-request/{key}
      /// </summary>
      /// <param name="key">Primary key of the table RecoveryRequests.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpDelete]
      [Route(RouteConstants.DeleteRecoveryRequest)]
      public async Task<IActionResult> DeleteRecoveryRequest(Guid key)
      {
         try
         {
            if (key == Guid.Empty)
               return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Message = MessageConstants.InvalidParameterError });

            var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

            if (requestInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = MessageConstants.NoMatchFoundError });

            requestInDb.DateModified = DateTime.Now;
            requestInDb.IsDeleted = true;
            requestInDb.IsSynced = false;

            context.RecoveryRequestRepository.Update(requestInDb);
            await context.SaveChangesAsync();

            return Ok(requestInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteRecoveryRequest", "RecoveryRequestController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = MessageConstants.GenericError });
         }
      }

      /// <summary>
      /// Checks whether the recovery request is duplicate or not. 
      /// </summary>
      /// <param name="request">RecoveryRequest object.</param>
      /// <returns>Boolean</returns>
      private async Task<bool> IsRecoveryRequestDuplicateByCellphone(string cellPhone)
      {
         try
         {
            var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByCellphone(cellPhone);

            if (requestInDb != null)
               return false;
            else
               return true;
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsRecoveryRequestDuplicateByCellphone", "RecoveryRequestController.cs", ex.Message);
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a recovery request by Username.
      /// </summary>
      /// <param name="Username">Username of a user.</param>
      /// <returns>Returns a recovery request if the Username is matched.</returns>
      private async Task<bool> IsRecoveryRequestDuplicateByUsername(string Username)
      {
         try
         {
            var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(Username);

            if (requestInDb != null)
               return false;
            else
               return true;
         }
         catch
         {
            throw;
         }
      }
   }
}