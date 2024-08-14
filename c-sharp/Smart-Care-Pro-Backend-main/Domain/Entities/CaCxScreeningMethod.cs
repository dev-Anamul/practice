using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// CaCxScreeningMethod entity.
   /// </summary>
   public class CaCxScreeningMethod : BaseModel
   {
      /// <summary>
      /// Primary key of the table CaCxScreeningMethods.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the screening method.
      /// </summary>
      [Required]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Obstetrics & Gynecology histories of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<GynObsHistory> GynObsHistories { get; set; }
   }
}