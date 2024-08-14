using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVitalRepository : IRepository<Vital>
    {
        /// <summary>
        /// The method is used to get a vital by key.
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Returns a vital if the key is matched.</returns>
        public Task<Vital> GetVitalByKey(Guid key);

        public Task<Vital> GetLatestVitalByClient(Guid clientId);
        public Task<Vital> GetLatestVitalByClientAndEncounterType(Guid clientId, EncounterType encounterType);

        /// <summary>
        /// The method is used to get the list of vitals.
        /// </summary>
        /// <returns>Returns a list of all vitals.</returns>
        public Task< IEnumerable<Vital>> GetVitals(Guid ClientId);
        public Task< IEnumerable<Vital>> GetVitals(Guid ClientId, int page, int pageSize, EncounterType? encounterType);
        public int GetVitalsTotalCount(Guid clientID, EncounterType? encounterType);


        /// <summary>
        /// The method is used to get a clir by key.
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Returns a vital if the key is matched.</returns>
        public Task<IEnumerable<Vital>> GetVitalsByClient(Guid clientId);
    }
}