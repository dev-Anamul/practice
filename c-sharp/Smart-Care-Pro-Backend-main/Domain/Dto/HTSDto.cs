using Domain.Entities;
using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    /// HTS dto.
    /// </summary>
    public class HTSDto
    {
        /// <summary>
        /// The row is assigned to the client's consent has taken or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public YesNoUnknown HasConsented { get; set; }

        /// <summary>
        /// The row is assigned to the test type of HIV.
        /// </summary>
        public HIVTest TestType { get; set; }

        /// <summary>
        /// The row is assigned to the tested as.
        /// </summary>
        public TestedAs TestedAs { get; set; }

        /// <summary>
        /// The row is assigned to the test serial number of HTS.
        /// </summary>
        [StringLength(30)]
        [Display(Name = "Input test serial no.")]
        public string TestSerialNo { get; set; }

        /// <summary>
        /// The row is assigned to the source of a client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public ClientSource ClientSource { get; set; }

        /// <summary>
        /// The row is assigned to the last tested date.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Last test date")]
        [IfFutureDate]
        public DateTime LastTested { get; set; }

        /// <summary>
        ///  The row is assigned to the partner's last test date.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Partner's last test date")]
        [IfFutureDate]
        public DateTime PartnerLastTestDate { get; set; }

        /// <summary>
        /// The row is assigned to the last test result.
        /// </summary>
        public HIVTestResult LastTestResult { get; set; }

        /// <summary>
        /// The row is assigned to the ART status.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public ARTStatus ARTStatus { get; set; }

        /// <summary>
        /// The row is assigned to the partner's HIV status.
        /// </summary>
        public HIVTestResult PartnerHIVStatus { get; set; }

        /// <summary>
        /// The row is assigned to the client has counselled or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public YesNo HasCounselled { get; set; }

        /// <summary>
        /// The row is assigned to the test modality.
        /// </summary>
        public HIVTestType TestModality { get; set; }

        /// <summary>
        /// The row is assigned to the HIV test result.
        /// </summary>
        public HIVTestResult TestResult { get; set; }

        /// <summary>
        /// The row is assigned to the HIV type.
        /// </summary>
        public HIVTypes HIVType { get; set; }

        /// <summary>
        /// The row is assigned to whether the DNA PCR sample is collected or not. 
        /// </summary>
        public bool IsDNAPCRSampleCollected { get; set; }

        /// <summary>
        /// The row is assigned to the sample collection date.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "DNA CPR Sample collection date")]
        public DateTime SampleCollectionDate { get; set; }

        /// <summary>
        /// The row is assigned to whether the result is received or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public bool IsResultReceived { get; set; }

        /// <summary>
        /// The row is assigned to the date of retest.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Retest date")]
        public DateTime RetestDate { get; set; }

        /// <summary>
        /// The row assigned to the consent for sms.
        /// </summary>
        public bool ConsentForSMS { get; set; }

        ///<summary>
        /// Foreign key. Primary key of the table HIVTestingReasons.
        /// </summary>
        public int HIVTestingReasonID { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("HIVTestingReasonID")]
        public virtual HIVTestingReason HIVTestingReason { get; set; }

        ///<summary>
        /// Foreign key. Primary key of the table ServicePoints.
        /// </summary>
        public int ServicePointID { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ServicePointID")]
        public virtual ServicePoint ServicePoint { get; set; }

        ///<summary>
        /// Foreign key. Primary key of the table VisitTypes.
        /// </summary>
        public int VisiteTypeID { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("VisiteTypeID")]
        public virtual VisitType VisitType { get; set; }

        /// <summary>
        /// Risk assessments of a client.
        /// </summary>
        public virtual IEnumerable<RiskAssessment> RiskAssessments { get; set; }
    }
}