using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// DrugFormulation entity.
    /// </summary>
    public class DrugFormulation : BaseModel
    {
        /// <summary>
        /// Primary Key of the table DrugFormulation.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Description of the DrugFormulation.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        public string Description { get; set; }

        /// <summary>
        /// Special Drugs of DrugFormulation.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<SpecialDrug> SpecialDrugs { get; set; }

        /// <summary>
        /// Special Drugs of DrugDefinition.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<GeneralDrugDefinition> DrugDefinitions { get; set; }
    }
}