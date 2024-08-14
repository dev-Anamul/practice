using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bithy
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TPTHistories entity.
   /// </summary>
   public class TPTHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TPTHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Is patient on TPT currently.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is patient on TPT currently?")]
      public bool IsOnTPT { get; set; }

      /// <summary>
      /// Start Date of the TPT.
      /// </summary>        
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "TPT start date")]
      public DateTime? TPTStartDate { get; set; }

      /// <summary>
      /// End Date of the TPT.
      /// </summary>        
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "TPT end date")]
      public DateTime? TPTEndDate { get; set; }

      /// <summary>
      /// Has client taken TPT last 3 year.
      /// </summary>
      [Display(Name = "Has client taken TPT last 3 year")]
      public bool IsTakenTPTThreeYears { get; set; }

      /// <summary>
      /// Is patient eligible for TPT.
      /// </summary>
      [Display(Name = "Is patient eligible")]
      public bool IsPatientEligible { get; set; }

      /// <summary>
      /// If eligible but not started state.
      /// </summary>
      [StringLength(300)]
      [Display(Name = "Reason not started")]
      public string ReasonNotStarted { get; set; }

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

      /// <summary>
      /// TakenTPTDrugs of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TakenTPTDrug> TakenTPTDrugs { get; set; }

      /// <summary>
      /// Not Mapped List
      /// </summary>
      [NotMapped]
      public int[] TPTTakenDrugList { get; set; }
   }
}