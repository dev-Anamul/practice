using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   public class CovidComorbidity : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table CovidComorbidity.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Source of Alert of the Covid Comorbidity Condition.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Covid Comorbidity Conditions")]
      public CovidComorbidityCondition CovidComorbidityConditions { get; set; }

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