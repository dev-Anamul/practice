using Domain.Entities;
using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 03.04.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Domain.Dto
{
    public class ChiefComplaintDto : EncounterBaseModel
    {
        public Guid InteractionId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Chief Complaints")]
        public string ChiefComplaints { get; set; }

        [StringLength(1000)]
        [Display(Name = "History Of Chief Complaints")]
        public string HistoryOfChiefComplaint { get; set; }

        [Display(Name = "History Summary")]
        public string HistorySummary { get; set; }

        [Display(Name = "Examination Summary")]
        public string ExaminationSummary { get; set; }

        [Display(Name = "HIV Status")]
        public HIVTestResult HIVStatus { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Last HIV test date")]
        [IfFutureDate]
        public DateTime? LastHIVTestDate { get; set; }

        [StringLength(90)]
        [Display(Name = "Testing location")]
        public string TestingLocation { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Potential HIV exposure date")]
        public DateTime? PotentialHIVExposureDate { get; set; }

        [Display(Name = "Recency Type")]
        public RecencyType? RecencyType { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Recency test date")]
        public DateTime? RecencyTestDate { get; set; }

        [Display(Name = "Child's exposure status")]
        public ChildExposureStatus? ChildExposureStatus { get; set; }


        [Display(Name = "Is child given ARV")]
        public bool IsChildGivenARV { get; set; }


        [Display(Name = "Is mother given ARV")]
        public bool IsMotherGivenARV { get; set; }

        [Display(Name = "TB Screenings")]
        public TBScreening? TBScreenings { get; set; }

        /// <summary>
        /// Date of NAT test of the client.
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "NAT test date")]
        public DateTime? NATTestDate { get; set; }

        public PositiveNegative NATResult { get; set; }

        public int[] ExposureList { get; set; }

        public List<QuestionsDto>? QuestionsList { get; set; }

        public int[] KeyPopulations { get; set; }

        public Guid EncounterId { get; set; }

        public Guid ClientId { get; set; }

        public virtual ChiefComplaint ChiefComplaint { get; set; }
    }
}