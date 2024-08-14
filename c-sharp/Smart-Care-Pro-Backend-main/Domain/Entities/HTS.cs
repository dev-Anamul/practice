using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
* Created by   : Stephan
* Date created : 12.09.2022
* Modified by  : Bella
* Last modified: 12.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HTS entity in the database.
   /// </summary>
   public class HTS : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table HTS.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Source of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Source of the client")]
      public ClientSource ClientSource { get; set; }

      /// <summary>
      /// Last test date of HIV of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Last test date")]
      [IfFutureDate]
      public DateTime? LastTested { get; set; }

      /// <summary>
      /// Last test result of the client.
      /// </summary>
      [Display(Name = "Last test result")]
      public HIVTestResult? LastTestResult { get; set; }

      /// <summary>
      /// HIV status of client's partner.
      /// </summary>
      [Display(Name = "Partner's HIV status")]
      public HIVTestResult? PartnerHIVStatus { get; set; }

      /// <summary>
      /// Last test date of client's partner.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Partner's last test date")]
      [IfFutureDate]
      public DateTime? PartnerLastTestDate { get; set; }

      /// <summary>
      /// Client has counselled or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has counselled")]
      public YesNo HasCounselled { get; set; }

      /// <summary>
      /// Client's consent has taken or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has consented")]
      public bool HasConsented { get; set; }

      /// <summary>
      /// Other reason of the client for not testing HIV.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Not Testing Reason")]
      public string NotTestingReason { get; set; }

      /// <summary>
      /// Test serial number of the client.
      /// </summary>
      [Display(Name = "Test no.")]
      [Column(TypeName="bigint")]
      public int? TestNo { get; set; }

      /// <summary>
      /// Determine Test Result.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Determine Test Result")]
      public TestResult DetermineTestResult { get; set; }

      /// <summary>
      /// Bioline Test Result.
      /// </summary>
      [Display(Name = "Bioline Test Result")]
      public TestResult? BiolineTestResult { get; set; }

      /// <summary>
      /// HIV type of the client.
      /// </summary>
      [Display(Name = "HIV type")]
      public HIVTypes? HIVType { get; set; }

      /// <summary>
      /// Client's DNA PCR sample is collected or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is DNA PCR sample collected")]
      public bool IsDNAPCRSampleCollected { get; set; }

      /// <summary>
      /// Date of DNA CPR sample collection of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "DNA PCR sample collection date")]
      [IfFutureDate]
      public DateTime? SampleCollectionDate { get; set; }

      /// <summary>
      /// Client's HIV test result is received or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is result received")]
      public bool IsResultReceived { get; set; }

      /// <summary>
      /// Retest date of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Retest date")]
      public DateTime? RetestDate { get; set; }

      /// <summary>
      /// Client's consent for sms is taken or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Consent for SMS")]
      public bool ConsentForSMS { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ClientTypes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ClientTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientTypeId")]
      [JsonIgnore]
      public virtual ClientType ClientType { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table VisitTypes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int VisitTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VisitTypeId")]
      [JsonIgnore]
      public virtual VisitType VisitType { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table ServicePoints.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ServicePointId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ServicePointId")]
      [JsonIgnore]
      public virtual ServicePoint ServicePoint { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table HIVTestingReasons.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int HIVTestingReasonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("HIVTestingReasonId")]
      [JsonIgnore]
      public virtual HIVTestingReason HIVTestingReason { get; set; }

      ///<summary>
      /// Foreign key. Primary key of the table HIVNotTestingReasons.
      /// </summary>        
      public int? HIVNotTestingReasonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("HIVNotTestingReasonId")]
      [JsonIgnore]
      public virtual HIVNotTestingReason HIVNotTestingReason { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
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
      /// Risk assessments of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<RiskAssessment> RiskAssessments { get; set; }

      /// <summary>
      /// List of risk assessments of the client.
      /// </summary>
      [NotMapped]
      public int[] RiskAssessmentList { get; set; }
   }
}