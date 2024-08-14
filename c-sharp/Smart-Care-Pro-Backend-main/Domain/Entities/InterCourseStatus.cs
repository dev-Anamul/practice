using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

namespace Domain.Entities
{
    public class InterCourseStatus : BaseModel
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
        /// InterCourseStatus of the GynObsHistories
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<GynObsHistory> GynObsHistory { get; set; }
    }
}
