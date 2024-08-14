using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IBloodTransfusionHistoryRepository : IRepository<BloodTransfusionHistory>
    {
        /// <summary>
        /// The method is used to get a BloodTransfusionHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table BloodTransfusionHistorys.</param>
        /// <returns>Returns a BloodTransfusionHistory if the key is matched.</returns>
        public Task<BloodTransfusionHistory> GetBloodTransfusionHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of BloodTransfusionHistory.
        /// </summary>
        /// <returns>Returns a list of all BloodTransfusionHistory.</returns>
        public Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistorys();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a BloodTransfusionHistory if the ClientID is matched.</returns>
        public Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClient(Guid clientId);
        public Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetBloodTransfusionHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get the list of BloodTransfusionHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all BloodTransfusionHistory by EncounterID.</returns>
        public Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByEncounter(Guid encounterId);
    }
}