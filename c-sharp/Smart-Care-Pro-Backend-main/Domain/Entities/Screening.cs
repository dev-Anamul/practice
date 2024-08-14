using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

namespace Domain.Entities
{
    public class Screening : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table Screening table.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Condition of IsPapSmearDone.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is-PapSmearDone")]
        public bool IsPapSmearDone { get; set; }

        /// <summary>
        /// Status of PapSmearTestResult.
        /// </summary>
        [Display(Name = "PapSmear Test Result")]
        public PapSmearTestResult? PapSmearTestResult { get; set; }

        /// <summary>
        /// Status of PapSmearGrade.
        /// </summary>
        [Display(Name = "PapSmear Grade")]
        public PapSmearGrade? PapSmearGrade { get; set; }

        /// <summary>
        /// Status of PepSmearComment.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "PapSmear Comment")]
        public string PapSmearComment { get; set; }

        /// <summary>
        /// Status of HPVTestType.
        /// </summary>
        [Display(Name = "HPV Test Type")]
        public HPVTestType? HPVTestType { get; set; }

        /// <summary>
        /// Status of HPVTestResult.
        /// </summary>
        [Display(Name = "HPV Test Result")]
        public HPVTestResult? HPVTestResult { get; set; }

        /// <summary>
        /// Status of HPVGenoTypeFound.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "HPV GenoType Found")]
        public string HPVGenoTypeFound { get; set; }

        /// <summary>
        /// Status of HPVGenoTypeComment.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "HPV GenoType Comment")]
        public string HPVGenoTypeComment { get; set; }

        /// <summary>
        /// Condition of IsVIADone.
        /// </summary>
        [Display(Name = "Is-VIA Done")]
        public bool? IsVIADone { get; set; }

        /// <summary>
        /// Status of VIAWhyNotDone.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "VIAWhyNotDone")]
        public string VIAWhyNotDone { get; set; }

        /// <summary>
        /// Condition of IsVIATransformationZoneSeen.
        /// </summary>
        [Display(Name = "Is-VIA TransformationZoneSeen")]
        public bool? IsVIATransformationZoneSeen { get; set; }

        /// <summary>
        /// Status of VIAStateType.
        /// </summary>
        [Display(Name = "VIA State Type")]
        public TransferZoneStateType? VIAStateType { get; set; }

        /// <summary>
        /// Condition of VIAIsLesionSeen.
        /// </summary>
        [Display(Name = "VIA Is LesionSeen")]
        public bool? VIAIsLesionSeen { get; set; }

        /// <summary>
        /// Condition of VIAIsAtypicalVessels .
        /// </summary>
        [Display(Name = "VIA Is Atypical Vessels ")]
        public bool? VIAIsAtypicalVessels { get; set; }

        /// <summary>
        /// Condition of VIAIsLesionExtendedIntoCervicalOs.
        /// </summary>
        [Display(Name = "VIA Is Lesion Extended Into Cervical Os")]
        public bool? VIAIsLesionExtendedIntoCervicalOs { get; set; }

        /// <summary>
        /// Condition of VIAIsMosiacism  .
        /// </summary>
        [Display(Name = "VIA Is Mosiacism ")]
        public bool? VIAIsMosiacism { get; set; }

        /// <summary>
        /// Condition of VIAIsLesionCoversMoreThreeQuaters.
        /// </summary>
        [Display(Name = "VIA Is Lesion Covers More 75%")]
        public bool? VIAIsLesionCovers { get; set; }

        /// <summary>
        /// Condition of VIAIsQueryICC .
        /// </summary>
        [Display(Name = "VIA Is Query ICC")]
        public bool? VIAIsQueryICC { get; set; }

        /// <summary>
        /// Condition of VIAIsPunctations.
        /// </summary>
        [Display(Name = "VIA Is Punctations")]
        public bool? VIAIsPunctations { get; set; }

        /// <summary>
        /// Condition of VIAIsEctopy .
        /// </summary>
        [Display(Name = "VIA Is Ectopy")]
        public bool? VIAIsEctopy { get; set; }


        /// <summary>
        /// Status of VIATestResult.
        /// </summary>
        [Display(Name = "VIA Test Result")]
        public TestResultsType? VIATestResult { get; set; }

        /// <summary>
        /// Status of VIAComments.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "VIA Comments")]
        public string VIAComments { get; set; }

        /// <summary>
        /// Condition of IsEDIDone.
        /// </summary>
        [Display(Name = "Is EDI Done")]
        public bool? IsEDIDone { get; set; }

        /// <summary>
        /// Status of EDIWhyNotDone.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "EDI Why Not Done")]
        public string EDIWhyNotDone { get; set; }

        /// <summary>
        /// Condition of IsEDITransformationZoneSeen .
        /// </summary>
        [Display(Name = "Is EDI Transformation Zone Seen")]
        public bool? IsEDITransformationZoneSeen { get; set; }

        /// <summary>
        /// Status of EDIStateType.
        /// </summary>
        [Display(Name = "EDI State Type")]
        public TransferZoneStateType? EDIStateType { get; set; }

        /// <summary>
        /// Condition of EDIIsLesionSeen .
        /// </summary>
        [Display(Name = "EDI Is Lesion Seen")]
        public bool? EDIIsLesionSeen { get; set; }

        /// <summary>
        /// Condition of EDIIsAtypicalVessels .
        /// </summary>
        [Display(Name = "EDI Is Atypical Vessels ")]
        public bool? EDIIsAtypicalVessels { get; set; }

        /// <summary>
        /// Condition of EDIIsLesionExtendedIntoCervicalOs  .
        /// </summary>
        [Display(Name = "EDI Is Lesion Extended Into CervicalOs")]
        public bool? EDIIsLesionExtendedIntoCervicalOs { get; set; }

        /// <summary>
        /// Condition of EDIIsMosiacism .
        /// </summary>
        [Display(Name = "EDI Is Mosiacism ")]
        public bool? EDIIsMosiacism { get; set; }

        /// <summary>
        /// Condition of EDIIsLesionCoversMoreThreeQuaters .
        /// </summary>
        [Display(Name = "EDI Is Lesion Covers More ThreeQuaters")]
        public bool? EDIIsLesionCoversMoreThreeQuaters { get; set; }


        /// <summary>
        /// Condition of EDIIsQueryICC.
        /// </summary>
        [Display(Name = "EDI Is Query ICC")]
        public bool? EDIIsQueryICC { get; set; }

        /// <summary>
        /// Condition of EDIIsPunctations .
        /// </summary>
        [Display(Name = "EDI Is Punctations")]
        public bool? EDIIsPunctations { get; set; }

        /// <summary>
        /// Condition of EDIIsEctopy .
        /// </summary>
        [Display(Name = "EDI Is Ectopy")]
        public bool? EDIIsEctopy { get; set; }

        /// <summary>
        /// Status of EDITestResult.
        /// </summary>
        [Display(Name = "EDI Test Result")]
        public TestResultsType? EDITestResult { get; set; }

        /// <summary>
        /// Status of EDIComments.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "EDI Comments")]
        public string EDIComments { get; set; }

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