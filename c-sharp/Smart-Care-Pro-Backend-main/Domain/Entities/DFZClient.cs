using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 09.01.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// DFZClient Entity.
    /// </summary>
    public class DFZClient : BaseModel
    {
        /// <summary>
        /// Primary key of the table DFZClient.
        /// </summary>      
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// HospitalNo of the DFZClient.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Hospital No")]
        public string HospitalNo { get; set; }

        /// <summary>
        /// ServiceNo of the DFZClient.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Service No")]
        public string ServiceNo { get; set; }

        /// <summary>
        /// Unit of the DFZClient.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// Foreign key.Primary key of the entity Client.
        /// </summary>
        [ForeignKey("Oid")]
        [JsonIgnore]
        public virtual Client? Client { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Defence Rank.
        /// </summary>
        public int? DFZRankId { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity DFZ Patient Type.
        /// </summary>
        public int? DFZPatientTypeId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DFZPatientTypeId")]
        [JsonIgnore]
        public virtual DFZPatientType DFZPatientType { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DFZRankId")]
        [JsonIgnore]
        public virtual DFZRank DFZRank { get; set; }

        [NotMapped]
        public int? ArmForceId { get; set; }
    }
}