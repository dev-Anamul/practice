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
   /// Breech entity.
   /// </summary>
   public class Breech : BaseModel
   {
      /// <summary>
      /// Primary key of the table Breeches.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table Breech.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// NewBornDetail of the Breech.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NewBornDetail> NewBornDetails { get; set; }
   }
}