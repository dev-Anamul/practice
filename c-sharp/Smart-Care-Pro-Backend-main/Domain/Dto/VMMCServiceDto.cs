using Domain.Entities;

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
    /// Contains details of VMMCServiceDto.
    /// </summary>
    public class VMMCServiceDto : VMMCService
    {
        /// <summary>
        /// The row is assigned to the IsConcentGiven of a Client.
        /// </summary>
        public bool IsConcentGiven { get; set; }

        /// <summary>
        /// The row is assigned to the MCNumber of a Client.
        /// </summary>
        public string MCNumber { get; set; }
    }
}