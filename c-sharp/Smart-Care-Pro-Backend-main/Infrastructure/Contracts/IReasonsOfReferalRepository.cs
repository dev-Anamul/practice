using Domain.Entities;

/*
 * Created by   : 
 * Date created : 
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IReasonsOfReferalRepository : IRepository<ReasonOfReferral>
    {
        /// <summary>
        /// The method is used to get the list of referrals.
        /// </summary>
        /// <returns>Returns a list of all referrals.</returns>
        public Task<IEnumerable<ReasonOfReferral>> GetReasonOfReferrals();

        /// <summary>
        /// The method is used to get a ReasonOfReferral by key.
        /// </summary>
        /// <param name="key">Primary key of the table ReasonOfReferrals.</param>
        /// <returns>Returns a ReasonOfReferral if the key is matched.</returns>
        public Task<ReasonOfReferral> GetReasonOfReferralByKey(int key);

        /// <summary>
        /// The method is used to get an ReasonOfReferral by ReasonOfReferral Description.
        /// </summary>
        /// <param name="reasonOfReferral">Description of an ReasonOfReferral.</param>
        /// <returns>Returns an ReasonOfReferral if the ReasonOfReferral name is matched.</returns>
        public Task<ReasonOfReferral> GetReasonOfReferralByName(string reasonOfReferral);
    }
}