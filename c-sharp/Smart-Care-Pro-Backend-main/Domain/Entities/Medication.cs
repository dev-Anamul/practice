using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 06.04.23
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// GeneralMedications entity.
    /// </summary>
    public class Medication : BaseModel
    {
        /// <summary>
        /// Primary Key of the table GeneralMedications.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Prescribed Dosage of General Medication.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
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

        /// <summary>
        /// Time Unit of taking Dose.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
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

        /// <summary>
        /// AdditionalDrugTitle of Medication.
        /// </summary>
        [StringLength(90)]
        public string AdditionalDrugTitle { get; set; }

        /// <summary>
        /// AdditionalDrugFormulation of Medication.
        /// </summary>
        [StringLength(90)]
        public string AdditionalDrugFormulation { get; set; }

        /// <summary>
        /// PrescribedQuantity of Medication.
        /// </summary>
        public int PrescribedQuantity { get; set; }

        /// <summary>
        ///Is Client Prescribe as Passer By
        /// </summary>
        public bool? IsPasserBy { get; set; }

        /// <summary>
        /// DispansedDrugTitle of Medication.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Dispansed Drug Title")]
        public string DispensedDrugTitle { get; set; }

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
        public string DispensedDrugsDosage { get; set; }

        /// <summary>
        /// DispensedDrugsItemPerDose of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Item PerDose")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DispensedDrugsItemPerDose { get; set; }

        /// <summary>
        /// DispensedDrugsFrequency of Medication.
        /// </summary>
        [Display(Name = "Dispensed Drugs Frequency")]
        [Column(TypeName = "decimal(18,2)")]
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
        public int? DispensedDrugsRouteId { get; set; }

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
        /// Frequency of Dose.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Frequency { get; set; }

        /// <summary>
        /// Note of Medication.
        /// </summary>
        [StringLength(500)]
        public string Note { get; set; }

        /// <summary>
        /// Is Client's Dispenced as Passer By
        /// </summary>
        public bool? IsDispencedPasserBy {  get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DrugRoute.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.DrugRoute)]
        public int RouteId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("RouteId")]
        [JsonIgnore]
        public virtual DrugRoute DrugRoute { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table GeneralDrugDefinition.
        /// </summary>
        public int? GeneralDrugId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("GeneralDrugId")]
        [JsonIgnore]
        public virtual GeneralDrugDefinition GeneralDrugDefinition { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table SpecialDrug.
        /// </summary>
        public int? SpecialDrugId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("SpecialDrugId")]
        [JsonIgnore]
        public virtual SpecialDrug SpecialDrug { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Prescription.
        /// </summary>
        public Guid PrescriptionId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("PrescriptionId")]
        [JsonIgnore]
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table FrequencyInterval.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.FrequencyInterval)]
        public int? FrequencyIntervalId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("FrequencyIntervalId")]
        [JsonIgnore]
        public virtual FrequencyInterval FrequencyInterval { get; set; }

        /// <summary>
        /// Dispensed Items of Medication.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DispensedItem> DispensedItems { get; set; }

        [NotMapped]
        /// <summary>
        /// DosageUnit of OtherMedication.
        /// </summary>
        public string DosageUnit { get; set; }

        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime? NextAppointmentDate { get; set; }
    }
}