using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// QuickExamination entity.
   /// </summary>
   public class QuickExamination : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table GuidedExaminations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Hair well spread of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Hair well spread")]
      public YesNo HairWellSpread { get; set; }

      /// <summary>
      /// Hair healthy of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Hair healthy")]
      public YesNo HairHealthy { get; set; }

      /// <summary>
      /// Head infection of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Head infection")]
      public YesNo HeadInfection { get; set; }

      /// <summary>
      /// Thrash of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Thrash { get; set; }

      /// <summary>
      /// Dental disease of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Dental disease")]
      public YesNo DentalDisease { get; set; }

      /// <summary>
      /// Cervical glands enlarged of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Cervical glands enlarged")]
      public YesNo CervicalGlandsEnlarged { get; set; }

      /// <summary>
      /// Neck abnormal of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Neck abnormal")]
      public YesNo NeckAbnormal { get; set; }

      /// <summary>
      /// Breast lumps of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Breast lumps")]
      public YesNo BreastLumps { get; set; }

      /// <summary>
      /// Armpits of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public NormalAbnormal Armpits { get; set; }

      /// <summary>
      /// Fibroid palpable of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fibroid palpable")]
      public YesNo FibroidPalpable { get; set; }

      // <summary>
      /// Scars of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Scars { get; set; }

      /// <summary>
      /// Liver palpable of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Liver palpable")]
      public YesNo LiverPalpable { get; set; }

      /// <summary>
      /// Tenderness of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Tenderness { get; set; }

      /// <summary>
      /// Masses of the QuickExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Masses { get; set; }

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