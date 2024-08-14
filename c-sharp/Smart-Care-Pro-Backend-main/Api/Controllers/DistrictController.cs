using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by: Lion
 * Date created: 12.09.2022
 * Modified by: Stephan
 * Last modified: 06.11.2022
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
    /// <summary>
    /// District controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class DistrictController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DistrictController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public DistrictController(IUnitOfWork context, ILogger<DistrictController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/district
        /// </summary>
        /// <param name="district">District object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDistrict)]
        public async Task<IActionResult> CreateDistrict(District district)
        {
            try
            {
                if (await IsDistrictDuplicate(district) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                district.DateCreated = DateTime.Now;
                district.IsDeleted = false;
                district.IsSynced = false;

                context.DistrictRepository.Add(district);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDistrictByKey", new { key = district.Oid }, district);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDistrict", "DistrictController.cs", ex.Message, district.CreatedIn, district.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/districts
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistricts)]
        public async Task<IActionResult> ReadDistricts()
        {
            try
            {
                var districtsInDb = await context.DistrictRepository.GetDistricts();

                // Map the District entities to DistrictDto
                var districtDtos = districtsInDb.Select(district =>
                {
                    return new DistrictDto
                    {
                        Oid = district.Oid,
                        Description = district.Description,
                        DistrictCode = district.DistrictCode,
                        ProvinceId = district.ProvinceId
                    };
                }).ToList();

                return Ok(districtDtos);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistricts", "DistrictController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/district/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Districts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistrictByKey)]
        public async Task<IActionResult> ReadDistrictByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var districtIndb = await context.DistrictRepository.GetDistrictByKey(key);

                if (districtIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(districtIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistrictByKey", "DistrictController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Districts.</param>
        /// <param name="district">District to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDistrict)]
        public async Task<IActionResult> UpdateDistrict(int key, District district)
        {
            try
            {
                if (key != district.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsDistrictDuplicate(district) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                district.DateModified = DateTime.Now;
                district.IsDeleted = false;
                district.IsSynced = false;

                context.DistrictRepository.Update(district);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDistrict", "DistrictController.cs", ex.Message, district.ModifiedIn, district.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Districts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDistrict)]
        public async Task<IActionResult> DeleteDistrict(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var districtInDb = context.DistrictRepository.Get(key);

                if (districtInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                districtInDb.DateModified = DateTime.Now;
                districtInDb.IsDeleted = true;
                districtInDb.IsSynced = false;

                context.DistrictRepository.Update(districtInDb);
                await context.SaveChangesAsync();

                return Ok(districtInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDistrict", "DistrictController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the district name is duplicate or not.
        /// </summary>
        /// <param name="district">District object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDistrictDuplicate(District district)
        {
            try
            {
                var districtInDb = await context.DistrictRepository.GetDistrictByName(district.Description);

                if (districtInDb != null)
                    if (districtInDb.Oid != district.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDistrictDuplicate", "DistrictController.cs", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// URL: sc-api/district/districtByProvince/{ProvinceId}
        /// </summary>
        /// <param name="ProvinceId">Foreign key of the table Districts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.DistrictByProvince)]
        public async Task<IActionResult> GetAllDistrictByProvince(int provinceId)
        {
            try
            {
                var districtInDb = await context.DistrictRepository.GetDistrictByProvince(provinceId);

                return Ok(districtInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetAllDistrictByProvince", "DistrictController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}