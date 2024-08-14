using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
   /// MedicineManufacturer entity.
   /// </summary>
   public class MedicineManufacturer : BaseModel
   {
      /// <summary>
      /// Primary Key of the table MedicineManufacturer.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Manufacturer Name of the MedicineManufacturer.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(50)]
      [DisplayName("Description")]
      public string Description { get; set; }

      /// <summary>
      /// Medicine Brands of MedicineManufacturer.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicineBrand> MedicineBrands { get; set; }
   }
}