using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INextOfKinRepository : IRepository<NextOfKin>
    {
        /// <summary>
        /// The method is used to get a kin of a client by name.
        /// </summary>
        /// <param name="surname">Surname of a kin.</param>
        /// <returns>Returns a kin if the surname is matched.</returns>
        public Task<NextOfKin> GetNextOfKinBySurname(string surname);

        /// <summary>
        /// The method is used to get a kin of a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table NextOfKins.</param>
        /// <returns>Returns a kin of a client if the key is matched.</returns>
        public Task<NextOfKin> GetNextOfKinByKey(Guid key);

        /// <summary>
        /// The method is used to get a kin of a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a kin of a client if the key is matched.</returns>
        public Task<IEnumerable<NextOfKin>> GetNextOfKinByClient(Guid clientId);
        public Task<IEnumerable<NextOfKin>> GetNextOfKinByClient(Guid clientId, int page, int pageSize);
        public int GetNextOfKinByClientTotalCount(Guid clientID);

        /// <summary>
        /// The method is used to get the list of kins of a client.
        /// </summary>
        /// <returns>Returns a list of all kins of a client.</returns>
        public Task<IEnumerable<NextOfKin>> GetNextOfKins();
    }
}