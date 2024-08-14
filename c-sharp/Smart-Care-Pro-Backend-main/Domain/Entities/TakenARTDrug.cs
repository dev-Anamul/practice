using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TakenARTDrug entity.
   /// </summary>
   public class TakenARTDrug : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table PriorARTExposers.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Start Date of the ART Drug.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Start Date")]
      public DateTime StartDate { get; set; }

      /// <summary>
      /// End Date of the ART Drug.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "End Date")]
      public DateTime EndDate { get; set; }

      /// <summary>
      /// Stopping Reason of the ART Drug.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Stopping Reason")]
      public string StoppingReason { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ARTDrugs.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ARTDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ARTDrugId")]
      [JsonIgnore]
      public virtual ARTDrug ARTDrug { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PriorARTExposers.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid PriorExposureId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PriorExposureId")]
      [JsonIgnore]
      public virtual PriorARTExposure PriorARTExposure { get; set; }
   }
}