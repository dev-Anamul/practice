using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ReferralModules entity.
   /// </summary>
   public class ReferralModule : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Is proceeded facility or not.
      /// </summary>
      [Display(Name = "Is proceeded facility")]
      public bool? IsProceededFacility { get; set; }

      /// <summary>
      /// Referral Reason of the client.
      /// </summary>
      [Display(Name = "Reason For Referral")]
      public string ReasonForReferral { get; set; }

      /// <summary>
      ///Type of Referral.
      /// </summary>
      public ReferralType? ReferralType { get; set; }

      /// <summary>
      /// Other Facility of the client.
      /// </summary>
      [Display(Name = "Other Facility")]
      public string OtherFacility { get; set; }

      /// <summary>
      /// Other Facility type of the client.
      /// </summary>
      [Display(Name = "Other Facility Type")]
      public string OtherFacilityType { get; set; }

      /// <summary>
      /// Additional comments of the client
      /// </summary>
      [Display(Name = "Comments")]
      public string Comments { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ServicePoints.
      /// </summary>
      [Required]
      public int ServicePointId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>

      [ForeignKey("ServicePointId")]
      [JsonIgnore]
      public virtual ServicePoint ServicePoint { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ReferredFacilities.
      /// </summary>
      public int? ReferredFacilityId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ReceivingFacilities.
      /// </summary>
      public int? ReceivingFacilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ReceivingFacilityId")]
      [JsonIgnore]
      public virtual Facility ReceivingFacility { get; set; }

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
      /// IdentifiedReferralReason of the ReferralModule.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedReferralReason> IdentifiedReferralReasons { get; set; }

      [NotMapped]
      public int[] IdentifiedReferralReasonsList { get; set; }
      [NotMapped]
      public string ReferredFacility { get; set; }
   }
}