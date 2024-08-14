using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Tomas
 * Last modified: 17.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// AllergicDrug entity.
   /// </summary>
   public class AllergicDrug : BaseModel
   {
      /// <summary>
      /// Primary key of the table AllergicDrugs.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Type of a drug.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Identified allergies of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedAllergy> IdentifiedAllergies { get; set; }
   }
}