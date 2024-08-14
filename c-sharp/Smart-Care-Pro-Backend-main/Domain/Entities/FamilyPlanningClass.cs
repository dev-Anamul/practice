using System.ComponentModel.DataAnnotations;
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
   /// Represents FamilyPlanningClass entity in the database.
   /// </summary>
   public class FamilyPlanningClass : BaseModel
   {
      /// <summary>
      /// Primary Key of the table FamilyPlanningClass.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field stores the description of the table FamilyPlanningClass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      // <summary>
      /// FamilyPlanningSubClasses of the FamilyPlanningClass.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FamilyPlanningSubclass> FamilyPlanningSubclasses { get; set; }
   }
}