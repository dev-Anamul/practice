using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 27.04.2023
 * Modified by  : Stephan
 * Last modified: 03.05.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// VisitPurpose entity.
   /// </summary>
   public class VisitPurpose : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table VisitPurposes table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Purposes of Visit.
      /// </summary>
      [Display(Name = "Visit Purposes")]
      public VisitPurposes? VisitPurposes { get; set; }

      /// <summary>
      /// Purposes of Visit.
      /// </summary>
      [Display(Name = "Reason for visit")]
      public ReasonForVisit? ReasonForVisit { get; set; }

      /// <summary>
      /// Pregnancy intension of Visit Purposes.
      /// </summary>
      [Display(Name = "Pregnancy intension")]
      public PregnancyIntension? PregnancyIntension { get; set; }

      /// <summary>
      /// Reason for followup of Visit Purposes.
      /// </summary>
      [Display(Name = "Reason for followup")]
      public ReasonForFollowUp? ReasonForFollowUp { get; set; }

      /// <summary>
      /// Other reason for followup of Visit Purposes.
      /// </summary>
      [Display(Name = "Other reason for followup")]
      [StringLength(90)]
      public string OtherReasonForFollowUp { get; set; }

      /// <summary>
      /// Reason for stopping of Visit Purposes.
      /// </summary>
      [Display(Name = "Reason for stopping")]
      public ReasonForStopping? ReasonForStopping { get; set; }

      /// <summary>
      /// Other reason for stopping of Visit Purposes.
      /// </summary>
      [Display(Name = "Other reason for stopping")]
      [StringLength(90)]
      public string OtherReasonForStopping { get; set; }

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