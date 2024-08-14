using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the GuidedExamination entity in the database.
   /// </summary>
   public class GuidedExamination : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table GuidedExaminations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// The field indicates whether sores are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Sores { get; set; }

      /// <summary>
      /// The field indicates whether a discharge is abnormal or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Abnormal discharge")]
      public YesNo AbnormalDischarge { get; set; }

      /// <summary>
      /// The field indicates whether Warts are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Warts { get; set; }

      /// <summary>
      /// The field indicates whether OtherAbnormalities are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Other abnormalities")]
      public YesNo OtherAbnormalities { get; set; }

      /// <summary>
      /// The field indicates whether the examination is normal or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Normal { get; set; }

      /// <summary>
      /// This field indicates the Vaginal muscle tone of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Vaginal muscle tone")]
      public VaginalMuscleTone VaginalMuscleTone { get; set; }

      /// <summary>
      /// This field indicates the Cervix colour of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Cervix colour")]
      public CervixColour CervixColour { get; set; }

      /// <summary>
      /// This field indicates the Cervical consistency of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Cervical consistency")]
      public CervicalConsistency CervicalConsistency { get; set; }

      /// <summary>
      /// This field indicates the Fibroid palpable of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fibroid palpable")]
      public YesNo FibroidPalpable { get; set; }

      /// <summary>
      /// The field indicates whether Scars are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Scars { get; set; }

      /// <summary>
      /// The field indicates whether Masses are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Masses { get; set; }

      /// <summary>
      /// The field indicates whether Liver palpable are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Liver palpable")]
      public YesNo LiverPalpable { get; set; }

      /// <summary>
      /// The field indicates whether Tenderness are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo Tenderness { get; set; }

      /// <summary>
      /// This field indicates the Uterus position of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Uterus position")]
      public UterusPosition UterusPosition { get; set; }

      /// <summary>
      /// The field indicates whether the Size is normal or abnormal.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public NormalAbnormal Size { get; set; }

      /// <summary>
      /// The field indicates whether TenderAdnexa are present or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public YesNo TenderAdnexa { get; set; }

      /// <summary>
      /// Other findings of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Other findings")]
      public string OtherFindings { get; set; }

      /// <summary>
      /// Cervical discharge of the GuidedExamination.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Cervical discharge")]
      public NormalAbnormal CervicalDischarge { get; set; }

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