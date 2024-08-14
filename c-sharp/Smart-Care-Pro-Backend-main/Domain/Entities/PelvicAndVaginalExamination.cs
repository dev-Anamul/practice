using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 27.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PelvicAndVaginalExaminations entity.
   /// </summary>
   public class PelvicAndVaginalExamination : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PelvicAndVaginalExaminations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Vulva of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Vulva Vulva { get; set; }

      /// <summary>
      /// Bleeding of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Bleeding { get; set; }

      /// <summary>
      /// Lochia of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Lochia Lochia { get; set; }

      /// <summary>
      /// Uterus contracted of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Uterus contracted")]
      public YesNo UterusContracted { get; set; }

      /// <summary>
      /// Episiotomy sultured of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Episiotomy suture")]
      public YesNo EpisiotomySuture { get; set; }

      /// <summary>
      /// Perineum of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Perineum Perineum { get; set; }

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