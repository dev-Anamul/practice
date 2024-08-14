using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 16.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class AdverseEvent : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table AdverseEvent.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Last test date of HIV of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "AEFI Date")]
      [IfFutureDate]
      public DateTime AEFIDate { get; set; }

      /// <summary>
      /// Swelling of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Swelling { get; set; }

      /// <summary>
      /// Joint of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Joint { get; set; }

      /// <summary>
      /// Swelling of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Malaise { get; set; }

      /// <summary>
      /// BodyAches of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool BodyAches { get; set; }

      /// <summary>
      /// Fever of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Fever { get; set; }

      /// <summary>
      /// AllergicReaction of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool AllergicReaction { get; set; }

      /// <summary>
      /// AllergicReaction of the table AdverseEvent.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool OtherAdverseEvent { get; set; }

      /// <summary>
      /// OtherAEFI of the table AdverseEvent.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "OtherAEFI")]
      public string OtherAEFI { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Immunizations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ImmunizationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ImmunizationId")]
      [JsonIgnore]
      public virtual ImmunizationRecord ImmunizationRecord { get; set; }
   }
}