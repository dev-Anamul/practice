using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 18.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class ICDDiagnosis : BaseModel
   {
      /// <summary>
      /// Primary key of the table ICDDiagnoses.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Code of ICD.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "ICD Code")]
      public string ICDCode { get; set; }

      /// <summary>
      /// Description.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(500)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// ParentID.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "ParentId")]
      public int ParentId { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table ICPC2Descriptions.
      /// </summary>
      public int? ICPC2Id { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ICPC2Id")]
      [JsonIgnore]
      public virtual ICPC2Description ICPC2Description { get; set; }

      /// <summary>
      /// Conditions of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Condition> Conditions { get; set; }

      /// <summary>
      /// Diagnoses of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Diagnosis> Diagnoses { get; set; }

      /// <summary>
      /// Cause of death of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DeathCause> DeathCauses { get; set; }
   }
}