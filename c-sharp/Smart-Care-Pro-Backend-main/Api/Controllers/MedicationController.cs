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
    /// General Medication controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicationController(IUnitOfWork context, ILogger<MedicationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/generalMedication
        /// </summary>
        /// <param name="generalMedication">GeneralMedication object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGeneralMedication)]
        public async Task<ActionResult<Medication>> CreateGeneralMedication(Medication generalMedication)
        {
            try
            {

                generalMedication.DateCreated = DateTime.Now;
                generalMedication.IsDeleted = false;
                generalMedication.IsSynced = false;

                var createPrescriptionInDb = context.MedicationRepository.Add(generalMedication);

                await context.SaveChangesAsync();

                return Ok(createPrescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGeneralMedication", "MedicationController.cs", ex.Message, generalMedication.CreatedIn, generalMedication.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generalMedications
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralMedications)]
        public async Task<IActionResult> ReadGeneralMedication()
        {
            try
            {
                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedication();

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralMedication", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generalMedication/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GeneralMedication.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralMedicationByKey)]
        public async Task<IActionResult> ReadGeneralMedicationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByKey(key);

                if (generalMedicationsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralMedicationByKey", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadDispenseDetailPrescriptionByKey)]
        public async Task<IActionResult> ReadDispenseDetailByPrescriptionKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByPrescription(key);

                if (generalMedicationsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDispenseDetailByPrescriptionKey", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/general-medication/key/{key}
        /// </summary>
        /// <param name="prescriptionId">Primary key of the table GeneralMedication.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralMedicationByPrescriptionId)]
        public async Task<IActionResult> ReadGeneralMedicationByPrescriptionId(Guid prescriptionId)
        {
            try
            {
                if (prescriptionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByPrescriptionId(prescriptionId);

                if (generalMedicationsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralMedicationByPrescriptionId", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpGet]
        [Route(RouteConstants.ReadGeneralMedicationByInteractionId)]
        public async Task<IActionResult> GetGeneralMedicationByInteractionId(Guid interactionId)
        {
            try
            {
                if (interactionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByInteractionId(interactionId);

                if (generalMedicationsInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetGeneralMedicationByInteractionId", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/general-medicationdto/key/{key}
        /// </summary>
        /// <param name="prescriptionId">Primary key of the table GeneralMedication.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneralMedicationDtoByPrescriptionId)]
        public async Task<IActionResult> ReadGeneralMedicationDtoByPrescriptionId(Guid prescriptionId)
        {
            try
            {
                if (prescriptionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                List<MedicationDto> generalMedicationsDto = new List<MedicationDto>();

                Prescription prescriptionInDb = await context.PrescriptionRepository.GetPrescriptionByKey(prescriptionId);

                if (prescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByPrescriptionId(prescriptionId);

                MedicationDto generalMedication = new MedicationDto();
                generalMedication.PrescriptionId = prescriptionId;
                generalMedication.PrescriptionDate = prescriptionInDb.PrescriptionDate;
                generalMedication.GeneralMedicationsList = generalMedicationsInDb.ToList();

                generalMedicationsDto.Add(generalMedication);

                return Ok(generalMedicationsDto);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneralMedicationDtoByPrescriptionId", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generalMedication/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GeneralMedication.</param>
        /// <param name="generalMedication">GeneralMedication to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPost]
        [Route(RouteConstants.UpdateGeneralMedication)]
        public async Task<IActionResult> UpdateGeneralMedication(Guid key, Medication generalMedication)
        {
            try
            {
                if (key != generalMedication.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);


                if (generalMedication.InteractionId != Guid.Empty)
                {
                    var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByKey(generalMedication.InteractionId);

                    if (generalMedicationsInDb.InteractionId == generalMedication.InteractionId)
                    {
                        generalMedicationsInDb.DateModified = DateTime.Now;
                        generalMedicationsInDb.ModifiedBy = generalMedication.ModifiedBy;
                        generalMedicationsInDb.ModifiedIn = generalMedicationsInDb.ModifiedIn;
                        generalMedicationsInDb.RouteId = generalMedication.RouteId;
                        generalMedicationsInDb.FrequencyIntervalId = generalMedication.FrequencyIntervalId;
                        generalMedicationsInDb.Frequency = generalMedication.Frequency;
                        generalMedicationsInDb.SpecialDrugId = generalMedication.SpecialDrugId;
                        generalMedicationsInDb.Note = generalMedication.Note;
                        generalMedicationsInDb.AdditionalDrugFormulation = generalMedication.AdditionalDrugFormulation;
                        generalMedicationsInDb.AdditionalDrugTitle = generalMedication.AdditionalDrugTitle;
                        generalMedicationsInDb.StartDate = generalMedication.StartDate;
                        generalMedicationsInDb.EndDate = generalMedication.EndDate;
                        generalMedicationsInDb.TimeUnit = generalMedication.TimeUnit;
                        generalMedicationsInDb.IsPasserBy= generalMedication.IsPasserBy;
                        generalMedicationsInDb.ItemPerDose = generalMedication.ItemPerDose;
                        generalMedicationsInDb.PrescribedDosage = generalMedication.PrescribedDosage;
                        generalMedicationsInDb.Duration = generalMedication.Duration;
                        generalMedicationsInDb.GeneralDrugId = generalMedication.GeneralDrugId;
                        generalMedicationsInDb.DosageUnit = generalMedication.DosageUnit;
                        generalMedicationsInDb.PrescribedDosage = generalMedication.PrescribedDosage;
                        generalMedicationsInDb.PrescribedQuantity = generalMedication.PrescribedQuantity;
                        generalMedication.IsSynced = false;
                        generalMedication.IsDeleted = false;

                        context.MedicationRepository.Update(generalMedicationsInDb);
                    }
                }

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGeneralMedication", "MedicationController.cs", ex.Message, generalMedication.ModifiedIn, generalMedication.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpDelete]
        [Route(RouteConstants.DeleteGeneralMedication)]
        public async Task<IActionResult> DeleteGeneralMedication(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicationInDb = await context.MedicationRepository.GetGeneralMedicationByKey(key);

                if (medicationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.MedicationRepository.Delete(medicationInDb);
                await context.SaveChangesAsync();

                return Ok(medicationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGeneralMedication", "MedicationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}