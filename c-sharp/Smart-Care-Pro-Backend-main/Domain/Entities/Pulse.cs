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
   /// Contains details of Pulse.
   /// </summary>
   public class Pulse : BaseModel
   {
      /// <summary>
      /// Primary key of Pulse table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Pulse Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The PulseDetails is required!")]
      [Display(Name = "Pulse Details")]
      public int PulseDetails { get; set; }

      /// <summary>
      /// Pulse measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The Time is required!")]
      [Display(Name = "Pulse Time")]
      public long PulseTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      /// <summary>
      /// Navigate property
      /// </summary>
      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}