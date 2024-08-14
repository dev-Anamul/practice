using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TBSuspectingReason entity.
   /// </summary>
   public class TBSuspectingReason : BaseModel
   {
      /// <summary>
      /// Primary Key of the table TBSuspectingReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of TB suspecting reason.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// IdentifiedReason of TBSuspectingReason.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedReason> IdentifiedReasons { get; set; }
   }
}