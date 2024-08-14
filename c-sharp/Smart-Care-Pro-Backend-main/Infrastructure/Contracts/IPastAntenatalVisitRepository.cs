using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Biplob Roy
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPastAntenatalVisitRepository : IRepository<PastAntenatalVisit>
    {
        /// <summary>
        /// The method is used to get a PastAntenatalVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table PastAntenatalVisits.</param>
        /// <returns>Returns a PastAntenatalVisit if the key is matched.</returns>
        public Task<PastAntenatalVisit> GetPastAntenatalVisitByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PastAntenatalVisit.
        /// </summary>
        /// <returns>Returns a list of all PastAntenatalVisit.</returns>
        public Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisits();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PastAntenatalVisit if the ClientID is matched.</returns>
        public Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PastAntenatalVisit if the ClientID is matched.</returns>
        public Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientID"></param>
        ///<param name = "encounterType" ></ param >
        /// <returns>Returns a PastAntenatalVisit if the ClientID is matched.</returns>
        public int GetPastAntenatalVisitByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of PastAntenatalVisit by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PastAntenatalVisit by EncounterID.</returns>
        public Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByEncounter(Guid EncounterID);
    }
}