using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Represents FamilyPlanningSubClass entity in the database.
   /// </summary>
   public class FamilyPlanningSubclass : BaseModel
   {
      /// <summary>
      /// Primary Key of the table FamilyPlanningSubClass.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field stores the description of the table FamilyPlanningSubClass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table FamilyPlanningClasses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ClassId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClassId")]
      [JsonIgnore]
      public virtual FamilyPlanningClass FamilyPlanningClass { get; set; }

      // <summary>
      /// FamilyPlans of the FamilyPlanningSubClass.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FamilyPlan> FamilyPlans { get; set; }
   }
}