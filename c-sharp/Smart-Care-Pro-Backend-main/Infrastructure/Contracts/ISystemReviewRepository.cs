using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    /// <summary>
    /// Implementation of ISystemReviewRepository interface.
    /// </summary>
    public interface ISystemReviewRepository : IRepository<SystemReview>
    {
        /// <summary>
        /// The method is used to get a system review by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemReviews.</param>
        /// <returns>Returns a system review if the key is matched.</returns>
        public Task<IEnumerable<SystemReview>> GetSystemReviewByPhysicalSystemId(int physicalSystemId);


        /// <summary>
        /// The method is used to get a system review by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemReviews.</param>
        /// <returns>Returns a system review if the key is matched.</returns>
        public Task<SystemReview> GetSystemReviewByKey(Guid key);

        /// <summary>
        /// The method is used to get a OPD visite by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisites.</param>
        /// <returns>Returns a OPD visite if the key is matched.</returns>
        public Task<IEnumerable<SystemReview>> GetSystemReviewByOPDVisit(Guid OPDVisitID);

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public Task<IEnumerable<SystemReview>> GetSystemReviewByClient(Guid ClientID);
        public Task<IEnumerable<SystemReview>> GetSystemReviewByClient(Guid ClientID, int page, int pageSize,EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of system reviews.
        /// </summary>
        /// <returns>Returns a list of all system reviews.</returns>
        public Task<IEnumerable<SystemReview>> GetSystemReviews();
        public int GetSystemReviewByClientTotalCount(Guid clientID, EncounterType? encounterType);
    }
}