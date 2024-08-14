using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
* Created by   : Stephan
* Date created : 06.02.2023
* Modified by  : Bella
* Last modified: 12.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   // <summary>
   /// Represents FluidIntake entity in the database.
   /// </summary>
   public class FluidIntake : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Vitals.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// IntakeTime of the FluidIntake.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "In take Time")]
      public TimeSpan IntakeTime { get; set; }

      /// <summary>
      /// IntakeType of the FluidIntake.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(50)]
      [Display(Name = "Intake Type")]
      public string IntakeType { get; set; }

      /// <summary>
      /// IntakeAmount of the FluidIntake.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Intake Amount")]
      public decimal IntakeAmount { get; set; }

      // <summary>
      /// Route of the FluidIntake.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Route")]
      public Route Route { get; set; }

      // <summary>
      /// Foreign Key. Primary key of the table Fluids.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid FluidId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FluidId")]
      [JsonIgnore]
      public virtual Fluid Fluid { get; set; }
   }
}