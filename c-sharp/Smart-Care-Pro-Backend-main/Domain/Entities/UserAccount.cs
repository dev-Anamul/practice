using Domain.Validations;
using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// UserAccount entity.
   /// </summary>
   public class UserAccount : BaseModel
   {
      /// <summary>
      /// Primary Key of the table UserAccounts.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// First name of the user.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [Display(Name = "First name")]
      [IfNotAlphabet]
      public string FirstName { get; set; }

      /// <summary>
      /// Surname of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [Display(Name = "Surname")]
      [IfNotAlphabet]
      public string Surname { get; set; }

      /// <summary>
      /// Date of birth of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date of birth")]
      [IfAgeBelow18]
      public DateTime DOB { get; set; }

      /// <summary>
      /// Sex of the user.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Sex")]
      [IfSexNotSelectedForUser]
      public Sex Sex { get; set; }

      /// <summary>
      /// Designation of the user.
      /// </summary> 
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Designation")]
      public string Designation { get; set; }

      /// <summary>
      /// NRC of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.ClientNRC)]
      [StringLength(11)]
      [RegularExpression(@"^[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9]/[0-9]$", ErrorMessage = MessageConstants.NRC)]
      public string NRC { get; set; }

      /// <summary>
      /// NRC of the user is available or not.
      /// </summary>
      [Display(Name = "I do not have NRC")]
      public bool NoNRC { get; set; }

      /// <summary>
      /// Contact address of the user. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldEmpty)]
      [StringLength(500)]
      [Display(Name = "Contact address")]
      public string ContactAddress { get; set; }

      /// <summary>
      /// Country code of the user.        
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(4)]
      [Display(Name = "Country code")]
      public string CountryCode { get; set; }

      /// <summary>
      /// Cellphone number of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(11)]
      [Display(Name = "Cellphone number")]
      [IfNotInteger]
      public string Cellphone { get; set; }

      /// <summary>
      /// Username of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(30)]
      [Display(Name = "Username")]
      public string Username { get; set; }

      /// <summary>
      /// Password of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.PasswordLengthError)]
      [Display(Name = "Password")]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      /// <summary>
      /// Password confirmation of the user.
      /// </summary>
      [NotMapped]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Compare("Password", ErrorMessage = MessageConstants.PasswordMatchError)]
      [Display(Name = "Confirm password")]
      [DataType(DataType.Password)]
      public string ConfirmPassword { get; set; }

      /// <summary>
      /// Type of user of a user account.
      /// </summary> 
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "User type")]
      public UserType UserType { get; set; }

      /// <summary>
      /// User account is active or not.
      /// </summary>
      [Display(Name = "Active status")]
      public bool IsAccountActive { get; set; }

      /// <summary>
      /// Facility access of the user.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FacilityAccess> FacilityAccesses { get; set; }

      /// <summary>
      /// Login histories of the user.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<LoginHistory> LoginHistories { get; set; }

      /// <summary>
      /// Investigation batch of the user.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Investigation> Investigations { get; set; }
   }
}