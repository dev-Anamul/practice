using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 01.01.2022
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// SystemExamination entity.
   /// </summary>
   public class SystemExamination : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table SystemExaminations.
      /// </summary
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Note of the system examination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      [Display(Name = "Note")]
      public string Note { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table PhysicalSystems.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PhysicalSystemId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PhysicalSystemId")]
      [JsonIgnore]
      public virtual PhysicalSystem PhysicalSystem { get; set; }

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

      /// <summary>
      /// List of the system examination.
      /// </summary>
      [NotMapped]
      public List<SystemExamination> SystemExaminations { get; set; }
   }
}