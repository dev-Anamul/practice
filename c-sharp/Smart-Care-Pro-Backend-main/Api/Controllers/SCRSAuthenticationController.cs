using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Utilities.Constants;
using Utilities.Encryptions;

namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SCRSAuthenticationController : ControllerBase
    {
        private readonly ILogger<UserAccountController> logger;
        private readonly IUnitOfWork context;
        public SCRSAuthenticationController(IUnitOfWork context, ILogger<UserAccountController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpPost]
        [Route(RouteConstants.SCRSToken)]
        public async Task<ActionResult> SCRSToken(SCRSAuthenticationDto sCRSAuthenticationDto)
        {
            try
            {
                var user = await context.UserAccountRepository.GetUserAccountByUsername(sCRSAuthenticationDto.UserName);

                if (user == null)
                    return BadRequest("User not found");

                var facilityAccess = await context.FacilityAccessRepository.FirstOrDefaultAsync(x => x.FacilityId == sCRSAuthenticationDto.FacilityId && x.UserAccountId == user.Oid && x.IsApproved == true);
                if (facilityAccess == null)
                    return BadRequest("Facility Forbidden");

                string token = sCRSAuthenticationDto.UserName + "||" + DateTime.Now.ToString() + "||" + sCRSAuthenticationDto.FacilityId;
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedToken = encryptionHelpers.Encrypt(token);

                // token = "ABCXYZ";
                 ApiResponse response = new ApiResponse();
                response.Message = encryptedToken;

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "SCRSAuthenticationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.SCRSTokenVerification)]
        public async Task<ActionResult> SCRSTokenVerification(SCRSTokenRequestDto sCRSTokenRequestDto)
        {
            try
            {
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string decryptedToken = encryptionHelpers.Decrypt(sCRSTokenRequestDto.Token);

                if (string.IsNullOrEmpty(decryptedToken))
                    return BadRequest("Invalid Token");

                string[] tokenValues = decryptedToken.Split("||");

                if (tokenValues.Length != 3)
                {
                    return BadRequest("Invalid Token");
                }

                DateTime TokenGeneratedateTime = Convert.ToDateTime(tokenValues[1]);
                DateTime currentTime = DateTime.Now;
                DateTime fiveMinutesAgo = currentTime.AddMinutes(-5);

                if (TokenGeneratedateTime < fiveMinutesAgo)
                {
                    return BadRequest("Token Expired");
                }

                string userName = tokenValues[0];
                string facilityId = tokenValues[2];

                var user = await context.UserAccountRepository.GetUserAccountByUsername(userName);

                if (user != null)
                {
                    long facilityKey = Convert.ToUInt32(facilityId);
                    SCRSLoginSuccessDto sCRSLoginSuccessDto = new SCRSLoginSuccessDto()
                    {
                        FirstName = user.FirstName,
                        LastName = user.Surname,
                        Sex = user.Sex,
                        DOB = user.DOB,
                        Designation = user.Designation,
                        Username = user.Username,
                        FacilityKey = facilityKey

                    };

                    return Ok(sCRSLoginSuccessDto);
                }
                else
                {
                    return BadRequest("Invalid Token");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "SCRSAuthenticationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.SCRSLogin)]
        public async Task<ActionResult> SCRSLogin(UserLoginDto UserLoginDto)
        {
            try
            {
                var user = await context.UserAccountRepository.GetUserAccountByUsername(UserLoginDto.Username);

                if (user == null || (user.Username.ToLower() != UserLoginDto.Username.ToLower()))
                    return BadRequest(MessageConstants.InvalidLogin);

                var requestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(UserLoginDto.Username);

                if (requestInDb != null)
                    return BadRequest(MessageConstants.RecoveryRequestExists);

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(UserLoginDto.Password);

                if (user.Password == encryptedPassword)
                {
                    SCRSLoginSuccessDto sCRSLoginSuccessDto = new SCRSLoginSuccessDto()
                    {
                        FirstName = user.FirstName,
                        LastName = user.Surname,
                        Sex = user.Sex,
                        DOB = user.DOB,
                        Designation = user.Designation,
                        Username = user.Username

                    };

                    return Ok(sCRSLoginSuccessDto);
                }
                else
                {
                    return BadRequest(MessageConstants.WrongPasswordError);
                }


            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "SCRSAuthenticationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpPost]
        [Route(RouteConstants.SCRSCheckFacilityPermission)]
        public async Task<IActionResult> SCRSCheckFacilityPermission(SCRSFacilityAccessDto sCRSFacilityAccessDto)
        {
            var user = await context.UserAccountRepository.GetUserAccountByUsername(sCRSFacilityAccessDto.Username);

            if (user == null || (user.Username.ToLower() != sCRSFacilityAccessDto.Username.ToLower()))
                return BadRequest(MessageConstants.InvalidLogin);

            if (user.UserType == Enums.UserType.SystemAdministrator)
            {
                return Ok(true);
            }

            var facilityAccess = await context.FacilityAccessRepository.FirstOrDefaultAsync(x => x.FacilityId == sCRSFacilityAccessDto.FacilityId && x.UserAccountId == user.Oid && x.IsApproved == true);
            if (facilityAccess == null)
                return BadRequest("Facility Forbidden");
            else
                return Ok(true);



        }
    }
}