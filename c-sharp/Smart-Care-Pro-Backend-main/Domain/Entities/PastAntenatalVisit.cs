using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// PastAntenatalVisit entity.
   /// </summary>
   public class PastAntenatalVisit : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PastAntenatalVisit.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Visit No for table PastAntenatalVisits.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Visit No")]
      public int VisitNo { get; set; }

      /// <summary>
      /// Visit date for table PastAntenatalVisit.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Visit date")]
      public DateTime VisitDate { get; set; }

      /// <summary>
      /// Findings for table PastAntenatalVisit.
      /// </summary>
      public string Findings { get; set; }

      /// <summary>
      /// Is admitted or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is admitted")]
      public bool IsAdmitted { get; set; }

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

      [NotMapped]
      public List<PastAntenatalVisit> PastAntenatalVisitList { get; set; } = new List<PastAntenatalVisit>();
   }
}