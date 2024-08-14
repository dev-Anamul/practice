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
    public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
    {
        /// <summary>
        /// The method is used to get a medical history by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalHistories.</param>
        /// <returns>Returns a medical history if the key is matched.</returns>
        public Task<MedicalHistory> GetMedicalHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of medical histories.
        /// </summary>
        /// <returns>Returns a list of all medical histories.</returns>
        public Task<IEnumerable<MedicalHistory>> GetMedicalHistories();

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByClient(Guid clientId);
        public Task<IEnumerable<MedicalHistory>> GetPastMedicalHistoriesByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetPastMedicalHistoriesByClientTotalCount(Guid clientID, EncounterType? encounterType);
        public int GetFamilyFoodHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType);
        public Task<IEnumerable<MedicalHistory>> GetFamilyFoodHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a visit by key.
        /// </summary>
        /// <param name="visitId">Primary key of the table Visits.</param>
        /// <returns>Returns a visit if the key is matched.</returns>
        public Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByVisitID(Guid visitId);
    }
}