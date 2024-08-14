using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// IdentifiedCurrentDeliveryComplications entity.
    /// </summary>
    public class IdentifiedCurrentDeliveryComplication : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table IdentifiedCurrentDeliveryComplications.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Complications of IdentifiedCurrentDeliveryComplications.
        /// </summary>
        public DeliveryComplications Complications { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table MotherDeliverySummaries.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Other")]
        public string Other { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table MotherDeliverySummaries.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public Guid DeliveryId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("DeliveryId")]
        [JsonIgnore]
        public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }
    }
}