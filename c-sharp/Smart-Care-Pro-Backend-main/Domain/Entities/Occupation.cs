using Domain.Validations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Occupation entity.
    /// </summary>
    public class Occupation : BaseModel
    {
        /// <summary>
        /// Primary key of the table Occupations.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Occupation of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        [IfNotAlphabet]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual IEnumerable<Client> Clients { get; set; }
    }
}