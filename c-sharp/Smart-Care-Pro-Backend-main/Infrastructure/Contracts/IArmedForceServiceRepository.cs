using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IArmedForceServiceRepository : IRepository<ArmedForceService>
    {
        /// <summary>
        /// The method is used to get an armedForceService by armedForceService name.
        /// </summary>
        /// <param name="armedForceService">Name of a armedForceService.</param>
        /// <returns>Returns a armedForceService if the armedForceService name is matched.</returns>
        public Task<ArmedForceService> GetArmedForceServiceByName(string description);

        /// <summary>
        /// The method is used to get an armedForceService by key.
        /// </summary>
        /// <param name="key">Primary key of the table ArmedForceServiceses.</param>
        /// <returns>Returns a armedForceService if the key is matched.</returns>
        public Task<ArmedForceService> GetArmedForceServiceByKey(int key);

        /// <summary>
        /// The method is used to get the list of armedForceServiceses.
        /// </summary>
        /// <returns>Returns a list of all armedForceServiceses.</returns>
        public Task<IEnumerable<ArmedForceService>> GetArmedForceServiceses();

    }
}
