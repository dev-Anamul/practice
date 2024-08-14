using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   public class Test : BaseModel
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
      [Display(Name = "Test Title")]
      [IfNotAlphabet]
      public string Title { get; set; }

      /// <summary>
      /// Title of Test.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "LONIC")]
      public string LONIC { get; set; }

      /// <summary>
      /// Description of Test.
      /// </summary>        
      [Display(Name = "Description")]
      [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
      public string Description { get; set; }

      /// <summary>
      /// MeasuringUnit of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Result Type")]
      public ResultType ResultType { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int SubtypeId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SubtypeId")]
      [JsonIgnore]
      public virtual TestSubtype TestSubtype { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<Investigation> Investigations { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<MeasuringUnit> MeasuringUnits { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<ResultOption> ResultOptions { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<TestItem> GroupItems { get; set; }
   }
}