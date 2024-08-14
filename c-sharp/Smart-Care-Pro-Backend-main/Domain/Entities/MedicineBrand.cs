using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 27.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// MedicineBrand entity.
   /// </summary>
   public class MedicineBrand : BaseModel
   {
      /// <summary>
      /// Primary Key of the table MedicineBrand.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Brand Name of the MedicineBrand.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(30)]
      [DisplayName("Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table DrugDefinition.
      /// </summary>
      public int? DrugDefinitionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DrugDefinitionId")]
      [JsonIgnore]
      public virtual GeneralDrugDefinition GeneralDrugDefinition { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table SpecialDrug.
      /// </summary>
      public int? SpecialDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SpecialDrugId")]
      [JsonIgnore]
      public virtual SpecialDrug SpecialDrug { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the MedicineManufacturer table.
      /// </summary>
      public int MedicineManufacturerId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("MedicineManufacturerId")]
      [JsonIgnore]
      public virtual MedicineManufacturer MedicineManufacturer { get; set; }

      /// <summary>
      /// Dispensed Items of Medicine Brand.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DispensedItem> DispensedItems { get; set; }
   }
}