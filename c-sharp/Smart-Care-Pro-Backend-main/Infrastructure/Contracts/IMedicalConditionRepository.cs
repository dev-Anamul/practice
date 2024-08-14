using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMedicalConditionRepository : IRepository<MedicalCondition>
    {
        /// <summary>
        /// The method is used to get a MedicalCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalConditions.</param>
        /// <returns>Returns a MedicalCondition if the key is matched.</returns>
        public Task<MedicalCondition> GetMedicalConditionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of MedicalCondition.
        /// </summary>
        /// <returns>Returns a list of all MedicalConditions.</returns>
        public Task<IEnumerable<MedicalCondition>> GetMedicalConditions();

        /// <summary>
        /// The method is used to get a MedicalCondition by Client Id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a MedicalConditions if the Client ID is matched.</returns>
        public Task<IEnumerable<MedicalCondition>> GetMedicalConditionByClient(Guid clientId);
        public Task<IEnumerable<MedicalCondition>> GetMedicalConditionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetMedicalConditionByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of MedicalCondition by Encounter.
        /// </summary>
        /// <returns>Returns a list of all MedicalCondition by EncounterID.</returns>
        public Task<IEnumerable<MedicalCondition>> GetMedicalConditionByEncounterID(Guid encounterId);
    }
}