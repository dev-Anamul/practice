using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   public class Result : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Investigation.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// result Date of the client.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Result Date")]
      [IfFutureDate]
      public DateTime ResultDate { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Display(Name = "Descriptive Result")]
      public string ResultDescriptive { get; set; }

      // <summary>
      /// maxumum range of the Test.
      /// </summary>
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Maximum Range")]
      public decimal? ResultNumeric { get; set; }

      /// <summary>
      /// OptionResult of Investigation.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Comment On Result")]
      public string CommentOnResult { get; set; }

      /// <summary>
      /// IsResultNormal of Investigation.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public IsResultNormal IsResultNormal { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      public int? MeasuringUnitId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("MeasuringUnitId")]
      [JsonIgnore]
      public virtual MeasuringUnit MeasuringUnit { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      public int? ResultOptionId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ResultOptionId")]
      [JsonIgnore]
      public virtual ResultOption ResultOption { get; set; }

      /// <summary>
      /// subtest of Test.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid InvestigationId { get; set; }

      // <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("InvestigationId")]
      [JsonIgnore]
      public virtual Investigation Investigation { get; set; }
   }
}