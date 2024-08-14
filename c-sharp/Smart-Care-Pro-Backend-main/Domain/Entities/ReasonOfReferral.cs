using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedReferralReasons entity.
   /// </summary>
   public class ReasonOfReferral : BaseModel
   {
      /// <summary>
      /// Primary key of the table ReferralReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table ReferralReasons.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// IdentifiedReferralReason of the ReasonOfReferralReasons.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedReferralReason> IdentifiedReferralReasons { get; set; }
   }
}