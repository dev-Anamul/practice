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
    public  class ThermoAblation : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table ThermoAblation table.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Condition of IsConsentObtained.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is-ConsentObtained")]
        public bool IsConsentObtained { get; set; }

        /// <summary>
        /// Condition of IsClientCounseled.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Client Counseled")]
        public bool IsClientCounseled { get; set; }

        /// <summary>
        /// Status of EstimatedTimeForProcedure.
        /// </summary>
        [Column(TypeName = "Smallint")]
        [Display(Name = "Estimated Time For Procedure")]
        public int EstimatedTimeForProcedure { get; set; }

        /// <summary>
        /// Status of ThermoAblationComment.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Thermo-Ablation Comment")]
        public string ThermoAblationComment { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table ThermoAblationTreatmentMethodId.
        /// </summary>
        public int? ThermoAblationTreatmentMethodId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ThermoAblationTreatmentMethodId")]
     //   [JsonIgnore]
        public virtual ThermoAblationTreatmentMethod ThermoAblationTreatmentMethod { get; set; }

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
    }
}