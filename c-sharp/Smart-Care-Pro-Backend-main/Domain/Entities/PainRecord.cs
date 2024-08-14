using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 06-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PainRecord entity.
   /// </summary>
   public class PainRecord : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PainRecords.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Pain Scale of the pain.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool PainScales { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PainScales.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PainScaleId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PainScaleId")]
      [JsonIgnore]
      public virtual PainScale PainScale { get; set; }

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