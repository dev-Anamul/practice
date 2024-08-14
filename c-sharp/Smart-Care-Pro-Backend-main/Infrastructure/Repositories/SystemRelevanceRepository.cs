using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tariqul Islam
 * Date created : 12-03-2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of SystemRelevanceRepository class.
    /// </summary>
    public class SystemRelevanceRepository : Repository<SystemRelevance>, ISystemRelevanceRepository
    {
        public SystemRelevanceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a SystemRelevance by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemRelevance.</param>
        /// <returns>Returns a SystemRelevance if the key is matched.</returns>
        public async Task<SystemRelevance> GetSystemsRelevanceByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of SystemRelevance  .
        /// </summary>
        /// <returns>Returns a list of all SystemRelevance.</returns>        
        public async Task<IEnumerable<SystemRelevance>> GetSystemRelevance()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}