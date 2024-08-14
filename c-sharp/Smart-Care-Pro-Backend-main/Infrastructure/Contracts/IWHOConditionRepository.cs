using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IWHOConditionRepository : IRepository<WHOCondition>
    {
        /// <summary>
        /// The method is used to get a WHO condition by key.
        /// </summary>
        /// <param name="key">Primary key of the table WHOConditions.</param>
        /// <returns>Returns a WHO condition if the key is matched.</returns>
        public Task<WHOCondition> GetWHOConditionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of WHOConditions.
        /// </summary>
        /// <returns>Returns a list of all WHOConditions.</returns>
        public Task<IEnumerable<WHOCondition>> GetWHOConditions();

        public Task<IEnumerable<WHOCondition>> GetWHOConditionsByClient(Guid ClientId);
        public Task<IEnumerable<WHOCondition>> GetWHOConditionsByClient(Guid ClientId, int page, int pageSize, EncounterType? encounterType);
        public int GetWHOConditionsByClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a WHOConditions by Encounter.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a WHOConditions if the OPD visit ID is matched.</returns>
        public Task<IEnumerable<WHOCondition>> GetWHOConditionsByEncounterId(Guid EncounterId);
    }
}