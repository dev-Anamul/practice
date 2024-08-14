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
   /// PastMedicalConditon entity.
   /// </summary>
   public class PastMedicalCondition : BaseModel
   {
      /// <summary>
      /// Primary Key of the table PastMedicalConditon.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table PastMedicalConditon.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// MedicalConditions of the PastMedicalConditon.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicalCondition> MedicalConditions { get; set; }
   }
}