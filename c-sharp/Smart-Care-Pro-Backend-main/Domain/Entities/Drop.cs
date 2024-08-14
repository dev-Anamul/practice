using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of Drops.
   /// </summary>
   public class Drop : BaseModel
   {
      /// <summary>
      /// Primary key of Drops table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Drops Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The DropsDetails is required!")]
      [Display(Name = "Drops Details")]
      public int DropsDetails { get; set; }

      /// <summary>
      /// Drops measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The DropsTime is required!")]
      [Display(Name = "Drops Time")]
      public long DropsTime { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}