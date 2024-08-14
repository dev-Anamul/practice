using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IBreechRepository : IRepository<Breech>
    {
        /// <summary>
        /// The method is used to get a Breech by key.
        /// </summary>
        /// <param name="key">Primary key of the table Breechs.</param>
        /// <returns>Returns a Breech if the key is matched.</returns>
        public Task<Breech> GetBreechByKey(int key);

        /// <summary>
        /// The method is used to get the list of Breechs.
        /// </summary>
        /// <returns>Returns a list of all Breechs.</returns>
        public Task<IEnumerable<Breech>> GetBreeches();

        /// <summary>
        /// The method is used to get an Breechs by Breechs Description.
        /// </summary>
        /// <param name="breechs">Description of an Breechs.</param>
        /// <returns>Returns an Breechs if the Breechs name is matched.</returns>
        public Task<Breech> GetBreechByName(string breechs);
    }
}