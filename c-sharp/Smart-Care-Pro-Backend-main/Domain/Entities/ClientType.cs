using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 17.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ClientType entity.
   /// </summary>
   public class ClientType : BaseModel
   {
      /// <summary>
      /// Primary key of the table ClientTypes.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the ClientType.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// HTS of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<HTS> HTS { get; set; }
   }
}