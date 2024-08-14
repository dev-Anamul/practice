using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of INextOfKinRepository interface.
    /// </summary>
    public class NextOfKinRepository : Repository<NextOfKin>, INextOfKinRepository
    {
        public NextOfKinRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a kin of a client by name.
        /// </summary>
        /// <param name="surname">Surname of a kin.</param>
        /// <returns>Returns a kin if the surname is matched.</returns>
        public async Task<NextOfKin> GetNextOfKinBySurname(string surname)
        {
            try
            {
                return await FirstOrDefaultAsync(k => k.Surname.ToLower().Trim() == surname.ToLower().Trim() && k.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a kin of a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table NextOfKins.</param>
        /// <returns>Returns a kin of a client if the key is matched.</returns>
        public async Task<NextOfKin> GetNextOfKinByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<NextOfKin>(k => k.InteractionId == key && k.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of kins of a client.
        /// </summary>
        /// <returns>Returns a list of all kins of a client.</returns>
        public async Task<IEnumerable<NextOfKin>> GetNextOfKins()
        {
            try
            {
                return await QueryAsync(k => k.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a kin of a client by Client Id.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a kin of a client if the key is matched.</returns>
        public async Task<IEnumerable<NextOfKin>> GetNextOfKinByClient(Guid clientId)
        {
            try
            {
                return await QueryAsync(k => k.ClientId == clientId && k.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<NextOfKin>> GetNextOfKinByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                return await LoadListWithChildAsync<NextOfKin>(p => p.IsDeleted == false && p.ClientId == clientId, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated));

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetNextOfKinByClientTotalCount(Guid clientID)
        {
            return context.NextOfKins.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
        }
    }
}