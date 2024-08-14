using Domain.Entities;
using Domain.Validations;
using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by: Stephan
 * Date created: 24.12.2022
 * Modified by: Stephan
 * Last modified: 06.01.2023
 */
namespace Domain.Dto
{
    /// <summary>
    /// UserAccount dto.
    /// </summary>
    public class UserAccountDto : BaseModel
    {
        /// <summary>
        /// primary key Oid of the user.
        /// </summary> 
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
        /// Contact Address of user
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldEmpty)]
        [StringLength(500)]
        [Display(Name = "Contact address")]
        public string ContactAddress { get; set; }

        /// <summary>
        /// Country code of the user.        
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
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

        public UserType UserType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool IsAccountActive { get; set; }
    }
}