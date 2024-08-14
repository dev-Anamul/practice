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
   /// Contains details of Moulding.
   /// </summary>
   public class Moulding : BaseModel
   {
      /// <summary>
      /// Primary key of Moulding table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Moulding Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The MouldingDetails is required!")]
      [StringLength(30)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Moulding measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The MouldingTime is required!")]
      [Display(Name = "Moulding Time")]
      public long MouldingTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}