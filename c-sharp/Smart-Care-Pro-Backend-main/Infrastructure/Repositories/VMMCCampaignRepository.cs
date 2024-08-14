using Domain.Entities;
using Infrastructure.Contracts;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVMMCCampaignRepository interface.
    /// </summary>
    public class VMMCCampaignRepository : Repository<VMMCCampaign>, IVMMCCampaignRepository
    {
        public VMMCCampaignRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get VMMCCampaign by key.
        /// </summary>
        /// <param name="key">Primary key of the table VMMCCampaign.</param>
        /// <returns>Returns a VMMCCampaign if the key is matched.</returns>
        public async Task<VMMCCampaign> GetVMMCCampaignByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VMMCCampaign.
        /// </summary>
        /// <returns>Returns a list of all VMMCCampaigns.</returns>
        public async Task<IEnumerable<VMMCCampaign>> GetVMMCCampaigns()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
