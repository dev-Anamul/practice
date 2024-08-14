using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   public class ExposureRisk : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Vitals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Source of Alert of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Exposer Risks")]
      public ExposureRisks ExposureRisks { get; set; }

      // <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid CovidId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CovidId")]
      [JsonIgnore]
      public virtual Covid Covid { get; set; }
   }
}
