using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 18.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Exposure entity.
   /// </summary>
   public class Exposure : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Exposures.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ExposureTypes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ExposureTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ExposureTypeId")]
      [JsonIgnore]
      public virtual ExposureType ExposureType { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ChiefComplaints.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ChiefComplaintId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ChiefComplaintId")]
      [JsonIgnore]
      public virtual ChiefComplaint ChiefComplaint { get; set; }
   }
}