using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Lion
 * Last modified: 19.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// RiskAssessment entity.
   /// </summary>
   public class RiskAssessment : BaseModel
   {
      /// <summary>
      /// Primary key of the table RiskAssessments.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table HIVRiskFactors.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int RiskFactorId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("RiskFactorId")]
      [JsonIgnore]
      public virtual HIVRiskFactor HIVRiskFactor { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table HTS.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid HTSId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("HTSId")]
      [JsonIgnore]
      public virtual HTS HTS { get; set; }
   }
}