using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICounsellingServiceRepository : IRepository<CounsellingService>
    {
        /// <summary>
        /// The method is used to get a counselling service by key.
        /// </summary>
        /// <param name="key">Primary key of the table CounsellingServices.</param>
        /// <returns>Returns a counselling service if the key is matched.</returns>
        public Task<CounsellingService> GetCounsellingServiceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of counselling services.
        /// </summary>
        /// <returns>Returns a list of all counselling services.</returns>
        public Task<IEnumerable<CounsellingService>> GetCounsellingServices();

        /// <summary>
        /// The method is used to get a counselling service by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a counselling service if the ClientID is matched.</returns>
        public Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClient(Guid ClientID);
        public Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClientLast24Hours(Guid ClientID);
        public Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetCounsellingServiceByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of counselling service by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all counselling service by EncounterID.</returns>
        public Task<IEnumerable<CounsellingService>> GetCounsellingServiceByEncounter(Guid EncounterID);
    }
}