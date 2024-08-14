using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Biplob Roy
 * Date created : 29.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPelvicAndVaginalExaminationRepository : IRepository<PelvicAndVaginalExamination>
    {
        /// <summary>
        /// The method is used to get a PelvicAndVaginalExamination by key.
        /// </summary>
        /// <param name="key">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Returns a PelvicAndVaginalExamination if the key is matched.</returns>
        public Task<PelvicAndVaginalExamination> GetPelvicAndVaginalExaminationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PelvicAndVaginalExaminations.
        /// </summary>
        /// <returns>Returns a list of all PelvicAndVaginalExaminations.</returns>
        public Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminations();

        /// <summary>
        /// The method is used to get a PelvicAndVaginalExamination by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PelvicAndVaginalExamination if the ClientID is matched.</returns>
        public Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByClient(Guid ClientID);
        public Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetPelvicAndVaginalExaminationByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of PelvicAndVaginalExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PelvicAndVaginalExamination by EncounterID.</returns>
        public Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByEncounter(Guid EncounterID);
    }
}