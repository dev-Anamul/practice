using Domain.Entities;

/*
 * Created by: Lion
 * Date created: 12.09.2022
 * Modified by: stephan
 * Last modified: 06.01.2023
 */
namespace Domain.Dto
{
    /// <summary>
    /// UserLoginSuccess Dto.
    /// </summary>
    public class UserLoginSuccessDto
    {
        /// <summary>
        /// The row is assigned to the user account of a user.
        /// </summary>
        public UserAccount UserAccount { get; set; }

        /// <summary>
        /// The row is assigned to the module access of a user.
        /// </summary>
        public List<ModuleAccess> ModuleAccess { get; set; }  

        /// <summary>
        /// The row is assigned to the facility of a user.
        /// </summary>
        public Facility Facility { get; set; }

        /// <summary>
        /// The row is assigned to the FacilityAccess of a user.
        /// </summary>
        public List<FacilityAccess> RevokedFacilityAccess { get; set; }
    }
}