using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the FetalHeartRates of Partograph.
   /// </summary>
   public class FetalHeartRate : BaseModel
   {
      /// <summary>
      /// Primary key of the table FetalHeartRates.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Fetal Rate description of the baby.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fetal Rate")]
      public int FetalRate { get; set; }

      /// <summary>
      /// Fatal rate measurement time of the baby.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fetal Rate Time")]
      public long FetalRateTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the table Partographs.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}