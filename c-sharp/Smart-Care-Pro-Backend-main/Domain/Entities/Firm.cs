using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 29-01-2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the Firm entity in the database.
   /// </summary>
   public class Firm : BaseModel
   {
      /// <summary>
      /// Primary key of the table Firms.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field stores the firm name.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Firm Name")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Departments.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DepartmentId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DepartmentId")]
      [JsonIgnore]
      public virtual Department Department { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Ward> Wards { get; set; }
   }
}