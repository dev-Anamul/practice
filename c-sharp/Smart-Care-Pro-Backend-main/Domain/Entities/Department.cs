using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Department entity.
   /// </summary>
   public class Department : BaseModel
   {
      /// <summary>
      /// Primary key of the table Department.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Department name of the Department.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Department Name")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Facilities .
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int FacilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FacilityId")]
      [JsonIgnore]
      public virtual Facility Facility { get; set; }

      /// <summary>
      /// Death record of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Firm> Firms { get; set; }
   }
}