using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 12.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class AssessmentPlanDto
    {
        public Guid Oid { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [Display(Name = "ASA classification")]
        public string ASAClassification { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [Display(Name = "IV Access")]
        public string IVAccess { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [Display(Name = "Bony landmarks")]
        public string BonyLandmarks { get; set; }

        /// <summary>
        /// Reference of the facility where the row is created.
        /// </summary>
        [Display(Name = "Created in")]
        public int? CreatedIn { get; set; }

        /// <summary>
        /// Creation date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Reference of the user who has created the row.
        /// </summary>
        [Display(Name = "Created by")]
        public Guid? CreatedBy { get; set; }
    }
}