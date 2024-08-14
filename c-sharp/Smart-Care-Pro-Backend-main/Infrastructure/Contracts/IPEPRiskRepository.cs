using Domain.Entities;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPEPRiskRepository : IRepository<Risks>
    {
        /// <summary>
        /// The method is used to get a PEP risk by name.
        /// </summary>
        /// <param name="pEPRisk">Name of a PEP risk.</param>
        /// <returns>Returns a PEP risk if the name is matched.</returns>
        public Task<Risks> GetPEPRiskByName(string pEPRisk);

        /// <summary>
        /// The method is used to get a PEP risk by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPRisks.</param>
        /// <returns>Returns a PEP risk if the key is matched.</returns>
        public Task<Risks> GetPEPRiskByKey(int key);

        /// <summary>
        /// The method is used to get the list of PEP risks.
        /// </summary>
        /// <returns>Returns a list of all PEP risks.</returns>
        public Task<IEnumerable<Risks>> GetPEPRisks();
    }
}