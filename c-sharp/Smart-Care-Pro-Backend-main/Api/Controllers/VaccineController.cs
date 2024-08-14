using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 25.12.2022
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

    public class VaccineController : ControllerBase
    {
        private Vaccine vaccine;

        private readonly IUnitOfWork context;
        private readonly ILogger<VaccineController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VaccineController(IUnitOfWork context, ILogger<VaccineController> logger)
        {
            vaccine = new Vaccine();
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/vaccine
        /// </summary>
        /// <param name="vaccine">Vaccine object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVaccine)]
        public async Task<ActionResult<Vaccine>> CreateVaccine(Vaccine vaccine)
        {
            try
            {
                vaccine.DateCreated = DateTime.Now;
                vaccine.IsDeleted = false;
                vaccine.IsSynced = false;

                context.VaccineRepository.Add(vaccine);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadVaccineByKey", new { key = vaccine.Oid }, vaccine);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVaccine", "VaccineController.cs", ex.Message, vaccine.CreatedIn, vaccine.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccines
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccines)]
        public async Task<IActionResult> ReadVaccines()
        {
            try
            {
                var vaccineInDb = await context.VaccineRepository.GetVaccines();
                vaccineInDb = vaccineInDb.OrderByDescending(x => x.DateCreated);
                return Ok(vaccineInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccines", "VaccineController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine/key/{key}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadVaccineByKey)]
        public async Task<IActionResult> ReadVaccineByKey(int key)
        {
            try
            {
                var vaccineInDb = await context.VaccineRepository.GetVaccineByKey(key);

                return Ok(vaccineInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadVaccineByKey", "VaccineController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/vaccine/by-vaccine-type/{VaccineTypeId}
        /// </summary>
        /// <param name="VaccineTypeId">Foreign key of the table Vaccines.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.VaccineNamesByVaccineType)]
        public async Task<IActionResult> VaccineNamesByVaccineType(int VaccineTypeId)
        {
            try
            {
                var vaccinesInDb = await context.VaccineRepository.GetVaccineNamesByVaccineType(VaccineTypeId);

                return Ok(vaccinesInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "VaccineNamesByVaccineType", "VaccineController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}