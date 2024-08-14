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
    public interface IANCScreeningRepository : IRepository<ANCScreening>
    {
        /// <summary>
        /// The method is used to get a ANCScreening by key.
        /// </summary>
        /// <param name="key">Primary key of the table ANCScreenings.</param>
        /// <returns>Returns a ANCScreening if the key is matched.</returns>
        public Task<ANCScreening> GetANCScreeningByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ANCScreening.
        /// </summary>
        /// <returns>Returns a list of all ANCScreening.</returns>
        public Task<IEnumerable<ANCScreening>> GetANCScreenings();

        /// <summary>
        /// The method is used to get a birth record by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ANCScreening if the clientId is matched.</returns>
        public Task<IEnumerable<ANCScreening>> GetANCScreeningByClient(Guid clientId);
        public Task<IEnumerable<ANCScreening>> GetANCScreeningByClientLast24Hours(Guid clientId);

        /// <summary>
        /// The method is used to get the list of ANCScreening by encounterId.
        /// </summary>
        /// <returns>Returns a list of all ANCScreening by encounterId.</returns>
        public Task<IEnumerable<ANCScreening>> GetANCScreeningByEncounter(Guid encounterId);
    }
}