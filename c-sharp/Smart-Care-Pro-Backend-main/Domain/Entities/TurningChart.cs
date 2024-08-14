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
   /// Turning Chart entity.
   /// </summary>
   public class TurningChart : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Vitals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Planning Date of NursingPlan of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "RecordDate")]
      [IfFutureDate]
      public DateTime RecordDate { get; set; }

      /// <summary>
      /// OperationTime of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Turning Time")]
      public TimeSpan TurningTime { get; set; }

      // <summary>
      /// Problem of NursingPlan of the client.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Comments")]
      public string Comments { get; set; }

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