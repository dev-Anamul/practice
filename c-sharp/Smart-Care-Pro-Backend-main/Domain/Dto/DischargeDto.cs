using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    /// <summary>
    /// Discharge Dto.
    /// </summary>
    public class DischargeDto : BaseModel
    {
        /// <summary>
        /// AdmissionID.
        /// </summary>
        public Guid AdmissionId { get; set; }

        /// <summary>
        /// IPDAdmissionID.
        /// </summary>
        public int IPDAdmissionId { get; set; }

        /// <summary>
        /// Admission note of the client.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Admission Note")]
        public string AdmissionNote { get; set; }

        /// <summary>
        /// Date of IPD discharge of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "IPD Discharge Date")]
        public DateTime IPDDischargeDate { get; set; }

        /// <summary>
        /// Discharge note of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(1000)]
        [Display(Name = "Discharge Note")]
        public string DischargeNote { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Beds.
        /// </summary>
        public int BedId { get; set; }
        public Guid ClientId { get; set; }
    }
}