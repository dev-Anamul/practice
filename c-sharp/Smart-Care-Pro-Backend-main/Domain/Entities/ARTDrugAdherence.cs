using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ARTDrugAdherence entity.
   /// </summary>
   public class ARTDrugAdherence : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table TakenTPTDrugs.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Does client have trouble taking pills.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Does client have trouble taking pills?")]
      public bool HaveTroubleTakingPills { get; set; }

      /// <summary>
      /// How  many doses missed in the last 14 days.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "How many doses missed in the last 14 days")]
      public DosesMissed HowManyDosesMissed { get; set; }

      /// <summary>
      /// Reduce pharmacy visits to
      /// </summary
      [Display(Name = "Reduce pharmacy visits to")]
      public ReducePharmacyVisit? ReducePharmacyVisitTo { get; set; }

      /// <summary>
      /// Refer patient for adherence counselling.
      /// </summary>
      [Display(Name = "Refer patient for adherence counselling")]
      public bool ReferForAdherenceCounselling { get; set; }

      /// <summary>
      /// Additional notes of the ART Drug Adherences.
      /// </summary>
      [StringLength(300)]
      [Display(Name = "Note")]
      public string Note { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Forgot")]
      public bool Forgot { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Illness")]
      public bool Illness { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Side effect")]
      public bool SideEffect { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Medicine finished")]
      public bool MedicineFinished { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Away from home")]
      public bool AwayFromHome { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Clinic run out of medicine")]
      public bool ClinicRunOutOfMedication { get; set; }

      /// <summary>
      /// Reason for missing.
      /// </summary>
      [Display(Name = "Other missing reason")]
      public bool OtherMissingReason { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Nausea")]
      public bool Nausea { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Vomitting")]
      public bool Vomitting { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Yellow eyes")]
      public bool YellowEyes { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Mouth sores")]
      public bool MouthSores { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Diarrhea")]
      public bool Diarrhea { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Headache/vivid dreams")]
      public bool Headache { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Rash")]
      public bool Rash { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Numbness/pain/burning in limbs")]
      public bool Numbness { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Insomnia")]
      public bool Insomnia { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "Depression")]
      public bool Depression { get; set; }

      /// <summary>
      /// Side effects.
      /// </summary>
      [Display(Name = "WeightGain")]
      public bool WeightGain { get; set; }

      /// <summary>
      /// Others side effects.
      /// </summary>
      [StringLength(300)]
      [Display(Name = "Others")]
      public string OtherSideEffect { get; set; }

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

      /// <summary>
      /// Foreign Key. Primary key of the table SpecialDrugs.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DrugId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DrugId")]
      [JsonIgnore]
      public virtual SpecialDrug SpecialDrug { get; set; }

      /// <summary>
      /// This is used to load the list in client side
      /// </summary>
      [NotMapped]
      public List<ARTDrugAdherence> ARTDrugAdherences { get; set; }
   }
}