using System.Text.Json.Serialization;
using System.ComponentModel;
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
    /// DrugRoutes entity.
    /// </summary>
    public class DrugRoute : BaseModel
    {
        /// <summary>
        /// Primary Key of the table DrugRoutes.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Route of the Drug.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DisplayName("Route")]
        public string Description { get; set; }

        /// <summary>
        /// General Medications of DrugRoute.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Medication> GeneralMedications { get; set; }

        /// <summary>
        /// Dispensed Items of DrugRoute.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DispensedItem> DispensedItems { get; set; }
    }
}