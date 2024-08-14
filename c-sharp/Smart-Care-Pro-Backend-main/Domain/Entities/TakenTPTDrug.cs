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
   /// TakenTPTDrug entity.
   /// </summary>
   public class TakenTPTDrug : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TakenTPTDrugs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TPTDrugs.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TPTDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TPTDrugId")]
      [JsonIgnore]
      public virtual TPTDrug TPTDrug { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table TPTHistories.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid TPTHistoryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TPTHistoryId")]
      [JsonIgnore]
      public virtual TPTHistory TPTHistory { get; set; }
   }
}