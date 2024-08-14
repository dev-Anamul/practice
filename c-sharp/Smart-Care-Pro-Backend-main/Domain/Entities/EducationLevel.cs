using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// EducationLevel entity.
   /// </summary>
   public class EducationLevel : BaseModel
   {
      /// <summary>
      /// Primary key of the table EducationLevels.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Education level of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Level of education")]
      public string Description { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Client> Clients { get; set; }
   }
}