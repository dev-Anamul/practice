using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 19.01.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HIVPreventionHistory entity in the database.
   /// </summary>
   public class HIVPreventionHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table HIVPreventionHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// The field indicates whether PEP is used before or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is PEP used")]
      public bool IsPEPUsedBefore { get; set; }

      /// <summary>
      /// The field indicates whether PrEP is used before or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsPrEPUsedBefore { get; set; }

      /// <summary>
      /// The field indicates whether Condom lubricant is used or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsCondomLubricantUsed { get; set; }

      /// <summary>
      /// The field indicates the Circumcision status.
      /// </summary>
      [Display(Name = "Is Circumcised")]
      public YesNoNotApplicable IsCircumcised { get; set; }

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