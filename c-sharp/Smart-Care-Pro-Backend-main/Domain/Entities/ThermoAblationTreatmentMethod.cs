using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

namespace Domain.Entities
{
    public class ThermoAblationTreatmentMethod : BaseModel
    {
        /// <summary>
        /// Primary key of the table ThermoAblation table.
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
        /// ThermoAblationTreatmentMethod of the ThermoAblation.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ThermoAblation> ThermoAblations { get; set; }
    }
}

