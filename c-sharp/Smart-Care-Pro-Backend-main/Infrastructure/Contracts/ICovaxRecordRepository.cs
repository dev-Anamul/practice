using Domain.Entities;

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
    public interface ICovaxRecordRepository : IRepository<CovaxRecord>
    {
        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovaxRecordes.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<CovaxRecord> GetCovaxRecordByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<CovaxRecord>> GetCovaxRecordes();

        /// <summary>
        /// The method is used to get a birth history by OPD visit.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a birth history if the Encounter is matched.</returns>
        public Task<IEnumerable<CovaxRecord>> GetCovaxRecordByEncounter(Guid EncounterID);
    }
}