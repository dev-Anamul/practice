using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    public class PreScreeningVisit : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table PreScreeningVisit.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Pre-Screening Visit")]
        public PreScreeningVisitType PreScreeningVisitType { get; set; }

        /// <summary>
        /// Condition of IsPostThermoAblation.
        /// </summary>
        [Display(Name = "Is-Post ThermoAblation")]
        public bool? IsPostThermoAblation { get; set; }

        /// <summary>
        /// Status of IsPostBiopsy.
        /// </summary>
        [Display(Name = "Is-Post Biopsy")]
        public bool? IsPostBiopsy { get; set; }

        /// <summary>
        /// Status of IsPostLeep.
        /// </summary>
        [Display(Name = "Is-Post Leep")]
        public bool? IsPostLeep { get; set; }

        /// <summary>
        /// Status of IsRoutineFollowup
        /// </summary>
        [Display(Name = "Is-Routine Followup")]
        public bool? IsRoutineFollowup { get; set; }

        /// <summary>
        /// Status of IsPostAntibiotic
        /// </summary>
        [Display(Name = "Is-Post Antibiotic")]
        public bool? IsPostAntibiotic { get; set; }

        /// <summary>
        /// Status of Other
        /// </summary>
        [Display(Name = "Other")]
        [StringLength(1000)]
        public string Other { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Clients.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public Guid ClientId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ClientId")]
        [JsonIgnore]
        public virtual Client Client { get; set; }

        [NotMapped]
        public bool? IsOnART { get; set; }
    }
}