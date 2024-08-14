using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// SystemsRelevance entity.
   /// </summary>
   public class SystemRelevance : BaseModel
   {
      /// <summary>
      /// Primary Key of the table SystemsRelevance.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Foreign key. Primary key of PhysicalSystem.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.PhysicalSystem)]
      public int PhysicalSystemId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PhysicalSystemId")]
      [JsonIgnore]
      public virtual PhysicalSystem PhysicalSystem { get; set; }

      /// <summary>
      /// Foreign key. Primary key of DrugDefinition.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.DrugDefinitions)]
      public int DrugDefinitionId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DrugDefinitionId")]
      [JsonIgnore]
      public virtual GeneralDrugDefinition GeneralDrugDefinitions { get; set; }
   }
}