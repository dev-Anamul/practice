using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : 
 * Date created : 
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ReasonsOfReferalRepository : Repository<ReasonOfReferral>, IReasonsOfReferalRepository
    {
        public ReasonsOfReferalRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to get the list of referral modules.
        /// </summary>
        /// <returns>Returns a list of all referral modules.</returns>
        public async Task<IEnumerable<ReasonOfReferral>> GetReasonOfReferrals()
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
        /// The method is used to get a ReasonOfReferral by key.
        /// </summary>
        /// <param name="key">Primary key of the table ReasonOfReferrals.</param>
        /// <returns>Returns a ReasonOfReferral  if the key is matched.</returns>
        public async Task<ReasonOfReferral> GetReasonOfReferralByKey(int key)
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
        /// The method is used to get an ReasonOfReferral by ReasonOfReferral Description.
        /// </summary>
        /// <param name="description">Name of an ReasonOfReferral.</param>
        /// <returns>Returns an ReasonOfReferral if the ReasonOfReferral description is matched.</returns>
        public async Task<ReasonOfReferral> GetReasonOfReferralByName(string reasonOfReferral)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == reasonOfReferral.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}