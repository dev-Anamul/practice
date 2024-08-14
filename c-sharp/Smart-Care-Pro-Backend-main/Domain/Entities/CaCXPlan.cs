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
    public class CaCXPlan : EncounterBaseModel
    {

        /// <summary>
        /// Primary key of the table PreScreeningVisit.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Condition of IsClientReffered.
        /// </summary>
        [Display(Name = "Is Client Reffered")]
        public bool IsClientReffered { get; set; }

        /// <summary>
        /// Status of IsLesionExtendsIntoTheCervicalOs.
        /// </summary>
        [Display(Name = "Is Lesion Extends Into The CervicalOs")]
        public bool IsLesionExtendsIntoTheCervicalOs { get; set; }

        /// <summary>
        /// Condition of IsQueryICC.
        /// </summary>
        [Display(Name = "Is Query ICC")]
        public bool IsQueryICC { get; set; }

        /// <summary>
        /// Status of IsAtypicalVessels.
        /// </summary>
        [Display(Name = "Is Atypical Vessels")]
        public bool IsAtypicalVessels { get; set; }

        /// <summary>
        /// Status of IsPunctationsOrMoasicm.
        /// </summary>
        [Display(Name = "Is Punctations Or Moasicm")]
        public bool IsPunctationsOrMoasicm { get; set; }

        /// <summary>
        /// Condition of IsLesionCovers.
        /// </summary>
        [Display(Name = "Is Lesion Covers")]
        public bool IsLesionCovers { get; set; }

        /// <summary>
        /// Status of IsPoly.
        /// </summary>
        [Display(Name = "Is Poly")]
        public bool IsPoly { get; set; }

        /// <summary>
        /// Status of IsLesionTooLargeForThermoAblation.
        /// </summary>
        [Display(Name = "Is Lesion Too Large For Thermo-Ablation")]
        public bool IsLesionTooLargeForThermoAblation { get; set; }

        /// <summary>
        /// Condition of IsLesionToThickForAblation .
        /// </summary>
        [Display(Name = "Is Lesion To Thick For Ablation ")]
        public bool IsLesionToThickForAblation { get; set; }

        /// <summary>
        /// Status of IsPostLEEPLesion.
        /// </summary>
        [Display(Name = "Is Post LEEP Lesion")]
        public bool IsPostLEEPLesion { get; set; }

        /// <summary>
        /// Status of Others
        /// </summary>
        [Display(Name = "Others")]
        [StringLength(90)]
        public string Others { get; set; }

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