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
    /// Forget Password Dto.
    /// </summary>
    public class ForgetPasswordDto
    {
        /// <summary>
        /// The row is assigned to the FacilityRequestID
        /// </summary>
        public Guid FacilityRequestID { get; set; }

        /// <summary>
        /// The row is assigned to the UserName
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public string UserName { get; set; }

        /// <summary>
        /// The row is assigned to the new password of a user account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        [Display(Name = "Login password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// The row is assigned to the confirm password of a user account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = MessageConstants.PasswordMatchError)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}