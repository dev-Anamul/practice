using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class ResultOption : BaseModel
   {
      /// <summary>
      /// Primary key of the table Test.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Title of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TestId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("TestId")]
      [JsonIgnore]
      public virtual Test Test { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<Result> Results { get; set; }
   }
}