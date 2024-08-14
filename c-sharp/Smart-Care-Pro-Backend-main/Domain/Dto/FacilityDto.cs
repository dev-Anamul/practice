using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using static Utilities.Constants.Enums;

namespace Domain.Dto
{
    public class FacilityDto
    {
        /// <summary>
        /// Primary key of the table Facilities.
        /// </summary>
        public int Oid { get; set; }

        /// <summary>
        /// The field stores the name of a facility.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The master codes of facilities
        /// </summary>
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

        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}