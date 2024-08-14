using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 27.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// VisitDetail entity.
   /// </summary>
   public class VisitDetail : EncounterBaseModel
   {
      // <summary>
      /// Primary key of the table VisitDetails.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// type of Visit for VisitDetails.
      /// </summary>
      [Display(Name = "Visit type")]
      public VisitDetailsType? VisitType { get; set; }

      /// <summary>
      /// Is Baby name given or not.
      /// </summary>
      [Display(Name = "Baby name given")]
      public bool IsBabyNameGiven { get; set; }

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