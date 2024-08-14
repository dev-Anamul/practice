using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 03.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class Disability : BaseModel
   {
      /// <summary>
      /// Primary key of the table Disabilities.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Disabilities if any.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Disabilities")]
      public string Description { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<ClientsDisability> ClientsDisabilities { get; set; }
   }
}