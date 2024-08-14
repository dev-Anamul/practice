/*
 * Created by: Lion
 * Date created: 12.09.2022
 * Modified by: Stephan
 * Last modified: 13.08.2023
 */
namespace Domain.Dto
{
    /// <summary>
    /// Verify Password Dto.
    /// </summary>
    public class VerifyPasswordDto
    {
        /// <summary>
        /// The row is assigned to the password of a user account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The row is assigned to the username of a user.
        /// </summary>
        public string UserName { get; set; }
    }
}