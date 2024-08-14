using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bithy
 * Date created : 22.02.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ResultOptionRepository : Repository<ResultOption>, IResultOptionRepository
    {
        public ResultOptionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ResultOption by key.
        /// </summary>
        /// <param name="key">Primary key of the table ResultOptions.</param>
        /// <returns>Returns a ResultOption  if the key is matched.</returns>
        public async Task<ResultOption> GetResultOptionByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ResultOption>> GetResultOptionByTest(int testid)
        {
            try
            {
                return await QueryAsync(d => d.TestId == testid && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ResultOption.
        /// </summary>
        /// <returns>Returns a list of all covid ResultOptions.</returns>
        public async Task<IEnumerable<ResultOption>> GetResultOptions()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an ResultOption by ResultOption Description.
        /// </summary>
        /// <param name="description">Name of an ResultOption.</param>
        /// <returns>Returns an ResultOption if the ResultOption description is matched.</returns>
        public async Task<ResultOption> GetResultOptionByName(string resultOption)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == resultOption.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}