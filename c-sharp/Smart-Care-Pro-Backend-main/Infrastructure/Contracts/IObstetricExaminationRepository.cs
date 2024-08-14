using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IObstetricExaminationRepository : IRepository<ObstetricExamination>
    {
        /// <summary>
        /// The method is used to get a ObstetricExamination by key.
        /// </summary>
        /// <param name="key">Primary key of the table ObstetricExaminations.</param>
        /// <returns>Returns a ObstetricExamination if the key is matched.</returns>
        public Task<ObstetricExamination> GetObstetricExaminationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ObstetricExamination.
        /// </summary>
        /// <returns>Returns a list of all ObstetricExamination.</returns>
        public Task<IEnumerable<ObstetricExamination>> GetObstetricExaminations();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ObstetricExamination if the ClientID is matched.</returns>
        public Task<IEnumerable<ObstetricExamination>> GetObstetricExaminationByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the list of ObstetricExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ObstetricExamination by EncounterID.</returns>
        public Task<IEnumerable<ObstetricExamination>> GetObstetricExaminationByEncounter(Guid encounterId);
    }
}