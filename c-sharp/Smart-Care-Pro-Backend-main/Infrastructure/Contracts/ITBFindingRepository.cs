using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface ITBFindingRepository : IRepository<TBFinding>
    {
        /// <summary>
        /// The method is used to get a TBFinding by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBFindings.</param>
        /// <returns>Returns a TBFinding if the key is matched.</returns>
        public Task<TBFinding> GetTBFindingByKey(int key);

        /// <summary>
        /// The method is used to get the list of TBFinding.
        /// </summary>
        /// <returns>Returns a list of all TBFindings.</returns>
        public Task<IEnumerable<TBFinding>> GetTBFindings();

        /// <summary>
        /// The method is used to get an TBFinding by TBFinding Description.
        /// </summary>
        /// <param name="tBFinding">Description of an TBFinding.</param>
        /// <returns>Returns an TBFinding if the TBFinding name is matched.</returns>
        public Task<TBFinding> GetTBFindingByName(string tBFinding);
    }
}