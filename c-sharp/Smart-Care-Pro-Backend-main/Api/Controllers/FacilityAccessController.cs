using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using Utilities.Encryptions;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 12.12.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// FacilityAccess controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FacilityAccessController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FacilityAccessController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FacilityAccessController(IUnitOfWork context, ILogger<FacilityAccessController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/facility-access
        /// </summary>
        /// <param name="facilityAccess">FacilityAccess object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacilityAccess)]
        public async Task<IActionResult> CreateFacilityAccess(FacilityAccess facilityAccess)
        {
            try
            {
                var facilityAccessInDb = await context.FacilityAccessRepository.CheckDuplicateFacilityAccessRequestByUserId(facilityAccess.UserAccountId, facilityAccess.FacilityId);

                if (facilityAccessInDb != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.AlreadyReceivedFacility);

                facilityAccess.IsDeleted = false;
                facilityAccess.IsSynced = false;
                facilityAccess.IsApproved = false;
                facilityAccess.IsIgnored = false;

                var facilityInDb = context.FacilityAccessRepository.Add(facilityAccess);

                await context.SaveChangesAsync();

                return CreatedAtAction("CreateFacilityAccess", new { key = facilityInDb.Oid }, facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFacilityAccess", "FacilityAccessController.cs", ex.Message, facilityAccess.CreatedIn, facilityAccess.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-access/key/{key}
        /// </summary>
        /// <param name="key">key of a FacilityAccess.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadfacilityAccessByKey)]
        public async Task<IActionResult> ReadFacilityAccessRequestByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByKey(key);

                if (facilityAccessInDb == null)
                    return NotFound(MessageConstants.NoMatchFoundError);

                return Ok(facilityAccessInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByKey", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-access-with-module-access/key/{key}
        /// </summary>
        /// <param name="key">key of a FacilityAccess.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessWithModulePermissionsByKey)]
        public async Task<IActionResult> ReadFacilityAccessWithModulePermissionsByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByKey(key);

                if (facilityAccessInDb == null)
                    return NotFound(MessageConstants.NoMatchFoundError);

                List<ModuleAccess> moduleAccess = new List<ModuleAccess>();

                var facilityAccess = await context.ModuleAccessRepository.GetModuleAccessesByFacilityAccess(facilityAccessInDb.Oid);
                moduleAccess = facilityAccess.ToList();

                List<PermissionDto> permissionDto = new List<PermissionDto>();

                foreach (EncounterType encounter in Enum.GetValues(typeof(EncounterType)))
                {
                    permissionDto.Add(new PermissionDto
                    {
                        Id = (int)encounter,
                        Name = encounter.ToString(),
                        Checked = moduleAccess.Where(x => x.ModuleCode == (int)encounter).Any()
                    });
                }

                ModuleAccessDto moduleAccessDto = new ModuleAccessDto()
                {
                    FacilityAccessId = facilityAccessInDb.Oid,
                    Modules = permissionDto,
                    facilityAccess = facilityAccessInDb
                };

                return Ok(moduleAccessDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessWithModulePermissionsByKey", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-accesses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccess)]
        public async Task<IActionResult> ReadFacilityAccessRequest()
        {
            try
            {
                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccesses();

                return Ok(facilityAccessInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequest", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-accesses-admin
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessForAdmin)]
        public async Task<IActionResult> ReadFacilityAccessRequestForAdmin()
        {
            try
            {
                var nonAdminUsersInDb = await context.FacilityAccessRepository.GetNonAdminUsersForAdmin();

                return Ok(nonAdminUsersInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestForAdmin", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-access/facility-access-by-facility/{FacilityId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessByFacilityID)]
        public async Task<IActionResult> ReadFacilityAccessRequestByFacilityID(int facilityId)
        {
            try
            {
                if (facilityId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessesByFacility(facilityId);

                return Ok(facilityAccessInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByFacilityID", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessLoginRequestByFacilityID)]
        public async Task<IActionResult> ReadFacilityAccessLoginRequestByFacilityID(int facilityId, string? searchNameOrCellPhone, int page, int pageSize)
        {
            try
            {
                if (facilityId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessesByLoginRequestFacility(facilityId, searchNameOrCellPhone, ((page - 1) * (pageSize)), pageSize);
                var facilityAccessCountPending = context.FacilityAccessRepository.GetFacilityAccessesByLoginRequestFacilityTotalCount(facilityId, searchNameOrCellPhone);
                var facilityAccessCountApproved = context.FacilityAccessRepository.GetFacilityAccessesByApprovedRequestFacilityTotalCount(facilityId, "");
                var facilityAccessCountRecovery = context.FacilityAccessRepository.GetFacilityAccessesByRecoveryRequestFacilityTotalCount(facilityId, "");


                PagedResultFacilityAdministratorDto<FacilityAccess> facilityAccessWithPagginDto = new PagedResultFacilityAdministratorDto<FacilityAccess>()
                {
                    Data = facilityAccessInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItemsActive = facilityAccessCountApproved,
                    TotalItemsPending = facilityAccessCountPending,
                    TotalItemsRecover = facilityAccessCountRecovery
                };

                return Ok(facilityAccessWithPagginDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByFacilityID", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessApprovedRequestByFacilityID)]
        public async Task<IActionResult> ReadFacilityAccessApprovedRequestByFacilityID(int facilityId, string? searchNameOrCellPhone, int page, int pageSize)
        {
            try
            {
                if (facilityId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessesByApprovedRequestFacility(facilityId, searchNameOrCellPhone, ((page - 1) * (pageSize)), pageSize);
                var facilityAccessCountApproved = context.FacilityAccessRepository.GetFacilityAccessesByApprovedRequestFacilityTotalCount(facilityId, searchNameOrCellPhone);
                var facilityAccessCountRecovery = context.FacilityAccessRepository.GetFacilityAccessesByRecoveryRequestFacilityTotalCount(facilityId, "");
                var facilityAccessCountPending = context.FacilityAccessRepository.GetFacilityAccessesByLoginRequestFacilityTotalCount(facilityId, "");

                PagedResultFacilityAdministratorDto<FacilityAccess> facilityAccessWithPagginDto = new PagedResultFacilityAdministratorDto<FacilityAccess>()
                {
                    Data = facilityAccessInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItemsActive = facilityAccessCountApproved,
                    TotalItemsPending = facilityAccessCountPending,
                    TotalItemsRecover = facilityAccessCountRecovery
                };

                return Ok(facilityAccessWithPagginDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByFacilityID", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessRecoveryRequestByFacilityID)]
        public async Task<IActionResult> ReadFacilityAccessRecoveryRequestByFacilityID(int facilityId, string? searchNameOrCellPhone, int page, int pageSize)
        {
            try
            {
                if (facilityId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessesByRecoveryRequestFacility(facilityId, searchNameOrCellPhone, ((page - 1) * (pageSize)), pageSize);
                var facilityAccessCountRecovery = context.FacilityAccessRepository.GetFacilityAccessesByRecoveryRequestFacilityTotalCount(facilityId, searchNameOrCellPhone);
                var facilityAccessCountApproved = context.FacilityAccessRepository.GetFacilityAccessesByApprovedRequestFacilityTotalCount(facilityId, "");
                var facilityAccessCountPending = context.FacilityAccessRepository.GetFacilityAccessesByLoginRequestFacilityTotalCount(facilityId, "");

                PagedResultFacilityAdministratorDto<FacilityAccess> facilityAccessWithPagginDto = new PagedResultFacilityAdministratorDto<FacilityAccess>()
                {
                    Data = facilityAccessInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItemsRecover = facilityAccessCountRecovery,
                    TotalItemsActive = facilityAccessCountApproved,
                    TotalItemsPending = facilityAccessCountPending
                };

                return Ok(facilityAccessWithPagginDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByFacilityID", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: care-pro/facility-access-by-useraccount/{useraccountId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityAccessByUserAccountId)]
        public async Task<IActionResult> ReadFacilityAccessByUserAccountId(Guid useraccountId)
        {
            try
            {
                if (useraccountId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityAccessesInDb = await context.FacilityAccessRepository.GetFacilityAccessesByUserAccountId(useraccountId);

                if (facilityAccessesInDb == null || !facilityAccessesInDb.Any())
                    return NotFound();

                var facilityAccessDtos = facilityAccessesInDb.Select(MapFacilityAccessToDto);

                return Ok(facilityAccessDtos);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityAccessRequestByFacilityID", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/reject-facility-access/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpPut]
        [Route(RouteConstants.RejectFacilityAccess)]
        public async Task<IActionResult> RejectFacilityAccess(Guid key, FacilityAccess facilityAccess)
        {
            try
            {
                if (key != facilityAccess.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByKey(key);

                if (facilityAccessInDb != null)
                {
                    facilityAccessInDb.IsIgnored = facilityAccess.IsIgnored;
                    facilityAccessInDb.DateModified = facilityAccess.DateModified;

                    context.FacilityAccessRepository.Update(facilityAccessInDb);
                    await context.SaveChangesAsync();
                }

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RejectFacilityAccess", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-access-revoke-login/{userAccountId}
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="facilityAccess"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPut]
        [Route(RouteConstants.RevokeLoginByUserAccountID)]
        public async Task<IActionResult> RevokeLogin(Guid userAccountId, FacilityAccess facilityAccess)
        {
            try
            {
                userAccountId = facilityAccess.UserAccountId;

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByUserAccountId(userAccountId);

                if (facilityAccessInDb == null)
                    return NotFound(MessageConstants.NoMatchFoundError);

                facilityAccessInDb.IsApproved = false;
                facilityAccessInDb.IsDeleted = true;
                facilityAccessInDb.DateRequested = DateTime.Now;
                facilityAccessInDb.DateModified = DateTime.Now;

                context.FacilityAccessRepository.Update(facilityAccessInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RevokeLogin", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: facility-access/MakeAdmin/{userAccountId}
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPut]
        [Route(RouteConstants.MakeAdmin)]
        public async Task<IActionResult> MakeAdmin(Guid userAccountId)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.FirstOrDefaultAsync(x => x.Oid == userAccountId);

                if (userAccountInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.AlreadyReceivedFacility);

                userAccountInDb.UserType = UserType.FacilityAdministrator;

                context.UserAccountRepository.Update(userAccountInDb);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "MakeAdmin", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility-access/{userAccountId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacilityAccessByUserAccountID)]
        public async Task<IActionResult> UpdateFacilityAccessByUserAccountID(Guid userAccountId, FacilityAccess facilityAccess)
        {
            try
            {
                userAccountId = facilityAccess.UserAccountId;

                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByUserAccountId(userAccountId);

                if (facilityAccessInDb == null)
                    return NotFound(MessageConstants.NoMatchFoundError);

                facilityAccessInDb.IsIgnored = false;
                facilityAccessInDb.IsApproved = true;
                facilityAccessInDb.IsDeleted = false;
                facilityAccessInDb.DateModified = DateTime.Now;

                context.FacilityAccessRepository.Update(facilityAccessInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFacilityAccessByUserAccountID", "FacilityAccessController.cs", ex.Message, facilityAccess.ModifiedIn, facilityAccess.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: approve-facility-access/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <param name="facilityAccess"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPut]
        [Route(RouteConstants.ApproveFacilityAccess)]
        public async Task<IActionResult> ApproveFacilityAccess(Guid key, FacilityAccess facilityAccess)
        {
            try
            {
                // Assuming you have a method to get the FacilityAccess by key
                var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByKey(key);

                if (facilityAccessInDb == null)
                    return NotFound();

                if (key != facilityAccess.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (!facilityAccessInDb.IsApproved && facilityAccess.IsApproved)
                {
                    facilityAccessInDb.IsApproved = facilityAccess.IsApproved;
                    facilityAccessInDb.DateApproved = DateTime.Now;

                    var facility = await context.FacilityRepository.GetFacilityByKey(facilityAccessInDb.FacilityId);

                    if (facility != null && facility.IsDFZ)
                    {
                        var nonDFZFacilities = await context.FacilityAccessRepository.GetNonDFZFacilities(facilityAccessInDb.UserAccountId);

                        foreach (var nonDFZFacility in nonDFZFacilities)
                        {
                            var nonDFZAccess = await context.FacilityAccessRepository.GetFacilityAccessByUserAndFacilityId(facilityAccessInDb.UserAccountId, nonDFZFacility.Oid);

                            if (nonDFZAccess != null)
                            {
                                nonDFZAccess.IsDeleted = true;
                                nonDFZAccess.IsSynced = false;
                                nonDFZAccess.IsApproved = false;
                                nonDFZAccess.IsIgnored = true;
                                context.FacilityAccessRepository.Update(nonDFZAccess);
                            }
                        }
                    }
                }

                facilityAccessInDb.IsDeleted = facilityAccess.IsDeleted;
                facilityAccessInDb.DateModified = facilityAccess.DateModified;

                context.FacilityAccessRepository.Update(facilityAccessInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "ApproveFacilityAccess", "FacilityAccessController.cs", ex.Message, facilityAccess.ModifiedIn, facilityAccess.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/login-recovery-facility-access
        /// </summary>
        /// <param name="forgetPasswordDto"></param>
        /// <returns>Http status code: Ok.</returns>
        //[HttpPost]
        //[Route(RouteConstants.LoginRecoveryFacilityAccess)]
        //public async Task<ActionResult> LoginRecovery(ForgetPasswordDto forgetPasswordDto)
        //{
        //   try
        //   {
        //      var userInDb = await context.UserAccountRepository.GetUserAccountByUsername(forgetPasswordDto.UserName);

        //      var facilityAccessInDb = await context.FacilityAccessRepository.GetFacilityAccessByKey(forgetPasswordDto.FacilityRequestID);

        //      facilityAccessInDb.IsIgnored = true;
        //      facilityAccessInDb.DateModified = DateTime.Now;
        //      facilityAccessInDb.IsApproved = true;
        //      facilityAccessInDb.ForgotPassword = false;

        //      context.FacilityAccessRepository.Update(facilityAccessInDb);

        //      EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
        //      string encryptedPassword = encryptionHelpers.Encrypt(forgetPasswordDto.NewPassword);
        //      userInDb.Password = encryptedPassword;

        //      context.UserAccountRepository.Update(userInDb);

        //      var recoveryRequestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(forgetPasswordDto.UserName);

        //      recoveryRequestInDb.IsDeleted = true;

        //      context.RecoveryRequestRepository.Update(recoveryRequestInDb);
        //      await context.SaveChangesAsync();

        //      return Ok(userInDb);
        //   }
        //   catch (Exception ex)
        //   {
        //      logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoginRecovery", "FacilityAccessController.cs", ex.Message);
        //      return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //   }
        //}

        [HttpPost]
        [Route(RouteConstants.LoginRecoveryFacilityAccess)]
        public async Task<ActionResult> LoginRecovery(ForgetPasswordDto forgetPasswordDto)
        {
            try
            {
                var userInDb = await context.UserAccountRepository.GetUserAccountByUsername(forgetPasswordDto.UserName);

                var facilityAccessesInDb = await context.FacilityAccessRepository.GetAllFacilityAccessesByUserAccountId(userInDb.Oid);

                foreach (var facilityAccess in facilityAccessesInDb)
                {
                    facilityAccess.IsIgnored = true;
                    facilityAccess.DateModified = DateTime.Now;
                    facilityAccess.IsApproved = true;
                    facilityAccess.ForgotPassword = false;

                    context.FacilityAccessRepository.Update(facilityAccess);
                }

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(forgetPasswordDto.NewPassword);
                userInDb.Password = encryptedPassword;

                context.UserAccountRepository.Update(userInDb);

                var recoveryRequestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByUsername(forgetPasswordDto.UserName);
                recoveryRequestInDb.IsDeleted = true;
                recoveryRequestInDb.IsRequestOpen = false;

                context.RecoveryRequestRepository.Update(recoveryRequestInDb);
                await context.SaveChangesAsync();

                return Ok(userInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "LoginRecovery", "FacilityAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        private FacilityAccessDto MapFacilityAccessToDto(FacilityAccess facilityAccess)
        {
            return new FacilityAccessDto()
            {
                Oid = facilityAccess.Oid,
                FacilityId = facilityAccess.FacilityId,
                Cellphone = facilityAccess.UserAccount.Cellphone,
                ContactAddress = facilityAccess.UserAccount.ContactAddress,
                CountryCode = facilityAccess.UserAccount.CountryCode,
                Designation = facilityAccess.UserAccount.Designation,
                DOB = facilityAccess.UserAccount.DOB,
                FacilityName = facilityAccess.Facility.Description,
                FirstName = facilityAccess.UserAccount.FirstName,
                ISDFZFacility = facilityAccess.Facility.IsDFZ,
                NoNRC = facilityAccess.UserAccount.NoNRC,
                NRC = facilityAccess.UserAccount.NRC,
                UserAccountId = facilityAccess.UserAccount.Oid,
                Sex = facilityAccess.UserAccount.Sex,
                Surname = facilityAccess.UserAccount.Surname
            };
        }
    }
}