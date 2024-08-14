using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ICPC2Description entity.
   /// </summary>
   public class ICPC2Description : BaseModel
   {
      /// <summary>
      /// Primary key of the table ICPC2Descriptions.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of ICPC2 of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table AnatomicAxes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int AnatomicAxisId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AnatomicAxisId")]
      [JsonIgnore]
      public virtual AnatomicAxis AnatomicAxis { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table PathologyAxes.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PathologyAxisId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PathologyAxisId")]
      [JsonIgnore]
      public virtual PathologyAxis PathologyAxis { get; set; }

      /// <summary>
      /// ICD diagnosis of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ICDDiagnosis> ICDDiagnoses { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DeathCause> DeathCauses { get; set; }
   }
}