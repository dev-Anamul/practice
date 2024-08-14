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
   /// IdentifiedPlacentaRemovals entity.
   /// </summary>
   public class IdentifiedPlacentaRemoval : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedPlacentaRemovals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Placenta of IdentifiedPlacentaRemoval.
      /// </summary>
      public Placenta? Placenta { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table IdentifiedPlacentaRemovals.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid PlacentaRemovalId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PlacentaRemovalId")]
      [JsonIgnore]
      public virtual PlacentaRemoval PlacentaRemoval { get; set; }
   }
}