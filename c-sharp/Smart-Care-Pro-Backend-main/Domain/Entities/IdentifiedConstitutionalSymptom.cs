using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 24.12.2022
 * Modified by  : Lion
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// IdentifiedConstitutionalSymptom entity.
   /// </summary>
   public class IdentifiedConstitutionalSymptom : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table IdentifiedConstitutionalSymptoms.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ConstitutionalSymptomTypeID.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ConstitutionalSymptomTypeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ConstitutionalSymptomTypeId")]
      [JsonIgnore]
      public virtual ConstitutionalSymptomType ConstitutionalSymptomType { get; set; }

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
      /// List of constitutional symptom type.
      /// </summary>
      [NotMapped]
      public int[] ConstitutionalSymptomTypeList { get; set; }
   }
}