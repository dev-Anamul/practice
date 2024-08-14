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
   public class NTGLevelTwoDiagnosis : BaseModel
   {
      /// <summary>
      /// Primary key of the table NTGLevelTwoDiagnoses.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of the NTG level two diagnosis.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table NTGLevelOneDiagnosis. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int NTGLevelOneId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NTGLevelOneId")]
      [JsonIgnore]
      public virtual NTGLevelOneDiagnosis NTGLevelOneDiagnosis { get; set; }

      /// <summary>
      /// NTG level three diagnosis of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NTGLevelThreeDiagnosis> NTGLevelThreeDiagnoses { get; set; }
   }
}