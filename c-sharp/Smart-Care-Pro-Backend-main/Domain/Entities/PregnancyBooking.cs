using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brain
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PregnancyBooking entity.
   /// </summary>
   public class PregnancyBooking : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Pregnancy confirmed date table PregnancyBookings.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Pregnancy confirmed date")]
      public DateTime PregnancyConfirmedDate { get; set; }

      /// <summary>
      /// Quickening date table PregnancyBookings.
      /// </summary>
      [Display(Name = "Quickening date")]
      public DateTime? QuickeningDate { get; set; }

      /// <summary>
      /// Quickening weeks table PregnancyBookings.
      /// </summary>
      [Display(Name = "Quickening weeks")]
      public int? QuickeningWeeks { get; set; }

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
      /// Pregnancy confirmed facility id table PregnancyBookings.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Pregnancy confirmed facility id")]
      public int PregnancyConfirmedFacilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PregnancyConfirmedFacilityId")]
      [JsonIgnore]
      public virtual Facility Facility { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPregnancyConfirmation> IdentifiedPregnencyConfirmations { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public int[] IdentifiedPregnancyConfirmationList { get; set; }
   }
}