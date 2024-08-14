using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IARTDrugAdherenceRepository : IRepository<ARTDrugAdherence>
    {
        /// <summary>
        /// The method is used to get an ART drug adherence by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugAdherences.</param>
        /// <returns>Returns an ART drug adherence if the key is matched.</returns>
        public Task<ARTDrugAdherence> GetARTDrugAdherenceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ART drug adherences.
        /// </summary>
        /// <returns>Returns a list of all ART drug adherences.</returns>
        public Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherences();

        /// <summary>
        /// The method is used to get an ART drug adherence by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns an ART drug adherence if the ClientID is matched.</returns>
        public Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClient(Guid clientId);
        public Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetARTDrugAdherenceByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of ART drug adherence by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ART drug adherence by EncounterID.</returns>
        public Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByEncounter(Guid encounterId);
    }
}