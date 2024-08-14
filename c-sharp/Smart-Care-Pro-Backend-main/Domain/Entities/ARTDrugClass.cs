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
   ///ARTDrugClass entity.
   /// </summary>
   public class ARTDrugClass : BaseModel
   {
      /// <summary>
      /// Primary Key of the table ARTDrugClasses.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Drug Class name of the ART Drug Class.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      ///// <summary>
      ///// ARTDrugs of the client.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ARTDrug> ARTDrugs { get; set; }
   }
}