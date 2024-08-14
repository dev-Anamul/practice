using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IChildsDevelopmentHistoryRepository : IRepository<ChildsDevelopmentHistory>
    {
        /// <summary>
        /// The method is used to get a child's development history by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChildsDevelopmentHistories.</param>
        /// <returns>Returns a child development history if the key is matched.</returns>
        public Task<ChildsDevelopmentHistory> GetChildsDevelopmentHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of child's development histories.
        /// </summary>
        /// <returns>Returns a list of all child's development histories.</returns>
        public Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistories();

        /// <summary>
        /// The method is used to get a chief complaints by Client Id.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a chief complaints if the Client ID is matched.</returns>
        public Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClient(Guid ClientID, int page, int pageSize);
        public Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClient(Guid ClientID);
        public Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClientLast24Hours(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of child's development histories by OPD visit ID.
        /// </summary>
        /// <returns>Returns a list of all child's development histories by OPD visit ID.</returns>
        public Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByOpdVisit(Guid OPDVisitID);
        public Task<int> GetChildsDevelopmentHistoryByClientCount(Guid ClientID);
    }
}