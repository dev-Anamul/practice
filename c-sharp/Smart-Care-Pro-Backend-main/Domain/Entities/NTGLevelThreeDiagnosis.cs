using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 03.01.2022
 * Modified by  :
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class NTGLevelThreeDiagnosis : BaseModel
   {
      /// <summary>
      /// Primary key of the table NTGLevelThreeDiagnoses.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of the NTG level three diagnosis.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Primary key of the table ICDDiagnosis.
      /// </summary>
      public int? ICDId { get; set; }

      /// <summary>
      /// Clinical features of the client.
      /// </summary>
      [Display(Name = "Clinical Features")]
      public string ClinicalFeatures { get; set; }

      /// <summary>
      /// Recommended investigations of the client.
      /// </summary>
      [Display(Name = "Recommended Investigations")]
      public string RecommendedInvestigations { get; set; }

      /// <summary>
      /// Investigation notes of the client.
      /// </summary>
      [Display(Name = "Investigation Notes")]
      public string InvestigationNotes { get; set; }

      /// <summary>
      /// Treatment notes of the client.
      /// </summary>
      [Display(Name = "Treatment Notes")]
      public string TreatmentNotes { get; set; }

      /// <summary>
      /// Name of a pharmacy.
      /// </summary>
      [Display(Name = "Pharmacy")]
      public string Pharmacy { get; set; }

      /// <summary>
      /// Complications of the client.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Complications")]
      public string Complications { get; set; }

      /// <summary>
      /// Prevention.
      /// </summary>
      [Display(Name = "Prevention")]
      public string Prevention { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table NTGLevelTwoDiagnoses. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int NTGLevelTwoId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NTGLevelTwoId")]
      [JsonIgnore]
      public virtual NTGLevelTwoDiagnosis NTGLevelTwoDiagnosis { get; set; }

      /// <summary>
      /// Diagnoses of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Diagnosis> Diagnoses { get; set; }

      /// <summary>
      /// Conditions of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Condition> Conditions { get; set; }
   }
}