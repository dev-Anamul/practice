using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 06.02.2023
* Modified by  : 
* Last modified: 
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// NursingPlan entity.
   /// </summary>
   public class NursingPlan : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Vitals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Planning Date of NursingPlan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Planning Date")]
      [IfFutureDate]
      public DateTime PlanningDate { get; set; }

      /// <summary>
      /// OperationTime of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Operation Time")]
      public TimeSpan PlanningTime { get; set; }

      /// <summary>
      /// Problem of NursingPlan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Problem")]
      public string Problem { get; set; }

      /// <summary>
      /// Objective of NursingPlan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Objective")]
      public string Objective { get; set; }

      /// <summary>
      /// Nursing Diagnosis of NursingPlan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Nursing Diagnosis")]
      public string NursingDiagnosis { get; set; }

      /// <summary>
      /// Nursing Intervention of NursingPlan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Nursing Intervention")]
      public string NursingIntervention { get; set; }

      /// <summary>
      /// Nursing Intervention of NursingPlan of the client.
      /// </summary>
      [Display(Name = "Nursing Intervention")]
      public string Evaluation { get; set; }

      // <summary>
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