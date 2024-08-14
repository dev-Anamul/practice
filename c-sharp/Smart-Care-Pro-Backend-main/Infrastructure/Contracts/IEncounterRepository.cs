using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IEncounterRepository : IRepository<Encounter>
    {
        /// <summary>
        /// The method is used to get an opd visit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisits.</param>
        /// <returns>Returns an opd visit if the key is matched.</returns>
        public Task<Encounter> GetEncounterByKey(Guid key);

        /// <summary>
        /// The method is used to get an opd visit by clientID.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisits.</param>
        /// <returns>Returns an opd visit if the key is matched.</returns>
        public Task<Encounter> GetEncounterByClient(Guid key);

        /// <summary>
        /// The method is used to get the list of opd visits.
        /// </summary>
        /// <returns>Returns a list of all opd visits.</returns>
        public Task<IEnumerable<Encounter>> GetEncounters();
        public Task<Encounter> GetEncounterByDate(DateTime? historicalDate);

        /// <summary>
        /// The method is used to get the IPD Admission if client is not discharched.
        /// </summary>
        public Task<Encounter> GetIPDAdmissionByClient(Guid ClientId);

        public Task<IEnumerable<Encounter>> ReadEncounterListByClient(Guid ClientId);
        public Task<IEnumerable<Encounter>> ReadAdmissionsByClient(Guid ClientId);
    }
}