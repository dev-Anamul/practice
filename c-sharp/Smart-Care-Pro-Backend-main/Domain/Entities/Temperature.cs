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
   /// Contains details of Temperatures.
   /// </summary>
   public class Temperature : BaseModel
   {
      /// <summary>
      /// Primary key of Temperatures table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Temperatures Details of the Patients.
      /// </summary>
      [Required(ErrorMessage = "The TemperaturesDetails is required!")]
      [Display(Name = "Temperatures Details")]
      public int TemperaturesDetails { get; set; }

      /// <summary>
      /// Temperatures measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The Time is required!")]
      [Display(Name = "Temperatures Time")]
      public long TemperatureTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}