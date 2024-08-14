using Domain.Entities;
using static Utilities.Constants.Enums;

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
    public interface ITreatmentPlanRepository : IRepository<TreatmentPlan>
    {
        /// <summary>
        /// The method is used to get a treatment plan by key.
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Returns a treatment plan if the key is matched.</returns>
        public Task<TreatmentPlan> GetTreatmentPlanByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of treatment plans.
        /// </summary>
        /// <returns>Returns a list of all treatment plans.</returns>
        public Task<IEnumerable<TreatmentPlan>> GetTreatmentPlans();

        /// <summary>
        /// The method is used to get a OPD visite by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisites.</param>
        /// <returns>Returns a OPD visite if the key is matched.</returns>
        public Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansOPDVisitID(Guid OpdVisitID);

        /// <summary>
        /// The method is used to get a OPD visite by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisites.</param>
        /// <returns>Returns a OPD visite if the key is matched.</returns>
        public Task<TreatmentPlan> GetTreatmentPlansEncounterId(Guid encounterId);

        /// <summary>
        /// The method is used to get a TreatmentPlan by Surgery key.
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <returns>Returns a TreatmentPlan if the key is matched.</returns>
        public Task<TreatmentPlan> GetTreatmentPlanBySurgeryId(Guid key);

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="clientID">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansClient(Guid clientID);
        public Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetTreatmentPlansClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get the last Encounter TreatmentPlan.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public Task<IEnumerable<TreatmentPlan>> GetLastEncounterTreatmentPlanByClient(Guid clientID);
        /// <summary>
        /// The method is used to get the last TreatmentPlan.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public Task<TreatmentPlan> GetLatestTreatmentPlanByClient(Guid ClientID);
        public Task<TreatmentPlan> GetLatestTreatmentPlanByClientForFluid(Guid ClientID);

    }
}