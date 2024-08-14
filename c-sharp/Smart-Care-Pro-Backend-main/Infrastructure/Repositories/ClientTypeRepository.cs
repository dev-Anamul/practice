using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Phoenix(1)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IClientTypeRepository interface.
    /// </summary>
    public class ClientTypeRepository : Repository<ClientType>, IClientTypeRepository
    {
        public ClientTypeRepository(DataContext context):base(context)
        {     
            
        }

        /// <summary>
        /// The method is used to get a type of client by client types.
        /// </summary>
        /// <param name="clientTypes">Client type name of a client.</param>
        /// <returns>Returns a type of client if the client type is matched.</returns>
        public async Task<ClientType> GetClientTypeByClientTypes(string clientTypes)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == clientTypes.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ClientTypes.</param>
        /// <returns>Returns a client type if the key is matched.</returns>
        public async Task<ClientType> GetClientTypeByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of client types.
        /// </summary>
        /// <returns>Returns a list of all client types.</returns>
        public async Task<IEnumerable<ClientType>> GetClientTypes()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}