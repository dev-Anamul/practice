using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ILoginHistoryRepository : IRepository<LoginHistory>
    {
        /// <summary>
        /// The method is used to get a login history by key.
        /// </summary>
        /// <param name="key">Primary key of the table LoginHistories.</param>
        /// <returns>Returns a login history if the key is matched.</returns>
        public Task<LoginHistory> GetLoginHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of login histories.
        /// </summary>
        /// <returns>Returns a list of all login histories.</returns>
        public Task<IEnumerable<LoginHistory>> GetloginHistories();
    }
}