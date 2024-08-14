using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella  
 * Date created : 01.01.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the GlasgowComaScale entity in the database.
   /// </summary>
   public class GlasgowComaScale : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table GlasgowComaScales.
      /// </summary
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Eye score of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Eye Score")]
      public EyeScore EyeScore { get; set; }

      /// <summary>
      /// Verbal score of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Verbal Score")]
      public VerbalScore VerbalScore { get; set; }

      /// <summary>
      /// Motor scale of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Motor Scale")]
      public MotorScore MotorScale { get; set; }

      /// <summary>
      /// Glasgow coma score of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Glasgow Coma Score")]
      public string GlasgowComaScore { get; set; }

      /// <summary>
      /// Result of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(500)]
      [Display(Name = "Result")]
      public string Result { get; set; }

      /// <summary>
      /// Right Pupils Light Reaction Size of the client.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Right Pupils Light Reaction Size")]
      public string RightPupilsLightReactionSize { get; set; }

      /// <summary>
      /// Right Pupils Light Reaction Reaction of the client.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "Right Pupils Light Reaction Reaction")]
      public string RightPupilsLightReactionReaction { get; set; }

      /// <summary>
      /// Left Pupils Light Reaction Size of the client.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Left Pupils Light Reaction Size")]
      public string LeftPupilsLightReactionSize { get; set; }

      /// <summary>
      /// Left Pupils Light Reaction Reaction of the client.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "Left Pupils Light Reaction Reaction")]
      public string LeftPupilsLightReactionReaction { get; set; }

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
   }
}