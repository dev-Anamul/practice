using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 05.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// WHOStagesCondition entity.
    /// </summary>
    public class WHOStagesCondition : BaseModel
    {
        /// <summary>
        /// Primary Key of the table WHOStagesConditions.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Conditions of the WHOStagesCondition.
        /// </summary>
        [StringLength(90)]
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [ForeignKey("Description")]
        public string Description { get; set; }

        /// <summary>
        /// For adolocent of the WHOStagesCondition.
        /// </summary>
        [Display(Name = "For adolocent")]
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public bool ForAdolescent { get; set; }

        /// <summary>
        /// For adult of the WHOStagesCondition.
        /// </summary>
        [Display(Name = "For adult")]
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public bool ForAdult { get; set; }

        /// <summary>
        /// For pregnant of the WHOStagesCondition.
        /// </summary>
        [Display(Name = "For pregnant")]
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public bool ForPregnant { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table WHOClinicalStages.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int WHOClinicalStageId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("WHOClinicalStageId")]
        [JsonIgnore]
        public virtual WHOClinicalStage WHOClinicalStage { get; set; }

        /// <summary>
        /// WhoConditions of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<WHOCondition> WhoConditions { get; set; }
    }
}