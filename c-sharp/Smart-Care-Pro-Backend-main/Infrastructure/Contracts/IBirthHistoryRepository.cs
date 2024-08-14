using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IBirthHistoryRepository : IRepository<BirthHistory>
    {
        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<BirthHistory> GetBirthHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<BirthHistory>> GetBirthHistories();

        /// <summary>
        /// The method is used to get a birth history by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth history if the ClientID is matched.</returns>
        public Task<IEnumerable<BirthHistory>> GetBirthHistoryByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a birth history by OPD visit.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth history if the Encounter is matched.</returns>
        public Task<IEnumerable<BirthHistory>> GetBirthHistoryByEncounter(Guid encounterId);
    }
}