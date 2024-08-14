using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */

namespace Api.Controllers
{
    /// <summary>
    /// ServicePoint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ServicePointController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ServicePointController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ServicePointController(IUnitOfWork context, ILogger<ServicePointController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/service-point
        /// </summary>
        /// <param name="servicePoint">ServicePoint object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        //[HttpPost]
        //[Route(RouteConstants.CreateServicePoint)]
        //public async Task<ActionResult<ServicePoint>> CreateServicePoint(ServicePoint servicePoint)
        //{
        //    try
        //    {
        //        if (await IsNTGDuplicate(servicePoint) == true)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

        //        servicePoint.DateCreated = DateTime.Now;
        //        servicePoint.IsDeleted = false;
        //        servicePoint.IsSynced = false;

        //        context.NTGLevelThreeDiagnosisRepository.Add(servicePoint);
        //        await context.SaveChangesAsync();

        //        return CreatedAtAction("ReadNTGLevelThreeDiagnosisByKey", new { key = servicePoint.Oid }, servicePoint);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateServicePoint", "ServicePointController.cs", ex.Message, servicePoint.CreatedIn, servicePoint.CreatedBy);
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}


        //[HttpPost]
        //[Route(RouteConstants.CreateServicePoint)]
        //public async Task<IActionResult> CreateServicePoint(ServicePoint servicePoint)
        //{
        //    try
        //    {
        //        if (await IsServicePointDuplicate(servicePoint) == true)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

        //        servicePoint.DateCreated = DateTime.Now;
        //        servicePoint.IsDeleted = false;
        //        servicePoint.IsSynced = false;

        //        context.ServicePointRepository.Add(servicePoint);
        //        await context.SaveChangesAsync();

        //        return CreatedAtAction("ReadServicePointByKey", new { key = servicePoint.Oid }, servicePoint);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateServicePoint", "ServicePointController.cs", ex.Message, servicePoint.CreatedIn, servicePoint.CreatedBy);
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        //[HttpGet]
        //[Route(RouteConstants.ReadServicePointByFacility)]
        //public async Task<IActionResult> ReadServicePointByFacility(int FacilityId)
        //{
        //    try
        //    {
        //        if (FacilityId <= 0)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var servicePoint = await context.ServicePointRepository.GetServicePointByFacility(FacilityId);

        //        if (servicePoint == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(servicePoint);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateResultOption", "ResultOptionController.cs", ex.Message, resultOption.CreatedIn, resultOption.CreatedBy);, "ReadServicePointByKey  Exception");
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}
        //[HttpGet]
        //[Route(RouteConstants.ReadServicePointByFacilityWithActivePatientCount)]
        //public async Task<IActionResult> ReadServicePointByFacilityWithActivePatientCount(int FacilityId)
        //{
        //    try
        //    {
        //        if (FacilityId <= 0)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var servicePoint = await context.ServicePointRepository.GetServicePointByFacilityWithActivePatientCount(FacilityId);

        //        if (servicePoint == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(servicePoint);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateResultOption", "ResultOptionController.cs", ex.Message, resultOption.CreatedIn, resultOption.CreatedBy);, "ReadServicePointByKey  Exception");
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// URL: sc-api/service-points
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadServicePoints)]
        public async Task<IActionResult> ReadServicePoints()
        {
            try
            {
                var servicePoint = await context.ServicePointRepository.GetServicePoints();

                return Ok(servicePoint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadServicePoints", "ServicePointController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/service-point/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadServicePointByKey)]
        public async Task<IActionResult> ReadServicePointByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var servicePoint = await context.ServicePointRepository.GetServicePointByKey(key);

                if (servicePoint == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(servicePoint);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadServicePointByKey", "ServicePointController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/service-point/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <param name="servicePoint">ServicePoint to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        //[HttpPut]
        //[Route(RouteConstants.UpdateServicePoint)]
        //public async Task<IActionResult> UpdateServicePoint(int key, ServicePoint servicePoint)
        //{
        //    try
        //    {
        //        if (key != servicePoint.Oid)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

        //        if (await IsServicePointDuplicate(servicePoint) == true)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

        //        servicePoint.DateModified = DateTime.Now;
        //        servicePoint.IsDeleted = false;
        //        servicePoint.IsSynced = false;

        //        context.ServicePointRepository.Update(servicePoint);
        //        await context.SaveChangesAsync();

        //        return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateResultOption", "ResultOptionController.cs", ex.Message, resultOption.CreatedIn, resultOption.CreatedBy);, "UpdateServicePoint  Exception");
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// URL: sc-api/service-point/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteServicePoint)]
        public async Task<IActionResult> DeleteServicePoint(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var servicePointInDb = await context.ServicePointRepository.GetServicePointByKey(key);

                if (servicePointInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                servicePointInDb.DateModified = DateTime.Now;
                servicePointInDb.IsDeleted = true;
                servicePointInDb.IsSynced = false;

                context.ServicePointRepository.Update(servicePointInDb);
                await context.SaveChangesAsync();

                return Ok(servicePointInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteServicePoint", "ServicePointController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the service point name is duplicate or not.
        /// </summary>
        /// <param name="servicePoint">ServicePoint object.</param>
        /// <returns>Boolean</returns>
        //private async Task<bool> IsServicePointDuplicate(ServicePoint servicePoint)
        //{
        //    try
        //    {
        //        var servicePointInDb = await context.ServicePointRepository.FirstOrDefaultAsync(x => (x.Description == servicePoint.Description || x.ServiceCode == servicePoint.ServiceCode) && x.FacilityId == servicePoint.FacilityId && x.IsDeleted == false && x.Oid != servicePoint.Oid);

        //        if (servicePointInDb != null)
        //            return true;

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateResultOption", "ResultOptionController.cs", ex.Message, resultOption.CreatedIn, resultOption.CreatedBy);, "IsServicePointDuplicate  Exception");
        //        throw;
        //    }
        //}
    }
}