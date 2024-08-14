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
    public interface IPEPRiskStatusRepository : IRepository<RiskStatus>
    {
        /// <summary>
        /// The method is used to get a PEP risk status by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPRiskStatuses.</param>
        /// <returns>Returns a PEP risk status if the key is matched.</returns>
        public Task<RiskStatus> GetPEPRiskStatusByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PEP risk statuses.
        /// </summary>
        /// <returns>Returns a list of all PEP risk statuses.</returns>
        public Task<IEnumerable<RiskStatus>> GetPEPRiskStatuses();

        public Task<IEnumerable<RiskStatus>> GetPEPRiskStatusByClientID(Guid clientId);

        public Task<IEnumerable<RiskStatus>> GetPEPRiskStatusByEncounterID(Guid EncounterID);
    }
}