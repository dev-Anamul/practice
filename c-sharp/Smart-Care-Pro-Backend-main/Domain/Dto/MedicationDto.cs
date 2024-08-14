using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Utilities.Constants.Enums;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 16.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class MedicationDto
    {
        /// <summary>
        /// Primary Key of the table Prescription.
        /// </summary>
        [Key]
        public Guid PrescriptionId { get; set; }
        public Guid EncounterId { get; set; }

        /// <summary>
        ///Check Prescription Is Dispansed or not.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Dispensation Date")]
        public DateTime? DispensationDate { get; set; }

        public List<Medication> GeneralMedicationsList { get; set; }
        public List<MedicationListDto> MedicationsList { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Prescription Date")]
        public DateTime PrescriptionDate { get; set; }
    }

    public class MedicationListDto
    {
        public Guid MedicationInteractionId { get; set; }
        /// <summary>
        /// Prescribed Dosage of General Medication.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "decimal(18,2)")]
        public string PrescribedDosage { get; set; }

        /// <summary>
        /// Item Per Dose.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemPerDose { get; set; }

        /// <summary>
        /// Duration of taking Dose.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int Duration { get; set; }

        public TimeUnit TimeUnit { get; set; }

        /// <summary>
        /// StartDate of Medication.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// EndDate of Medication.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        /// </summary>
        [StringLength(90)]
        public string AdditionalDrugTitle { get; set; }

        /// <summary>
        /// AdditionalDrugFormulation of Medication.
        /// </summary>
        [StringLength(90)]
        public string AdditionalDrugFormulation { get; set; }

        /// <summary>
        /// PrescribedQuentity of Medication.
        /// </summary>
        public int PrescribedQuentity { get; set; }

        /// <summary>
        /// DispansedDrugTitle of Medication.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Dispansed Drug Title")]
        public string DispansedDrugTitle { get; set; }

        /// <summary>
        /// DispensedDrugsBrand of Medication.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Dispensed Drugs Brand")]
        public string DispensedDrugsBrand { get; set; }

        /// <summary>
        /// DispensedDrugsBrand of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Formulation ")]
        public string DispensedDrugsFormulation { get; set; }

        /// <summary>
        /// DispansedDrugsDosage of Medication.
        /// </summary>
        [Display(Name = "Dispansed Drugs Dosage")]
        public string DispansedDrugsDosage { get; set; }

        /// <summary>
        /// DispensedDrugsItemPerDose of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Item PerDose")]
        public decimal? DispensedDrugsItemPerDose { get; set; }

        /// <summary>
        /// DispensedDrugsFrequency of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Frequency")]
        public decimal? DispensedDrugsFrequency { get; set; }

        /// <summary>
        /// DispensedDrugDuration of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drug Duration")]
        public int? DispensedDrugDuration { get; set; }

        /// <summary>
        /// DispensedDrugsFrequencyInterval of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Frequency Interval")]
        public int? DispensedDrugsFrequencyIntervalId { get; set; }

        /// <summary>
        /// DispensedDrugsTimeUnit of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Time Unit")]
        public TimeUnit? DispensedDrugsTimeUnit { get; set; }

        /// <summary>
        /// DispensedDrugsRuteId of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Rute")]
        public int? DispensedDrugsRuteId { get; set; }

        /// <summary>
        /// DispensedDrugsQuantity of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Quantity")]
        public int? DispensedDrugsQuantity { get; set; }

        /// <summary>
        /// DispensedDrugsStartDate of Medication.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime? DispensedDrugsStartDate { get; set; }

        /// <summary>
        /// DispensedDrugsStartDate of Medication.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime? DispensedDrugsEndDate { get; set; }

        /// <summary>
        /// ReasonForReplacement of Medication.
        /// </summary>
        [StringLength(200)]
        [Display(Name = "Reason For Replacement")]
        public string ReasonForReplacement { get; set; }

        /// <summary>
        ///Is Client Prescribe as Passer By
        /// </summary>
        public bool? IsPasserBy { get; set; }

        /// <summary>
        /// Frequency of Dose.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Frequency { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table FrequencyInterval.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.FrequencyInterval)]
        public int? FrequencyIntervalId { get; set; }

        /// <summary>
        /// Note of Medication.
        /// </summary>
        [StringLength(500)]
        public string Note { get; set; }

        public string DrugRoute_Route { get; set; }
        public string DispenseDrugRouteRoute { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>

        public string GenericDrugsDescription { get; set; }
        public string GenericDrugsFormulationsDescription { get; set; }
        public string GenericDrugsDrugDosageUnitesDescription { get; set; }

        public string SpecialDrugDescription { get; set; }
        public string SpecialDrugFormulations { get; set; }
        public string SpecialDrugDrugDosageUnitsDescription { get; set; }

        public string FrequencyIntervalTimeInterval { get; set; }

        public string GenericDrug { get; set; }
        public string DosageUnit { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime? NextAppoinmentDate { get; set; }
        /// <summary>
        /// Date of the Prescription.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Prescription Date")]
        public DateTime PrescriptionDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Dispensation Date")]

        /// <summary>
        /// Is Client's Dispenced as Passer By
        /// </summary>
        public bool? IsDispencedPasserBy { get; set; }

        public DateTime? DispensationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public int? CreatedIn { get; set; }
        public string ClinicianName { get; set; }
        public string FacilityName { get; set; }

    }
}