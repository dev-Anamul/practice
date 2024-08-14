using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAnestheticPlanRepository : IRepository<AnestheticPlan>
    {
        /// <summary>
        /// The method is used to get an anesthetic plan by key.
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlans.</param>
        /// <returns>Returns an anesthetic plan if the key is matched.</returns>
        public Task<AnestheticPlan> GetAnestheticPlanByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of anesthetic plans.
        /// </summary>
        /// <returns>Returns a list of all anesthetic plans.</returns>
        public Task<IEnumerable<AnestheticPlan>> GetAnestheticPlans();

        /// <summary>
        /// The method is used to get an anesthetic plan by EncounterID.
        /// </summary>
        /// <param name="encounterId">Primary key of the table EncounterBaseModel.</param>
        /// <returns>Returns an anesthetic plan if the EncounterID is matched.</returns>
        public Task<IEnumerable<AnestheticPlan>> GetAnestheticPlanByEncounter(Guid encounterId);

        /// <summary>
        /// The method is used to get an anesthetic plan by SurgeryID.
        /// </summary>
        /// <param name="surgeryId">Primary key of the table Surgeries.</param>
        /// <returns>Returns an anesthetic plan if the SurgeryID is matched.</returns>
        public Task<AnestheticPlan> GetAnestheticPlanBySurgery(Guid surgeryId);

        /// <summary>
        /// The method is used to get an anesthetic plan by SurgeryID.
        /// </summary>
        /// <param name="surgeryId"></param>
        /// <returns>Returns List of anesthetic plan if the SurgeryID is matched.</returns>
        public Task <IEnumerable<AnestheticPlan>> GetAnestheticPlanListBySurgery(Guid surgeryId);
    }
}