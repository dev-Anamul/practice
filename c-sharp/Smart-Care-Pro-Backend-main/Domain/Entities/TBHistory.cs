using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   public class TBHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table TBHistories table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Currently have TB or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Does patient currently have TB?")]
      public CurrentlyHaveTB CurrentlyHaveTB { get; set; }

      /// <summary>
      /// kind of TB.
      /// </summary>
      [Display(Name = "What kind of TB was it?")]
      public KindOfTB? KindOfTB { get; set; }

      /// <summary>
      /// ATT completed status.
      /// </summary>
      [Display(Name = "Was ATT completed?")]
      public WasATTCompleted? WasATTCompleted { get; set; }

      /// <summary>
      /// ATT not completed reason.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "If NO why?")]
      public string ATTNotCompletedReason { get; set; }

      /// <summary>
      /// Is On TB Medication or not.
      /// </summary>
      [Display(Name = "Is On TB Medication")]
      public bool IsOnAntiTBMedication { get; set; }

      /// <summary>
      /// How many month of TB course.
      /// </summary>
      [Display(Name = "Month of TB course")]
      public MonthOfTBCourse MonthOfTBCourse { get; set; }

      /// <summary>
      /// Is on treditional medication or not.
      /// </summary>
      [Display(Name = "Is on traditional medication")]
      public bool IsOnTraditionalMedication { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// UsedTBIdentificationMethods of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<UsedTBIdentificationMethod> UsedTBIdentificationMethods { get; set; }

      /// <summary>
      /// TakenTBDrugs of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TakenTBDrug> TakenTBDrugs { get; set; }

      [NotMapped]
      public int[] TBIdentificationMethodList { get; set; }

      [NotMapped]
      public int[] TBTakenDrugList { get; set; }
   }
}