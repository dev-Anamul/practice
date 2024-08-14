using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PrEPStoppingReason entity.
   /// </summary>
   public class StoppingReason : BaseModel
   {
      /// <summary>
      /// Primary key of the table PrEPStoppingReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Reason for stopping PrEP.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(900)]
      [Display(Name = "Stopping Reason")]
      public string Description { get; set; }

      ///// <summary>
      ///// Plans of the client.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Plan> Plans { get; set; }
   }
}