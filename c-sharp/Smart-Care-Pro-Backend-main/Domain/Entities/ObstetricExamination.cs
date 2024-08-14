using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ObstericExamination entity.
   /// </summary>
   public class ObstetricExamination : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ObstericExaminations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// SFH for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int SFH { get; set; }

      /// <summary>
      /// Presentation for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Presentation Presentation { get; set; }

      /// <summary>
      /// Engaged for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Engaged Engaged { get; set; }

      /// <summary>
      /// Lie for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Lie Lie { get; set; }

      /// <summary>
      /// Fetal heart for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fetal heart")]
      public FetalHeart FetalHeart { get; set; }

      /// <summary>
      /// Fetal heart rate for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fetal heart rate")]
      public int FetalHeartRate { get; set; }

      /// <summary>
      /// Constraction for table ObstericExaminations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Contractions Contraction { get; set; }

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