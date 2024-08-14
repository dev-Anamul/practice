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
   /// Represents FluidOutput entity in the database.
   /// </summary>
   public class FluidOutput : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table FluidOutputs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      // <summary>
      /// OutputTime of the FluidOutput.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Output Time")]
      public TimeSpan OutputTime { get; set; }

      /// <summary>
      /// OutputType of the FluidOutput.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(50)]
      [Display(Name = "Output Type")]
      public string OutputType { get; set; }

      /// <summary>
      /// OutputAmount of the FluidOutput.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Output Amount")]
      public decimal OutputAmount { get; set; }

      // <summary>
      /// Route of the FluidOutput.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Intake Amount")]
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