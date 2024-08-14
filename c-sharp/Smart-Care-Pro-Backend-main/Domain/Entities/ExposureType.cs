using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
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
    /// ExposureType entity.
    /// </summary>
    public class ExposureType : BaseModel
    {
        /// <summary>
        /// Primary key of the table ExposureTypes.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Type of exposure of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Type of exposure")]
        public string Description { get; set; }

        /// <summary>
        /// Exposures of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Exposure> Exposures { get; set; }
    }
}