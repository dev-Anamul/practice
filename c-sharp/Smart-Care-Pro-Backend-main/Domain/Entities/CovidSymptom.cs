using System.ComponentModel.DataAnnotations;
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
   /// CovidSymptom entity.
   /// </summary>
   public class CovidSymptom : BaseModel
   {
      /// <summary>
      /// Primary key of the table CovidSymptom.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the CovidSymptom.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Covid Symptom Screening list
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<CovidSymptomScreening> CovidSymptomScreenings { get; set; }
   }
}