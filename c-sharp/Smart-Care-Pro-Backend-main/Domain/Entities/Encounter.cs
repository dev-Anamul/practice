using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Encounter entity.
    /// </summary>
    public class Encounter : BaseModel
    {
        /// <summary>
        /// Primary Key of the table Encounters.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// Date of OPD visit of the client.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "OPD Visit Date")]
        public DateTime? OPDVisitDate { get; set; }

        /// <summary>
        /// Date of IPD admission of the client.
        /// </summary>
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "IPD Admission Date")]
        public DateTime? IPDAdmissionDate { get; set; }

        /// <summary>
        /// Admission note of the client.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Admission Note")]
        public string AdmissionNote { get; set; }

        /// <summary>
        /// Admission reason of the client.
        /// </summary>
        [Display(Name = "Admission Reason")]
        public string AdmissionReason { get; set; }

        /// <summary>
        /// Referral details of the client.
        /// </summary>
        [Display(Name = "Referral Details")]
        public string ReferralDetails { get; set; }

        /// <summary>
        /// Date of IPD discharge of the client.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "IPD Discharge Date")]
        public DateTime? IPDDischargeDate { get; set; }

        /// <summary>
        /// Discharge note of the client.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Discharge Note")]
        public string DischargeNote { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Beds. 
        /// </summary>
        public int? BedId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("BedId")]
        [JsonIgnore]
        public virtual Bed Bed { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Clients. 
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ClientId")]
        [JsonIgnore]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Interaction> Interactions { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Partograph> Partographs { get; set; }
    }
}