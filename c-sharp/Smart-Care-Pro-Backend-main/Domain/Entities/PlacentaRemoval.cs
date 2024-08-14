using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PlacentaRemoval entity.
   /// </summary>
   public class PlacentaRemoval : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PlacentaRemovals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Is placenta removal completed or not.
      /// </summary>
      [Display(Name = "Is placenta removal completed")]
      public bool IsPlacentaRemovalCompleted { get; set; }

      /// <summary>
      /// Other Information for PlacentaRemovals.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PlacentaRemovals.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeliveryId")]
      [JsonIgnore]
      public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }

      /// <summary>
      /// IdentifiedPlacentaRemoval of the PlacentaRemoval.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPlacentaRemoval> IdentifiedPlacentaRemovals { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public Placenta[] PlacentaList { get; set; }
   }
}