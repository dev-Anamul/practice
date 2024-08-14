using Domain.Entities;

/*
 * Created by   : Tariqul Islam
 * Date created : 05.03.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IQuickExaminationRepository : IRepository<QuickExamination>
    {
        /// <summary>
        /// The method is used to get a QuickExaminationByKey by key.
        /// </summary>
        /// <param name="key">Primary key of the table QuickExaminationByKey.</param>
        /// <returns>Returns a QuickExaminationByKey if the key is matched.</returns>
        public Task<QuickExamination> ReadQuickExaminationByKey(Guid key);

        /// <summary>
        /// The method is used to get a QuickExaminationByKey by key.
        /// </summary>
        /// <returns>Returns a QuickExaminationByKey if the key is matched.</returns>
        public Task<IEnumerable<QuickExamination>> ReadQuickExaminations();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a QuickExamination if the ClientID is matched.</returns>
        public Task<IEnumerable<QuickExamination>> ReadQuickExaminationByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of QuickExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all QuickExamination by EncounterID.</returns>
        public Task<IEnumerable<QuickExamination>> ReadQuickExaminationByEncounter(Guid EncounterID);
    }
}
