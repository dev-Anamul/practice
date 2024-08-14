using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IHTSRepository : IRepository<HTS>
    {
        /// <summary>
        /// The method is used to get HTS by ClientID.
        /// </summary>
        /// <param name="key">ClientID of HTS.</param>
        /// <returns>Returns HTS if the ClientID is matched.</returns>
        public Task<IEnumerable<HTS>> GetHTSByClient(Guid key);

        /// <summary>
        /// The method is used to get HTS by key.
        /// </summary>
        /// <param name="key">Primary key of the table HTS.</param>
        /// <returns>Returns HTS if the key is matched.</returns>
        public Task<HTS> GetHTSByKey(Guid key);

        /// <summary>
        ///  The method is used to get HTS by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns HTS if the ClientId is matched.</returns>
        public Task<HTS> GetLatestHTSByClient(Guid clientId);

        /// <summary>
        /// The method is used to get HTS by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns HTS if the ClientId is matched.</returns>
        public Task<IEnumerable<HTS>> GetHTS(Guid clientId);
        public Task<IEnumerable<HTS>> GetHTSLast24Hours(Guid clientId);
        public Task<IEnumerable<HTS>> GetHTS(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetHTSTotalCount(Guid clientID, EncounterType? encounterType);
    }
}