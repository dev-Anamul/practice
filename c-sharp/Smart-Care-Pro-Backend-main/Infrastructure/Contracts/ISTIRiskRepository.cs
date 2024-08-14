using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 03.05.2023
 * Modified by  : Brian
 * Last modified: 03.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ISTIRiskRepository : IRepository<STIRisk>
    {
        /// <summary>
        /// The method is used to get the list of STIRisk.
        /// </summary>
        /// <returns>Returns a list of all STIRisks.</returns>
        public Task<IEnumerable<STIRisk>> GetSTIRisks();

        /// <summary>
        /// The method is used to get a STIRisk by key.
        /// </summary>
        /// <param name="key">Primary key of the table STIRisks.</param>
        /// <returns>Returns a STIRisk if the key is matched.</returns>
        public Task<STIRisk> GetSTIRiskByKey(int key);

        /// <summary>
        /// The method is used to get an STIRisk by STIRisk Description.
        /// </summary>
        /// <param name="sTIRisk">Description of an STIRisk.</param>
        /// <returns>Returns an STIRisk if the STIRisk name is matched.</returns>
        public Task<STIRisk> GetSTIRiskByName(string sTIRisk);
    }
}