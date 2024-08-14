using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 01.01.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class Assessment : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Assessments.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// General condition of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "General Condition")]
      public GeneralCondition GeneralCondition { get; set; }

      /// <summary>
      /// Nutritional status of the client.
      /// </summary>
      [Display(Name = "Nutritional status")]
      [StringLength(90)]
      public string NutritionalStatus { get; set; }

      /// <summary>
      /// JVP of the client.
      /// </summary>
      [StringLength(90)]
      public string JVP { get; set; }

      /// <summary>
      /// Hydration status of the client.
      /// </summary>
      [Display(Name = "Hydration status")]
      [StringLength(90)]
      public string HydrationStatus { get; set; }

      /// <summary>
      /// Glucose of the client if any.
      /// </summary>
      public bool Glucose { get; set; }

      /// <summary>
      /// Scoring of the client.
      /// </summary>
      public Scoring? Scoring { get; set; }

      /// <summary>
      /// Varicose Vein of the client.
      /// </summary>
      [Display(Name = "Varicose vein")]
      public PresentNotPresent? VaricoseVein { get; set; }

      /// <summary>
      /// Albumin of the client.
      /// </summary>
      public Albumin? Albumin { get; set; }

      /// <summary>
      /// Passed Meconium of the client.
      /// </summary>
      [Display(Name = "Urine Output")]
      public NormalAbnormal? UrineOutput { get; set; }

      /// <summary>
      /// Feeding status of the client.
      /// </summary>
      public Feeding? Feeding { get; set; }

      /// <summary>
      /// Passed Meconium of the client.
      /// </summary>
      [Display(Name = "Passed meconium")]
      public YesNo? PassedMeconium { get; set; }

      /// <summary>
      /// Urine passed of the client.
      /// </summary>
      [Display(Name = "Urine passed")]
      public YesNo? UrinePassed { get; set; }

      /// <summary>
      /// Child Card Issued of the client.
      /// </summary>
      [Display(Name = "Child card issued")]
      public YesNo? ChildCardIssued { get; set; }

      /// <summary>
      /// is Vitamin A given to the client or not.
      /// </summary>
      [Display(Name = "Vitamin A given")]
      public YesNo? VitaminAGiven { get; set; }

      /// <summary>
      /// is Father living or not.
      /// </summary>
      [Display(Name = "Father living")]
      public YesNo? FatherLiving { get; set; }

      /// <summary>
      /// is Mother living or not.
      /// </summary>
      [Display(Name = "Mother living")]
      public YesNo? MotherLiving { get; set; }

      /// <summary>
      /// Pallor of the client.
      /// </summary>
      [Display(Name = "Pallor")]
      public Pallor? Pallor { get; set; }

      /// <summary>
      /// Edema of the client.
      /// </summary>
      [Display(Name = "Edema")]
      public Grade? Edema { get; set; }

      /// <summary>
      /// Clubbing of the client.
      /// </summary>
      [Display(Name = "Clubbing")]
      public Grade? Clubbing { get; set; }

      /// <summary>
      /// Status of jundice of the client.
      /// </summary>
      [Display(Name = " Jaundice")]
      public PresentNotPresent? Jaundice { get; set; }

      /// <summary>
      /// Status of cyanosis of the client.
      /// </summary>
      [Display(Name = "Cyanosis")]
      public PresentNotPresent? Cyanosis { get; set; }

      /// <summary>
      /// Hb of the Assessment.
      /// </summary>
      [StringLength(50)]
      public string Hb { get; set; }

      /// <summary>
      /// Pueperal condition of the Assessment.
      /// </summary>
      [Display(Name = "Pueperal condition")]
      public PueperalCondition? PuerperalCondition { get; set; }

      /// <summary>
      /// Breast feeding of the Assessment.
      /// </summary>
      [Display(Name = "Breast feeding")]
      public YesNo? BreastFeeding { get; set; }

      /// <summary>
      /// Involution of uterus of the Assessment.
      /// </summary>
      [Display(Name = "Involution of uterus")]
      public YesNo? InvolutionOfUterus { get; set; }

      /// <summary>
      /// Lochia of uterus of the Assessment.
      /// </summary>
      public Lochia? Lochia { get; set; }

      /// <summary>
      /// Perineum condition of the Assessment.
      /// </summary>
      [Display(Name = "Perineum condition")]
      public PerineumCondition? PerineumCondition { get; set; }

      /// <summary>
      /// Episiotomy condition of the Assessment.
      /// </summary>
      [Display(Name = "Episiotomy condition")]
      public PerineumCondition? EpisiotomyCondition { get; set; }

      /// <summary>
      /// Additional notes of the Assessment.
      /// </summary>
      [Display(Name = "Additional notes")]
      public string AdditionalNotes { get; set; }

      /// <summary>
      /// Fontanelles notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Fontanelles { get; set; }

      /// <summary>
      /// Skull notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Skull { get; set; }

      /// <summary>
      /// Eyes notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Eyes { get; set; }

      /// <summary>
      /// Mouth notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Mouth { get; set; }

      /// <summary>
      /// Chest notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Chest { get; set; }

      /// <summary>
      /// Back notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Back { get; set; }

      /// <summary>
      /// Limbs notes of the Assessment.
      /// </summary>
      public NormalAbnormal? Limbs { get; set; }

      /// <summary>
      /// Genitals of the Assessment.
      /// </summary>
      public NormalAbnormal? Genitals { get; set; }

      /// <summary>
      /// Symmetrical moro reaction of the Assessment.
      /// </summary>
      [Display(Name = "Symmetrical moro reaction")]
      public NormalAbnormal? SymmetricalMoroReaction { get; set; }

      /// <summary>
      /// Moro reaction of the Assessment.
      /// </summary>
      [Display(Name = "Moro reaction")]
      public NormalAbnormal? MoroReaction { get; set; }

      /// <summary>
      /// Is good grasp reflex or not.
      /// </summary>
      [Display(Name = "Is good grasp reflex")]
      public bool IsGoodGraspReflex { get; set; }

      /// <summary>
      /// Is meconium passed or not.
      /// </summary>
      [Display(Name = "Is meconium passed")]
      public bool IsMeconiumPassed { get; set; }

      /// <summary>
      /// Is good head sontrol or not.
      /// </summary>
      [Display(Name = "Is good head sontrol")]
      public bool IsGoodHeadControl { get; set; }

      /// <summary>
      /// Ortolani sign of Assment.
      /// </summary>
      [Display(Name = "Ortolani sign")]
      public PresentNotPresent? OrtolaniSign { get; set; }

      /// <summary>
      /// Rooting refelx of Assessment.
      /// </summary>
      [Display(Name = "Rooting Reflex")]
      [StringLength(90)]
      public string RootingReflex { get; set; }

      /// <summary>
      /// Sucking Reflex of Assessment.
      /// </summary>
      [Display(Name = "Sucking Reflex")]
      [StringLength(90)]
      public string SuckingReflex { get; set; }

      /// <summary>
      /// Palmar Reflex of Assessment.
      /// </summary>
      [Display(Name = "Palmar Reflex")]
      [StringLength(90)]
      public string PalmarReflex { get; set; }

      /// <summary>
      /// Plantar grasp of Assessment.
      /// </summary>
      [Display(Name = "Plantar grasp")]
      [StringLength(90)]
      public string PlantarGrasp { get; set; }

      /// <summary>
      /// Stepping reflex of Assessment.
      /// </summary>
      [Display(Name = "Stepping reflex")]
      [StringLength(90)]
      public string SteppingReflex { get; set; }

      /// <summary>
      /// Galant reflex of Assessment.
      /// </summary>
      [Display(Name = "Galant reflex")]
      [StringLength(90)]
      public string GalantReflex { get; set; }

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
      /// IdentifiedEyesAssessment of the Assessment.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedEyesAssessment> IdentifiedEyesAssessments { get; set; }

      /// <summary>
      /// IdentifiedCordStump of the Assessment.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedCordStump> IdentifiedCordStumps { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public EyesCondition[] EyesConditionList { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public CordStumpCondition[] CordStumpConditionList { get; set; }
   }
}