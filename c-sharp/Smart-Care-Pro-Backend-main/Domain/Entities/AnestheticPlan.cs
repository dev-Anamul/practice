using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// AnestheticPlan entity.
   /// </summary>
   public class AnestheticPlan : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table AnestheticPlans.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// History of the client.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Client history")]
      public string ClientHistory { get; set; }

      /// <summary>
      /// Examination of the client.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Client examination")]
      public string ClientExamination { get; set; }

      /// <summary>
      /// Anesthetic plans of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      [Display(Name = "Anesthetic plans")]
      public string AnestheticPlans { get; set; }

      /// <summary>
      /// Instructions for the patient.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Patient instructions")]
      public string PatientInstructions { get; set; }

      /// <summary>
      /// Anesthesia start time of the client.
      /// </summary>
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Anesthesia start time")]
      public TimeSpan? AnesthesiaStartTime { get; set; }

      /// <summary>
      /// Anesthesia end time of the client.
      /// </summary>
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Anesthesia end time")]
      public TimeSpan? AnesthesiaEndTime { get; set; }

      /// <summary>
      /// Post anesthesia of the client.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Post anesthesia")]
      public string PostAnesthesia { get; set; }

      /// <summary>
      /// Pre operative adverse of the client.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Pre operative adverse")]
      public string PreOperativeAdverse { get; set; }

      /// <summary>
      /// Post operative of the client.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Post operative")]
      public string PostOperative { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Surgeries.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid SurgeryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SurgeryId")]
      [JsonIgnore]
      public virtual Surgery Surgery { get; set; }

      /// <summary>
      /// Skin preparations of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SkinPreparation> SkinPreparations { get; set; }
   }
}