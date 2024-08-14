using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DSDAssesment entity.
   /// </summary>
   public class DSDAssessment : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DSDAssesments.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Client is Stable On Care or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Client Stable On Care")]
      public bool IsClientStableOnCare { get; set; }

      /// <summary>
      /// Should Continue DSD or not.
      /// </summary>
      [Display(Name = "Should Continue DSD")]
      public bool ShouldContinueDSD { get; set; }

      /// <summary>
      /// Should Refer To Clinician or not.
      /// </summary>
      [Display(Name = "Should Refer To Clinician")]
      public bool ShouldReferToClinician { get; set; }

      /// <summary>
      /// Health Post is available or not.
      /// </summary>
      [Display(Name = "Health Post")]
      public bool HealthPost { get; set; }

      /// <summary>
      /// Scholar available or not.
      /// </summary>
      public bool Scholar { get; set; }

      /// <summary>
      /// Mobile ART Distribution Model available or not.
      /// </summary>
      [Display(Name = "Mobile ART Distribution Model")]
      public bool MobileARTDistributionModel { get; set; }

      /// <summary>
      /// Rural Adherence Model available or not.
      /// </summary>
      [Display(Name = "Rural Adherence Model")]
      public bool RuralAdherenceModel { get; set; }

      /// <summary>
      /// Community Post is available or not.
      /// </summary>
      [Display(Name = "Community Post")]
      public bool CommunityPost { get; set; }

      /// <summary>
      /// Fast Track is available or not.
      /// </summary>
      [Display(Name = "Fast Track")]
      public bool FastTrack { get; set; }

      /// <summary>
      /// Weekend is available or not.
      /// </summary>
      public bool Weekend { get; set; }

      /// <summary>
      /// Central Dispending Unit available or not.
      /// </summary>
      [Display(Name = "Central Dispending Unit")]
      public bool CentralDispensingUnit { get; set; }

      /// <summary>
      /// Community ART Distribition Points available or not.
      /// </summary>
      [Display(Name = "Community ART Distribition Points")]
      public bool CommunityARTDistributionPoints { get; set; }

      /// <summary>
      /// Others DSD Assesments if any
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

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
   }
}