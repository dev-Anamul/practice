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
   /// ANCScreening entity.
   /// </summary>
   public class ANCScreening : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCScreening.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Is History of bleeding or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "History of bleeding")]
      public bool HistoryofBleeding { get; set; }

      /// <summary>
      /// Is Draining or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Draining { get; set; }

      /// <summary>
      /// Is PV mucas or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "PV mucus")]
      public bool PVMucus { get; set; }

      /// <summary>
      /// Is Contraction or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool Contraction { get; set; }

      /// <summary>
      /// Is PV bleeding or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "PV bleeding")]
      public bool PVBleeding { get; set; }

      /// <summary>
      /// Is Fetal movements or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Fetal movements")]
      public bool FetalMovements { get; set; }

      /// <summary>
      /// Is syphilis done or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is syphilis done")]
      public bool IsSyphilisDone { get; set; }

      /// <summary>
      /// Syphilis test date for table ANCScreenings.
      /// </summary>
      [Display(Name = "Syphilis test date")]
      public DateTime? SyphilisTestDate { get; set; }

      /// <summary>
      /// Syphilis test type table ANCScreenings.
      /// </summary>
      [Display(Name = "Syphilis test type")]
      public SyphilisTestType SyphilisTestType { get; set; }

      /// <summary>
      /// Sysphilis result table ANCScreenings.
      /// </summary>
      [Display(Name = "Syphilis result")]
      public PositiveNegative SyphilisResult { get; set; }

      /// <summary>
      /// Is hepatities done or not.
      /// </summary>
      [Display(Name = "Is hepatities done")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsHepatitisDone { get; set; }

      /// <summary>
      /// Hepatities test date for table ANCScreenings.
      /// </summary>
      [Display(Name = "Hepatities test date")]
      public DateTime? HepatitisTestDate { get; set; }

      /// <summary>
      /// Hepatities test type table ANCScreenings.
      /// </summary>
      [Display(Name = "Hepatities test type")]
      public SyphilisTestType HepatitisTestType { get; set; }

      /// <summary>
      /// Hepatities result table ANCScreenings.
      /// </summary>
      [Display(Name = "Hepatities result")]
      public PositiveNegative HepatitisResult { get; set; }

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