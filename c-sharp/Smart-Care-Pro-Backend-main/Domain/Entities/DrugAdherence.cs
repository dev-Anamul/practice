using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   /// DrugAdherence entity.
   /// </summary>
   public class DrugAdherence : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DrugAdherences.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo IsTakingMedications { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool HaveTroubleTakingPills { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo DosesMissed { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public ReasonForMissing? ReasonForMissing { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsPatientComplainedOnMedication { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string Note { get; set; }

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
      /// ReasonForMissing List of the client.
      /// </summary>
      [NotMapped]
      public int[] ReasonForMissingList { get; set; }
   }
}