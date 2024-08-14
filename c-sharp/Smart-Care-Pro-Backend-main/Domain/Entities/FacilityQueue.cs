using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
* Created by   : Lion
* Date created : 12.11.2023
* Modified by  :
* Last modified:
* Reviewed by  :
* Date reviewed:
*/

namespace Domain.Entities
{
    public class FacilityQueue : BaseModel
    {
        /// <summary>
        /// Primary key of the table FacilityQueue table.
        /// </summary
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of Facilities Service Point
        /// </summary>
        [Required]
        [StringLength(90)]
        public string Description { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Facilities.
        /// </summary>
        [Required]
        public int FacilityId { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        [ForeignKey("FacilityId")]
        [JsonIgnore]
        public Facility Facility { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ServiceQueue> ServiceQueue { get; }
    }
}