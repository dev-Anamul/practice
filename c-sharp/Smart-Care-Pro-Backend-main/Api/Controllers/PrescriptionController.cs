using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   :Stephan 
 * Date created : 15.02.2023
 * Modified by  : Brian
 * Last modified: 13.03.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Prescription controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PrescriptionController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PrescriptionController(IUnitOfWork context, ILogger<PrescriptionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/prescription
        /// </summary>
        /// <param name="Prescription">Prescription object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePrescription)]
        public async Task<IActionResult> CreatePrescriptions(PrescriptionsDto prescriptionsDto)
        {
            try
            {
                prescriptionsDto.InteractionId = Guid.NewGuid();

                Interaction interaction = new Interaction();

                interaction.Oid = prescriptionsDto.InteractionId;
                interaction.EncounterId = prescriptionsDto.EncounterID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Prescription, EncounterType.Prescriptions);
                interaction.CreatedBy = prescriptionsDto.CreatedBy;
                interaction.CreatedIn = prescriptionsDto.CreatedIn;
                interaction.DateCreated = DateTime.Now;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                var encounter = await context.EncounterRepository.GetEncounterByKey(prescriptionsDto.EncounterID); 

                Prescription prescription = new Prescription();

                prescription.InteractionId = prescriptionsDto.InteractionId;
                prescription.PrescriptionDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated ?? DateTime.Now;
                prescription.DispensationDate = null;
                prescription.ClientId = prescriptionsDto.ClientID;
                prescription.EncounterId = prescriptionsDto.EncounterID;

                prescription.CreatedIn = prescriptionsDto.CreatedIn;
                prescription.CreatedBy = prescriptionsDto.CreatedBy;
                prescription.DateCreated = DateTime.Now;
                prescription.IsDeleted = false;
                prescription.IsSynced = false;

                context.PrescriptionRepository.Add(prescription);

                foreach (var item in prescriptionsDto.PrescriptionsList)
                {
                    item.InteractionId = Guid.NewGuid();
                    item.PrescriptionId = prescriptionsDto.InteractionId;

                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.CreatedIn = prescriptionsDto.CreatedIn;
                    item.CreatedBy = prescriptionsDto.CreatedBy;

                    if (item.GeneralDrugId > 0)
                        context.MedicationRepository.Add(item);

                    if (item.SpecialDrugId > 0)
                    {
                        Medication medication = new Medication()
                        {
                            InteractionId = item.InteractionId,
                            PrescribedDosage = item.PrescribedDosage,
                            ItemPerDose = item.ItemPerDose,
                            Frequency = item.Frequency,
                            FrequencyIntervalId = item.FrequencyIntervalId,
                            Duration = item.Duration,
                            TimeUnit = item.TimeUnit,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Note = item.Note,
                            RouteId = item.RouteId,
                            IsPasserBy = item.IsPasserBy,
                            PrescribedQuantity = item.PrescribedQuantity,
                            SpecialDrugId = item.SpecialDrugId,
                            PrescriptionId = item.PrescriptionId,
                            DateCreated = item.DateCreated,
                            IsDeleted = item.IsDeleted,
                            IsSynced = item.IsSynced,
                            CreatedIn = prescriptionsDto.CreatedIn,
                            CreatedBy = prescriptionsDto.CreatedBy,
                        };
                        context.MedicationRepository.Add(medication);
                    }

                    if (item.SpecialDrugId == null && item.GeneralDrugId == null)
                    {
                        Medication medication = new Medication()
                        {
                            InteractionId = item.InteractionId,
                            AdditionalDrugTitle = item.AdditionalDrugTitle,
                            AdditionalDrugFormulation = item.AdditionalDrugFormulation,
                            //DosageUnit = item.DosageUnit,
                            PrescribedDosage = item.PrescribedDosage,
                            ItemPerDose = item.ItemPerDose,
                            Frequency = item.Frequency,
                            FrequencyIntervalId = item.FrequencyIntervalId,
                            Duration = item.Duration,
                            TimeUnit = item.TimeUnit,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            PrescribedQuantity = item.PrescribedQuantity,
                            Note = item.Note,
                            IsPasserBy = item.IsPasserBy,
                            RouteId = item.RouteId,
                            PrescriptionId = item.PrescriptionId,
                            DateCreated = item.DateCreated,
                            IsDeleted = item.IsDeleted,
                            IsSynced = item.IsSynced,
                            CreatedIn = prescriptionsDto.CreatedIn,
                            CreatedBy = prescriptionsDto.CreatedBy,
                        };
                        context.MedicationRepository.Add(medication);
                    }
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("CreatePrescriptions", new { key = prescription.InteractionId }, prescriptionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePrescriptions", "PrescriptionController.cs", ex.Message, prescriptionsDto.CreatedIn, prescriptionsDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpPost]
        [Route(RouteConstants.MultiplePrescription)]
        public async Task<IActionResult> AddMorePrescriptions(PrescriptionsDto prescriptionsDto)
        {
            try
            {
                prescriptionsDto.InteractionId = Guid.NewGuid();

                Interaction interaction = new Interaction();
                interaction.Oid = prescriptionsDto.InteractionId;
                interaction.EncounterId = prescriptionsDto.EncounterID;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Prescription, EncounterType.Prescriptions);
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = prescriptionsDto.CreatedBy;
                interaction.CreatedIn = prescriptionsDto.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                foreach (var item in prescriptionsDto.PrescriptionsList)
                {
                    item.InteractionId = Guid.NewGuid();
                    item.PrescriptionId = prescriptionsDto.InteractionId;

                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;
                    item.PrescriptionId = prescriptionsDto.PrescreptionID;
                    item.CreatedBy = prescriptionsDto.CreatedBy;
                    item.CreatedIn = prescriptionsDto.CreatedIn;

                    if (item.GeneralDrugId > 0)
                        context.MedicationRepository.Add(item);

                    if (item.SpecialDrugId > 0)
                    {
                        Medication medication = new Medication()
                        {
                            InteractionId = item.InteractionId,
                            PrescribedDosage = item.PrescribedDosage,
                            ItemPerDose = item.ItemPerDose,
                            Frequency = item.Frequency,
                            FrequencyIntervalId = item.FrequencyIntervalId,
                            Duration = item.Duration,
                            TimeUnit = item.TimeUnit,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Note = item.Note,
                            RouteId = item.RouteId,
                            IsPasserBy = item.IsPasserBy,
                            PrescribedQuantity = item.PrescribedQuantity,
                            SpecialDrugId = item.SpecialDrugId,
                            PrescriptionId = prescriptionsDto.PrescreptionID,
                            DateCreated = item.DateCreated,
                            IsDeleted = item.IsDeleted,
                            IsSynced = item.IsSynced,
                            CreatedIn = prescriptionsDto.CreatedIn,
                            CreatedBy = prescriptionsDto.CreatedBy,
                        };
                        context.MedicationRepository.Add(medication);
                    }

                    if (item.SpecialDrugId == null && item.GeneralDrugId == null)
                    {
                        Medication medication = new Medication()
                        {
                            InteractionId = item.InteractionId,
                            AdditionalDrugTitle = item.AdditionalDrugTitle,
                            AdditionalDrugFormulation = item.AdditionalDrugFormulation,
                            PrescribedDosage = item.PrescribedDosage,
                            ItemPerDose = item.ItemPerDose,
                            Frequency = item.Frequency,
                            FrequencyIntervalId = item.FrequencyIntervalId,
                            Duration = item.Duration,
                            TimeUnit = item.TimeUnit,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            PrescribedQuantity = item.PrescribedQuantity,
                            Note = item.Note,
                            IsPasserBy = item.IsPasserBy,
                            RouteId = item.RouteId,
                            PrescriptionId = item.PrescriptionId,
                            DateCreated = item.DateCreated,
                            IsDeleted = item.IsDeleted,
                            IsSynced = item.IsSynced,
                            CreatedIn = prescriptionsDto.CreatedIn,
                            CreatedBy = prescriptionsDto.CreatedBy
                        };

                        context.MedicationRepository.Add(medication);
                    }
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("CreatePrescriptions", new { key = prescriptionsDto.PrescreptionID }, prescriptionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "AddMorePrescriptions", "PrescriptionController.cs", ex.Message, prescriptionsDto.CreatedIn, prescriptionsDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpPost]
        [Route(RouteConstants.CreateDispance)]
        public async Task<IActionResult> CreateDispance(PrescriptionsDto prescriptionsDto)
        {
            try
            {
                var prescription = await context.PrescriptionRepository.FirstOrDefaultAsync(x => x.InteractionId == prescriptionsDto.PrescriptionsList.Select(x => x.PrescriptionId).FirstOrDefault());
                if (prescription == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);


                foreach (var item in prescriptionsDto.PrescriptionsList)
                {
                    var medication = await context.MedicationRepository.FirstOrDefaultAsync(x => x.InteractionId == item.InteractionId);
                    medication.ModifiedBy = prescriptionsDto.ModifiedBy;
                    medication.ModifiedIn = prescriptionsDto.ModifiedIn;
                    medication.DateModified = DateTime.Now;
                    medication.IsDispencedPasserBy = item.IsDispencedPasserBy;
                    medication.DispensedDrugsDosage = item.PrescribedDosage;
                    medication.DispensedDrugsItemPerDose = item.ItemPerDose;
                    medication.DispensedDrugDuration = item.Duration;
                    medication.DispensedDrugsTimeUnit = item.TimeUnit;
                    if ((item?.StartDate != null&&item?.StartDate!= DateTime.MinValue) || (item?.EndDate != null &&item?.EndDate != DateTime.MinValue)) 
                    {
                        medication.DispensedDrugsStartDate = item?.StartDate;
                        medication.DispensedDrugsEndDate = item?.EndDate;
                    }
                    else
                    {
                        double days = (medication.EndDate - medication.StartDate).TotalDays;
                        medication.DispensedDrugsStartDate = DateTime.Now;
                        medication.DispensedDrugsEndDate = DateTime.Now.AddDays(days);
                    }
                    medication.DispensedDrugsRouteId = item.RouteId;
                    medication.DispensedDrugsBrand = item.DispensedDrugsBrand;
                    medication.DispensedDrugsFormulation = item.AdditionalDrugFormulation;
                    medication.DispensedDrugTitle = item.DispensedDrugTitle;
                    medication.DispensedDrugsFrequency = item.DispensedDrugsFrequency;
                    medication.DispensedDrugsFrequencyIntervalId = item.FrequencyIntervalId;
                    if (medication.DispensedDrugsQuantity == null)
                        medication.DispensedDrugsQuantity = item.DispensedDrugsQuantity;
                    else
                        medication.DispensedDrugsQuantity += item.DispensedDrugDuration;

                    medication.DateCreated = item.DateCreated;
                    medication.IsDeleted = false;
                    medication.IsSynced = false;


                    if (item.SpecialDrugId != null && item.SpecialDrugId > 0)
                    {
                        DateTime NextAptDate = item.EndDate;
                        if (item.NextAppointmentDate != null)
                        {
                            NextAptDate = item.NextAppointmentDate.Value;
                        }
                        DrugPickUpSchedule drugPickupSchedule = new DrugPickUpSchedule()
                        {
                            DrugPickUpDate = NextAptDate,
                            ClientId = prescriptionsDto.ClientID,
                            EncounterId = prescriptionsDto.EncounterID,
                            DateCreated = DateTime.Now,
                            InteractionId = new Guid(),
                            IsDeleted = false,
                            IsSynced = false
                        };

                        context.DrugPickupScheduleRepository.Add(drugPickupSchedule);

                    }
                    medication.ReasonForReplacement = item.ReasonForReplacement;
                    medication.DispensedDrugsBrand = item.DispensedDrugsBrand;
                    medication.DispensedDrugsQuantity = item.DispensedDrugsQuantity;

                    context.MedicationRepository.Update(medication);
                }
                prescription.DispensationDate = DateTime.Now;
                context.PrescriptionRepository.Update(prescription);
                await context.SaveChangesAsync();

                return CreatedAtAction("CreatePrescriptions", new { key = prescription.InteractionId }, prescriptionsDto);


            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDispance", "PrescriptionController.cs", ex.Message, prescriptionsDto.CreatedIn, prescriptionsDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prescriptions
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrescriptions)]
        public async Task<IActionResult> ReadPrescriptions()
        {
            try
            {
                var prescriptionInDb = await context.PrescriptionRepository.GetPrescription();

                return Ok(prescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrescriptions", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prescription/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Prescription.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrescriptionByKey)]
        public async Task<IActionResult> ReadPrescriptionByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var prescriptionInDb = await context.PrescriptionRepository.GetPrescriptionByKey(key);

                if (prescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(prescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrescriptionByKey", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prescription/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Prescription.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrescriptionByClientId)]
        public async Task<IActionResult> ReadPrescriptionByClientId(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                if (pageSize == 0)
                {

                    var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByClient(clientId);


                    return Ok(generalMedicationsInDb);
                }
                else
                {
                    var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByClient(clientId, ((page - 1) * (pageSize)), pageSize);

                    PagedResultDto<MedicationDto> medicationDto = new PagedResultDto<MedicationDto>()
                    {
                        Data = generalMedicationsInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.MedicationRepository.GetGeneralMedicationByClientTotalCount(clientId)
                    };

                    return Ok(medicationDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrescriptionByClientId", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/Latest-prescription/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadLatestPrescriptionByClientId)]
        public async Task<IActionResult> ReadLatestPrescriptionByClientId(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                List<MedicationDto> generalMedicationsDto = new List<MedicationDto>();

                var generalMedicationsInDb = await context.MedicationRepository.GetLatestGeneralMedicationByClient(clientId);


                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadLatestPrescriptionByClientId", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/prescription-for-dispense/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Prescription.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrescriptionForDispenseByClientId)]
        public async Task<IActionResult> ReadPrescriptionForDispenseByClientId(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var generalMedicationsInDb = await context.MedicationRepository.GetGeneralMedicationByClient(clientId);

                return Ok(generalMedicationsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrescriptionForDispenseByClientId", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/prescriptions/by-date
        /// </summary>
        /// <param>Primary key of the table Prescription.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPrescriptionsByDate)]
        public async Task<IActionResult> ReadPrescriptionsByDate()
        {
            try
            {
                var prescriptionInDb = await context.PrescriptionRepository.GetPrescriptionsByToday();

                if (prescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(prescriptionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPrescriptionsByDate", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        [HttpGet]
        [Route(RouteConstants.ReadPharmacyDashBoard)]
        public async Task<IActionResult> ReadPharmacyDashBoard(int FacilityId, int page, int pageSize, string? PatientName = "", string? PreCriptionDateSearch = "")
        {
            try
            {
                var prescriptionInDb = await context.PrescriptionRepository.GetPharmacyDashBoard(FacilityId, ((page - 1) * (pageSize)), pageSize, PatientName, PreCriptionDateSearch);

                if (prescriptionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                int totalItems = context.PrescriptionRepository.GetPharmacyDashBoardTotalCount(FacilityId, PatientName, PreCriptionDateSearch);
                int dispensationTotalItems = context.PrescriptionRepository.GetPharmacyDashBoardDispensationTotalCount(FacilityId, PatientName, PreCriptionDateSearch);

                var pharmacyDashBoardDto = new PharmacyDashBoardDto
                {
                    prescriptions = prescriptionInDb.ToList(),
                    TotalItems = totalItems,
                    DispensationTotalItems = dispensationTotalItems,
                    PageNumber = page,
                    PageSize = pageSize
                };
                return Ok(pharmacyDashBoardDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPharmacyDashBoard", "PrescriptionController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/prescription/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Prescription.</param>
        /// <param name="Prescription">Prescription to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePrescription)]
        public async Task<IActionResult> UpdatePrescription(Guid key, Prescription prescription)
        {
            try
            {
                if (key != prescription.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var prescriptionInDb = await context.PrescriptionRepository.GetPrescriptionByKey(prescription.InteractionId);

                prescriptionInDb.DispensationDate = prescription.DispensationDate;
                prescriptionInDb.ClientId = prescription.ClientId;

                prescriptionInDb.IsSynced = false;
                prescriptionInDb.IsDeleted = false;

                context.PrescriptionRepository.Update(prescriptionInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePrescription", "PrescriptionController.cs", ex.Message, prescription.ModifiedIn, prescription.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}