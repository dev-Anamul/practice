using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ITBSuspectingReasonRepository : IRepository<TBSuspectingReason>
    {
        /// <summary>
        /// The method is used to get a TBSuspectingReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBSuspectingReason.</param>
        /// <returns>Returns a TBSuspectingReason if the key is matched.</returns>
        public Task<TBSuspectingReason> GetTBSuspectingReasonByKey(int key);

        /// <summary>
        /// The method is used to get the list of TBSuspectingReasons.
        /// </summary>
        /// <returns>Returns a list of all TBSuspectingReason.</returns>
        public Task<IEnumerable<TBSuspectingReason>> GetTBSuspectingReasons();
    }
}