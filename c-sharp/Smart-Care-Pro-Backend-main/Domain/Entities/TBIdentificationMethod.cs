using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// TBIdentificationMethod entity.
   /// </summary>
   public class TBIdentificationMethod : BaseModel
   {
      /// <summary>
      /// Primary Key of the table TBIdentificationMethods.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// TB Identification Method Name of the TBIdentificationMethods.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "TB identification method")]
      public string Description { get; set; }

      /// <summary>
      /// UsedTBIdentificationMethods of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<UsedTBIdentificationMethod> UsedTBIdentificationMethods { get; set; }
   }
}