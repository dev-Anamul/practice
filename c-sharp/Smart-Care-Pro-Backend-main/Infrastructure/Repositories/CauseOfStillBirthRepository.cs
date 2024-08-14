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
    public class CauseOfStillBirthRepository : Repository<CauseOfStillbirth>, ICauseOfStillBirthRepository
    {
        public CauseOfStillBirthRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get CauseOfStillBirth by key.
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfStillBirths.</param>
        /// <returns>Returns a CauseOfStillBirth if the key is matched.</returns>
        public async Task<CauseOfStillbirth> GetCauseOfStillBirthByKey(int key)
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
        /// The method is used to get the list of CauseOfStillBirths.
        /// </summary>
        /// <returns>Returns a list of all CauseOfStillBirth.</returns>
        public async Task<IEnumerable<CauseOfStillbirth>> GetCauseOfStillBirths()
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
        /// The method is used to get an causeOfStillBirth by CauseOfStillBirth Description.
        /// </summary>
        /// <param name="description">Name of an CauseOfStillBirth.</param>
        /// <returns>Returns an CauseOfStillBirth if the CauseOfStillBirth description is matched.</returns>
        public async Task<CauseOfStillbirth> GetCauseOfStillBirthByName(string causeOfStillBirth)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == causeOfStillBirth.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}