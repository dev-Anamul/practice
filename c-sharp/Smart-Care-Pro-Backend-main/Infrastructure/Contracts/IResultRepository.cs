using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IResultRepository : IRepository<Result>
    {
        /// <summary>
        /// The method is used to get a result by key.
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <returns>Returns a result if the key is matched.</returns>
        public Task<Result> GetResultByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of results.
        /// </summary>
        /// <returns>Returns a list of all results.</returns>
        public Task<IEnumerable<Result>> GetResults();

        public Task<Result> GetLatestResultByClient(Guid ClientID);

    }
}