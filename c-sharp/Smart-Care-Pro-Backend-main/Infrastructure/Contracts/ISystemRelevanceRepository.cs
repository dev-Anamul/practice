using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ISystemRelevanceRepository : IRepository<SystemRelevance>
    {
        /// <summary>
        /// The method is used to get a SystemsRelevance.
        /// </summary>
        /// <param name="key">Primary key of the table SystemsRelevance.</param>
        /// <returns>Returns a SystemsRelevance if the key is matched.</returns>
        public Task<SystemRelevance> GetSystemsRelevanceByKey(int key);

        /// <summary>
        /// The method is used to get the list of SystemsRelevance  .
        /// </summary>
        /// <returns>Returns a list of all SystemsRelevance.</returns>
        public Task<IEnumerable<SystemRelevance>> GetSystemRelevance();
    }
}
