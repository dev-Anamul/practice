using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// UsedTBIdentificationMethod entity.
   /// </summary>
   public class UsedTBIdentificationMethod : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table UsedTBIdentificationMethods.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBIdentificationMethods.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TBIdentificationMethodId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBIdentificationMethodId")]
      [JsonIgnore]
      public virtual TBIdentificationMethod TBIdentificationMethod { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBHistories.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid TBHistoryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBHistoryId")]
      [JsonIgnore]
      public virtual TBHistory TBHistory { get; set; }
   }
}