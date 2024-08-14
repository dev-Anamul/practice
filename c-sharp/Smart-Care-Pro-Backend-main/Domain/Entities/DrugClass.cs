using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
   /// DrugClasses entity.
   /// </summary>
   public class DrugClass : BaseModel
   {
      /// <summary>
      /// Primary Key of the table DrugClass.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the Drug Class.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [DisplayName("Drug Class")]
      public string Description { get; set; }

      /// <summary>
      /// Sub Class of a DrugClass.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DrugSubclass> DrugSubclasses { get; set; }
   }
}