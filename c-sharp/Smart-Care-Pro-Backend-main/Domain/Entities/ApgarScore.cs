using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ApgarScores entity.
   /// </summary>
   public class ApgarScore : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ApgarScores.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// ApgarTimeSpan of the table ApgarScore.
      /// </summary>
      [Display(Name = "Apgar time span")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public ApgarTimeSpan ApgarTimeSpan { get; set; }

      /// <summary>
      /// Appearance of the table ApgarScore.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Appearance Appearance { get; set; }

      /// <summary>
      /// Pulses of the table ApgarScore.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Pulses Pulses { get; set; }

      /// <summary>
      /// Grimace of the table ApgarScore.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Grimace Grimace { get; set; }

      /// <summary>
      /// Activity of the table ApgarScore.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Activity Activity { get; set; }

      /// <summary>
      /// Respiration of the table ApgarScore.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Respiration Respiration { get; set; }

      /// <summary>
      /// Total score of the table ApgarScore.
      /// </summary>
      [Display(Name = "Total score")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TotalScore { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Neonatal.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid NeonatalId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NeonatalId")]
      [JsonIgnore]
      public virtual NewBornDetail NewBornDetail { get; set; }

      /// <summary>
      /// Just Used to View the ApgarScore in Client Side.
      /// </summary>
      [NotMapped]
      public List<ApgarScore> ApgarScores { get; set; }
   }
}