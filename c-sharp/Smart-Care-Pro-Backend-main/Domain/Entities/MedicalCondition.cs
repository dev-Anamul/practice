using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// MedicalCondition entity.
   /// </summary>
   public class MedicalCondition : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table GuidedExaminations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Does any heath conditon screened of the table MedicalConditions.
      /// </summary>
      [Display(Name = "Does any heath conditon screened")]
      public YesNo? DoesAnyHealthConditonScreened { get; set; }

      /// <summary>
      /// Does risk of STI increased of the table MedicalConditions.
      /// </summary>
      [Display(Name = "Does risk of STI increased")]
      public YesNo? DoesRiskOfSTIIncreased { get; set; }

      /// <summary>
      /// Last unprotected sex days of the table MedicalConditions.
      /// </summary>
      [Display(Name = "Last unprotected sex days")]
      public int? LastUnprotectedSexDays { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PastMedicalConditons.
      /// </summary>
      public int? PastMedicalConditonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PastMedicalConditonId")]
      [JsonIgnore]
      public virtual PastMedicalCondition PastMedicalConditon { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table STIAssesments.
      /// </summary>
      public int? STIRiskId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("STIRiskId")]
      [JsonIgnore]
      public virtual STIRisk STIRisk { get; set; }

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
   }
}