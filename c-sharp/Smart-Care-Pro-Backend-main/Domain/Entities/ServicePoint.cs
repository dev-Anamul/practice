using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Brian
 * Last modified: 19.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// ServicePoints entity.
    /// </summary>
    public class ServicePoint : BaseModel
    {
        /// <summary>
        /// Primary key of the table ServicePoints.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Service point of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Service point")]
        public string Description { get; set; }

        /// <summary>
        /// HTS of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HTS> HTS { get; set; }

        /// <summary>
        /// ReferralModule of a Service Points.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ReferralModule> ReferralModules { get; set; }
    }
}