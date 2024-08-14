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
    /// Contains details of Volumes.
    /// </summary>
    public class VolumeDto
    {
        /// <summary>
        /// The row is assigned to the PartographID of a Client.
        /// </summary>
        public Guid PartographId { get; set; }

        /// <summary>
        /// The row is assigned to the List of Data of a Client.
        /// </summary>
        public List<string[]> Data { get; set; }
    }
}
