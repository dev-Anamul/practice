using System.ComponentModel.DataAnnotations;
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
    /// Contains details of ArmedForceService.
    /// </summary>
    public class ArmedForceService : BaseModel
    {
        /// <summary>
        /// Primary key of ArmedForceService table.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Description of the Armed force service.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// DFZ patient types of the Armed force service
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DFZPatientType> DFZPatientTypes  { get; set; }
    }
}