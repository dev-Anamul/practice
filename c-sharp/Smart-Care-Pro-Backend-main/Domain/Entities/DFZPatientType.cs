using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    /// Contains details of DFZPatientType.
    /// </summary>
    public class DFZPatientType : BaseModel
    {
        /// <summary>
        /// Primary key of DFZPatientType table.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Description  of the DFZ patient type.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// IsDependent status of the DFZ patient type.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public YesNo IsDependent { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Armed force service.
        /// </summary>
        public int ArmedForceId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ArmedForceId")]
        [JsonIgnore]
        public virtual ArmedForceService ArmedForceService { get; set; }

        /// <summary>
        /// DFZ ranks of the DFZ patient type
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DFZRank> DFZRanks { get; set; }

        /// <summary>
        /// DFZ dependents of the DFZ patient type
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DFZDependent> DFZDependents { get; set; }
    }
}