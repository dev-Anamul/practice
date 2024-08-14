using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains information of the facilities.
   /// </summary>
   public class Facility : BaseModel
   {
      /// <summary>
      /// Primary key of the table Facilities.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// The field stores the name of a facility.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Facility name")]
      public string Description { get; set; }

      /// <summary>
      /// The master codes of facilities
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Facility Master Code")]
      public string FacilityMasterCode { get; set; }

      /// <summary>
      /// The field stores the HMIS code of a facility.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "HMIS code")]
      public string HMISCode { get; set; }

      /// <summary>
      /// The field stores the longitude of a facility.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Longitude")]
      public string Longitude { get; set; }

      /// <summary>
      /// The field stores the latitude of a facility.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Latitude")]
      public string Latitude { get; set; }

      /// <summary>
      /// The 
      /// </summary>
      [Display(Name = "Location")]
      public Location Location { get; set; }

      [Display(Name = "Facility Type")]
      public FacilityType FacilityType { get; set; }

      public Ownership Ownership { get; set; }

      /// <summary>
      /// The field stores indicates whether a facility is private or not.
      /// </summary>
      public bool IsPrivateFacility { get; set; }

      /// <summary>
      /// The field stores indicates whether a facility is active or not.
      /// </summary>
      public bool IsLive { get; set; }

      /// <summary>
      /// The field stores indicates whether a DFZ facility.
      /// </summary>
      public bool IsDFZ { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Districts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DistrictId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DistrictId")]
      [JsonIgnore]
      public virtual District District { get; set; }

      /// <summary>
      /// Client attached to a facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<AttachedFacility> AttachedFacilities { get; set; }

      /// <summary>
      /// Departments of a facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Department> Departments { get; set; }

      /// <summary>
      /// Users of a facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FacilityAccess> FacilityAccesses { get; set; }

      /// <summary>
      /// Login histories of a facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<LoginHistory> LoginHistories { get; set; }

      /// <summary>
      /// ReferralModule of a facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ReferralModule> ReferralModules { get; set; }

      ///// <summary>
      ///// PregnancyBooking of the facility.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<PregnancyBooking> PregnancyBookings { get; set; }

      /// <summary>
      /// FacilityQueue of the facility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FacilityQueue> FacilityQueues { get; set; }
   }
}