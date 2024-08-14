using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Lion
 * Date created : 08.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// WHOConditions entity.
   /// </summary>
   public class WHOCondition : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table WHOStagesConditions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key of WHOStagesCondition
      /// </summary>
      public int WHOStagesConditionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("WHOStagesConditionId")]
      [JsonIgnore]
      public virtual WHOStagesCondition WHOStagesCondition { get; set; }

      /// <summary>
      /// Foreign Key of Client.
      /// </summary>
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}