using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 15.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// AttachedFacility entity.
   /// </summary>
   public class AttachedFacility : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table AttachedFacilities.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Type Of Facility Entry.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.TypeOfEntry)]
      [Display(Name = "Type of entry")]
      public TypeOfEntry TypeOfEntry { get; set; }

      /// <summary>
      /// Date of Facility Attachment.
      /// </summary>        
      [Column(TypeName = "smalldatetime")]
      public DateTime? DateAttached { get; set; }

      /// <summary>
      /// Current Facility Id.
      /// </summary>
      public int? AttachedFacilityId { get; set; }

      /// <summary>
      /// Last Facility of the client.
      /// </summary>
      public int? SourceFacilityId { get; set; }

      /// <summary>
      /// Name of Source Facility.
      /// </summary>
      [NotMapped]
      public string SourceFacilityName { get; set; }

      /// <summary>
      /// Temp Token Number.
      /// </summary>
      [NotMapped]
      public string TokenNumber { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Client.
      /// </summary>
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AttachedFacilityId")]
      [JsonIgnore]
      public virtual Facility Facility { get; set; }
   }
}