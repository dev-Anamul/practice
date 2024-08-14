using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// IdentifiedTBFinding entity.
   /// </summary>
   public class IdentifiedTBFinding : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TakenTPTDrugs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBFindings.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TBFindingId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBFindingId")]
      [JsonIgnore]
      public virtual TBFinding TBFinding { get; set; }

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
      /// List of the IdentifiedTBFinding.
      /// </summary>
      [NotMapped]
      public int[] IdentifiedTBFindingList { get; set; }
   }
}