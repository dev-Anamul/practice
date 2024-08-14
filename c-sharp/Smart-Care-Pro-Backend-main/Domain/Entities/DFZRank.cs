using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Contains details of DefenceRank.
    /// </summary>
    public class DFZRank : BaseModel
    {
        /// <summary>
        /// Primary key of DFZRank table.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Abbreviated form of the DFZ Rank.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(25)]
        [Display(Name = "AbbreviatedRank")]
        public string AbbreviatedRank { get; set; }

        /// <summary>
        /// Description  of the DFZ Rank.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DFZ Patient Type.
        /// </summary>
        public int PatientTypeId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("PatientTypeId")]
        [JsonIgnore]
        public virtual DFZPatientType DFZPatientType  { get; set; }

        /// <summary>
        /// DFZ clients of the DFZ rank.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DFZClient> DFZClients { get; set; }
    }
}