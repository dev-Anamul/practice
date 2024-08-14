using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 28.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// DrugDistribution entity.
    /// </summary>
    public class DispensedItem : BaseModel
    {
        /// <summary>
        /// Primary Key of the table DispensedItem.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Reason for drug replacement.
        /// </summary>
        [StringLength(50)]
        public string ReasonForReplacement { get; set; }

        /// <summary>
        /// Modified Dosage of DispensedItem.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ModifiedDosage { get; set; }

        /// <summary>
        /// Item Per Dose.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ItemPerDose { get; set; }

        /// <summary>
        /// Frequency of Dose.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Frequency { get; set; }

        /// <summary>
        /// Is Client's Dispenced as Passer By
        /// </summary>
        public bool? IsDispencedPasserBy { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table FrequencyInterval.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.FrequencyInterval)]
        public int? FrequencyIntervalId { get; set; }

        /// <summary>
        /// Duration of taking Dose.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int? Duration { get; set; }

        /// <summary>
        /// Time Unit of taking Dose.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public TimeUnit? TimeUnit { get; set; }

        /// <summary>
        /// StartDate of Medication.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// EndDate of Medication.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// NumberOfUnitDispensed of the Dispense.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int NumberOfUnitDispensed { get; set; }

        /// <summary>
        /// Note of Medication.
        /// </summary>
        [StringLength(500)]
        public string Note { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Dispense.
        /// </summary>
        public Guid DispenseId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DispenseId")]
        [JsonIgnore]
        public virtual Dispense Dispense { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table GeneralMedication.
        /// </summary>
        public Guid? GeneralMedicationId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("GeneralMedicationId")]
        [JsonIgnore]
        public virtual Medication GeneralMedication { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table MedicineBrand.
        /// </summary>
        public int MedicineBrandId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("MedicineBrandId")]
        [JsonIgnore]
        public virtual MedicineBrand MedicineBrand { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DrugDefinition.
        /// </summary>
        public int? DrugDefinitionId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DrugDefinitionId")]
        [JsonIgnore]
        public virtual GeneralDrugDefinition DrugDefinition { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DrugRoute.
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.DrugRoute)]
        public int? RouteId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("RouteId")]
        [JsonIgnore]
        public virtual DrugRoute DrugRoute { get; set; }
    }
}