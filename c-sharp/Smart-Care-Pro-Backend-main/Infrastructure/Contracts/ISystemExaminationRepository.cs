using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 01.01.2023
 * Modified by  : Stephan
 * Last modified: 14.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    /// <summary>
    /// Implementation of ISystemExaminationRepository interface.
    /// </summary>
    public interface ISystemExaminationRepository : IRepository<SystemExamination>
    {
        /// <summary>
        /// The method is used to get a system examination by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Returns a system examination if the key is matched.</returns>
        public Task<SystemExamination> GetSystemExaminationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of system examinations.
        /// </summary>
        /// <returns>Returns a list of all system examinations.</returns>
        public Task<IEnumerable<SystemExamination>> GetSystemExaminations();

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public Task<IEnumerable<SystemExamination>> GetSystemExaminationsByClientID(Guid clientID);
        public Task<IEnumerable<SystemExamination>> GetSystemExaminationsByClientID(Guid clientID, int page, int pageSize, EncounterType? encounterType);
        public int GetSystemExaminationsByClientIDTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a OPD visite by key.
        /// </summary>
        /// <param name="EncounterID">Primary key of the table OPDEncounters.</param>
        /// <returns>Returns a OPD visite if the key is matched.</returns>
        public Task<IEnumerable<SystemExamination>> GetSystemReviewByEncounter(Guid EncounterID);
    }
}