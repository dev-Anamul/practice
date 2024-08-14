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
    public interface IContraceptiveHistoryRepository : IRepository<ContraceptiveHistory>
    {
        /// <summary>
        /// The method is used to get a contraceptive history by key.
        /// </summary>
        /// <param name="key">Primary key of the table ContraceptiveHistories.</param>
        /// <returns>Returns a contraceptive histories if the key is matched.</returns>
        public Task<ContraceptiveHistory> GetContraceptiveHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of contraceptive histories.
        /// </summary>
        /// <returns>Returns a list of all contraceptive histories.</returns>
        public Task<IEnumerable<ContraceptiveHistory>> GetContraceptiveHistories();
        public Task<IEnumerable<ContraceptiveHistory>> GetContraceptiveHistoryByGynObsHistoryId(Guid gynObsHistoryId);
    }
}