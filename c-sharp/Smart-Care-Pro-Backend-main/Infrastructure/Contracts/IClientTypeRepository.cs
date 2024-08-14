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
    public interface IClientTypeRepository : IRepository<ClientType>
    {
        /// <summary>
        /// The method is used to get a type of client by client types.
        /// </summary>
        /// <param name="clientTypes">Client type name of a client.</param>
        /// <returns>Returns a type of client if the client type is matched.</returns>
        public Task<ClientType> GetClientTypeByClientTypes(string clientTypes);

        /// <summary>
        /// The method is used to get a client type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ClientTypes.</param>
        /// <returns>Returns a client type if the key is matched.</returns>
        public Task<ClientType> GetClientTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of client types.
        /// </summary>
        /// <returns>Returns a list of all client types.</returns>
        public Task<IEnumerable<ClientType>> GetClientTypes();
    }
}