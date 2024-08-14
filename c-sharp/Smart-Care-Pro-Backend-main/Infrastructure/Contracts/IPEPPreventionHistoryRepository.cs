using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : Bithy
 * Last modified: 22.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPEPPreventionHistoryRepository : IRepository<HIVPreventionHistory>
    {
        /// <summary>
        /// The method is used to get a PEP prevention history by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPPreventionHistories.</param>
        /// <returns>Returns a PEP prevention history if the key is matched.</returns>
        public Task<HIVPreventionHistory> GetPEPPreventionHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get a pep prevention history by Client Id.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a pep prevention history if the Client ID is matched.</returns>
        public Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByClient(Guid ClientID);
        public Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetPEPPreventionHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a Client by EncounterId.
        /// </summary>
        /// <param name="EncounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a HIVPreventionHistory if the EncounterId is matched.</returns>
        public Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByEncounterId(Guid EncounterId);

        /// <summary>
        /// The method is used to get the list of PEP prevention histories.
        /// </summary>
        /// <returns>Returns a list of all PEP prevention histories.</returns>
        public Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistories();
    }
}