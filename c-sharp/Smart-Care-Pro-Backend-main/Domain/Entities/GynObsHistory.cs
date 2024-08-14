using Domain.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Contains details of the GynObsHistory entity in the database.
    /// </summary>
    public class GynObsHistory : EncounterBaseModel
    {
        /// <summary>
        /// Primary key of the table GynObsHistories.
        /// </summary>
        [Key]
        public Guid InteractionId { get; set; }

        /// <summary>
        /// Menstrual history of the client.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Menstrual history")]
        public string MenstrualHistory { get; set; }

        /// <summary>
        /// Breastfeeding status of the client.
        /// </summary>
        [Display(Name = "Is breastfeeding")]
        public YesNoUnknown? IsBreastFeeding { get; set; }

        /// <summary>
        /// LMP of the client.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "LNMP")]
        public DateTime? LNMP { get; set; }

        /// <summary>
        /// This field indicates the Patient is pregnant or not.
        /// </summary>
        [Display(Name = "Is pregnant")]
        public YesNoUnknown? IsPregnant { get; set; }

        /// <summary>
        /// The field stores the Note of the obstetrics history.
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Obstetrics history note")]
        public string ObstetricsHistoryNote { get; set; }

        /// <summary>
        /// The field stores the Gestational age of the client.
        /// </summary>
        [Display(Name = "Gestational age")]
        public int? GestationalAge { get; set; }

        /// <summary>
        /// The field stores the Estimated delivery date of the client.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "EDD")]
        public DateTime? EDD { get; set; }

        /// <summary>
        /// The field indicates whether the Client's CaCx is screened or not.
        /// </summary>
        [Display(Name = "Is CaCxScreened")]
        public YesNoUnknown? IsCaCxScreened { get; set; }

        /// <summary>
        /// The field stores the date of client's CaCx last screened.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "CaCx last screened")]
        public DateTime? CaCxLastScreened { get; set; }

        /// <summary>
        /// The field stores the CaCx result of the client.
        /// </summary>
        [Display(Name = "CaCx result")]
        public CaCxResult? CaCxResult { get; set; }

        /// <summary>
        /// The field indicates whether Is Child Tested for HIV or not.
        /// </summary>
        [Display(Name = "IsChild Tested For HIV")]
        public YesNoUnknown? IsChildTestedForHIV { get; set; }

        /// <summary>
        /// Is client Screened for syphilis  
        /// </summary>
        [Display(Name = "IsScreened For Syphilis")]
        public YesNoUnknown? IsScreenedForSyphilis { get; set; }

        /// <summary>
        /// Have Treated With Benzathine Penicillin
        /// </summary>
        [Display(Name = "Have you been treated with benzathine penicillin ?")]
        public YesNoUnknown? HaveTreatedWithBenzathinePenicillin { get; set; }

        /// <summary>
        /// Breast feeding choice for table GynObsHistories
        /// </summary>
        [Display(Name = "Breast feeding choice")]
        public BreastFeedingChoice? BreastFeedingChoice { get; set; }

        /// <summary>
        /// Breast feeding type.
        /// </summary>
        [Display(Name = "Breast feeding type")]
        public BreastFeedingType? BreastFeedingType { get; set; }

        /// <summary>
        /// Note of BreastFeeding
        /// </summary>
        [Display(Name = "Breast Feeding Note")]
        [StringLength(1000)]
        public string BreastFeedingNote { get; set; }

        /// <summary>
        /// Is Client On Family Planning
        /// </summary>
        [Display(Name = "Is Cient On Family Planning")]
        public bool? IsClientOnFP { get; set; }

        /// <summary>
        /// Is Client Need Family Planning
        /// </summary>
        [Display(Name = "Is Client Need FamilyPlanning")]
        public bool? IsClientNeedFP { get; set; }

        /// <summary>
        /// Current FP
        /// </summary>
        [Display(Name = "Current FP")]
        [StringLength(300)]
        public string CurrentFP { get; set; }

        /// <summary>
        /// Is Client Counselled
        /// </summary>
        public bool? HasCounselled { get; set; }

        /// <summary>
        /// Contraceptive given 
        /// </summary>
        [Display(Name = "Contraceptive given")]
        [StringLength(300)]
        public string ContraceptiveGiven { get; set; }

        /// <summary>
        /// have any Previous sexual history or not
        /// </summary>
        [Display(Name = "Previous sexual history")]
        public bool? PreviousSexualHistory { get; set; }

        /// <summary>
        /// Previous got pregnant or not
        /// </summary>
        [Display(Name = "Previous got pregnant")]
        public YesNo? PreviouslyGotPregnant { get; set; }

        /// <summary>
        /// Total number of pregnancy 
        /// </summary>
        [Display(Name = "Total number of pregnancy")]
        public int? TotalNumberOfPregnancy { get; set; }

        /// <summary>
        /// Total birth given by the client
        /// </summary>
        [Display(Name = "Total birth given")]
        public int? TotalBirthGiven { get; set; }

        /// <summary>
        /// Alive children for GynObsHistory
        /// </summary>
        [Display(Name = "Alive children")]
        public int? AliveChildren { get; set; }

        /// <summary>
        /// Ceaserian history for GynObsHistory
        /// </summary>
        [Display(Name = "Cesarean History")]
        public YesNo? CesareanHistory { get; set; }

        /// <summary>
        /// Recent client given birth for GynObsHistory
        /// </summary>
        [Display(Name = "Recent client given birth")]
        public YesNo? RecentClientGivenBirth { get; set; }

        /// <summary>
        /// Date of delivery for GynObsHistory
        /// </summary>
        [Display(Name = "Date of delivery")]
        public DateTime? DateOfDelivery { get; set; }

        /// <summary>
        /// Post partum for GynObsHistory
        /// </summary>
        [Display(Name = "Postpartum")]
        public YesNo? Postpartum { get; set; }

        /// <summary>
        /// Last delivery time for GynObsHistory
        /// </summary>
        [Display(Name = "Last delivery time")]
        [StringLength(90)]
        public string LastDeliveryTime { get; set; }

        // <summary>
        /// Miscarriage staus for GynObsHistory
        /// </summary>
        [Display(Name = "Miscarriage staus")]
        public YesNo? MiscarriageStatus { get; set; }

        /// <summary>
        /// Miscarriage within four weeks for GynObsHistory
        /// </summary>
        [Display(Name = "Miscarriage within four weeks")]
        public YesNo? MiscarriageWithinFourWeeks { get; set; }

        /// <summary>
        /// Post abortion sepsis for GynObsHistory
        /// </summary>
        [Display(Name = "Post abortion sepsis")]
        public YesNo? PostAbortionSepsis { get; set; }

        // <summary>
        /// Menarche Age for Client
        /// </summary>
        [Column(TypeName = "Tinyint")]
        public int? AgeAtMenarche { get; set; }

        /// <summary>
        /// Menstrual Blood Flow of the client
        /// </summary>
        public MenstrualBloodFlow? MenstrualBloodFlow { get; set; }

        /// <summary>
        /// Mestrual Cycle Regularity of the client
        /// </summary>
        public MenstrualCycleRegularity? MenstrualCycleRegularity { get; set; }

        /// <summary>
        /// Is Mens Accociated with pain
        /// </summary>
        public bool? IsMensAssociatedWithPain { get; set; }

        /// <summary>
        /// Client's First Sexual Intercourse Age
        /// </summary>
        [Column(TypeName = "Tinyint")]
        public int? FirstSexualIntercourseAge { get; set; }

        /// <summary>
        /// Number of Sexual Partners of the client
        /// </summary>
        [Column(TypeName = "Tinyint")]
        public int? NumberOfSexualPartners { get; set; }

        /// <summary>
        /// Age of First pregnancy of the client
        /// </summary>
        [Column(TypeName = "Tinyint")]
        public int? FirstPregnancyAge { get; set; }

        /// <summary>
        /// Is anything used to Clean vagina
        /// </summary>
        public bool? IsAnythingUsedToCleanVagina { get; set; }

        /// <summary>
        /// Item that vagina been cleaned
        /// </summary>
        [StringLength(20)]
        public string ItemUsedToCleanVagina { get; set; }

        /// <summary>
        /// Bleeding status during or after coitus
        /// </summary>
        public bool? IsBleedingDuringOrAfterCoitus { get; set; }

        /// <summary>
        /// Fever status of the clinet
        /// </summary>
        public bool? HasFever { get; set; }

        /// <summary>
        /// Lower Abdominal pain status of the client
        /// </summary>
        public bool? HasLowerAbdominalPain { get; set; }

        /// <summary>
        /// Abnormal Vagina Discharge Status of the client
        /// </summary>
        public bool? HasAbnormalVaginalDischarge { get; set; }

        /// <summary>
        /// Other Concerns related coitus
        /// </summary>
        [StringLength(1000)]
        public string OtherConcern { get; set; }

        /// <summary>
        /// Examination Record of that client
        /// </summary>
        [StringLength(1000)]
        public string Examination { get; set; }

        /// <summary>
        /// Intercourse Status of the client.
        /// </summary>
        public int? IntercourseStatusId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("IntercourseStatusId")]
        //[JsonIgnore]
        public virtual InterCourseStatus InterCourseStatus { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table CaCxScreeningMethods.
        /// </summary>
        public int? CaCxScreeningMethodId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("CaCxScreeningMethodId")]
        [JsonIgnore]
        public virtual CaCxScreeningMethod CaCxScreeningMethod { get; set; }

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
        /// Contraceptive history of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ContraceptiveHistory> ContraceptiveHistories { get; set; }

        /// <summary>
        /// List of contraceptive history.
        /// </summary>
        [NotMapped]
        public int[] ContraceptiveHistoryList { get; set; }

        [NotMapped]
        public ContraceptiveHistoryDTO[] ContraceptiveHistory { get; set;}
    }
}