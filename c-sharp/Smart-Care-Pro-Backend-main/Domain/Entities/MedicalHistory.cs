using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 24.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// MedicalHistory entity.
   /// </summary>
   public class MedicalHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table MedicalHistories.
      /// </summary
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Medical history of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      public string History { get; set; }

      /// <summary>
      /// Type of information of the medical history.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public InformationType InformationType { get; set; }

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
      /// Navigation property. 
      /// </summary>
      [NotMapped]
      public IEnumerable<RiskStatus> PEPRisk { get; set; }
   }
}