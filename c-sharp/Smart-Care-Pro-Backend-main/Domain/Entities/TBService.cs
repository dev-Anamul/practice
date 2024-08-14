using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 05.04.2023
 * Modified by  :
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TBService entity.
   /// </summary>
   public class TBService : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table TBServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Case Id Number of TBService.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Case Id Number")]
      public string CaseIdNumber { get; set; }

      /// <summary>
      /// Is Client Health Careworker.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Health Careworker")]
      public bool IsHealthCareWorker { get; set; }

      /// <summary>
      /// Is Client Inmet.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Inmet")]
      public bool IsInmate { get; set; }

      /// <summary>
      /// Is Client Miner.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Miner")]
      public bool IsMiner { get; set; }

      /// <summary>
      /// Is Client Miner.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is ExMiner")]
      public bool IsExMiner { get; set; }

      /// <summary>
      /// Other Patient Category.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Other Patient Category")]
      public string OtherPatientCategory { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Treatment Started")]
      public DateTime TreatmentStarted { get; set; }

      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date Discharged")]
      public DateTime? DateDischarged { get; set; }

      /// <summary>
      /// Religion of the client.
      /// </summary>
      [Display(Name = "Treatment Outcome")]
      public TreatmentOutcome? TreatmentOutcome { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.Town)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Dot> Dots { get; set; }
   }
}