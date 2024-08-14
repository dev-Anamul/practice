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
   /// CauseOfStillBirth entity.
   /// </summary>
   public class CauseOfStillbirth : BaseModel
   {
      /// <summary>
      /// Primary key of the table CauseOfStillbirths.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table CauseOfStillBirth.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// NewBornDetail of the CauseOfStillBirth.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NewBornDetail> NewBornDetails { get; set; }
   }
}