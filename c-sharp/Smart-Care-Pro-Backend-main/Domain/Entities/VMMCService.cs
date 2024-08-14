using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
   /// VMMCService entity.
   /// </summary>
   public class VMMCService : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table VMMCServices.
      /// </summary>
      [Key, ForeignKey("Client")]
      public Guid Oid { get; set; }

      /// <summary>
      /// Is concent given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is concent given")]
      public bool IsConsentGiven { get; set; }

      /// <summary>
      /// MC number for VMMC service.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "MC number")]
      public string MCNumber { get; set; }

      /// <summary>
      /// Presented HIV status if known.
      /// </summary>
      [Display(Name = "Presented HIV status")]
      public PresentedHIVStatus? PresentedHIVStatus { get; set; }

      /// <summary>
      /// Is HIV status evidence presented and verified or not.
      /// </summary>
      [Display(Name = "HIV status evidence presented and verified")]
      public bool HIVStatusEvidencePresented { get; set; }

      /// <summary>
      /// Is pre-test counselling offerd or not.
      /// </summary>
      [Display(Name = "Is pre-test counselling offerd")]
      public bool IsPreTestCounsellingOffered { get; set; }

      /// <summary>
      /// Is HIV testing service offered or not.
      /// </summary>
      [Display(Name = "Is HIV testing service offered")]
      public bool IsHIVTestingServiceOffered { get; set; }

      /// <summary>
      /// Is post test counselling offerd or not.
      /// </summary>
      [Display(Name = "Is post test counselling offerd")]
      public bool IsPostTestCounsellingOffered { get; set; }

      /// <summary>
      /// Is referred to ART if positive or not.
      /// </summary>
      [Display(Name = "Is referred to ART if positive")]
      public bool IsReferredToARTIfPositive { get; set; }

      /// <summary>
      /// Has dentures or not.
      /// </summary>
      [Display(Name = "Has dentures")]
      public bool HasDentures { get; set; }

      /// <summary>
      /// Has loose teeth or not.
      /// </summary>
      [Display(Name = "Has loose teeth")]
      public bool HasLooseTeeth { get; set; }

      /// <summary>
      /// Has abnormalities of the neck or not.
      /// </summary>
      [Display(Name = "Has abnormalities of the neck")]
      public bool HasAbnormalitiesOfTheNeck { get; set; }

      /// <summary>
      /// Mandible size if any change.
      /// </summary>
      [Display(Name = "Mandible size")]
      public MandibleSize MandibleSize { get; set; }

      /// <summary>
      /// Tongue size size if have any change.
      /// </summary>
      [Display(Name = "Tongue size")]
      public TongueSize TongueSize { get; set; }

      /// <summary>
      /// Inter incisor gap for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Inter incisor gap")]
      public string InterincisorGap { get; set; }

      /// <summary>
      /// Movement of head nek for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Movement of head neck")]
      public string MovementOfHeadNeck { get; set; }

      /// <summary>
      /// Thyromental distance for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Thyromental distance")]
      public string ThyromentalDistance { get; set; }

      /// <summary>
      /// Atlanto occiptal flexion for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Atlanto occiptal flexion")]
      public string AtlantoOccipitalFlexion { get; set; }

      /// <summary>
      /// Mallampati class for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Mallampati class")]
      public string MallampatiClass { get; set; }

      /// <summary>
      /// ASA classification for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "ASA classification")]
      public string ASAClassification { get; set; }

      /// <summary>
      /// IV Access for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "IV Access")]
      public string IVAccess { get; set; }

      /// <summary>
      /// Bony landmarks for VMMCService.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Bony landmarks")]
      public string BonyLandmarks { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// OptedCircumcisionReason of the VMMCService.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<OptedCircumcisionReason> OptedCircumcisionReasons { get; set; }

      /// <summary>
      /// OptedVMMCCampaign of the VMMCService.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<OptedVMMCCampaign> OptedVMMCCampaigns { get; set; }

      /// <summary>
      /// Complications of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Complication> Complications { get; set; }

      /// <summary>
      /// List of Circumcision Reason to the client.
      /// </summary>
      [NotMapped]
      public int[] CircumcisionReasonList { get; set; }

      /// <summary>
      /// List of VMMC Campaign to the client.
      /// </summary>
      [NotMapped]
      public int[] VMMCCampaignList { get; set; }
   }
}