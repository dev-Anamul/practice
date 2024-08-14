using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PriorSensitization entity.
   /// </summary>
   public class PriorSensitization : BaseModel
   {
      /// <summary>
      /// Primary key of the table PriorSensitizations.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description for the table PriorSensitizations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// IdentifiedPriorSensitization of the PriorSensitization.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPriorSensitization> IdentifiedPriorSensitizations { get; set; }
   }
}