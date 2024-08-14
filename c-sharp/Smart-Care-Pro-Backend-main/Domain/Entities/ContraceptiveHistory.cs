using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Tomas
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// ContraceptiveHistory entity.
    /// </summary>
    public class ContraceptiveHistory : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table ContraceptiveHistories.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Contraceptives using time period.
        /// </summary>
        [Column(TypeName= "Tinyint")]
        public int? UsedFor { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Contraceptives.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int ContraceptiveId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("ContraceptiveId")]
        [JsonIgnore]
        public virtual Contraceptive Contraceptive { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table GynObsHistories.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public Guid GynObsHistoryId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("GynObsHistoryId")]
        [JsonIgnore]
        public virtual GynObsHistory GynObsHistory { get; set; }
    }
}