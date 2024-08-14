using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Lion
* Date created : 12.08.2023
* Modified by  : 
* Last modified: 
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
    /// <summary>
    /// Question entity.
    /// </summary>
    public class Question : BaseModel
    {
        /// <summary>
        /// Primary key of the table Questions.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Questions.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Questions")]
        public string Description { get; set; }

        ///// <summary>
        ///// HIVRiskScreenings of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HIVRiskScreening> HIVRiskScreenings { get; set; }
    }
}