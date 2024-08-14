using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 24.12.2022
 * Modified by  : Bella
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedTBSymptom entity.
   /// </summary>
   public class IdentifiedTBSymptom : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedTBSymptoms.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBSymptoms.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TBSymptomId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBSymptomId")]
      [JsonIgnore]
      public virtual TBSymptom TBSymptom { get; set; }

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
      /// List of TB symptom.
      /// </summary>
      [NotMapped]
      public int[] TBSymptomList { get; set; }
   }
}