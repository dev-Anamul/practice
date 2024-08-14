using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 16.01.2023
 * Modified by   : Stephan
 * Last modified : 20.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class VaccineDoseController : ControllerBase
    {
        private VaccineDose vaccineDose;

        private readonly IUnitOfWork context;
        private readonly ILogger<VaccineDoseController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VaccineDoseController(IUnitOfWork context, ILogger<VaccineDoseController> logger)
        {
            vaccineDose = new VaccineDose();
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/vaccine-dose
        /// </summary>
        /// <param name="vaccineDose"></param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVaccineDose)]
        public async Task<ActionResult<VaccineDose>> CreateVaccineDose(VaccineDose vaccineDose)
        {
            try
            {
                vaccineDose.DateCreated = DateTime.Now;
                vaccineDose.IsDeleted = false;
                vaccineDose.IsSynced = false;

                context.VaccineDoseRepository.Add(vaccineDose);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVaccineDoseByKey", new { key = vaccineDose.Oid }, vaccineDose);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVaccineDose", "VaccineDoseController.cs", ex.Message, vaccineDose.CreatedIn, vaccineDose.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccines-doses
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccineDoses)]
        public async Task<IActionResult> ReadVaccineDoses()
        {
            try
            {
                var vaccineDoseInDb = await context.VaccineDoseRepository.GetVaccineDoses();
                vaccineDoseInDb = vaccineDoseInDb.OrderByDescending(x => x.DateCreated);
                return Ok(vaccineDoseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccineDoses", "VaccineDoseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-dose/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccineDoseByKey)]
        public async Task<IActionResult> ReadVaccineDoseByKey(int key)
        {
            try
            {
                var vaccineDoseInDb = await context.VaccineDoseRepository.GetVaccineDoseByKey(key);

                return Ok(vaccineDoseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccineDoseByKey", "VaccineDoseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine-dose/by-vaccine-name/{vaccineId}
        /// </summary>
        /// <param name="vaccineId">Foreign key of the table VaccineDoses.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.VaccineDosesByVaccineName)]
        public async Task<IActionResult> VaccineDosesByVaccineName(int vaccineId)
        {
            try
            {
                var vaccineDoseInDb = await context.VaccineDoseRepository.GetVaccineDoseByVaccineName(vaccineId);

                return Ok(vaccineDoseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "VaccineDosesByVaccineName", "VaccineDoseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/vaccine-dose/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VaccineDose.</param>
        /// <param name="vaccineDose">VaccineDose to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateVaccineDose)]
        public async Task<IActionResult> UpdateVaccineDose(int key, VaccineDose vaccineDose)
        {
            try
            {
                if (key != vaccineDose.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                vaccineDose.DateModified = DateTime.Now;
                vaccineDose.IsDeleted = false;
                vaccineDose.IsSynced = false;

                context.VaccineDoseRepository.Update(vaccineDose);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateVaccineDose", "VaccineDoseController.cs", ex.Message, vaccineDose.ModifiedIn, vaccineDose.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/vaccine-dose/{key}
        /// </summary>
        /// <param name="key">Primary key of the table VaccineDose.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteVaccineDose)]
        public async Task<IActionResult> DeleteVaccineDose(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var vaccineDoseInDb = await context.VaccineDoseRepository.GetVaccineDoseByKey(key);

                if (vaccineDoseInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                vaccineDoseInDb.DateModified = DateTime.Now;
                vaccineDoseInDb.IsDeleted = true;
                vaccineDoseInDb.IsSynced = false;

                context.VaccineDoseRepository.Update(vaccineDoseInDb);
                await context.SaveChangesAsync();

                return Ok(vaccineDoseInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteVaccineDose", "VaccineDoseController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}