using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// VMMCCampaign entity.
   /// </summary>
   public class VMMCCampaign : BaseModel
   {
      /// <summary>
      /// Primary Key of the table VMMCCampaigns.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// MC number for VMMC Campaign.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// OptedVMMCCampaign of the VMMCCampaign.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<OptedVMMCCampaign> OptedVMMCCampaigns { get; set; }
   }
}