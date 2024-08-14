using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Medicines controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicinesController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicinesController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicinesController(IUnitOfWork context, ILogger<MedicinesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/medicine
        /// </summary>
        /// <param name="medicines">Medicine object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicine)]
        public async Task<IActionResult> CreateMedicine(MedicineCreateDto medicines)
        {
            try
            {
                var medicinesList = medicines.Data?.Select(x => new Medicine()
                {
                    Description = Convert.ToString(x[1]),
                    MedicinesTime = Convert.ToInt64(x[0]),
                    PartographId = medicines.PartographId,
                }).ToList() ?? new List<Medicine>();

                foreach (var item in medicinesList)
                {
                    context.MedicinesRepository.UpdateMedicine(item);
                }

                await context.SaveChangesAsync();

                return Ok(medicinesList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateMedicine", "MedicinesController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}