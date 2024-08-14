using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDFZRankRepository : IRepository<DFZRank>
    {
        /// <summary>
        /// The method is used to get a DFZRank by DFZRank name.
        /// </summary>
        /// <param name="DFZRankName">Name of a DFZRank.</param>
        /// <returns>Returns a facility if the facility name is matched.</returns>
        public Task<DFZRank> GetDFZRankByName(string DFZRankName);

        /// <summary>
        /// The method is used to get a DFZRank by key.
        /// </summary>
        /// <param name="key">Primary key of the table DFZRanks.</param>
        /// <returns>Returns a DFZRank if the key is matched.</returns>
        public Task<DFZRank> GetDFZRankByKey(int key);

        /// <summary>
        /// The method is used to get the DFZRanks by patientTypeId.
        /// </summary>
        /// <param name="patientTypeId">patientTypeId of the table DFZRanks.</param>
        /// <returns>Returns a DFZRank if the patientTypeId is matched.</returns>
        public Task<IEnumerable<DFZRank>> GetDFZRankByPatientType(int patientTypeId);

        /// <summary>
        /// The method is used to get the list of DFZRanks.
        /// </summary>
        /// <returns>Returns a list of all DFZRanks.</returns>
        public Task<IEnumerable<DFZRank>> GetDFZRanks();
    }
}