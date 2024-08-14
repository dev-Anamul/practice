using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 06.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class MeasuringUnit : BaseModel
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
      /// minimum range of the Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Minimum Range")]
      public decimal MinimumRange { get; set; }

      // <summary>
      /// maximum range of the Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Maximum Range")]
      public decimal MaximumRange { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TestId { get; set; }

      // <summary>
      /// Foreign key. Primary key of the table Tests.
      /// </summary>
      [ForeignKey("TestId")]
      [JsonIgnore]
      public virtual Test Test { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Result> Results { get; set; }
   }
}
