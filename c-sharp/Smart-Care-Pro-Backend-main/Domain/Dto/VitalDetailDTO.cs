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
    /// Contains details of VitalDetailDTO.
    /// </summary>
    public class VitalDetailDTO
    {
        /// <summary>
        /// The row is assigned to the vitals of a Client.
        /// </summary>
        public List<Vital> vitals { get; set; }

        /// <summary>
        /// The row is assigned to the Client of a Client.
        /// </summary>
        public Client Client { get;set; }

        /// <summary>
        /// The row is assigned to the hTS of a Client.
        /// </summary>
        public HTS hTS { get; set; }    
    }
}