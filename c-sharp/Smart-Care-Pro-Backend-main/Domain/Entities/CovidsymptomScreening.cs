using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 18.02.2023
* Modified by  : 
* Last modified: 
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// CovidsymptomScreening entity.
   /// </summary>
   public class CovidSymptomScreening : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table CovidSymptomScreening.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      // <summary>
      /// Foreign Key. Primary key of the table CovidSymptom.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int CovidSymptomId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CovidSymptomId")]
      [JsonIgnore]
      public virtual CovidSymptom CovidSymptom { get; set; }

      // <summary>
      /// Foreign Key. Primary key of the table Covid.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid CovidId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CovidId")]
      [JsonIgnore]
      public virtual Covid Covid { get; set; }
   }
}