using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 06.04.2023
 * Modified by  :
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ITBServiceRepository : IRepository<TBService>
    {
        /// <summary>
        /// The method is used to get a TBService by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBServices.</param>
        /// <returns>Returns a TBService if the key is matched.</returns>
        public Task<TBService> GetTBServiceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of TBServices.
        /// </summary>
        /// <returns>Returns a list of all TBServices.</returns>
        public Task<IEnumerable<TBService>> GetTBServices();

        /// <summary>
        /// The method is used to get a TBService by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TBService if the ClientID is matched.</returns>
        public Task<IEnumerable<TBService>> GetTBServiceByClient(Guid ClientID);
        public Task<IEnumerable<TBService>> GetTBServiceByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetTBServiceByClientTotalCount(Guid clientID, EncounterType? encounterType);
        public Task<TBService> GetActiveTBServiceByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get a TBService by OPD visit.
        /// </summary>
        /// <param name="OPDVisitID"></param>
        /// <returns>Returns a TBService if the Encounter is matched.</returns>
        public Task<IEnumerable<TBService>> GetTBServiceByOpdVisit(Guid OPDVisitID);
    }
}