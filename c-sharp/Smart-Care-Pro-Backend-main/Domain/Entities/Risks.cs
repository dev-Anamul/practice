using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 18.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// PEPRisk entity.
    /// </summary>
    public class Risks : BaseModel
    {
        /// <summary>
        /// Primary key of the table PEPRisks.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// PEP risk of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "PEP risk")]
        public string Description { get; set; }

        /// <summary>
        /// PEP risk statuses of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<RiskStatus> RiskStatuses { get; set; }
    }
}