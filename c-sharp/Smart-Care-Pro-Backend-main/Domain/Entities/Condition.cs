using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 04.01.2023
 * Modified by  : Tomas
 * Last modified: 17.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Condition entity.
   /// </summary>
   public class Condition : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Conditions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Type of a condition of the client.
      /// </summary>        
      [Display(Name = "Condition Type")]
      public ConditionType ConditionType { get; set; }

      /// <summary>
      /// Date of diagnosis of the client.
      /// </summary>
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date Diagnosed")]
      public DateTime? DateDiagnosed { get; set; }

      /// <summary>
      /// Resolved date of the client.
      /// </summary>
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date Resolved")]
      public DateTime? DateResolved { get; set; }

      /// <summary>
      /// Client's diagnosis is on going or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is On going")]
      public bool IsOngoing { get; set; }

      /// <summary>
      /// Certainty of the client.
      /// </summary>        
      [Display(Name = "Certainity")]
      public Certainty Certainty { get; set; }

      /// <summary>
      /// Comments.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Comments")]
      public string Comments { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table NTGLevelThreeDiagnoses.
      /// </summary>
      /// 
      [DiagnosisNTG_OR_ICD11(ErrorMessage = MessageConstants.DiagnosisTypeValidatorMessage)]
      public int? NTGId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NTGId")]
      [JsonIgnore]
      public virtual NTGLevelThreeDiagnosis NTGLevelThreeDiagnosis { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ICDDiagnoses.
      /// </summary>
      /// 
      [DiagnosisNTG_OR_ICD11(ErrorMessage = MessageConstants.DiagnosisTypeValidatorMessage)]
      public int? ICDId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ICDId")]
      [JsonIgnore]
      public virtual ICDDiagnosis ICDDiagnosis { get; set; }

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
   }
}