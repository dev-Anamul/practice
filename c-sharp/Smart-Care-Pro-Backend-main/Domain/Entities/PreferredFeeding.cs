using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   public class PreferredFeeding : BaseModel
   {
      /// <summary>
      /// Primary key of the table PreferredFeedings.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Descriptions of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Identified Preferred Feedings of the Client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPreferredFeeding> IdentifiedPreferredFeedings { get; set; }
   }
}