using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 24.12.2022
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// SystemReview entity.
   /// </summary>
   public class SystemReview : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table SystemReviews.
      /// </summary
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Note of the review of system.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      public string Note { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table PhysicalSystems.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PhysicalSystemId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PhysicalSystemId")]
      [JsonIgnore]
      public virtual PhysicalSystem PhysicalSystem { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// List of the review of system.
      /// </summary>
      [NotMapped]
      public List<SystemReview> SystemReviews { get; set; }
   }
}