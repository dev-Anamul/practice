using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DeathCause entity.
   /// </summary>   
   public class DeathCause : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DeathCauses.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// ICD11(Main cause of death) of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "CauseType")]
      public CauseType CauseType { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ICPC2Descriptions.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ICPC2Id { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ICPC2Id")]
      [JsonIgnore]
      public virtual ICPC2Description ICPC2Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ICPC2Descriptions.
      /// </summary>
      public int? ICD11Id { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ICD11Id")]
      [JsonIgnore]
      public virtual ICDDiagnosis ICDDiagnosis { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table DeathRecords.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeathRecordId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeathRecordId")]
      [JsonIgnore]
      public virtual DeathRecord DeathRecord { get; set; }
   }
}