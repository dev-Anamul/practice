using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 04.01.2023
 * Modified by  : Tomas
 * Last modified: 17.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ConstitutionalSymptom entity.
   /// </summary>
   public class ConstitutionalSymptom : BaseModel
   {
      /// <summary>
      /// Primary key of the table ConstitutionalSymptoms.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the ConstitutionalSymptom.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Constitutional symptom types of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ConstitutionalSymptomType> ConstitutionalSymptomTypes { get; set; }
   }
}