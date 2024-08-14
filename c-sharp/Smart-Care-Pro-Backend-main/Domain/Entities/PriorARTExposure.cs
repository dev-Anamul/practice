using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// PriorARTExposure entity.
    /// </summary>
    public class PriorARTExposure : EncounterBaseModel
    {
        /// <summary>
        /// Primary Key of the table PriorARTExposures.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// State reason for prior ART Exposure.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "State reason for prior ART exposure")]
        public ExposureReason ExposureReason { get; set; }

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

        /// <summary>
        /// TakenARTDrugs of the ART.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TakenARTDrug> TakenARTDrugs { get; set; }

        /// <summary>
        /// NotMapped   
        /// </summary>
        [NotMapped]
        public int[] ARTTakenDrugList { get; set; }

        /// <summary>
        /// NotMapped
        /// </summary>
        [NotMapped]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateTaken { get; set; }

        /// <summary>
        /// NotMapped
        /// </summary>
        [NotMapped]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// NotMapped
        /// </summary>
        [NotMapped]
        public string StoppingReason { get; set; }
    }
}