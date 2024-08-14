using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IIdentifiedTBSymptomRepository : IRepository<IdentifiedTBSymptom>
    {
        /// <summary>
        /// The method is used to get a IdentifiedTBSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptom.</param>
        /// <returns>Returns a IdentifiedTBSymptom if the key is matched.</returns>
        public Task<IdentifiedTBSymptom> GetIdentifiedTBSymptomByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of IdentifiedTBSymptoms.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedTBSymptoms.</returns>
        public Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptoms();

        /// <summary>
        /// The method is used to get a IdentifiedTBSymptom by OPDID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a IdentifiedTBSymptom if the clientId is matched.</returns>
        public Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByClient(Guid clientId);
        public Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedTBSymptomByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a IdentifiedTBSymptom by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedTBSymptom if the EncounterId is matched.</returns>
        public Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByEncounterId(Guid encounterId);
    }
}