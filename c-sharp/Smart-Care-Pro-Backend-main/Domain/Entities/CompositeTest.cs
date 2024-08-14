using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// CompositeTest entity.
   /// </summary>
   public class CompositeTest : BaseModel
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
      /// Test Item list
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TestItem> TestItems { get; set; }

      [NotMapped]
      public int[] TestList { get; set; }
   }
}