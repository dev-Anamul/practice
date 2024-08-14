using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Facility controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FacilityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FacilityController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        public FacilityController(IUnitOfWork context, ILogger<FacilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/facility
        /// </summary>
        /// <param name="facility">Facility object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacility)]
        public async Task<IActionResult> CreateFacility(Facility facility)
        {
            try
            {
                if (await IsFacilityDuplicate(facility) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NoMatchFoundError);

                facility.DateCreated = DateTime.Now;
                facility.IsDeleted = false;
                facility.IsSynced = false;

                context.FacilityRepository.Add(facility);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFacilityByKey", new { key = facility.Oid }, facility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFacility", "FacilityController.cs", ex.Message, facility.CreatedIn, facility.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/facilities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilities)]
        public async Task<IActionResult> ReadFacilities()
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetFacilities();

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilities", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/facilities
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSelectFacilityData)]
        public async Task<IActionResult> ReadSelectFacilityData(Guid userId)
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetAllActiveFacilities(userId);
                var districtsInDb = await context.DistrictRepository.GetDistrictsWithOutProvince();
                var provincesInDb = await context.ProvinceRepository.GetProvinces();

                SelectFacilityDto selectFacilityDto = new SelectFacilityDto()
                {
                    Facilities = facilityInDb == null ? new List<Facility>() : facilityInDb.ToList(),
                    Provinces = provincesInDb == null ? new List<Province>() : provincesInDb.ToList(),
                    Districts = districtsInDb == null ? new List<District>() : districtsInDb.ToList(),
                };

                return Ok(selectFacilityDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSelectFacilityData", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadRequestFacilityData)]
        public async Task<IActionResult> ReadRequestFacilityData(Guid userId)
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetAllRequestActiveFacilities(userId);
                var districtsInDb = await context.DistrictRepository.GetDistrictsWithOutProvince();
                var provincesInDb = await context.ProvinceRepository.GetProvinces();

                SelectFacilityDto selectFacilityDto = new SelectFacilityDto()
                {
                    Facilities = facilityInDb == null ? new List<Facility>() : facilityInDb.ToList(),
                    Provinces = provincesInDb == null ? new List<Province>() : provincesInDb.ToList(),
                    Districts = districtsInDb == null ? new List<District>() : districtsInDb.ToList(),
                };

                return Ok(selectFacilityDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSelectFacilityData", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/active-facilites
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadActiveFacility)]
        public async Task<IActionResult> ReadActiveFacility()
        {
            try
            {
                var facilitiesInDb = await context.FacilityRepository.GetAllActiveFacilities();

                // Map the Facility entities to FacilityDto
                var facilityDtos = facilitiesInDb.Select(facility =>
                {
                    return new FacilityDto
                    {
                        Oid = facility.Oid,
                        Description = facility.Description,
                        FacilityMasterCode = facility.FacilityMasterCode,
                        HMISCode = facility.HMISCode,
                        Longitude = facility.Longitude,
                        Latitude = facility.Latitude,
                        Location = facility.Location,
                        FacilityType = facility.FacilityType,
                        Ownership = facility.Ownership,
                        IsPrivateFacility = facility.IsPrivateFacility,
                        IsLive = facility.IsLive,
                        IsDFZ = facility.IsDFZ,
                        DistrictId = facility.DistrictId
                        // You may need to map the District property as well if needed
                    };
                }).ToList();

                return Ok(facilityDtos);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadActiveFacility", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/facility-by-district/{districtId}
        /// </summary>
        /// <param name="districtId">Foreign key of the table Districts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadActivefacilityByDistrict)]
        public async Task<IActionResult> ReadActivefacilityByDistrict(int districtId, int page, int pageSize, string? searchFacility)
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetActiveFacilitiesByDistrice(districtId, searchFacility, ((page - 1) * (pageSize)), pageSize);

                int totalItems = context.FacilityRepository.GetFacilitiesWithPagingTotalCount(searchFacility);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                FacilityPaginationDto facilityPaginationDto = new FacilityPaginationDto()
                {
                    facilities = facilityInDb == null ? new List<Facility>() : facilityInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                return Ok(facilityPaginationDto);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllFacilityByDistrict", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: sc-api/facilities-with-paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchFacility"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilitiesWithPagging)]
        public async Task<IActionResult> ReadFacilitiesWithPaging(int page, int pageSize, string? searchFacility)
        {
            var facilityInDb = await context.FacilityRepository.GetFacilitiesWithPaging(searchFacility, ((page - 1) * (pageSize)), pageSize);
            int totalItems = context.FacilityRepository.GetFacilitiesWithPagingTotalCount(searchFacility);

            FacilityPaginationDto facilityPaginationDto = new FacilityPaginationDto()
            {
                facilities = facilityInDb == null ? new List<Facility>() : facilityInDb.ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return Ok(facilityPaginationDto);
        }

        /// <summary>
        /// URL: sc-api/facility/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByKey)]
        public async Task<IActionResult> ReadFacilityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByKey", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/facility-name/{facilityName}
        /// </summary>
        /// <param name="facilityName">Facility name of the table Facilities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByFacilityName)]
        public async Task<IActionResult> ReadFacilityByFacilityName(string facilityName)
        {
            try
            {
                if (string.IsNullOrEmpty(facilityName))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByName(facilityName);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByFacilityName", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/facility-by-district/{districtId}
        /// </summary>
        /// <param name="districtId">Foreign key of the table Districts.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadfacilityByDistrict)]
        public async Task<IActionResult> ReadAllFacilityByDistrict(int districtId, int page, int pageSize, string? searchFacility)
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetFacilityByDistrict(districtId, searchFacility, ((page - 1) * (pageSize)), pageSize);

                int totalItems = context.FacilityRepository.GetFacilitiesWithPagingTotalCount(searchFacility);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                FacilityPaginationDto facilityPaginationDto = new FacilityPaginationDto()
                {
                    facilities = facilityInDb == null ? new List<Facility>() : facilityInDb.ToList(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                return Ok(facilityPaginationDto);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadAllFacilityByDistrict", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/facility-by-facility-type/{type}
        /// </summary>
        /// <param name="type">type identifier of the table Facility.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.facilityByFacilityType)]
        public async Task<IActionResult> ReadAllFacilityByFacilityType(bool type)
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetAllFacilityByFacilityType(type);

                return Ok(facilityInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities.</param>
        /// <param name="facility">Facility to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacility)]
        public async Task<IActionResult> UpdateFacility(int key, Facility facility)
        {
            try
            {
                if (key != facility.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsFacilityDuplicate(facility) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                facility.DateModified = DateTime.Now;
                facility.IsDeleted = false;
                facility.IsSynced = false;

                context.FacilityRepository.Update(facility);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFacility", "FacilityController.cs", ex.Message, facility.ModifiedIn, facility.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacility)]
        public async Task<IActionResult> DeleteFacility(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                facilityInDb.DateModified = DateTime.Now;
                facilityInDb.IsDeleted = true;
                facilityInDb.IsSynced = false;

                context.FacilityRepository.Update(facilityInDb);
                await context.SaveChangesAsync();

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFacility", "FacilityController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: check-facility-duplicate
        /// </summary>
        /// <param name="facility">Facility object</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFacilityDuplicate(Facility facility)
        {
            try
            {
                var userFacilityInDb = await context.FacilityRepository.GetFacilityByName(facility.Description);

                if (userFacilityInDb != null)
                    if (userFacilityInDb.Oid != facility.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFacilityDuplicate", "FacilityController.cs", ex.Message);
                throw;
            }
        }
    }
}