using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// NeonatalBirthOutcomes entity.
   /// </summary>
   public class NeonatalBirthOutcome : BaseModel
   {
      /// <summary>
      /// Primary key of the table NeonatalBirthOutcome.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table NeonatalBirthOutcome.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// NewBornDetail of the NeonatalBirthOutcome.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NewBornDetail> NewBornDetails { get; set; }
   }
}