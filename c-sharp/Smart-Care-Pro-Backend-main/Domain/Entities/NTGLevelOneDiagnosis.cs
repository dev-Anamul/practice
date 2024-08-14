using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 03.01.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    public class NTGLevelOneDiagnosis : BaseModel
    {
        /// <summary>
        /// Primary key of the table NTGLevelOneDiagnoses.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the NTG level one diagnosis.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Diagnosis")]
        public string Description { get; set; }

        /// <summary>
        /// Identified allergies of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<NTGLevelTwoDiagnosis> NTGLevelTwoDiagnoses { get; set; }
    }
}