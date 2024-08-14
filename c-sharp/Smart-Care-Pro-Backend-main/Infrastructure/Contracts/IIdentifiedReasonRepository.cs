using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IIdentifiedReasonRepository : IRepository<IdentifiedReason>
    {
        /// <summary>
        /// The method is used to get a IdentifiedReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedReason.</param>
        /// <returns>Returns a IdentifiedReason if the key is matched.</returns>
        public Task<IdentifiedReason> GetIdentifiedReasonByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of IdentifiedReasons.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedReasons.</returns>
        public Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasons();

        /// <summary>
        /// The method is used to get a IdentifiedReason by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a IdentifiedReason if the ClientID is matched.</returns>
        public Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByClient(Guid clientId);
        public Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedReasonByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a IdentifiedReason by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedReason if the EncounterId is matched.</returns>
        public Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByEncounterId(Guid encounterId);
    }
}