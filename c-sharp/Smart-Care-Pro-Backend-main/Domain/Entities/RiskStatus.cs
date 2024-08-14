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
   /// PEPRiskStatus entity.
   /// </summary>
   public class RiskStatus : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PEPRiskStatuses.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table PEPRisks.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int RiskId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("RiskId")]
      [JsonIgnore]
      public virtual Risks Risk { get; set; }

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
      /// List of risk status.
      /// </summary>
      [NotMapped]
      public int[] RiskList { get; set; }
   }
}