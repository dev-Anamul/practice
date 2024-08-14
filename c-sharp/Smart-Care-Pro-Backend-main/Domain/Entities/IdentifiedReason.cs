using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedReason entity.
   /// </summary>
   public class IdentifiedReason : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TakenTPTDrugs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBSuspectingReasons.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TBSuspectingReasonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBSuspectingReasonId")]
      [JsonIgnore]
      public virtual TBSuspectingReason TBSuspectingReason { get; set; }

      ///<summary>
      ///Foreign key. Primary key of table Clients
      ///<summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      ///<summary>
      ///Navigation property
      ///<summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Clients { get; set; }

      /// <summary>
      /// List of the IdentifiedReason.
      /// </summary>
      [NotMapped]
      public int[] IdentifiedReasonList { get; set; }
   }
}