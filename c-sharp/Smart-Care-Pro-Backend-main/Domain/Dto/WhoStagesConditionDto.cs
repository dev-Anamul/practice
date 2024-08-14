using System.ComponentModel.DataAnnotations;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class WhoStagesConditionDto
    {
        /// <summary>
        /// The row is assigned to the WHO Stages Condition of a Client.
        /// </summary>
        [Required]
        public int[] WhoStagesConditionID { get; set; }

        /// <summary>
        /// The row is assigned to the ClientId of a Client.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// The row is assigned to the EncounterID of a Client.
        /// </summary>
        public Guid EncounterID { get; set; }

        /// <summary>
        /// The row is assigned to the EncounterType of a Client.
        /// </summary>
        public EncounterType EncounterType { get; set; }

        /// <summary>
        /// The row is assigned to the CreatedIn of a Client.
        /// </summary>
        public int? CreatedIn { get; set; }

        /// <summary>
        /// The row is assigned to the DateCreated of a Client.
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// The row is assigned to the CreatedBy of a Client.
        /// </summary>
        public Guid? CreatedBy { get; set; }

    }
}