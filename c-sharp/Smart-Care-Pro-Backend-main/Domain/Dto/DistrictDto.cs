using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Domain.Dto
{
    public class DistrictDto
    {
        /// <summary>
        /// Primary key of the table Districts.
        /// </summary>
        public int Oid { get; set; }

        /// <summary>
        /// Name of a district.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "District name")]
        public string Description { get; set; }

        /// <summary>
        /// District Code.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(4)]
        [Display(Name = "District Code")]
        public string DistrictCode { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Provinces. 
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int ProvinceId { get; set; }
    }
}