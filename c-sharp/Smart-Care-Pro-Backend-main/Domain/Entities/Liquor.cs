using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// Contains details of Liquor.
   /// </summary>
   public class Liquor : BaseModel
   {
      /// <summary>
      /// Primary key of Liquor table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Liquor Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(30)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Liquor measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Liquor Time")]
      public long LiquorTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}