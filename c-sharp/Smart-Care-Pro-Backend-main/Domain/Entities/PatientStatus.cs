using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 18.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Patient Status entity.
   /// </summary>
   public class PatientStatus : BaseModel
   {
      /// <summary>
      /// Primary Key of the table PatientStatuses.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Status of the Patient of the table PatientStatuses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Patient Status")]
      public PatientsStatus Status { get; set; }

      /// <summary>
      /// StatusDate of the table PatientStatuses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Status Date")]
      public DateTime StatusDate { get; set; }

      /// <summary>
      /// OtherReasons of the table PatientStatuses.
      /// </summary>        
      [Display(Name = "Other Reason")]
      public string OtherReason { get; set; }

      /// <summary>
      /// Referral Reason of the table PatientStatuses.
      /// </summary>       
      [Display(Name = "Referral Reason")]
      public ReferralReason? ReferralReason { get; set; }

      /// <summary>
      /// Clinician Id
      /// </summary>
      public Guid? ClinicianId { get; set; }

      /// <summary>
      /// Referring Facility Id
      /// </summary>
      public int? ReferringFacilityId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string ReferringFacilityName { get; set; }

      /// <summary>
      /// ReferredFacilityId of the table PatientStatuses.
      /// </summary>
      public int? ReferredFacilityId { get; set; }

      /// <summary>
      /// ReferredFacilityName of the table PatientStatuses.
      /// </summary>
      [NotMapped]

      [Display(Name = "Referred Facility Name")]
      public string ReferredFacilityName { get; set; }

      /// <summary>
      /// ReasonForInactive of the table PatientStatuses.
      /// </summary>
      [Display(Name = "Reason For Inactive")]
      public ReasonForInactive? ReasonForInactive { get; set; }

      /// <summary>
      /// ARVStoppingReasons of the table PatientStatuses.
      /// </summary>     
      [Display(Name = "ARV Stopping Reason")]
      public string ARVStoppingReason { get; set; }

      /// <summary>
      /// ReasonForStoppingART of the table PatientStatuses.
      /// </summary>        
      [Display(Name = "Reason For Stopping ART")]
      public ReasonForStoppingART? ReasonForStoppingART { get; set; }

      /// <summary>
      /// ReasonForReactivation of the table PatientStatuses.
      /// </summary>
      [Display(Name = "Reason For Reactivation ")]
      public ReasonForReactivation? ReasonForReactivation { get; set; }

      /// <summary>
      /// ARTRegisterId of the table PatientStatuses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ARTRegisterId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ARTRegisterId")]
      [JsonIgnore]
      public virtual ARTService ARTRegister { get; set; }
   }
}