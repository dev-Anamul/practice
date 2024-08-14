using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 12.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class TestSubtype : BaseModel
   {
      /// <summary>
      /// Primary key of the table TestSubtype.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of a TestSubtype.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Title Name")]
      [IfNotAlphabet]
      public string Description { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TestTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TestTypeId")]
      [JsonIgnore]
      public virtual TestType TestType { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<Test> Tests { get; set; }
   }
}