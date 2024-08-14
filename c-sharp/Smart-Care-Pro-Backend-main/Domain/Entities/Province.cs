using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Brian
 * Last modified: 19.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Province entity.
   /// </summary>
   public class Province : BaseModel
   {
      /// <summary>
      /// Primary key of the table Provinces.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of a province.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      [IfNotAlphabet]
      public string Description { get; set; }

      [StringLength(10)]
      [Display(Name = "Province Code")]
      public string ProvinceCode { get; set; }

      /// <summary>
      /// Districts of a province.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<District> Districts { get; set; }
   }
}