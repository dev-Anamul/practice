using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 01.01.2023
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TreatmentPlan entity.
   /// </summary>
   public class TreatmentPlan : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table TreatmentPlans.
      /// </summary
      [Key]
      public Guid InteractionId { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Treatment plans")]
      public string TreatmentPlans { get; set; }

      /// <summary>
      /// SurgeryID for Surgery entity
      /// </summary>
      public Guid? SurgeryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SurgeryId")]
      [JsonIgnore]
      public virtual Surgery Surgery { get; set; }

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