using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 24.12.2022
* Modified by  : Stephan
* Last modified: 12.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Vaccine entity.
   /// </summary>
   public class Vaccine : BaseModel
   {
      /// <summary>
      /// Primary key of the table Vaccines.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of the vaccines.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Vaccines")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table VaccinesTypes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int VaccineTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VaccineTypeId")]
      [JsonIgnore]
      public virtual VaccineType VaccineType { get; set; }

      /// <summary>
      /// Immunization records of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<VaccineDose> VaccineDoses { get; set; }
   }
}