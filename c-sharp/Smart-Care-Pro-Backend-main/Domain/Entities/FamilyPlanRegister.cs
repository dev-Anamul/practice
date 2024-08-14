using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Represents FamilyPlanRegister entity in the database.
   /// </summary>
   public class FamilyPlanRegister : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table FamilyPlanRegisters.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// This field indicates who referred client.
      /// </summary>
      [Display(Name = "Who referred client?")]
      public ReferredBy ReferredBy { get; set; }

      /// <summary>
      /// This field indicates other referrals of the client.
      /// </summary>
      [Display(Name = "Other referrals")]
      [StringLength(90)]
      public string OtherReferrals { get; set; }

      /// <summary>
      /// This field indicates the family planning year.
      /// </summary>
      [Display(Name = "Family planning year")]
      [StringLength(90)]
      public string FamilyPlanningYear { get; set; }

      /// <summary>
      /// This field indicates who client stays with.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Who client stays with")]
      public ClientStaysWith ClientStaysWith { get; set; }

      /// <summary>
      /// This field indicates is communication consent or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Communication Consent")]
      public YesNo CommunicationConsent { get; set; }

      /// <summary>
      /// This field indicates the communication preferences.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Communication preference(s)")]
      public CommunicationPreference CommunicationPreference { get; set; }

      /// <summary>
      /// This field indicates the type of alternative contact.
      /// </summary>
      [Display(Name = "Type of alternative contact")]
      public AlternativeContact TypeOfAlternativeContacts { get; set; }

      /// <summary>
      /// This field indicates the other alternative contacts of the Referral.
      /// </summary>
      [Display(Name = "Other alternative contacts")]
      [StringLength(90)]
      public string OtherAlternativeContacts { get; set; }

      /// <summary>
      /// This field indicates the contact name of the client.
      /// </summary> 
      [Display(Name = "Contact name")]
      [StringLength(90)]
      public string ContactName { get; set; }

      /// <summary>
      /// This field indicates the contact phone number of the client.
      /// </summary>
      [Display(Name = "Contact phone number")]
      [StringLength(11)]
      public string ContactPhoneNumber { get; set; }

      /// <summary>
      /// This field indicates the contact address of the client.
      /// </summary>
      [Display(Name = "Contact address")]
      [StringLength(250)]
      public string ContactAddress { get; set; }

      /// <summary>
      /// This field indicates the Patient type.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Patient type")]
      public PatientType PatientType { get; set; }

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