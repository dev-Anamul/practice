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
    public interface IImmunizationRecordRepository : IRepository<ImmunizationRecord>
    {
        /// <summary>
        /// The method is used to get a immunization record by key.
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <returns>Returns a immunization record if the key is matched.</returns>
        public Task<ImmunizationRecord> GetImmunizationRecordByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of immunization records.
        /// </summary>
        /// <returns>Returns a list of all immunization records.</returns>
        public Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecords();

        /// <summary>
        /// The method is used to get a immunization record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a immunization record if the ClientID is matched.</returns>
        public Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClient(Guid clientId);
        public Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetImmunizationRecordByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a Immunization Records by OPDID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a Immunization Records if the EncounterId is matched.</returns>
        public Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByEncounter(Guid encounterId);
    }
}