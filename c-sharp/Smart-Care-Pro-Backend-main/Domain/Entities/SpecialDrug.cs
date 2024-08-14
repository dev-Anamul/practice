using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// SpecialDrug entity.
   /// </summary>
   public class SpecialDrug : BaseModel
   {
      /// <summary>
      /// Primary Key of the table SpecialDrug.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Display Title of the SpecialDrug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(500)]
      [Display(Name = "Display Title")]
      public string Description { get; set; }

      /// <summary>
      /// Strength of the SpecialDrug.
      /// </summary>
      [Display(Name = "Strength")]
      [StringLength(90)]
      public string Strength { get; set; }

      /// <summary>
      /// Dosage UnitId For the SpecialDrug.
      /// </summary>
      public int? DosageUnitId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DosageUnitId")]
      //[JsonIgnore]
      public virtual DrugDosageUnit DrugDosageUnit { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table DrugFormulation.
      /// </summary>
      public int? FormulationId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FormulationId")]
      //[JsonIgnore]
      public virtual DrugFormulation DrugFormulation { get; set; }

      /// <summary>
      /// Foreign key. Primary key of DrugRegimen.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.DrugRegimens)]
      public int RegimenId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("RegimenId")]
      [JsonIgnore]
      public virtual DrugRegimen DrugRegimen { get; set; }

      /// <summary>
      /// ARTDrugAdherences of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ARTDrugAdherence> ARTDrugAdherences { get; set; }

      /// <summary>
      /// General Medications of Prescription.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Medication> Medications { get; set; }

      /// <summary>
      /// Medicine Brands of MedicineManufacturer.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicineBrand> MedicineBrands { get; set; }
   }
}