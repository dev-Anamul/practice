using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 05.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DrugPickupSchedule entity.
   /// </summary>
   public class DrugPickUpSchedule : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table Prescription.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Date of the DrugPickupSchedule.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "DrugPickup Date")]
      public DateTime DrugPickUpDate { get; set; }

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
   }
}