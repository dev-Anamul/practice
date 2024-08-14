using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Contains details of Acetones.
    /// </summary>
    public class Acetone : BaseModel
    {
        /// <summary>
        /// Primary key of Acetones table.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// Acetone Details of the Patients.
        /// </summary>
        [Required(ErrorMessage = "The Description is required!")]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Acetone measurement time of the patients.
        /// </summary>
        [Required(ErrorMessage = "The Time is required!")]
        [Display(Name = "Acetone measurement Time")]
        public long AcetoneTime { get; set; }

        /// <summary>
        /// Foreign key, Primary key of the Partograph table.
        /// </summary>
        public Guid PartographId { get; set; }

        [ForeignKey("PartographId")]
        [JsonIgnore]
        public virtual Partograph Partograph { get; set; }
    }
}