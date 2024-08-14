using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// KeyPopulationDemographic entity.
   /// </summary>
   public class KeyPopulationDemographic : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table KeyPopulationDemographics.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table KeyPopulations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int KeyPopulationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("KeyPopulationId")]
      [JsonIgnore]
      public virtual KeyPopulation KeyPopulation { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
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