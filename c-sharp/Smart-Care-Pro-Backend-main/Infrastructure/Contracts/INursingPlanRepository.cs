using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INursingPlanRepository : IRepository<NursingPlan>
    {
        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public Task<NursingPlan> GetNursingPlanByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth nursingPlan.
        /// </summary>
        /// <returns>Returns a list of all birth nursingPlan.</returns>
        public Task<IEnumerable<NursingPlan>> GetNursingPlans();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth record if the ClientID is matched.</returns>
        public Task<IEnumerable<NursingPlan>> GetNursingPlanByClient(Guid clientId);
        public Task<IEnumerable<NursingPlan>> GetNursingPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetNursingPlanByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<NursingPlan>> GetNursingPlanByEncounterId(Guid encounterId);
    }
}