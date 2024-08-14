using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 24.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ChiefComplaint entity.
   /// </summary>
   public class ChiefComplaint : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ChiefComplaints.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Chief complaints of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      [Display(Name = "Chief Complaints")]
      public string ChiefComplaints { get; set; }

      /// <summary>
      /// History of chief complaints of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      [Display(Name = "History Of Chief Complaints")]
      public string HistoryOfChiefComplaint { get; set; }

      /// <summary>
      /// History summary of the client.
      /// </summary>
      [Display(Name = "History Summary")]
      public string HistorySummary { get; set; }

      /// <summary>
      /// Examination summary of the client.
      /// </summary>
      [Display(Name = "Examination Summary")]
      public string ExaminationSummary { get; set; }

      /// <summary>
      /// HIV status of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "HIV Status")]
      public HIVTestResult HIVStatus { get; set; }

      /// <summary>
      /// Last test date of HIV of the client.
      /// </summary>
      [DataType(DataType.Date)]
      //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Last HIV test date")]
      [IfFutureDate]
      public DateTime? LastHIVTestDate { get; set; }

      /// <summary>
      /// Location of testing of the client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Testing location")]
      public string TestingLocation { get; set; }

      /// <summary>
      /// Date of potential HIV exposure of the client.
      /// </summary>
      [DataType(DataType.Date)]
      //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Potential HIV exposure date")]
      public DateTime? PotentialHIVExposureDate { get; set; }

      /// <summary>
      /// Recency type of the client.
      /// </summary>
      [Display(Name = "Recency Type")]
      public RecencyType? RecencyType { get; set; }

      /// <summary>
      /// Date of recency test of the client.
      /// </summary>
      [DataType(DataType.Date)]
      //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Recency test date")]
      public DateTime? RecencyTestDate { get; set; }

      /// <summary>
      /// Exposure status of a child.
      /// </summary>
      [Display(Name = "Child's exposure status")]
      public ChildExposureStatus? ChildExposureStatus { get; set; }

      /// <summary>
      /// Is child given ARV.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is child given ARV")]
      public bool IsChildGivenARV { get; set; }

      /// <summary>
      /// Is mother given ARV.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is mother given ARV")]
      public bool IsMotherGivenARV { get; set; }

      /// <summary>
      /// Date of NAT test of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "NAT test date")]
      public DateTime? NATTestDate { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public PositiveNegative? NATResult { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [Display(Name = "TB Screenings")]
      public TBScreening? TBScreenings { get; set; }

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

      /// <summary>
      /// Exposures of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Exposure> Exposures { get; set; }

      /// <summary>
      /// Exposure List of the client.
      /// </summary>
      [NotMapped]
      public int[] ExposureList { get; set; }

      [NotMapped]
      public List<KeyPopulationDemographic> keyPopulationDemographics { get; set; }

      [NotMapped]
      public List<HIVRiskScreening> hIVRiskScreenings { get; set; }
   }
}