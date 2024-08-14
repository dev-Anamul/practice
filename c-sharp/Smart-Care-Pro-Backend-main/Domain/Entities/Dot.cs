using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Dot entity.
   /// </summary>
   public class Dot : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table Dots.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Dot start date.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Dot start date")]
      public DateTime DotStartDate { get; set; }

      /// <summary>
      /// Dot start date.
      /// </summary>
      [Display(Name = "Dot end date")]
      public DateTime? DotEndDate { get; set; }

      /// <summary>
      /// Dot Plan.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Dot plan")]
      public DotPlan DotPlan { get; set; }

      /// <summary>
      /// Disease site if any.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Disease site")]
      public DiseaseSite DiseaseSite { get; set; }

      /// <summary>
      /// type of TB.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "TB type")]
      public TBType TBType { get; set; }

      /// <summary>
      /// type of Susceptible pt if any.
      /// </summary>
      [Display(Name = "Susceptible pt. type")]
      public SusceptiblePTType? SusceptiblePTType { get; set; }

      /// <summary>
      /// Current TB susceptible regimen.
      /// </summary>
      [Display(Name = "Current TB susceptible regimen")]
      public TBSusceptibleRegimen? TBSusceptibleRegimen { get; set; }

      /// <summary>
      /// MDR/DR registration group if any.
      /// </summary>
      [Display(Name = "MDR/DR registration group")]
      public MDRDRRegimenGroup? MDRDRRegimenGroup { get; set; }

      /// <summary>
      /// Current regimen .
      /// </summary>
      [Display(Name = "Current regimen")]
      public MDRDRRegimen? MDRDRRegimen { get; set; }

      /// <summary>
      /// Transition to new phase.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Transition to new phase")]
      public Phase Phase { get; set; }

      /// <summary>
      /// Additional notes of the Dots.
      /// </summary>
      [StringLength(300)]
      public string Remarks { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBServices.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.Town)]
      public Guid TBServiceId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBServiceId")]
      [JsonIgnore]
      public virtual TBService TBService { get; set; }

      /// <summary>
      /// Dot of the DotCalanders.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DotCalendar> DotCalendars { get; set; }
   }
}