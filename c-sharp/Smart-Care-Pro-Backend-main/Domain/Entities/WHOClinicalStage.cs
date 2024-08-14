using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 05.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// WHOClinicalStage entity.
   /// </summary>
   public class WHOClinicalStage : BaseModel
   {
      /// <summary>
      /// Primary Key of the table WHOClinicalStages.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Clinical Stage of the WHOClinicalStages.
      /// </summary>
      [StringLength(90)]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [ForeignKey("Description")]
      public string Description { get; set; }

      /// <summary>
      /// WHO Stages Condition of the WHOClinicalStages.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<WHOStagesCondition> WHOStagesConditions { get; set; }
   }
}