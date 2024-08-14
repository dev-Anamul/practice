using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Md Sayem
 * Last modified: 25.03.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Prescription entity.
   /// </summary>
   public class Prescription : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table Prescription.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Date of the Prescription.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Prescription Date")]
      public DateTime PrescriptionDate { get; set; }

      /// <summary>
      ///Check Prescription Is Dispansed or not.
      /// </summary>
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Dispensation Date")]
      public DateTime? DispensationDate { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.ClientArea)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// General Medications of Prescription.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Medication> Medications { get; set; }

      /// <summary>
      /// Dispenses of Prescription.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Dispense> Dispenses { get; set; }
   }
}