using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// STIRisk entity.
   /// </summary>
   public class STIRisk : BaseModel
   {
      /// <summary>
      /// Primary Key of the table STIRisk.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table STIRisk.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public string Description { get; set; }

      /// <summary>
      /// MedicalConditions of the STIRisk.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicalCondition> MedicalConditions { get; set; }
   }
}