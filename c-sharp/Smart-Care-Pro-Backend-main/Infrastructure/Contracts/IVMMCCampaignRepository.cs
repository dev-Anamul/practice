using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVMMCCampaignRepository : IRepository<VMMCCampaign>
    {
        /// <summary>
        /// The method is used to get a VMMCCampaign by key.
        /// </summary>
        /// <param name="key">Primary key of the table VMMCCampaign.</param>
        /// <returns>Returns a VMMCCampaign if the key is matched.</returns>
        public Task<VMMCCampaign> GetVMMCCampaignByKey(int key);

        /// <summary>
        /// The method is used to get the list of VMMCCampaigns.
        /// </summary>
        /// <returns>Returns a list of all VMMCCampaigns.</returns>
        public Task<IEnumerable<VMMCCampaign>> GetVMMCCampaigns();
    }
}