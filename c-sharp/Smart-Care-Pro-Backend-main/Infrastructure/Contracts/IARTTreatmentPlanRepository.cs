using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 10.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IARTTreatmentPlanRepository : IRepository<ARTTreatmentPlan>
    {
        /// <summary>
        /// The method is used to get an ARTTreatmentPlan by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTTreatmentPlan.</param>
        /// <returns>Returns an ARTTreatmentPlan if the key is matched.</returns>
        public Task<ARTTreatmentPlan> GetARTTreatmentPlanByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ARTTreatmentPlan.
        /// </summary>
        /// <returns>Returns a list of all ARTTreatmentPlan.</returns>
        public Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlan();

        /// <summary>
        /// The method is used to get a TreatmentPlan by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a TreatmentPlan if the ClientID is matched.</returns>
        public Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClient(Guid clientId);
        public Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetARTTreatmentPlanByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a ARTTreatmentPlan by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a ARTTreatmentPlan if the Encounter is matched.</returns>
        public Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByEncounter(Guid encounterId);
    }
}