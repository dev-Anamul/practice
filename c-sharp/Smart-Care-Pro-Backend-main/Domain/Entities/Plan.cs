using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Plan entity.
   /// </summary>
   public class Plan : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Plan.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Plans of Plan.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Plans Plans { get; set; }

      /// <summary>
      /// Date of starting Plan.
      /// </summary>
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "PrEP Start Date")]
      public DateTime? StartDate { get; set; }

      /// <summary>
      /// Date of stopping Plan.
      /// </summary>
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "PrEP Stop Date")]
      public DateTime? StopDate { get; set; }

      /// <summary>
      /// Note of Plan.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Note")]
      public string Note { get; set; }

      /// <summary>
      /// Is Urinalysis Normal of the client.
      /// </summary>
      [Display(Name = "Is Urinalysis Normal")]
      public bool IsUrinalysisNormal { get; set; }

      /// <summary>
      /// Has Acute HIV Infection Symptoms of the client.
      /// </summary>
      [Display(Name = "Has Acute HIV Infection Symptoms")]
      public bool HasAcuteHIVInfectionSymptoms { get; set; }

      /// <summary>
      /// Is Able To Adhere Daily PrEP of the client.
      /// </summary>
      [Display(Name = "Is Able To Adhere Daily PrEP")]
      public bool IsAbleToAdhereDailyPrEP { get; set; }

      /// <summary>
      /// Has Greater Fifty Creatinine Clearance of the client.
      /// </summary>
      [Display(Name = "Has Greater Fifty Creatinine Clearance")]
      public bool HasGreaterFiftyCreatinineClearance { get; set; }

      /// <summary>
      /// Is Potential HIV Exposer More Than Six Weeks Old of the client.
      /// </summary>
      [Display(Name = "Is Potential HIV Exposer More Than Six Weeks Old")]
      public bool IsPotentialHIVExposureMoreThanSixWeeksOld { get; set; }

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
      /// Foreign Key. Primary key of the table PrEPStoppingReasons.
      /// </summary>
      public int? StoppingReasonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("StoppingReasonId")]
      [JsonIgnore]
      public virtual StoppingReason? StoppingReason { get; set; }
   }
}