using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 12.02.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    /// <summary>
    /// Contains details of UserLoginDto.
    /// </summary>
    public class UserLoginDto
    {
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}