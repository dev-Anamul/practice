using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    /// Contains information of the DFZDependents.
    /// </summary>
    public class DFZDependent : BaseModel
    {
        /// <summary>
        /// Primary key of the table DFZDependents.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// The property is Relation with DFZ Client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Relation Type")]
        public RelationType RelationType { get; set; }

        /// <summary>
        /// Description of a DFZ dependent.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Description of Relation")]
        public string Description { get; set; }

        /// <summary>
        /// HospitalNo of the DFZ dependent.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Hospital No")]
        public string HospitalNo { get; set; }

        /// <summary>
        /// The property is unique Identity of DFZ Client.
        /// </summary>
        public Guid PrincipalId { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DFZ dependent client.
        /// </summary>
        public Guid DependentClientId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DependentClientId")]
        [JsonIgnore]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table DFZ patient type.
        /// </summary>
        public int DFZPatientTypeId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DFZPatientTypeId")]
        [JsonIgnore]
        public virtual DFZPatientType DFZPatientType { get; set; }
    }
}