using Domain.Dto;
using Domain.Entities;
using Domain.Validations;
using Domain.Validators;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using Utilities.Encryptions;
using static Utilities.Constants.Enums;

/*
 * Created by: Lion
 * Date created: 12.08.2022
 * Modified by: Stephan
 * Last modified: 26.12.2022
 */
namespace Api.Controllers
{
    /// <summary>
    /// UserAccount controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class UserAccountController : ControllerBase
    {
        private UserAccount user;

        private readonly IUnitOfWork context;
        private readonly ILogger<UserAccountController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public UserAccountController(IUnitOfWork context, ILogger<UserAccountController> logger)
        {
            user = new UserAccount();

            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/user-account
        /// </summary>
        /// <param name="userAccount">UserAccountDto object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateUserAccount)]
        public async Task<ActionResult<UserAccount>> CreateUserAccount(UserAccount userAccount)
        {
            try
            {
                var userWithSameNRC = await context.UserAccountRepository.GetUserNRC(userAccount.NRC);

                if (userWithSameNRC != null && userAccount.NoNRC == false)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateUserNRCError);

                var userAccountWithSameUsername = await context.UserAccountRepository.GetUserAccountByUsername(userAccount.Username);

                if (userAccountWithSameUsername != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UsernameTaken);

                if (userAccount.CountryCode == "+260" && userAccount.Cellphone[0] == '0')
                    userAccount.Cellphone = userAccount.Cellphone.Substring(1);

                var userAccountWithSameCellphone = await context.UserAccountRepository.GetUserAccountByCellphone(userAccount.Cellphone);

                if (userAccountWithSameCellphone != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateCellphoneError);

                if (userAccount.NoNRC == true)
                    userAccount.NRC = "000000/00/0";

                userAccount.DateCreated = DateTime.Now;
                userAccount.IsDeleted = false;
                userAccount.IsSynced = false;

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(userAccount.Password);
                userAccount.Password = encryptedPassword;

                user = context.UserAccountRepository.Add(userAccount);
                await context.SaveChangesAsync();

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateUserAccount", "UserAccountController.cs", ex.Message, userAccount.CreatedIn, userAccount.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/user-account/user-check/{userName}
        /// </summary>
        /// <param name="username">Username of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.CheckUserName)]
        public async Task<IActionResult> GetUserAccountByUsername(string userName)
        {
            try
            {
                var userInDb = await context.UserAccountRepository.GetUserAccountByUsername(userName);

                return Ok(userInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserAccountByUsername", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/user-account/user-check-by-cell
        /// </summary>
        /// <param name="userMobile">Mobile number of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.CheckUserMobile)]
        public async Task<IActionResult> GetUserAccountByMobile(string userMobile, string? countryCode, string? userId)
        {
            try
            {
                countryCode = countryCode == null ? "" : countryCode;
                if (userId == null)
                {
                    var userInDb = await context.UserAccountRepository.GetUserAccountByCellphoneNCountryCode(userMobile, countryCode);

                    return Ok(userInDb);
                }
                else
                {
                    var userInDb = await context.UserAccountRepository.GetUserAccountByCellphoneNCountryCode(userMobile, countryCode, new Guid(userId));

                    return Ok(userInDb);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserAccountByMobile", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/user-account/user-check-by-cell
        /// </summary>
        /// <param name="userMobile">Mobile number of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.CheckUserNRC)]

        public async Task<IActionResult> GetUserAccountByNRC(string nrc, string? userId)
        {
            try
            {
                if (nrc == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = MessageConstants.InvalidRequest });

                if (userId == null)
                {
                    var userInDb = await context.UserAccountRepository.GetUserAccountByNRC(nrc);

                    if (userInDb == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = "false" });
                    else
                        return StatusCode(StatusCodes.Status202Accepted, new ApiResponse { Message = "true" });

                }
                else
                {
                    var userInDb = await context.UserAccountRepository.GetUserAccountByNRC(nrc, new Guid(userId));

                    if (userInDb == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = "false" });
                    else
                        return StatusCode(StatusCodes.Status202Accepted, new ApiResponse { Message = "true" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserAccountByNRC", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse { Message = "false" });
            }
        }

        /// <summary>
        /// URL: sc-api/user-accounts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccounts)]
        public async Task<IActionResult> ReadUserAccounts()
        {
            try
            {
                var userAccount = await context.UserAccountRepository.GetUserAccounts();

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccounts", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByKey)]
        public async Task<IActionResult> ReadUserAccountByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetUserAccountByKey(key);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByKey", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/Firstname/{Firstname}
        /// </summary>
        /// <param name="firstName">Firstname of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByFirstname)]
        public async Task<IActionResult> ReadUserAccountByFirstName(string firstName)
        {
            try
            {
                if (string.IsNullOrEmpty(firstName))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetUserAccountByFirstName(firstName);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByFirstName", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/surname/{surname}
        /// </summary>
        /// <param name="surname">Surname of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountBySurname)]
        public async Task<IActionResult> ReadUserAccountBySurname(string surName)
        {
            try
            {
                if (string.IsNullOrEmpty(surName))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetUserAccountBySurname(surName);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountBySurname", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/cellphone/{cellphone}
        /// </summary>
        /// <param name="cellphone">Cellphone of a user.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByCellphone)]
        public async Task<IActionResult> ReadUserAccountByCellphone(string cellphone)
        {
            try
            {
                if (string.IsNullOrEmpty(cellphone))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetUserAccountByCellphone(cellphone);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByCellphone", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts.</param>
        /// <param name="userAccount">UserAccount to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateUserAccount)]
        public async Task<IActionResult> UpdateUserAccount(Guid key, UserAccountDto userAccount)
        {
            try
            {
                if (key != userAccount.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var userWithSameNRC = await context.UserAccountRepository.GetUserNRC(userAccount.NRC);

                if (userWithSameNRC.Oid != userAccount.Oid)
                {
                    if (userWithSameNRC != null && userAccount.NoNRC == false)
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateUserNRCError);
                }

                var userInDb = await context.UserAccountRepository.GetUserAccountByKey(userAccount.Oid);

                userInDb.FirstName = userAccount.FirstName;
                userInDb.Surname = userAccount.Surname;
                userInDb.DOB = userAccount.DOB;
                userInDb.Sex = userAccount.Sex;
                userInDb.Designation = userAccount.Designation;
                userInDb.NRC = userAccount.NRC;
                userInDb.NoNRC = userAccount.NoNRC;
                userInDb.ContactAddress = userAccount.ContactAddress;
                userInDb.CountryCode = userAccount.CountryCode;
                userInDb.Cellphone = userAccount.Cellphone;
                userInDb.DateModified = userAccount.DateModified;
                userInDb.ModifiedBy = userAccount.ModifiedBy;
                userInDb.IsSynced = userAccount.IsSynced;

                context.UserAccountRepository.Update(userInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateUserAccount", "UserAccountController.cs", ex.Message, userAccount.ModifiedIn, userAccount.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/{key}
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUserAccount)]
        public async Task<IActionResult> DeleteUserAccount(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByKey(key);

                if (userAccountInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                userAccountInDb.DateModified = DateTime.Now;
                userAccountInDb.IsDeleted = true;
                userAccountInDb.IsSynced = false;

                context.UserAccountRepository.Update(userAccountInDb);
                await context.SaveChangesAsync();

                return Ok(userAccountInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUserAccount", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/login
        /// </summary>
        /// <param name="LoginDto">UserLogin of a user account.</param>
        /// <returns>Http status code: Ok.</returns>

        [HttpPost]
        [Route(RouteConstants.UserLogin)]
        public async Task<ActionResult<UserAccount>> UserLoginForUI(LoginDto loginDto)
        {
            try
            {
                UserAccount user = await context.UserAccountRepository.GetUserAccountByUsername(loginDto.Username);

                if (user == null || (user.Username.ToLower() != loginDto.Username.ToLower()))
                    return BadRequest(MessageConstants.InvalidLogin);

                var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(loginDto.Username);

                if (requestInDb != null)
                    return BadRequest(MessageConstants.RecoveryRequestExists);

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(loginDto.Password);

                if (user.Password == encryptedPassword)
                {
                    UserAccountDto userInfo = new UserAccountDto()
                    {
                        Oid = user.Oid,
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        DOB = user.DOB,
                        Sex = user.Sex,
                        Designation = user.Designation,
                        NRC = user.NRC,
                        NoNRC = user.NoNRC,
                        ContactAddress = user.ContactAddress,
                        CountryCode = user.CountryCode,
                        Cellphone = user.Cellphone,
                        IsAccountActive = user.IsAccountActive,
                        ConfirmPassword = user.ConfirmPassword,
                        Password = user.Password,
                        UserType = user.UserType,
                        Username=user.Username                       
                    };

                    return Ok(userInfo);
                }

                return BadRequest(MessageConstants.InvalidLogin);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        ///// <summary>
        ///// URL: sc-api/user-account/login
        ///// </summary>
        ///// <param name="LoginDto">UserLogin of a user account.</param>
        ///// <returns>Http status code: Ok.</returns>

        //[HttpPost]
        //[Route(RouteConstants.UserLogin)]
        //public async Task<ActionResult<string>> UserLogin(LoginDto loginDto)
        //{
        //    try
        //    {
        //        user = await context.UserAccountRepository.GetUserAccountByUsername(loginDto.Username);

        //        if (user == null || (user.Username.ToLower() != loginDto.Username.ToLower()))
        //            return BadRequest(MessageConstants.InvalidLogin);

        //        var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(loginDto.Username);

        //        if (requestInDb != null)
        //            return BadRequest(MessageConstants.RecoveryRequestExists);

        //        EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
        //        string encryptedPassword = encryptionHelpers.Encrypt(loginDto.Password);

        //        if (user.Password == encryptedPassword)
        //        {
        //            var moduleAccesses = await context.ModuleAccessRepository.LoadListWithChildAsync<ModuleAccess>(
        //                f => f.FacilityAccess.UserAccountId == user.Oid && f.FacilityAccess.IsApproved == true && f.IsDeleted == false && f.IsSynced == false,
        //                f => f.FacilityAccess,
        //                u => u.FacilityAccess.UserAccount,
        //                fa => fa.FacilityAccess.Facility);

        //            UserLoginSuccessDto userLoginSuccessDTO = new UserLoginSuccessDto() { UserAccount = user, ModuleAccess = moduleAccesses.ToList() };

        //            return Ok(userLoginSuccessDTO);

        //        }

        //        return BadRequest(MessageConstants.InvalidLogin);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "UserAccountController.cs", ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// URL: sc-api/user-account/login
        /// </summary>
        /// <param name="LoginDto">UserLogin of a user account.</param>
        /// <returns>Http status code: Ok.</returns>

        [HttpPost]
        [Route(RouteConstants.GetUserAccessByUserName)]
        public async Task<ActionResult<string>> GetUserAccessByUserName(string userName)
        {
            try
            {
                user = await context.UserAccountRepository.GetUserAccountByUsername(userName);

                if (user == null)
                    return BadRequest("User not found");

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();

                var moduleAccesses = await context.ModuleAccessRepository.LoadListWithChildAsync<ModuleAccess>(
                    f => f.FacilityAccess.UserAccountId == user.Oid && f.FacilityAccess.IsApproved == true && f.IsDeleted == false && f.IsSynced == false,
                    f => f.FacilityAccess,
                    u => u.FacilityAccess.UserAccount,
                    fa => fa.FacilityAccess.Facility);

                var revokedFacilityAccess = await context.FacilityAccessRepository.LoadListWithChildAsync<FacilityAccess>(
                    fa => fa.UserAccountId == user.Oid && fa.IsDeleted == true,
                    p => p.UserAccount,
                    f => f.Facility);

                UserLoginSuccessDto userLoginSuccessDTO = new UserLoginSuccessDto() { UserAccount = user, ModuleAccess = moduleAccesses.ToList(), RevokedFacilityAccess = revokedFacilityAccess.ToList() };

                return Ok(userLoginSuccessDTO);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserAccessByUserName", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/user-access-by-username/{userName}
        /// </summary>
        /// <param name="userName">UserLogin of a user account.</param>
        /// <returns>Http status code: Ok.</returns>

        [HttpGet]
        [Route(RouteConstants.ReadUserAccessByUserName)]
        public async Task<ActionResult<string>> ReadUserAccessByUserName(string userName)
        {
            try
            {
                user = await context.UserAccountRepository.GetUserAccountByUsername(userName);

                if (user == null)
                    return BadRequest("User not found");

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();

                var moduleAccesses = await context.ModuleAccessRepository.LoadListWithChildAsync<ModuleAccess>(
                    f => f.FacilityAccess.UserAccountId == user.Oid && f.FacilityAccess.IsApproved == true && f.IsDeleted == false && f.IsSynced == false,
                    f => f.FacilityAccess,
                    u => u.FacilityAccess.UserAccount,
                    fa => fa.FacilityAccess.Facility);

                var revokedFacilityAccess = await context.FacilityAccessRepository.LoadListWithChildAsync<FacilityAccess>(
                    fa => fa.UserAccountId == user.Oid && fa.IsDeleted == true,
                    p => p.UserAccount,
                    f => f.Facility);

                UserLoginSuccessDto userLoginSuccessDTO = new UserLoginSuccessDto() { UserAccount = user, ModuleAccess = moduleAccesses.ToList(), RevokedFacilityAccess = revokedFacilityAccess.ToList() };

                return Ok(userLoginSuccessDTO);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserAccessByUserName", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/verify-password
        /// </summary>
        /// <param name="changePasswordDto">VerifyPassword of a user account.</param>
        /// <returns>Http status code: BadRequest.</returns>
        [HttpPost]
        [Route(RouteConstants.VerifyPassword)]
        public async Task<ActionResult<string>> VerifyPassword(VerifyPasswordDto changePasswordDto)
        {
            try
            {
                user = await context.UserAccountRepository.GetUserAccountByUsername(changePasswordDto.UserName);

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();

                string encryptedPassword = encryptionHelpers.Encrypt(changePasswordDto.Password);

                if (user.Password == encryptedPassword)
                    return Ok("true");

                return BadRequest(MessageConstants.WrongPasswordError);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "VerifyPassword", "UserAccountController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/update-password
        /// </summary>
        /// <param name="changePasswordDto">UpdatePassword of a user account.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.UpdatePassword)]
        public async Task<ActionResult<string>> UpdatePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                user = await context.UserAccountRepository.GetUserAccountByUsername(changePasswordDto.Username);

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                //string encryptedOldPassword = encryptionHelpers.Encrypt(changePasswordDto.Password);

                if (user.Password != changePasswordDto.Password)
                {
					string encryptedOldPassword = encryptionHelpers.Encrypt(changePasswordDto.Password);

                    if(user.Password != encryptedOldPassword)
						return BadRequest(MessageConstants.WrongPasswordError);
				}
                    

                string encryptedPassword = encryptionHelpers.Encrypt(changePasswordDto.NewPassword);
                user.Password = encryptedPassword;

                context.UserAccountRepository.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePassword", "UserAccountController.cs", ex.Message, "", "");
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/user-account/recovery-request
        /// </summary>
        /// <param name="recoveryDto">RecoveryRequest of a user account.</param>
        /// <returns>Http status code: Ok.</returns>

        [HttpPost]
        [Route(RouteConstants.RecoveryRequest)]
        public async Task<IActionResult> RecoveryRequest([FromBody] RecoveryRequestDto recoveryDto)
        {
            try
            {
                var check = context.UserAccountRepository.GetbyCellPhoneOrUsername(recoveryDto.Username, recoveryDto.Cellphone);

                UserAccount user = check;

                if(recoveryDto.CountryCode != null)
                {
                    if (user.CountryCode != recoveryDto.CountryCode)
                        return NotFound(MessageConstants.NoMatchFoundError);
                }

                if (check != null)
                {
                    var userRecoveryTable = new RecoveryRequest()
                    {
                        Username = check.Username,
                        Cellphone = check.Cellphone,
                        DateRequested = DateTime.Now,
                        IsRequestOpen = true
                    };

                    context.RecoveryRequestRepository.Add(userRecoveryTable);
                    await context.SaveChangesAsync();

                    return Ok(userRecoveryTable);
                }
                else
                {
                    return NotFound(MessageConstants.NoMatchFoundError);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RecoveryRequest", "UserAccountController.cs", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}