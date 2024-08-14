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
   /// IdentifiedPregnencyConfirmations entity.
   /// </summary>
   public class IdentifiedPregnancyConfirmation : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedPregnencyConfirmation.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table GynConfirmation.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int GynConfirmationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("GynConfirmationId")]
      [JsonIgnore]
      public virtual GynConfirmation GynConfirmation { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table pregnencyBooking.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid PregnancyBookingId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PregnancyBookingId")]
      [JsonIgnore]
      public virtual PregnancyBooking PregnancyBooking { get; set; }
   }
}