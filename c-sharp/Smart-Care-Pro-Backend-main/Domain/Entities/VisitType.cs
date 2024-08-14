using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 12.09.2022
* Modified by  : Stephan
* Last modified: 12.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
    /// <summary>
    /// VisitType entity.
    /// </summary>
    public class VisitType : BaseModel
    {
        /// <summary>
        /// Primary key of the table VisitTypes.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Type of visits of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// HTS of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HTS> HTS { get; set; }
    }
}