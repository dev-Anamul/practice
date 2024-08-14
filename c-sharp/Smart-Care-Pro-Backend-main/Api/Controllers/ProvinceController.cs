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
 */

namespace Api.Controllers
{
    /// <summary>
    /// Province controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ProvinceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ProvinceController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ProvinceController(IUnitOfWork context, ILogger<ProvinceController> logger)
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
        [Route(RouteConstants.CreateProvince)]
        public async Task<IActionResult> CreateProvince(Province province)
        {
            try
            {
                if (await IsProvinceDuplicate(province) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NoMatchFoundError);

                province.DateCreated = DateTime.Now;
                province.IsDeleted = false;
                province.IsSynced = false;

                context.ProvinceRepository.Add(province);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadProvinceByKey", new { key = province.Oid }, province);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateProvince", "ProvinceController.cs", ex.Message, province.CreatedIn, province.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/provinces
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinces)]
        public async Task<IActionResult> ReadProvinces()
        {
            try
            {
                var provinceIndb = await context.ProvinceRepository.GetProvinces();

                return Ok(provinceIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinces", "ProvinceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/provinces/by-user/{userId}
        /// </summary>
        /// <param name="userId">Primary key of the table PriorARTExposers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvincesByUserID)]
        public async Task<IActionResult> ReadProvincesByUser(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var user = await context.UserAccountRepository.GetByIdAsync(userId);
                if (user == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                if (user.UserType == Enums.UserType.SystemAdministrator || user.UserType == Enums.UserType.FacilityAdministrator)
                {
                    var provinces = await context.ProvinceRepository.GetProvinces();

                    return Ok(provinces);
                }
                else
                {
                    var FacilityAccess = await context.FacilityAccessRepository.LoadListWithChildAsync<FacilityAccess>(x => x.UserAccountId == userId, x => x.Facility, y => y.Facility.District, p => p.Facility.District.Provinces);

                    var provinces = FacilityAccess.Select(x => x.Facility.District.Provinces).Distinct().ToList();

                    return Ok(provinces);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvincesByUser", "ProvinceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByKey)]
        public async Task<IActionResult> ReadProvinceByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceIndb = await context.ProvinceRepository.GetProvinceByKey(key);

                if (provinceIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(provinceIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinceByKey", "ProvinceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <param name="province">Province to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateProvince)]
        public async Task<IActionResult> UpdateProvince(int key, Province province)
        {
            try
            {
                if (key != province.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsProvinceDuplicate(province) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                province.DateModified = DateTime.Now;
                province.IsDeleted = false;
                province.IsSynced = false;

                context.ProvinceRepository.Update(province);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateProvince", "ProvinceController.cs", ex.Message, province.ModifiedIn, province.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteProvince)]
        public async Task<IActionResult> DeleteProvince(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByKey(key);

                if (provinceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                provinceInDb.DateModified = DateTime.Now;
                provinceInDb.IsDeleted = true;
                provinceInDb.IsSynced = false;

                context.ProvinceRepository.Update(provinceInDb);
                await context.SaveChangesAsync();

                return Ok(provinceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteProvince", "ProvinceController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the province name is duplicate or not.
        /// </summary>
        /// <param name="province">Province object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsProvinceDuplicate(Province province)
        {
            try
            {
                var provinceInDb = await context.ProvinceRepository.GetProvinceByName(province.Description);

                if (provinceInDb != null)
                    if (provinceInDb.Oid != province.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsProvinceDuplicate", "ProvinceController.cs", ex.Message);
                throw;
            }
        }
    }
}