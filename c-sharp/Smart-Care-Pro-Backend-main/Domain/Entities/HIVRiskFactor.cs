using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HIVRiskFactor entity in the database.
   /// </summary>

   public class HIVRiskFactor : BaseModel
   {
      /// <summary>
      /// Primary key of the table HIVRiskFactors.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field stores the Risk factors of HIV.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "HIV risk factor")]
      public string Description { get; set; }

      /// <summary>
      /// Risk assessments of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<RiskAssessment> RiskAssessments { get; set; }
   }
}