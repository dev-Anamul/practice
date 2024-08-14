using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IANCServiceRepository : IRepository<ANCService>
    {
        /// <summary>
        /// The method is used to get a ANCService by key.
        /// </summary>
        /// <param name="key">Primary key of the table ANCServices.</param>
        /// <returns>Returns a ANCService if the key is matched.</returns>
        public Task<ANCService> GetANCServiceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ANCService.
        /// </summary>
        /// <returns>Returns a list of all ANCService.</returns>
        public Task<IEnumerable<ANCService>> GetANCServices();

        /// <summary>
        /// The method is used to get a birth record by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns list of ANCService if the clientId is matched.</returns>
        public Task<IEnumerable<ANCService>> GetANCServiceByClient(Guid clientId);
        public Task<IEnumerable<ANCService>> GetANCServiceByClientLast24Hour(Guid clientId);

        /// <summary>
        /// Get latest ANC service by ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ANCService if the clientId is matched</returns>
        public Task<ANCService> GetLatestANCServiceByClient(Guid clientId);

        /// <summary>
        /// Get ANC Service by EncounterId
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns list of ANCService if the encounterId is matched.</returns>
        public Task<IEnumerable<ANCService>> GetANCServiceByEncounter(Guid encounterId);
    }
}