using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;


namespace Api.Controllers
{

    /// <summary>
    /// MedicineManufacture controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicineManufactureController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicineManufactureController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicineManufactureController(IUnitOfWork context, ILogger<MedicineManufactureController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        /// <summary>
        /// URL: sc-api/medicine-manufacturer
        /// </summary>
        /// <param name="medicineManufacturer">MedicineManufacturer object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicineManufacturer)]
        public async Task<ActionResult<MedicineManufacturer>> CreateMedicineManufacturer(MedicineManufacturer medicineManufacturer)
        {
            try
            {
                if (await IsMedicineManufacturerDuplicate(medicineManufacturer) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                medicineManufacturer.DateCreated = DateTime.Now;
                medicineManufacturer.IsDeleted = false;
                medicineManufacturer.IsSynced = false;

                context.MedicineManufactureRepository.Add(medicineManufacturer);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMedicineManufacturerByKey", new { key = medicineManufacturer.Oid }, medicineManufacturer);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMedicineManufacturer", "MedicineManufactureController.cs", ex.Message, medicineManufacturer.CreatedIn, medicineManufacturer.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/medicine-manufacturers
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicineManufacturer)]
        public async Task<IActionResult> ReadMedicineManufacturer()
        {
            try
            {
                var medicineManufacturerInDb = await context.MedicineManufactureRepository.GetMedicineManufacturer();

                return Ok(medicineManufacturerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicineManufacturer", "MedicineManufactureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-manufacturer/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicineManufacturer.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicineManufacturerByKey)]
        public async Task<IActionResult> ReadMedicineManufacturerByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicineManufacturerInDb = await context.MedicineManufactureRepository.GetMedicineManufacturerByKey(key);

                if (medicineManufacturerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(medicineManufacturerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicineManufacturerByKey", "MedicineManufactureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-manufacturer
        /// </summary>
        /// <param name="key">Primary key of the table MedicineManufacturer.</param>
        /// <param name="medicineManufacturerInDb">MedicineManufacturer to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMedicineManufacturer)]
        public async Task<IActionResult> UpdateMedicineManufacturer(int key, MedicineManufacturer medicineManufacturer)
        {
            try
            {
                if (key != medicineManufacturer.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                medicineManufacturer.DateModified = DateTime.Now;
                medicineManufacturer.IsDeleted = false;
                medicineManufacturer.IsSynced = false;

                context.MedicineManufactureRepository.Update(medicineManufacturer);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicineManufacturer", "MedicineManufactureController.cs", ex.Message, medicineManufacturer.ModifiedIn, medicineManufacturer.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-manufacturer
        /// </summary>
        /// <param name="key">Primary key of the table MedicineManufacturer.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMedicineManufacturer)]
        public async Task<IActionResult> DeleteMedicineManufacturer(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicineManufacturerInDb = await context.MedicineManufactureRepository.GetMedicineManufacturerByKey(key);

                if (medicineManufacturerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                medicineManufacturerInDb.DateModified = DateTime.Now;
                medicineManufacturerInDb.IsDeleted = true;
                medicineManufacturerInDb.IsSynced = false;

                context.MedicineManufactureRepository.Update(medicineManufacturerInDb);
                await context.SaveChangesAsync();

                return Ok(medicineManufacturerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMedicineManufacturer", "MedicineManufactureController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the MedicineManufacturer name is duplicate or not.
        /// </summary>
        /// <param name="medicineManufacturer">MedicineManufacturer object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsMedicineManufacturerDuplicate(MedicineManufacturer medicineManufacturer)
        {
            try
            {
                var medicineManufacturerInDb = await context.MedicineManufactureRepository.GetMedicineManufacturerByName(medicineManufacturer.Description);

                if (medicineManufacturerInDb != null)
                    if (medicineManufacturerInDb.Oid != medicineManufacturerInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsMedicineManufacturerDuplicate", "MedicineManufactureController.cs", ex.Message);
                throw;
            }
        }
    }
}
