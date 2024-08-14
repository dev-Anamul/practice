using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    /// <summary>
    /// Admission Dto.
    /// </summary>
    public class AdmissionDto : BaseModel
    {
        /// <summary>
        /// AdmissionID.
        /// </summary>
        public Guid AdmissionID { get; set; }

        /// <summary>
        /// Admission note of the client.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Admission Note")]
        public string AdmissionNote { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Beds.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int? BedID { get; set; }

        public int DepartmentID { get; set; }

        public int FirmID { get; set; }

        public int WardID { get; set; }

        /// <summary>
        /// Admission Date
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Admission Date")]
        //[IfAgeBelow18]
        public DateTime? AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }
        public string DischargeNote { get; set; }

        public Guid ClientID { get; set; }

        public string Type { get; set; }
    }
}