using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ComplicationType entity.
   /// </summary>
   public class ComplicationType : BaseModel
   {
      /// <summary>
      /// Primary key of the table ComplicationTypes.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of complication type of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Identify complications of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedComplication> IdentifiedComplications { get; set; }
   }
}