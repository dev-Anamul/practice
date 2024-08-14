using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Tomas
 * Last modified: 17.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Allergy entity.
    /// </summary>
    public class Allergy : BaseModel
    {
        /// <summary>
        /// Primary key of the table Allergies.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Description of an allergy.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Description")]
        public string Description { get; set; }

      /// <summary>
      /// Identified allergies of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedAllergy> IdentifiedAllergies { get; set; }
   }
}