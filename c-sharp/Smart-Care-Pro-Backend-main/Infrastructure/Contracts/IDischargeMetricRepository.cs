using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDischargeMetricRepository : IRepository<DischargeMetric>
    {
        /// <summary>
        /// The method is used to get a DischargeMetric by key.
        /// </summary>
        /// <param name="key">Primary key of the table DischargeMetrics.</param>
        /// <returns>Returns a DischargeMetric if the key is matched.</returns>
        public Task<DischargeMetric> GetDischargeMetricByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of DischargeMetrics.
        /// </summary>
        /// <returns>Returns a list of all DischargeMetrics.</returns>
        public Task<IEnumerable<DischargeMetric>> GetDischargeMetrics();

        /// <summary>
        /// The method is used to get a DischargeMetric by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a DischargeMetric if the ClientID is matched.</returns>
        public Task<IEnumerable<DischargeMetric>> GetDischargeMetricByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the list of DischargeMetric by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all DischargeMetric by EncounterID.</returns>
        public Task<IEnumerable<DischargeMetric>> GetDischargeMetricByEncounter(Guid EncounterID);
    }
}