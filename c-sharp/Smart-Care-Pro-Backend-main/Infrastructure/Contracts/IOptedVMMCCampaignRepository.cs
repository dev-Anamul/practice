using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IOptedVMMCCampaignRepository : IRepository<OptedVMMCCampaign>
    {
        /// <summary>
        /// The method is used to get a OptedVMMCCampaign by InteractionId.
        /// </summary>
        /// <param name="interactionId">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Returns a OptedVMMCCampaign if the InteractionId is matched.</returns>
        public Task<OptedVMMCCampaign> GetOptedVMMCCampaignByKey(Guid interactionId);

        /// <summary>
        /// The method is used to get the list of OptedVMMCCampaigns.
        /// </summary>
        /// <returns>Returns a list of all OptedVMMCCampaigns.</returns>
        public Task<IEnumerable<OptedVMMCCampaign>> GetOptedVMMCCampaigns();

        /// <summary>
        /// The method is used to get a OptedVMMCCampaign by VMMCCampaignId.
        /// </summary>
        /// <param name="vmmcCampaignId">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Returns a OptedVMMCCampaign if the VMMCCampaignId is matched.</returns>
        public Task<OptedVMMCCampaign> GetOptedVMMCCampaignByVMMCCampaignId(int vmmcCampaignId);

        /// <summary>
        /// The method is used to get a OptedVMMCCampaign by VMMCServiceId.
        /// </summary>
        /// <param name="vmmcServiceId">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Returns a OptedVMMCCampaign if the VMMCServiceId is matched.</returns>
        public Task<IEnumerable<OptedVMMCCampaign>> GetVMMCCampaignByVMMCService(Guid vmmcServiceId);
    }
}