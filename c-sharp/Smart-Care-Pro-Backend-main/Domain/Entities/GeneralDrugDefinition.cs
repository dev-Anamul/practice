using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains information of GeneralDrugDefinition entity.
   /// </summary>
   public class GeneralDrugDefinition : BaseModel
   {
      /// <summary>
      /// Primary Key of the table GeneralDrugDefinitions.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the GeneralDrugDefinition.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(200)]
      public string Description { get; set; }

      /// <summary>
      /// Strength of the DrugDefinition.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      public decimal Strength { get; set; }

      /// <summary>
      /// Foreign key. Primary key of DrugDosageUnits.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DosageUnitId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DosageUnitId")]
      [JsonIgnore]
      public virtual DrugDosageUnit DrugDosageUnit { get; set; }

      /// <summary>
      /// Foreign key. Primary key of DrugFormulations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int FormulationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FormulationId")]
      [JsonIgnore]
      public virtual DrugFormulation DrugFormulation { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table DrugUtilities.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DrugUtilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DrugUtilityId")]
      [JsonIgnore]
      public virtual DrugUtility DrugUtility { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table GenericDrugs.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int GenericDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("GenericDrugId")]
      [JsonIgnore]
      public virtual GenericDrug GenericDrug { get; set; }

      // <summary>
      /// System relevances of GeneralDrugDefinition.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SystemRelevance> SystemRelevances { get; set; }

      /// <summary>
      /// General Medications of GeneralDrugDefinitions.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Medication> Medications { get; set; }

      /// <summary>
      /// Medicine Brands of GeneralDrugDefinitions.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicineBrand> MedicineBrands { get; set; }

      /// <summary>
      /// Dispensed Items of General Drug Definitions.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DispensedItem> DispensedItems { get; set; }
   }
}