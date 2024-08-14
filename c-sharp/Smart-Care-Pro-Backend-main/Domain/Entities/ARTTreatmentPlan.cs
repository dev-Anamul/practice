using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 15.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ARTTreatmentPlan entity.
   /// </summary>
   public class ARTTreatmentPlan : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ARTTreatmentPlans.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// ART plans of Client
      /// </summary>
      [Display(Name = "Art Plan")]
      public ArtPlan? ArtPlan { get; set; }

      /// <summary>
      /// CTX Plan of Client
      /// </summary>
      [Display(Name = "TPT Plan")]
      public TPTPlan? TPTPlan { get; set; }

      /// <summary>
      /// CTX Plan of Client
      /// </summary>
      [Display(Name = "CTX Plan")]
      public CTXPlan? CTXPlan { get; set; }

      /// <summary>
      /// EAC Plan of Client
      /// </summary>
      [Display(Name = "EAC Plan")]
      public EACPlan? EACPlan { get; set; }

      /// <summary>
      /// DSD Plan of Client
      /// </summary>
      [Display(Name = "DSD Plan")]
      public DSDPlan? DSDPlan { get; set; }

      /// <summary>
      /// Fluconazole Plan of Client
      /// </summary>
      [Display(Name = "Fluconazole Plan")]
      public FluconazolePlan? FluconazolePlan { get; set; }

      /// <summary>
      /// TPT Eligible Today of Client
      /// </summary>
      public bool TPTEligibleToday { get; set; }

      /// <summary>
      /// Have TPT Provided Today of Client
      /// </summary>
      [Display(Name = "Have TPT Provided Today")]
      public YesNo? HaveTPTProvidedToday { get; set; }

      /// <summary>
      /// Have TPT Provided Today of Client
      /// </summary>
      [Display(Name = "If NO please state WHY")]
      [StringLength(300)]
      public string TPTNote { get; set; }

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
   }
}