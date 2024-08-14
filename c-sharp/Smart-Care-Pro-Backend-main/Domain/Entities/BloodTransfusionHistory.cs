using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// BloodTransfusionHistory entity.
   /// </summary>
   public class BloodTransfusionHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table BloodTransfusionHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Number of units for table BloodTransfusionHistories.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Number of units")]
      public int NumberOfUnits { get; set; }

      /// <summary>
      /// Blood group for table BloodTransfusionHistories.
      /// </summary>
      [Display(Name = "Blood group")]
      public BloodGroup? BloodGroup { get; set; }

      /// <summary>
      /// Kin blood group for table BloodTransfusionHistories.
      /// </summary>
      [Display(Name = "Kin blood group")]
      public BloodGroup? KinBloodGroup { get; set; }

      /// <summary>
      /// RH sensitivity for table BloodTransfusionHistories.
      /// </summary>
      [Display(Name = "RH sensitivity")]
      public RHSensitivity? RHSensitivity { get; set; }

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

      /// <summary>
      /// IdentifiedPriorSensitization of the BloodTransfusionHistories.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPriorSensitization> IdentifiedPriorSensitizations { get; set; }

      /// <summary>
      ///  IdentifiedPriorSensitizationsList for the client side
      /// </summary>
      [NotMapped]
      public int[] IdentifiedPriorSensitizationsList { get; set; }
   }
}