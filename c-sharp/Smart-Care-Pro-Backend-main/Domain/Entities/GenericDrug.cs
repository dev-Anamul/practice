using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella  
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the GenericDrug entity in the database.
   /// </summary>
   public class GenericDrug : BaseModel
   {
      /// <summary>
      /// Primary Key of the table GenericDrug.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field stores the name of the GenericDrug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      ///  This field stores the PregnancyRisk of GenericDrug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public PregnancyRisk PregnancyRisk { get; set; }

      /// <summary>
      /// This field stores the Breast Feeding Risk of GenericDrug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public BreastFeedingRisk BreastFeedingRisk { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the DrugSubclass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int SubclassId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("SubclassId")]
      [JsonIgnore]
      public virtual DrugSubclass DrugSubclass { get; set; }

      /// <summary>
      /// Drug Definitions of Generic Drug.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<GeneralDrugDefinition> GeneralDrugDefinitions { get; set; }
   }
}