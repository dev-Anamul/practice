using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    /// <summary>
    /// ChangePassword Dto.
    /// </summary>
    public class ChangePasswordQuestionDto
    {
        /// <summary>
        /// The row is assigned to the username of a user.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        //[StringLength(30)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// The row is assigned to the password of a user account.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Login password")]
        public string Password { get; set; }

        /// <summary>
        /// The row is assigned to the new password of a user account.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Login password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// The row is assigned to the confirm password of a user account.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = MessageConstants.PasswordMatchError)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
        public string CountryCode { get; set; }
        public string Cellphone { get; set; }
        public bool IsSendRecoveryReqeust { get; set; }
    }
}