using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// <summary>
   /// Fluid entity.
   /// </summary>
   public class Fluid : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Fluids.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// This field indicates the record date of the fluid.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "RecordDate")]
      [IfFutureDate]
      public DateTime RecordDate { get; set; }

      /// <summary>
      /// This field indicates the Doctors Order of Nursing plan of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(500)]
      [Display(Name = "Doctors Order")]
      public string DoctorsOrder { get; set; }

      // <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// FluidIntakes of the Fluids.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FluidIntake> FluidIntakes { get; set; }

      /// <summary>
      /// FluidOutputs of the Fluids.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FluidOutput> FluidOutputs { get; set; }
   }
}