using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

namespace Domain.Entities
{
   public class IdentifiedPreferredFeeding : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ChiefComplaints.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PreferredFeedings.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PreferredFeedingId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PreferredFeedingId")]
      [JsonIgnore]
      public virtual PreferredFeeding PreferredFeeding { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PreferredFeedings.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}