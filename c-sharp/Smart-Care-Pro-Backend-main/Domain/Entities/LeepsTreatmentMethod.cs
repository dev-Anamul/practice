using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 19.02.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    public class LeepsTreatmentMethod: BaseModel
    {
        /// <summary>
        /// Primary key of the table LeepsTreatmentMethod table.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Condition of Description.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Description")]
        [StringLength(90)]
        public string Description { get; set; }

        /// <summary>
        /// LeepsTreatmentMethod of the Leeps.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Leeps> Leeps{ get; set; }
    }
}
