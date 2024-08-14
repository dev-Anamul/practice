using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   /// IdentifiedPPHTreatments entity.
   /// </summary>
   public class IdentifiedPPHTreatment : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedPPHTreatments.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Treatment for PPH of table IdentifiedPPHTreatments.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Treatment for PPH")]
      public TreatmentsOfPPH TreatmentsOfPPH { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PPHTreatments.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid PPHTreatmentsId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PPHTreatmentsId")]
      [JsonIgnore]
      public virtual PPHTreatment PPHTreatment { get; set; }
   }
}