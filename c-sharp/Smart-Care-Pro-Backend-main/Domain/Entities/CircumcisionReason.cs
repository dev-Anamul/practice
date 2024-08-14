using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// CircumcisionReasons entity.
   /// </summary>
   public class CircumcisionReason : BaseModel
   {
      /// <summary>
      /// Primary Key of the table CircumcisionReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description for CircumcisionReason.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// OptedCircumcisionReason of the CircumcisionReason.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<OptedCircumcisionReason> OptedCircumcisionReasons { get; set; }
   }
}