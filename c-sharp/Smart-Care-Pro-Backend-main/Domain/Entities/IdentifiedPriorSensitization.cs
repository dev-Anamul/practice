using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedPriorSensitization entity.
   /// </summary>
   public class IdentifiedPriorSensitization : EncounterBaseModel
   {
      // <summary>
      /// Primary key of the table IdentifiedPriorSensitization.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table BloodTransfusionHistories.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid BloodTransfusionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("BloodTransfusionId")]
      [JsonIgnore]
      public virtual BloodTransfusionHistory BloodTransfusionHistory { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PriorSensitizations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PriorSensitizationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PriorSensitizationId")]
      [JsonIgnore]
      public virtual PriorSensitization PriorSensitization { get; set; }
   }
}