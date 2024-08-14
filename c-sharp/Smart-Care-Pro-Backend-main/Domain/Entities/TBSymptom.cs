using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 24.12.2022
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TBSymptom entity.
   /// </summary>
   public class TBSymptom : BaseModel
   {
      /// <summary>
      /// Primary key of the table TBSymptoms.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// TB symptom of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "TB Symptom")]
      public string Description { get; set; }

      /// <summary>
      /// Identified TB symptoms of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedTBSymptom> IdentifiedTBSymptoms { get; set; }
   }
}