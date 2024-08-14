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
   /// Contains details of Cervix.
   /// </summary>
   public class Cervix : BaseModel
   {
      /// <summary>
      /// Primary key of Cervix table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Cervix Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The CervixDetails is required!")]
      [Display(Name = "Cervix Details")]
      public int CervixDetails { get; set; }

      /// <summary>
      /// Cervix measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The CervixTime is required!")]
      [Display(Name = "Cervix Time")]
      public long CervixTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}