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
   /// IdentifiedDeliveryInterventions entity.
   /// </summary>
   public class IdentifiedDeliveryIntervention : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedDeliveryInterventions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Interventions of IdentifiedDeliveryIntervention.
      /// </summary>
      public Interventions? Interventions { get; set; }

      /// <summary>
      /// Other information of IdentifiedDeliveryIntervention.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table MotherDeliverySummaries.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeliveryId")]
      [JsonIgnore]
      public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }
   }
}