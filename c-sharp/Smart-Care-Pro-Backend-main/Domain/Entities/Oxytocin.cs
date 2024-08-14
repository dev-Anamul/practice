using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of Oxytocin.
   /// </summary>
   public class Oxytocin : BaseModel
   {
      /// <summary>
      /// Primary key of Oxytocin table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Oxytocin Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The OxytocinDetails is required!")]
      [Display(Name = "Oxytocin Details")]
      public int OxytocinDetails { get; set; }

      /// <summary>
      /// Oxytocin measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The OxytocinTime is required!")]
      [Display(Name = "Oxytocin Time")]
      public long OxytocinTime { get; set; }

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