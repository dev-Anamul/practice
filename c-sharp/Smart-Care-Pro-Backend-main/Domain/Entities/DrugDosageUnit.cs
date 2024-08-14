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
   /// DrugDosageUnit entity.
   /// </summary>
   public class DrugDosageUnit : BaseModel
   {
      /// <summary>
      /// Primary Key of the table DrugDosageUnit.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the DrugDosageUnit.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// Drug Definitions of Drug Dosage Unit.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SpecialDrug> SpecialDrugs { get; set; }

      /// <summary>
      /// Drug Definitions of Drug Dosage Unit.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<GeneralDrugDefinition> DrugDefinitions { get; set; }
   }
}