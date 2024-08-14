using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : Brian
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IResultOptionRepository : IRepository<ResultOption>
    {
        /// <summary>
        /// The method is used to get a ResultOption by key.
        /// </summary>
        /// <param name="key">Primary key of the table ResultOptions.</param>
        /// <returns>Returns a ResultOption if the key is matched.</returns>
        public Task<ResultOption> GetResultOptionByKey(int key);

        public Task<IEnumerable<ResultOption>> GetResultOptionByTest(int testid);

        /// <summary>
        /// The method is used to get the list of resultOption.
        /// </summary>
        /// <returns>Returns a list of all ResultOptions.</returns>
        public Task<IEnumerable<ResultOption>> GetResultOptions();

        /// <summary>
        /// The method is used to get an ResultOption by ResultOption Description.
        /// </summary>
        /// <param name="ResultOption">Description of an ResultOption.</param>
        /// <returns>Returns an ResultOption if the ResultOption name is matched.</returns>
        public Task<ResultOption> GetResultOptionByName(string resultOption);
    }
}