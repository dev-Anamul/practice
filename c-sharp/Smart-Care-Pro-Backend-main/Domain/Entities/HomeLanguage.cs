using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HomeLanguage entity in the database.
   /// </summary>
   public class HomeLanguage : BaseModel
   {
      /// <summary>
      /// Primary key of the table HomeLanguages.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// The field stores the Home language of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Home language")]
      public string Description { get; set; }

      [JsonIgnore]
      public virtual IEnumerable<Client> Clients { get; set; }
   }
}