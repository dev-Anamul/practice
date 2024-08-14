using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : Lion
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of DescentOfHead.
   /// </summary>
   public class DescentOfHead : BaseModel
   {
      /// <summary>
      /// Primary key of DescentOfHead table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// DescentOfHead Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The DescentOfHead is required!")]
      [StringLength(30)]
      [Display(Name = "Descent Of Head")]
      public int DescentOfHeadDetails { get; set; }

      /// <summary>
      /// Cervix measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The DescentOfHead is required!")]
      [Display(Name = "Descent Of Head Time")]
      public long DescentOfHeadTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
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