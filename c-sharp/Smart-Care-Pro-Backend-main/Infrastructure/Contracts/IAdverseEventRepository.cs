using Domain.Entities;
/*
 * Created by   : Stephan
 * Date created : 16.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAdverseEventRepository : IRepository<AdverseEvent>
    {
        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table AdverseEvents.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public Task<AdverseEvent> GetAdverseEventByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<AdverseEvent>> GetAdverseEvents();

        /// <summary>
        /// The method is used to get a birth record by ImmunizationID.
        /// </summary>
        /// <param name="immunizationId"></param>
        /// <returns>Returns a birth record if the ImmunizationID is matched.</returns>
        public Task<IEnumerable<AdverseEvent>> GetAdverseEventByImmunization(Guid immunizationId);

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<AdverseEvent>> GetAdverseEventByEncounter(Guid encounterId);
    }
}