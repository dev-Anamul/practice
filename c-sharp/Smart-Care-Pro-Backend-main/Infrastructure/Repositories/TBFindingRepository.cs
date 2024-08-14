using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 06.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 06.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TBFindingRepository : Repository<TBFinding>, ITBFindingRepository
    {
        public TBFindingRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TBFinding by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBFindings.</param>
        /// <returns>Returns a TBFinding  if the key is matched.</returns>
        public async Task<TBFinding> GetTBFindingByKey(int key)
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

        /// <summary>
        /// The method is used to get the list of TBFinding.
        /// </summary>
        /// <returns>Returns a list of all covid TBFinding.</returns>
        public async Task<IEnumerable<TBFinding>> GetTBFindings()
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
        /// The method is used to get an TBFinding by TBFinding Description.
        /// </summary>
        /// <param name="description">Name of an TBFinding.</param>
        /// <returns>Returns an TBFinding if the TBFinding description is matched.</returns>
        public async Task<TBFinding> GetTBFindingByName(string tBFinding)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == tBFinding.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}