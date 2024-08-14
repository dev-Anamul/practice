using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IConditionRepository : IRepository<Condition>
    {
        /// <summary>
        /// The method is used to get a condition by key.
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <returns>Returns a condition if the key is matched.</returns>
        public Task<Condition> GetConditionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of conditions.
        /// </summary>
        /// <returns>Returns a list of all conditions.</returns>
        public Task<IEnumerable<Condition>> GetConditions();

        /// <summary>
        /// The method is used to get a client by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a Client if the ClientID is matched.</returns>
        public Task<IEnumerable<Condition>> GetConditionByClient(Guid ClientID);
        public Task<IEnumerable<Condition>> GetConditionByClientLast24Hours(Guid ClientID);
        public Task<IEnumerable<Condition>> GetConditionByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetConditionByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a OPD visit by OPDVisitID.
        /// </summary>
        /// <param name="OPDVisitID"></param>
        /// <returns>Returns a Client if the OPDVisitID is matched.</returns>
        public Task<IEnumerable<Condition>> GetConditionByOPDVisitID(Guid encounterId);
    }
}