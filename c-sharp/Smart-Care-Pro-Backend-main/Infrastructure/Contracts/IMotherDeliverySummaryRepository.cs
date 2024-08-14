using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMotherDeliverySummaryRepository : IRepository<MotherDeliverySummary>
    {
        /// <summary>
        /// The method is used to get a mother delivery summary by key.
        /// </summary>
        /// <param name="key">Primary key of the table MotherDeliverySummaries.</param>
        /// <returns>Returns a mother delivery summary if the key is matched.</returns>
        public Task<MotherDeliverySummary> GetMotherDeliverySummaryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of mother delivery summary.
        /// </summary>
        /// <returns>Returns a list of all mother delivery summary.</returns>
        public Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaries();

        /// <summary>
        /// The method is used to get a mother delivery summary by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a mother delivery summary if the clientId is matched.</returns>
        public Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaryByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a mother delivery summary by OPD visit.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a mother delivery summary if the Encounter is matched.</returns>
        public Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaryByEncounter(Guid encounterId);
    }
}
