using Domain.Validations;
using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// Client entity.
    /// </summary>
    public class Client : BaseModel
    {
        /// <summary>
        /// Primary key of the table Clients.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// First name of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.ClientFirstName)]
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Surname of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.ClientSurname)]
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Sex of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.ClientSex)]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; }

        /// <summary>
        /// Date of birth of the client.
        /// </summary>        
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date of birth")]
        [IfFutureDate]
        public DateTime DOB { get; set; }

        /// <summary>
        /// Date of birth of the client is estimated or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "DOB estimated available")]
        public bool IsDOBEstimated { get; set; }

        /// <summary>
        /// NRC of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.ClientNRC)]
        [StringLength(11)]
        [RegularExpression(@"^[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-9]$", ErrorMessage = MessageConstants.NRC)]
        public string NRC { get; set; }

        /// <summary>
        /// NRC of the client is available or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "I do not have NRC")]
        public bool NoNRC { get; set; }

        /// <summary>
        /// NAPSA Number of the client.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "NAPSA number")]
        public string NAPSANumber { get; set; }

        /// <summary>
        /// Underfive Card Number client.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "Underfive Card Number")]
        public string UnderFiveCardNumber { get; set; }

        /// <summary>
        /// NUPN Number client.
        /// </summary>
        [StringLength(30)]
        [Display(Name = "NUPN")]
        public string NUPN { get; set; }

        /// <summary>
        /// Registration date of the client.
        /// </summary>        
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Registration date")]
        [IfFutureDate]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// First name of the client's father.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Father's first name")]
        [ClientIfAgeGreater18]
        public string FathersFirstName { get; set; }

        /// <summary>
        /// Surname of the client's father.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Father's surname")]
        [ClientIfAgeGreater18]
        public string FathersSurname { get; set; }

        /// <summary>
        /// NRC of the client's father.
        /// </summary>
        [StringLength(11)]
        [RegularExpression(@"^[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-9]$", ErrorMessage = MessageConstants.NRC)]
        public string FathersNRC { get; set; }

        /// <summary>
        /// Father NAPSA Number of the client.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "NAPSA number")]
        public string FatherNAPSANumber { get; set; }

        /// <summary>
        /// Father Nationality of the client.
        /// </summary>
        [Display(Name = "Nationality")]
        [ClientIfAgeGreater18]
        public int? FatherNationality { get; set; }

        /// <summary>
        /// Client's father is deceased or not.
        /// </summary>
        [Display(Name = "Is father deceased")]
        public bool IsFatherDeceased { get; set; }

        /// <summary>
        /// First name of the client's mother.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Mother's first name")]
        [ClientIfAgeGreater18]
        public string MothersFirstName { get; set; }

        /// <summary>
        /// Surname of the client's mother.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Mother's surname")]
        [ClientIfAgeGreater18]
        public string MothersSurname { get; set; }

        /// <summary>
        /// NRC of the client's mother.
        /// </summary>
        [StringLength(11)]
        [RegularExpression(@"^[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-9]$", ErrorMessage = MessageConstants.NRC)]
        public string MothersNRC { get; set; }

        /// <summary>
        /// Mother NAPSA Number of the client.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "NAPSA number")]
        public string MotherNAPSANumber { get; set; }

        /// <summary>
        /// Mother Nationality of the client.
        /// </summary>
        [Display(Name = "Nationality")]
        [ClientIfAgeGreater18]
        public int? MotherNationality { get; set; }

        /// <summary>
        /// Client's mother is deceased or not.
        /// </summary>
        [Display(Name = "Is mother deceased")]
        public bool IsMotherDeceased { get; set; }

        /// <summary>
        /// First name of the client's guardian.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Guardian's first name")]
        public string GuardiansFirstName { get; set; }

        /// <summary>
        /// Surname of the client's guardian.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [Display(Name = "Guardian's surname")]
        public string GuardiansSurname { get; set; }

        /// <summary>
        /// NRC of the client's guardian.
        /// </summary>
        [StringLength(11)]
        [RegularExpression(@"^[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-9]$", ErrorMessage = MessageConstants.NRC)]
        public string GuardiansNRC { get; set; }

        /// <summary>
        /// Guardian NAPSA Number of the client.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "NAPSA number")]
        public string GuardianNAPSANumber { get; set; }

        /// <summary>
        /// Guardin Nationality of the client.
        /// </summary>
        [Display(Name = "Nationality")]
        [ClientIfAgeGreater18]
        public int? GuardianNationality { get; set; }

        /// <summary>
        /// Guardin Relationship with the client.
        /// </summary>        
        [Display(Name = "Guardian Relationship")]
        [ClientIfAgeGreater18]
        public GuardianRelationship? GuardianRelationship { get; set; }

        /// <summary>
        /// Legal name of the client's spouse.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [Display(Name = "Legal name of spouse")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [ClientSpouse]
        public string SpousesLegalName { get; set; }

        /// <summary>
        /// Surname of the client's spouse.
        /// </summary>
        [StringLength(60)]
        [MaxLength(60), MinLength(2)]
        [Display(Name = "Surname of spouse")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = MessageConstants.IfNotAlphabet)]
        [ClientSpouse]
        public string SpousesSurname { get; set; }

        /// <summary>
        /// Marital status of the client.
        /// </summary>        
        [Display(Name = "Marital status")]
        public MaritalStatus? MaritalStatus { get; set; }

        /// <summary>
        /// Cellphone country code of the client.
        /// </summary> 
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(4)]
        [Display(Name = "Country code of cellphone")]
        [IfInvalidCountryCode]
        public string CellphoneCountryCode { get; set; }

        /// <summary>
        /// Cellphone number of the client.
        /// </summary>  
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(11)]
        [Display(Name = "Cellphone number")]
        [IfNotInteger]
        public string Cellphone { get; set; }

        /// <summary>
        /// Country code of other cellphone of the client.
        /// </summary>
        [StringLength(4)]
        [Display(Name = "Country code of other cellphone")]
        [IfInvalidCountryCode]
        public string OtherCellphoneCountryCode { get; set; }

        /// <summary>
        /// Other cellphone number of the client.
        /// </summary>
        [StringLength(11)]
        [Display(Name = "Other cellphone number")]
        [IfNotInteger]
        public string OtherCellphone { get; set; }

        /// <summary>
        /// Cellphone number of the client is available or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Cellphone available status")]
        public bool NoCellphone { get; set; }

        /// <summary>
        /// Landline country code of the client.
        /// </summary>
        [StringLength(4)]
        [Display(Name = "Country code of landline")]
        [IfInvalidCountryCode]
        public string LandlineCountryCode { get; set; }

        /// <summary>
        /// Landline number of the client.
        /// </summary>
        [StringLength(11)]
        [Display(Name = "Landline number")]
        [IfNotInteger]
        public string Landline { get; set; }

        /// <summary>
        /// Email address of the client.
        /// </summary>
        [StringLength(60)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        //[IfNotEmailInCorrectFormat]
        public string Email { get; set; }

        /// <summary>
        /// House hold number of the client.
        /// </summary>
        [StringLength(30)]
        [Display(Name = "Household number")]
        public string HouseholdNumber { get; set; }

        /// <summary>
        /// Road of the client.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Road")]
        public string Road { get; set; }

        /// <summary>
        /// Area of the client.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Area")]
        public string Area { get; set; }

        /// <summary>
        /// Landmarks of the client.
        /// </summary>
        [StringLength(500)]
        [Display(Name = "Landmarks")]
        public string Landmarks { get; set; }

        /// <summary>
        /// Client is born in Zambia or not.
        /// </summary>
        [Display(Name = "Is zambian born")]
        public bool IsZambianBorn { get; set; }

        /// <summary>
        /// Place of birth of the client.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "Birth Place")]
        [ClientIfOutsideZambia]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Town Name of the Client.
        /// </summary>
        [Display(Name = "Town Name")]
        [StringLength(90)]
        public string TownName { get; set; }

        /// <summary>
        /// Religion of the client.
        /// </summary>
        [Display(Name = "Religion")]
        public ReligiousDenomination? Religion { get; set; }

        /// <summary>
        /// HIV Types of the client.
        /// </summary>
        [Display(Name = "HIV Status")]
        public HIVStatus? HIVStatus { get; set; }

        /// <summary>
        /// Client is deceased or not.
        /// </summary>
        [Display(Name = "Is Deceased")]
        public bool IsDeceased { get; set; }

        /// <summary>
        /// Client is on ART or not.
        /// </summary>
        [Display(Name = "Is On ART")]
        public bool IsOnART { get; set; }

        /// <summary>
        /// Is Client Admitted or not.
        /// </summary>
        [Display(Name = "Is Admitted")]
        public bool IsAdmitted { get; set; }

        /// <summary>
        /// Place of birth of the client.
        /// </summary>
        [Display(Name = "Is DFZClient")]
        public bool IsDFZClient { get; set; }

        /// <summary>
        /// Mother Profile ID of the client.
        /// </summary>        
        [Display(Name = "Mother's Profile Id")]
        public Guid? MotherProfileId { get; set; }

        /// <summary>
        /// Father Profile ID of the client.
        /// </summary>
        [Display(Name = "Father's Profile Id")]
        public Guid? FatherProfileId { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Countries.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.Country)]
        public int CountryId { get; set; }

        /// <summary>
        /// country of the client.
        /// </summary>
        [ForeignKey("CountryId")]
        [JsonIgnore]
        public virtual Country Country { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Districts.
        /// </summary>
        [ClientDistrictIfZambianBorn]
        public int? DistrictId { get; set; }

        /// <summary>
        /// District of the clients.
        /// </summary>
        [ForeignKey("DistrictId")]
        [JsonIgnore]
        public virtual District District { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table HomeLanguages.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.HomeLanguage)]
        public int HomeLanguageId { get; set; }

        /// <summary>
        /// Home Language of the client.
        /// </summary>
        [ForeignKey("HomeLanguageId")]
        [JsonIgnore]
        public virtual HomeLanguage HomeLanguage { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table EducationLevels.
        /// </summary>        
        public int? EducationLevelId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("EducationLevelId")]
        [JsonIgnore]
        public virtual EducationLevel EducationLevel { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Occupations.
        /// </summary>        
        public int? OccupationId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        [ForeignKey("OccupationId")]
        [JsonIgnore]
        public virtual Occupation Occupation { get; set; }

        /// <summary>
        /// Vitals of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Vital> Vitals { get; set; }

        ///// <summary>
        ///// HTS of the  client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HTS> HTS { get; set; }

        ///// <summary>
        ///// ImmunizationRecord of the  client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ImmunizationRecord> ImmunizationRecords { get; set; }

        ///// <summary>
        ///// System reviews of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<SystemReview> SystemReviews { get; set; }

        ///// <summary>
        ///// System examinations of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<SystemExamination> SystemExaminations { get; set; }

        ///// <summary>
        ///// Medical Histories of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MedicalHistory> MedicalHistories { get; set; }

        ///// <summary>
        ///// Conditions of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Condition> Conditions { get; set; }

        ///// <summary>
        ///// Obstetrics & Gynecology histories of the client
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<GynObsHistory> GynObsHistories { get; set; }

        ///// <summary>
        ///// Feeding histories of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FeedingHistory> FeedingHistories { get; set; }

        ///// <summary>
        ///// Childs Development History of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ChildsDevelopmentHistory> ChildsDevelopmentHistories { get; set; }

        ///// <summary>
        ///// Assessments of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Assessment> Assessments { get; set; }

        ///// <summary>
        ///// Glasgow coma scales of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<GlasgowComaScale> GlasgowComaScales { get; set; }

        /// <summary>
        /// Surgery record of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Surgery> Surgeries { get; set; }

        ///// <summary>
        ///// Treatment plans of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TreatmentPlan> TreatmentPlans { get; set; }

        ///// <summary>
        ///// HIV prevention histories of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HIVPreventionHistory> HIVPreventionHistories { get; set; }

        ///// <summary>
        ///// PEP risk statuses of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<RiskStatus> RiskStatuses { get; set; }

        ///// <summary>
        ///// Plans of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Plan> Plans { get; set; }

        ///// <summary>
        ///// HIVRiskScreenings of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<HIVRiskScreening> HIVRiskScreenings { get; set; }

        ///// <summary>
        /////Nursing Plan  of the client.
        ///// </summary
        [JsonIgnore]
        public virtual IEnumerable<NursingPlan> NursingPlans { get; set; }

        ///// <summary>
        ///// Encounter of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Encounter> Encounters { get; set; }

        /// <summary>
        ///Investigation Batch of the client.
        /// </summary
        [JsonIgnore]
        public virtual IEnumerable<Investigation> Investigations { get; set; }

        ///// <summary>
        /////Pain Record Plan  of the client.
        ///// </summary
        [JsonIgnore]
        public virtual IEnumerable<PainRecord> PainRecords { get; set; }

        ///// <summary>
        /////TurningChart Plan  of the client.
        ///// </summary
        [JsonIgnore]
        public virtual IEnumerable<TurningChart> TurningCharts { get; set; }

        ///// <summary>
        /////Covax Record of the client.
        ///// </summary
        [JsonIgnore]
        public virtual IEnumerable<Covax> Covax { get; set; }

        ///// <summary>
        /////Covid of the client.
        ///// </summary
        [JsonIgnore]
        public virtual IEnumerable<Covid> Covid { get; set; }

        ///// <summary>
        ///// Death record of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DeathRecord> DeathRecords { get; set; }

        ///// <summary>
        ///// Birth record of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<BirthRecord> BirthRecords { get; set; }

        ///// <summary>
        ///// Birth History of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<BirthHistory> BirthHistories { get; set; }

        ///// <summary>
        ///// Chief Complaints of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ChiefComplaint> ChiefComplaints { get; set; }

        ///// <summary>
        ///// Identified Constitutional Symptoms of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedConstitutionalSymptom> IdentifiedConstitutionalSymptoms { get; set; }

        ///// <summary>
        ///// Identified TB Symptoms of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedTBSymptom> IdentifiedTBSymptoms { get; set; }

        ///// <summary>
        ///// Identified Allergies of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedAllergy> IdentifiedAllergies { get; set; }

        ///// <summary>
        ///// Diagnosis of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Diagnosis> Diagnoses { get; set; }

        ///// <summary>
        ///// DrugAdherences of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DrugAdherence> DrugAdherences { get; set; }

        ///// <summary>
        ///// Key Population Demographics of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<KeyPopulationDemographic> KeyPopulationDemographics { get; set; }

        ///// <summary>
        ///// Nutritions of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Nutrition> Nutritions { get; set; }

        ///// <summary>
        ///// Counselling services of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<CounsellingService> CounsellingServices { get; set; }

        ///// <summary>
        ///// Fluids of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Fluid> Fluids { get; set; }

        ///// <summary>
        ///// AttachedFacility of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<AttachedFacility> AttachedFacilities { get; set; }

        ///// <summary>
        ///// FamilyMembers of the Clients.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FamilyMember> FamilyMembers { get; set; }

        ///// <summary>
        ///// Next Of Kins of the Clients.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<NextOfKin> NextOfKins { get; set; }

        ///// <summary>
        ///// DSD assessments of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DSDAssessment> DSDAssessments { get; set; }

        ///// <summary>
        ///// Visit purposes of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<VisitPurpose> VisitPurposes { get; set; }

        ///// <summary>
        ///// ART responses of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ARTResponse> ARTResponses { get; set; }

        ///// <summary>
        ///// PriorARTExposers of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PriorARTExposure> PriorARTExposures { get; set; }

        ///// <summary>
        ///// TPTHistories of the ART.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TPTHistory> TPTHistories { get; set; }

        ///// <summary>
        ///// ARTDrugAdherences of the ART.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ARTDrugAdherence> ARTDrugAdherences { get; set; }

        ///// <summary>
        ///// TB histories of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TBHistory> TBHistories { get; set; }

        ///// <summary>
        ///// ART Treatment Plans of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ARTTreatmentPlan> ARTTreatmentPlans { get; set; }

        ///// <summary>
        ///// Clients Disability Plans of the client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ClientsDisability> ClientsDisabilities { get; set; }

        ///// <summary>
        ///// PMTCT of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PMTCT> PMTCT { get; set; }

        ///// <summary>
        ///// ReferralModule of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ReferralModule> ReferralModules { get; set; }

        ///// <summary>
        ///// MotherDeliverySummary of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MotherDeliverySummary> MotherDeliverySummaries { get; set; }

        ///// <summary>
        ///// FeedingMethod of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FeedingMethod> FeedingMethods { get; set; }

        ///// <summary>
        ///// DischargeMetric of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DischargeMetric> DischargeMetrics { get; set; }

        ///// <summary>
        ///// Service Queue of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ServiceQueue> ServiceQueues { get; set; }

        ///// <summary>
        ///// BloodTransfusionHistory of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<BloodTransfusionHistory> BloodTransfusionHistories { get; set; }

        ///// <summary>
        ///// Caregiver of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Caregiver> Caregivers { get; set; }

        ///// <summary>
        ///// ChildDetail of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ChildDetail> ChildDetails { get; set; }

        ///// <summary>
        ///// VisitDetails of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<VisitDetail> VisitDetails { get; set; }

        ///// <summary>
        ///// VaginalPosition of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<VaginalPosition> VaginalPositions { get; set; }

        ///// <summary>
        ///// WHO Condition of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<WHOCondition> WHOConditions { get; set; }

        ///// <summary>
        ///// ANCService of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ANCService> ANCServices { get; set; }

        ///// <summary>
        ///// ANCScreening of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ANCScreening> ANCScreenings { get; set; }

        ///// <summary>
        ///// PregnancyBooking of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PregnancyBooking> PregnancyBookings { get; set; }

        ///// <summary>
        ///// GuidedExaminations of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<GuidedExamination> GuidedExaminations { get; set; }

        ///// <summary>
        ///// MotherDetail of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MotherDetail> MotherDetails { get; set; }

        ///// <summary>
        ///// QuickExaminations of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<QuickExamination> QuickExaminations { get; set; }

        ///// <summary>
        ///// ScreeningAndPrevention of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ScreeningAndPrevention> ScreeningAndPreventions { get; set; }

        ///// <summary>
        ///// ObstericExamination of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ObstetricExamination> ObstericExaminations { get; set; }

        ///// <summary>
        ///// Drug Pickup Schedule of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DrugPickUpSchedule> DrugPickUpSchedules { get; set; }

        /////// <summary>
        /////// Drug Pickup Schedule of the Client.
        /////// </summary>
        //[JsonIgnore]
        //public virtual IEnumerable<DFZClient> DFZClients { get; set; }

        /// <summary>
        /// TBService of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TBService> TBServices { get; set; }

        ///// <summary>
        ///// IdentifiedTBFinding of the Client.
        ///// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedTBFinding> IdentifiedTBFindings { get; set; }

        /// <summary>
        /// InsertionAndRemovalProcedures of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<InsertionAndRemovalProcedure> InsertionAndRemovalProcedures { get; set; }

        /// <summary>
        /// IdentifiedReason of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedReason> IdentifiedReasons { get; set; }

        /// <summary>
        /// PelvicAndVaginalExaminations of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PelvicAndVaginalExamination> PelvicAndVaginalExaminations { get; set; }

        /// <summary>
        /// Prescriptions of the client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Prescription> Prescriptions { get; set; }

        /// <summary>
        /// PastAntenatalVisit of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PastAntenatalVisit> PastAntenatalVisits { get; set; }

        /// <summary>
        /// Identified Preferred Feedings of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<IdentifiedPreferredFeeding> IdentifiedPreferredFeedings { get; set; }

        /// <summary>
        /// MedicalConditions of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MedicalCondition> MedicalConditions { get; set; }

        /// <summary>
        /// FamilyPlans of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FamilyPlan> FamilyPlans { get; set; }

        /// <summary>
        /// FamilyPlanRegisters of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FamilyPlanRegister> FamilyPlanRegisters { get; set; }

        /// <summary>
        /// DFZDependents of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DFZDependent> DFZDependents { get; set; }

        /// <summary>
        /// PreScreeningVisits of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PreScreeningVisit> PreScreeningVisits { get;set; }

        /// <summary>
        /// ThermoAblations of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ThermoAblation> ThermoAblations { get; set; }

        /// <summary>
        /// PreScreeningVisits of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Leeps> Leeps { get; set; }

        /// <summary>
        /// CaCXPlans of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<CaCXPlan> CaCXPlans { get; set; }

        /// <summary>
        /// CaCX Screening of the Client.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Screening> Screenings { get; set; }

        [NotMapped]
        public virtual DFZClient DFZClient { get; set; }

    }
}