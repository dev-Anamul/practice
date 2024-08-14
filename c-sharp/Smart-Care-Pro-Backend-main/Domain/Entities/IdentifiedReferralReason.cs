using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedReferralReasons entity.
   /// </summary>
   public class IdentifiedReferralReason : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedReferralReasons.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ReferralModules.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ReferralModuleId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ReferralModuleId")]
      [JsonIgnore]
      public virtual ReferralModule ReferralModule { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ReasonOfReferral.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ReasonOfReferralId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ReasonOfReferralId")]
      [JsonIgnore]
      public virtual ReasonOfReferral ReasonOfReferral { get; set; }
   }
}