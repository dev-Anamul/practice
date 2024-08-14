using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Represents FamilyPlan entity in the database.
   /// </summary>
   public class FamilyPlan : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table FamilyPlans.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Any sexual violence symptoms of the table FamilyPlans.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Any sexual violence symptoms")]
      public YesNo? AnySexualViolenceSymptoms { get; set; }

      /// <summary>
      /// The field indicates whether the client is pregnant or not.
      /// </summary>
      [Display(Name = "Client is not pregnant")]
      public YesNo? ClientIsNotPregnant { get; set; }

      /// <summary>
      /// The field stores the reason of not pregnant.
      /// </summary>
      [Display(Name = "Reason of not pregnant")]
      public ReasonOfNotPregnant? ReasonOfNotPregnant { get; set; }

      /// <summary>
      /// The field indicates has consent for FP.
      /// </summary>
      [Display(Name = "Has consent for FP")]
      public YesNo? HasConsentForFP { get; set; }

      /// <summary>
      /// FP method plan of the table FamilyPlans.
      /// </summary>
      [Display(Name = "FP method plan")]
      public FPMethodPlan? FPMethodPlan { get; set; }

      /// <summary>
      /// FP method plan request of the table FamilyPlans.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "FP method plan request")]
      public FPMethodPlanRequest? FPMethodPlanRequest { get; set; }

      /// <summary>
      /// he field indicates the Client Preferences.
      /// </summary>
      [Display(Name = "Client Preferences based on")]
      public ClientPreferences? ClientPreferences { get; set; }

      /// <summary>
      /// The field indicates selected family plan.
      /// </summary>
      [Display(Name = "Selected family plan")]
      public SelectedFamilyPlan? SelectedFamilyPlan { get; set; }

      /// <summary>
      /// Classify FP method of the table FamilyPlans.
      /// </summary>
      [Display(Name = "Classify FP method")]
      public ClassifyFPMethod? ClassifyFPMethod { get; set; }

      /// <summary>
      /// The field indicates family plans of the table FamilyPlans.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Family plans")]
      public FamilyPlans? FamilyPlans { get; set; }

      /// <summary>
      /// The field indicates the FP provided place.
      /// </summary>
      [Display(Name = "FP provided place")]
      public FPProvidedPlace? FPProvidedPlace { get; set; }

      /// <summary>
      /// The field indicates the reason for no plan.
      /// </summary>
      [Display(Name = "Reason for No plan")]
      public ReasonForNoPlan? ReasonForNoPlan { get; set; }

      /// <summary>
      /// The field indicates is client receive preferred options or not.
      /// </summary>
      [Display(Name = "Client receive preferred options")]
      public YesNo? ClientReceivePreferredOptions { get; set; }

      /// <summary>
      /// The field indicates client not receive preferred option.
      /// </summary>
      [Display(Name = "Client not receive preferred option")]
      public ClientNotReceivePreferredOption? ClientNotReceivePreferredOptions { get; set; }

      /// <summary>
      /// The field indicates backup method used of FamilyPlans.
      /// </summary>
      [Display(Name = "Backup method used")]
      public BackupMethodUsed? BackupMethodUsed { get; set; }

      /// <summary>
      /// The field indicates whether HIV testing needed or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is HIV testing need")]
      public bool IsHIVTestingNeed { get; set; }

      /// <summary>
      /// The field indicates whether STI or not.
      /// </summary>
      [Display(Name = "Is STI")]
      public bool IsSTI { get; set; }

      /// <summary>
      /// The field indicates whether cervical cancer or not.
      /// </summary>
      [Display(Name = "Is cervical cancer")]
      public bool IsCervicalCancer { get; set; }

      /// <summary>
      /// The field indicates whether breast cancer or not.
      /// </summary>
      [Display(Name = "Is breast cancer")]
      public bool IsBreastCancer { get; set; }

      /// <summary>
      /// The field indicates whether prostate cancer or not.
      /// </summary>
      [Display(Name = "Is prostate cancer")]
      public bool IsProstateCancer { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table FamilyPlanningSubClass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int SubclassId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SubclassId")]
      [JsonIgnore]
      public virtual FamilyPlanningSubclass FamilyPlanningSubclass { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}