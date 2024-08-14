using Domain.Entities;
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
namespace Domain.Dto
{
    /// <summary>
    /// Client entity.
    /// </summary>
    public class ClientRelationDto
    {
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

        [Display(Name = "DFZ PatientType Id")]
        public int DFZPatientTypeId { get; set; }

        [StringLength(90)]
        [Display(Name = "HospitalNo")]
        public string HospitalNo { get; set; }

        public int CountryId { get; set; }

        public int? DistrictId { get; set; }

        public int HomeLanguageId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? OccupationId { get; set; }

        public RelationType RelationType { get; set; }

        [StringLength(90)]
        [Display(Name = "Description of Relation")]
        public string Description { get; set; }

        public Guid PrincipalId { get; set; }

        public int? CreatedIn { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        public Guid? CreatedBy { get; set; }

        public int? ModifiedIn { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsSynced { get; set; }
        public virtual DFZClient DFZClient { get; set; }

    }
}