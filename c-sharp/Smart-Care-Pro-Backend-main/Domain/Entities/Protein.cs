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
   /// Contains details of Proteins.
   /// </summary>
   public class Protein : BaseModel
   {
      /// <summary>
      /// Primary key of Proteins table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Proteins Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The ProteinsDetails is required!")]
      [StringLength(50)]
      [Display(Name = "Proteins Details")]
      public string ProteinsDetails { get; set; }

      /// <summary>
      /// Proteins measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The Time is required!")]
      [Display(Name = "Cervix Time")]
      public long ProteinsTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}