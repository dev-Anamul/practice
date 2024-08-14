using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Lion
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedAllergy entity.
   /// </summary>
   public class IdentifiedAllergy : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedAllergies.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Severity of the identified allergy.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Severity")]
      public Severity Severity { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Allergies.
      /// </summary> 
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int AllergyId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AllergyId")]
      [JsonIgnore]
      public virtual Allergy Allergy { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table AllergicDrugs.
      /// </summary> 
      public int? AllergicDrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AllergicDrugId")]
      [JsonIgnore]
      public virtual AllergicDrug AllergicDrug { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// List of identified allergies.
      /// </summary>
      [NotMapped]
      public List<IdentifiedAllergy> IdentifiedAllergies { get; set; }
   }
}