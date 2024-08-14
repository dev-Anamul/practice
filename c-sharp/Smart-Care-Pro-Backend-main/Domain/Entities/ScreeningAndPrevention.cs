using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ScreeningAndPreventions entity.
   /// </summary>
   public class ScreeningAndPrevention : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ScreeningAndPrevention.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Malaria dose no for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Malaria dose")]
      public MalariaDose MalariaDose { get; set; }

      /// <summary>
      /// Malaria dose no for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Malaria dose no")]
      public int? MalariaDoseNo { get; set; }

      /// <summary>
      /// Amenia dose for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Amenia dose")]
      public AmeniaDose AmeniaDose { get; set; }

      /// <summary>
      /// Tetanus dose for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Tetanus dose")]
      public TetanusDose TetanusDose { get; set; }

      /// <summary>
      /// Tetanus dose for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Tetanus dose no")]
      public int? TetanusDoseNo { get; set; }

      /// <summary>
      /// Tetanus dose for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Tetanus dose no")]
      public SyphilisDose SyphilisDose { get; set; }

      /// <summary>
      /// Tetanus dose for table ScreeningAndPrevention.
      /// </summary>
      [Display(Name = "Hepatitis B dose")]
      public HepatitisBDose HepatitisBDose { get; set; }

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