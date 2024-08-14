using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 27.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedCordStumps entity.
   /// </summary>
   public class IdentifiedCordStump : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedEyesAssesments.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Cord stump condition of the table IdentifiedCordStumps.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Cord stump condition")]
      public CordStumpCondition CordStumpCondition { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Assessments.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid AssessmentId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AssessmentId")]
      [JsonIgnore]
      public virtual Assessment Assessment { get; set; }
   }
}