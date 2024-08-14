using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 29.01.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HIVRiskScreening entity in the database.
   /// </summary>
   public class HIVRiskScreening : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table HIVRiskScreenings.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// The field stores the answer.
      /// </summary>
      public bool Answer { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Questions.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int QuestionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("QuestionId")]
      [JsonIgnore]
      public virtual Question Question { get; set; }

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