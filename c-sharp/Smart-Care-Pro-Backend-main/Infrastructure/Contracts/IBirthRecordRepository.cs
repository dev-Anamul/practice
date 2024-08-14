using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IBirthRecordRepository : IRepository<BirthRecord>
    {
        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public Task<BirthRecord> GetBirthRecordByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<BirthRecord>> GetBirthRecords();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth record if the ClientID is matched.</returns>
        public Task<IEnumerable<BirthRecord>> GetBirthRecordByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<BirthRecord>> GetBirthRecordByEncounter(Guid encounterId);
    }
}
