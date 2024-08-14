using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 24.12.2022
* Modified by  : Stephan
* Last modified: 18.01.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// VaccineType entity.
   /// </summary>
   public class VaccineType : BaseModel
   {
      /// <summary>
      /// Primary key of the table VaccineTypes.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Type of the vaccine.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.ClientFirstName)]
      [StringLength(90)]
      [Display(Name = "Vaccine Types")]
      public string Description { get; set; }

      /// <summary>
      /// List of the vaccines.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Vaccine> Vaccines { get; set; }
   }
}