using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TBFinding entity.
   /// </summary>
   public class TBFinding : BaseModel
   {
      /// <summary>
      /// Primary Key of the table TBFindings.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the TBFinding.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// IdentifiedTBFinding of the TBFinding.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedTBFinding> IdentifiedTBFindings { get; set; }
   }
}