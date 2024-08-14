using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PathologyAxis entity.
   /// </summary>
   public class PathologyAxis : BaseModel
   {
      /// <summary>
      /// Primary key of the table PathologyAxes.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of pathology axis of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Description of ICPC2 of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ICPC2Description> ICPC2Descriptions { get; set; }
   }
}