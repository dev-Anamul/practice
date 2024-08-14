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
   /// CauseOfNeonatalDeaths entity.
   /// </summary>
   public class CauseOfNeonatalDeath : BaseModel
   {
      /// <summary>
      /// Primary key of the table CauseOfNeonatalDeath.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the table CauseOfNeonatalDeath.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// NeonatalDeath of the CauseOfNeonatalDeath.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NeonatalDeath> NeonatalDeaths { get; set; }
   }
}