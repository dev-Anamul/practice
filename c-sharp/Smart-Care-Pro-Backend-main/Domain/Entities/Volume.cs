using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Merry
 * Date created : 12.02.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of Volumes.
   /// </summary>
   public class Volume : BaseModel
   {
      /// <summary>
      /// Primary key of Acetones table.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Volumes Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The VolumesDetails is required!")]
      [StringLength(50)]
      [Display(Name = "Volumes Details")]
      public string VolumesDetails { get; set; }

      /// <summary>
      /// Volumes measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The Time is required!")]
      [Display(Name = "Acetones Time")]
      public long VolumesTime { get; set; }

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