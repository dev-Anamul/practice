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
   ///TBDrug entity.
   /// </summary>
   public class TBDrug : BaseModel
   {
      /// <summary>
      /// Primary Key of the table TBDrugs.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Drug name of the TBDrug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Drug Name")]
      public string Description { get; set; }

      /// <summary>
      /// TakenTBDrugs of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TakenTBDrug> TakenTBDrugs { get; set; }
   }
}