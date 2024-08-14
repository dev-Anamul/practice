using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ILoginHistoryRepository interface.
    /// </summary>
    public class LoginHistoryRepository : Repository<LoginHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a login history by key.
        /// </summary>
        /// <param name="key">Primary key of the table LoginHistories.</param>
        /// <returns>Returns a login history if the key is matched.</returns>
        public async Task<LoginHistory> GetLoginHistoryByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(l => l.Oid == key && l.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of login histories.
        /// </summary>
        /// <returns>Returns a list of all login histories.</returns>
        public async Task<IEnumerable<LoginHistory>> GetloginHistories()
        {
            try
            {
                return await QueryAsync(h => h.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}