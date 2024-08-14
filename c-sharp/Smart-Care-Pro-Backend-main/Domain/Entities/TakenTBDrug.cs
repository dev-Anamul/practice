using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// <summary>
   /// TakenTBDrug entity.
   /// </summary>
   public class TakenTBDrug : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TakenTBDrugs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TBDrugs.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TBDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TBDrugId")]
      [JsonIgnore]
      public virtual TBDrug TBDrug { get; set; }

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