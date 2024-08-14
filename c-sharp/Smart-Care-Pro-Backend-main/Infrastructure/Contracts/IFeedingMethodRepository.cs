using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IFeedingMethodRepository : IRepository<FeedingMethod>
    {
        /// <summary>
        /// The method is used to get a FeedingMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table FeedingMethods.</param>
        /// <returns>Returns a FeedingMethod if the key is matched.</returns>
        public Task<FeedingMethod> GetFeedingMethodByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of FeedingMethods.
        /// </summary>
        /// <returns>Returns a list of all FeedingMethods.</returns>
        public Task<IEnumerable<FeedingMethod>> GetFeedingMethods();

        /// <summary>
        /// The method is used to get a FeedingMethod by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a FeedingMethod if the ClientId is matched.</returns>
        public Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClient(Guid ClientId);
        public Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClientLast24Hours(Guid ClientId);
        public Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClient(Guid ClientId, int page, int pageSize, EncounterType? encounterType);
        public int GetFeedingMethodByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of FeedingMethod by EncounterId.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a list of all FeedingMethod by EncounterId.</returns>
        public Task<IEnumerable<FeedingMethod>> GetFeedingMethodByEncounter(Guid EncounterId);
    }
}