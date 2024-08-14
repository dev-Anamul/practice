using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// CovaxRecord Entity
   /// </summary>
   public class CovaxRecord : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table CovaxRecord.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Covax.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid CovaxId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CovaxId")]
      [JsonIgnore]
      public virtual Covax Covax { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ImmunizationRecord.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ImmunizationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ImmunizationId")]
      [JsonIgnore]
      public virtual ImmunizationRecord ImmunizationRecord { get; set; }
   }
}