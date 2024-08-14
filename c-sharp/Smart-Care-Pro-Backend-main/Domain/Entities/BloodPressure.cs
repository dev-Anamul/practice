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
   /// Contains details of BloodPressure.
   /// </summary>
   public class BloodPressure : BaseModel
   {

      /// <summary>
      /// Primary key of BloodPressure table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// SystolicPressure of the Patient.
      /// </summary>
      [Required(ErrorMessage = "The Systolic Pressure is required!")]
      [Display(Name = "Systolic Pressure")]
      public int SystolicPressure { get; set; }

      /// <summary>
      /// DiastolicPressure of the Patient.
      /// </summary>
      [Required(ErrorMessage = "The Diastolic Pressure is required!")]
      [Display(Name = "Diastolic Pressure")]
      public int DiastolicPressure { get; set; }

      /// <summary>
      /// BloodPressure measurement time of the patients.
      /// </summary>
      [Required(ErrorMessage = "The Time is required!")]
      [Display(Name = "Time")]
      public long BloodPressureTime { get; set; }

      /// <summary>
      /// Foreign key, Primary key of the Partograph table.
      /// </summary>
      public Guid PartographId { get; set; }

      [ForeignKey("PartographId")]
      [JsonIgnore]
      public virtual Partograph Partograph { get; set; }
   }
}