using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class CauseOfNeonatalDeathRepository : Repository<CauseOfNeonatalDeath>, ICauseOfNeonatalDeathRepository
    {
        public CauseOfNeonatalDeathRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get CauseOfNeonatalDeath by key.
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfNeonatalDeaths.</param>
        /// <returns>Returns a CauseOfNeonatalDeath if the key is matched.</returns>
        public async Task<CauseOfNeonatalDeath> GetCauseOfNeonatalDeathByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(b => b.Oid == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of CauseOfNeonatalDeaths.
        /// </summary>
        /// <returns>Returns a list of all CauseOfNeonatalDeath.</returns>
        public async Task<IEnumerable<CauseOfNeonatalDeath>> GetCauseOfNeonatalDeaths()
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
        /// The method is used to get an CauseOfNeonatalDeath by CauseOfNeonatalDeath Description.
        /// </summary>
        /// <param name="description">Name of an CauseOfNeonatalDeath.</param>
        /// <returns>Returns an CauseOfNeonatalDeath if the CauseOfNeonatalDeath description is matched.</returns>
        public async Task<CauseOfNeonatalDeath> GetCauseOfNeonatalDeathByName(string causeOfNeonatalDeath)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == causeOfNeonatalDeath.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}