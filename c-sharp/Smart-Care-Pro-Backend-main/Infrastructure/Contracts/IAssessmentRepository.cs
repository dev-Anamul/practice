using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 01.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        /// <summary>
        /// The method is used to get a Assessment by key.
        /// </summary>
        /// <param name="key">Primary key of the table Assessments.</param>
        /// <returns>Returns a Assessment if the key is matched.</returns>
        public Task<Assessment> GetAssessmentByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of Assessment.
        /// </summary>
        /// <returns>Returns a list of all Assessments.</returns>
        public Task<IEnumerable<Assessment>> GetAssessments();

        /// <summary>
        /// The method is used to get a chief complaints by Client Id.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns>Returns a chief complaints if the Client ID is matched.</returns>
        public Task<IEnumerable<Assessment>> GetAssessmentsByClient(Guid clientId);
        public Task<IEnumerable<Assessment>> GetAssessmentsByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<Assessment>> GetAssessmentsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetAssessmentsByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of Assessment by encounter.
        /// </summary>
        /// <returns>Returns a list of all Assessment by encounter.</returns>
        public Task<IEnumerable<Assessment>> GetAssessmentByEncounter(Guid encounterId);
    }
}