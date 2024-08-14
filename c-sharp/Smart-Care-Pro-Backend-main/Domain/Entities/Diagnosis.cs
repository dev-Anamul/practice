using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 03.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Diagnosis entity.
   /// </summary>
   public class Diagnosis : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Diagnoses.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Type of the diagnosis.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public DiagnosisType DiagnosisType { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table NTGLevelThreeDiagnoses.
      /// </summary>
      public int? NTGId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NTGId")]
      [JsonIgnore]
      public virtual NTGLevelThreeDiagnosis NTGLevelThreeDiagnosis { get; set; }

      /// <summary>
      /// IsSelectedForSurgery for Surgery entity
      /// </summary>
      public bool IsSelectedForSurgery { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ICDDiagnoses.
      /// </summary>
      /// 
      public int? ICDId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ICDId")]
      [JsonIgnore]
      public virtual ICDDiagnosis ICDDiagnosis { get; set; }

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
      /// Foreign key. SurgeryID for Surgery entity
      /// </summary>
      public Guid? SurgeryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SurgeryId")]
      [JsonIgnore]
      public virtual Surgery Surgery { get; set; }

      /// <summary>
      /// List of ICD11.
      /// </summary>
      [NotMapped]
      public int[]? ICD11 { get; set; }

      /// <summary>
      /// List of NTG.
      /// </summary>
      [NotMapped]
      public int[]? NTG { get; set; }

      [NotMapped]
      public Diagnosis[] Diagnoses { get; set; }
   }
}