using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Domain.Entities
{
   public class ARTResponse : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ARTResponses table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has client been on ART > 6 mos")]
      public bool OnARTMoreThanSixMonths { get; set; }

      /// <summary>
      /// Condition of Clinical monitoring.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Clinical monitoring")]
      public ClinicalMonitoring ClinicalMonitoring { get; set; }

      /// <summary>
      /// Status of Immunologic monitoring.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Immunologic monitoring")]
      public ImmunologicMonitoring ImmunologicMonitoring { get; set; }

      /// <summary>
      /// Status of Virologic monitoring.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Virologic monitoring")]
      public VirologicMonitoring VirologicMonitoring { get; set; }

      /// <summary>
      /// Status of Stable on care
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Stable on care status")]
      public StableOnCareStatus StableOnCareStatus { get; set; }

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