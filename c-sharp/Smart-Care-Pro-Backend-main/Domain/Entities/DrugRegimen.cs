using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Lion
 * Last modified: 06.04.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// DrugRegimen entity.
    /// </summary>
    public class DrugRegimen : BaseModel
    {
        /// <summary>
        /// Primary Key of the table DrugRegimen.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Description of the DrugRegimen.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        public string Description { get; set; }

        /// <summary>
        /// Regimen For DrugRegimen.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public RegimenFor RegimenFor { get; set; }

        /// <summary>
        /// Special Drugs of Drug Regimen.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<SpecialDrug> SpecialDrugs { get; set; }
    }
}