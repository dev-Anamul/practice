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
    public class Leeps : EncounterBaseModel
    {

        /// <summary>
        /// Primary key of the table ThermoAblation table.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Condition of IsLesionExtendsIntoTheCervicalOs.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Lesion Extends Into The Cervical Os")]
        public bool IsLesionExtendsIntoTheCervicalOs { get; set; }

        /// <summary>
        /// Condition of IsQueryICC.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Query ICC")]
        public bool IsQueryICC { get; set; }

        /// <summary>
        /// Condition of IsAtypicalVessels.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Atypical Vessels")]
        public bool IsAtypicalVessels { get; set; }

        /// <summary>
        /// Condition of IsPunctationsOrMoasicm.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Punctations Or Moasicm")]
        public bool IsPunctationsOrMoasicm { get; set; }

        /// <summary>
        /// Condition of IsLesionCovers.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Lesion Covers")]
        public bool IsLesionCovers { get; set; }

        /// <summary>
        /// Condition of IsPoly.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Poly")]
        public bool IsPoly { get; set; }

        /// <summary>
        /// Condition of IsLesionTooLargeForThermoAblation.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Lesion Too Large For Thermo-Ablation")]
        public bool IsLesionTooLargeForThermoAblation { get; set; }

        /// <summary>
        /// Condition of IsLesionToThickForAblation  .
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Lesion To Thick For Ablation ")]
        public bool IsLesionToThickForAblation { get; set; }

        /// <summary>
        /// Condition of IsPostLEEPLesion.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Post LEEP Lesion")]
        public bool IsPostLEEPLesion { get; set; }

        /// <summary>
        /// Condition of IsConsentObtained  .
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is Consent Obtained ")]
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
        public int? EstimatedTimeForProcedure { get; set; }

        /// <summary>
        /// Status of AssesmentComment.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "AssesmentComment")]
        public string AssesmentComment { get; set; }

        /// <summary>
        /// Status of ProcedureComment.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "ProcedureComment")]
        public string ProcedureComment { get; set; }

        /// <summary>
        /// Status of LeepTreatmentMethodId.
        /// </summary>
        public int? LeepTreatmentMethodId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("LeepTreatmentMethodId")]
      //  [JsonIgnore]
        public virtual LeepsTreatmentMethod LeepsTreatmentMethod { get; set; }

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