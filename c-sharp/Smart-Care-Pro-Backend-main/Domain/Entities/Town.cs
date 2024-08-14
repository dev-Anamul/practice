using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
   /// Town entity.
   /// </summary>
   public class Town : BaseModel
   {
      /// <summary>
      /// Primary key of the table Towns.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of a town.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Town name")]
      [IfNotAlphabet]
      public string Description { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Districts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DistrictId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DistrictId")]
      [JsonIgnore]
      public virtual District District { get; set; }
   }
}