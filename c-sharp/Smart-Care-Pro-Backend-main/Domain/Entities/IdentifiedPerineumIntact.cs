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
   /// IdentifiedPeriuneumIntacts entity.
   /// </summary>
   public class IdentifiedPerineumIntact : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PeriuneumIntacts.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Periuneums of IdentifiedPeriuneumIntact.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Perineum Perineums { get; set; }

      /// <summary>
      /// Is Tear repaired or not.
      /// </summary>
      [Display(Name = "Tear repaired")]
      public bool TearRepaired { get; set; }

      /// <summary>
      /// Tear degree of IdentifiedPeriuneumIntact.
      /// </summary>
      [Display(Name = "Tear degree")]
      public TearDegree? TearDegree { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table IdentifiedPeriuneumIntacts.
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