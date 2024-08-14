using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 27.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Dispense entity.
   /// </summary>
   public class Dispense : BaseModel
   {
      /// <summary>
      /// Primary key of the table Dispense.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Dispense Date of the Dispense.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Dispense Date")]
      public DateTime DispenseDate { get; set; }

      /// <summary>
      /// NextAppointmentDate of the Dispense.
      /// </summary>
      //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Next Appointment Date")]
      public DateTime? NextAppointmentDate { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Prescription.
      /// </summary>
      public Guid PrescriptionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PrescriptionId")]
      [JsonIgnore]
      public virtual Prescription Prescription { get; set; }

      /// <summary>
      /// Drugs Distribution of Dispense.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DispensedItem> DispensedItems { get; set; }
   }
}