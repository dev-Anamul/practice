using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
   /// DrugUtility entity.
   /// </summary>
   public class DrugUtility : BaseModel
   {
      /// <summary>
      /// Primary Key of the table DrugUtility.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the DrugUtility.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Drug Utility")]
      public string Description { get; set; }

      /// <summary>
      /// Drug Definitions of Drug Utility.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<GeneralDrugDefinition> DrugDefinitions { get; set; }
   }
}