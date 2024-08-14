using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// VMMCCampaigns entity.
   /// </summary>
   public class OptedVMMCCampaign : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table OptedVMMCCampaigns.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table VMMCCampaigns.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int VMMCCampaignId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VMMCCampaignId")]
      [JsonIgnore]
      public virtual VMMCCampaign VMMCCampaign { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table VMMCServices.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid VMMCServiceId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VMMCServiceId")]
      [JsonIgnore]
      public virtual VMMCService VMMCService { get; set; }
   }
}