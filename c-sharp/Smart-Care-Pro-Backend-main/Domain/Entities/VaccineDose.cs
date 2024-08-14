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
   /// VaccineDose entity.
   /// </summary>
   public class VaccineDose : BaseModel
   {
      /// <summary>
      /// Primary key of the table VaccineDoses.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of the Dose.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Vaccines.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int VaccineId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VaccineId")]
      [JsonIgnore]
      public virtual Vaccine Vaccine { get; set; }

      /// <summary>
      /// Immunization records of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ImmunizationRecord> ImmunizationRecords { get; set; }
   }
}