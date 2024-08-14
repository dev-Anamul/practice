using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 04.01.2023
 * Modified by  : Tomas
 * Last modified: 17.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ConstitutionalSymptomType entity.
   /// </summary>
   public class ConstitutionalSymptomType : BaseModel
   {
      /// <summary>
      /// Primary key of the table ConstitutionalSymptomTypes.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Symptom Type of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ConstitutionalSymptoms.
      /// </summary> 
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ConstitutionalSymptomId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ConstitutionalSymptomId")]
      [JsonIgnore]
      public virtual ConstitutionalSymptom ConstitutionalSymptom { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedConstitutionalSymptom> IdentifiedConstitutionalSymptoms { get; set; }
   }
}