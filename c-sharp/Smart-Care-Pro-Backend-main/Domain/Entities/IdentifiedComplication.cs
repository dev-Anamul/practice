using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifyComplication entity.
   /// </summary>
   public class IdentifiedComplication : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifyComplications.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Complications.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ComplicationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ComplicationId")]
      [JsonIgnore]
      public virtual Complication Complication { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ComplicationTypes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ComplicationTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ComplicationTypeId")]
      [JsonIgnore]
      public virtual ComplicationType ComplicationType { get; set; }

      /// <summary>
      /// List of the IdentifiedComplication.
      /// </summary>
      [NotMapped]
      public int[] IdentifiedComplicationList { get; set; }
   }
}