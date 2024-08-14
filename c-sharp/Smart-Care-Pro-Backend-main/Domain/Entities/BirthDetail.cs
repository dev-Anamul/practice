using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    public class BirthDetail : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of BirthDetails table.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Check the delivery is successful or not
        /// </summary>
        [Display(Name = "Is successful delivery")]
        public byte IsSuccessfulDelivery { get; set; }

        /// <summary>
        /// Details of the birth history
        /// </summary>
        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; } = null!;

        /// <summary>
        /// Gender details of the patients
        /// </summary>       
        [Display(Name = "Gender")]
        public Sex? Gender { get; set; }

        /// <summary>
        /// Weight of the baby at the time of birth
        /// </summary>
        [StringLength(15)]
        [Display(Name = "Weight")]
        public string Weight { get; set; } = null!;

        /// <summary>
        /// Check the delivery type
        /// </summary>
        [Display(Name = "Type of delivery")]
        public TypeOfDelivery? TypeOfDelivery { get; set; }

        /// <summary>
        /// Date of Birth
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Time of Birth
        /// </summary>
        [DataType(DataType.Time)]
        [Column(TypeName = "time")]
        [Display(Name = "Birth Time")]
        public TimeSpan? BirthTime { get; set; }

        /// <summary>
        /// Partograph Id for client side
        /// </summary>
        [NotMapped]
        public Guid PartographId { get; set; }
    }
}